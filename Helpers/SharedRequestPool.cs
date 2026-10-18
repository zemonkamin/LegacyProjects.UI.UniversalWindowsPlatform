using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Helpers
{
    // One producer per key. A recycled image cancels only its own subscription;
    // the transfer is cancelled when no visible consumer still needs it.
    public sealed class SharedRequestPool<T>
    {
        private readonly object _gate = new object();
        private readonly Dictionary<string, Pending> _pending = new Dictionary<string, Pending>();

        public async Task<T> RunAsync(string key, Func<CancellationToken, Task<T>> produce, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            Pending pending;
            lock (_gate)
            {
                if (!_pending.TryGetValue(key, out pending))
                {
                    pending = new Pending();
                    _pending.Add(key, pending);
                    pending.Users = 1;
                    pending.Task = ExecuteAsync(key, pending, produce);
                    pending.Observation = pending.Task.ContinueWith(task => { var observed = task.Exception; }, CancellationToken.None,
                        TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                }
                else pending.Users++;
            }
            try
            {
                var cancelled = new TaskCompletionSource<bool>();
                using (cancellation.Register(() => cancelled.TrySetResult(true)))
                {
                    if (await Task.WhenAny(pending.Task, cancelled.Task).ConfigureAwait(false) != pending.Task)
                        cancellation.ThrowIfCancellationRequested();
                    cancellation.ThrowIfCancellationRequested();
                    return await pending.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                lock (_gate)
                {
                    pending.Users--;
                    if (pending.Users == 0 && !pending.Finished)
                    {
                        Pending current;
                        if (_pending.TryGetValue(key, out current) && current == pending) _pending.Remove(key);
                        pending.Cancellation.Cancel();
                    }
                }
            }
        }

        private async Task<T> ExecuteAsync(string key, Pending pending, Func<CancellationToken, Task<T>> produce)
        {
            try { return await produce(pending.Cancellation.Token).ConfigureAwait(false); }
            finally
            {
                lock (_gate)
                {
                    pending.Finished = true;
                    Pending current;
                    if (_pending.TryGetValue(key, out current) && current == pending) _pending.Remove(key);
                    pending.Cancellation.Dispose();
                }
            }
        }

        private sealed class Pending
        {
            internal readonly CancellationTokenSource Cancellation = new CancellationTokenSource();
            internal Task<T> Task;
            internal Task Observation;
            internal int Users;
            internal bool Finished;
        }
    }
}
