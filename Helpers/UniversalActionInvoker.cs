using System;
using System.Reflection;
using Windows.UI.Xaml;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Helpers
{
    internal static class UniversalActionInvoker
    {
        public static void Invoke(object owner, object actionTarget, string actionTargetTypeName, string actionName, object eventArgs, object item, int index)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

            try
            {
                Type targetType = null;
                object target = actionTarget;

                if (target != null)
                {
                    targetType = target.GetType();
                }
                else
                {
                    targetType = ResolveActionTargetType(actionTargetTypeName);
                }

                if (targetType == null)
                {
                    return;
                }

                MethodInfo method = FindActionMethod(targetType, actionName, owner, eventArgs, item, index);
                if (method == null)
                {
                    return;
                }

                if (!method.IsStatic && target == null)
                {
                    target = Activator.CreateInstance(targetType);
                }

                if (!method.IsStatic && target == null)
                {
                    return;
                }

                object[] parameters;
                if (!TryBuildActionParameters(method, owner, eventArgs, item, index, actionName, out parameters))
                {
                    return;
                }

                method.Invoke(method.IsStatic ? null : target, parameters);
            }
            catch
            {
            }
        }

        private static Type ResolveActionTargetType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            if (Application.Current != null)
            {
                Assembly appAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
                if (appAssembly != null)
                {
                    type = appAssembly.GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        private static MethodInfo FindActionMethod(Type targetType, string actionName, object owner, object eventArgs, object item, int index)
        {
            if (targetType == null || string.IsNullOrWhiteSpace(actionName))
            {
                return null;
            }

            MethodInfo fallbackMethod = null;

            foreach (MethodInfo method in targetType.GetRuntimeMethods())
            {
                if (method == null || method.Name != actionName)
                {
                    continue;
                }

                object[] parameters;
                if (TryBuildActionParameters(method, owner, eventArgs, item, index, actionName, out parameters))
                {
                    return method;
                }

                if (fallbackMethod == null)
                {
                    fallbackMethod = method;
                }
            }

            return fallbackMethod;
        }

        private static bool TryBuildActionParameters(MethodInfo method, object owner, object eventArgs, object item, int index, string actionName, out object[] values)
        {
            values = null;

            if (method == null)
            {
                return false;
            }

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters == null || parameters.Length == 0)
            {
                values = new object[0];
                return true;
            }

            if (parameters.Length == 1)
            {
                object value;
                if (TryGetActionParameterValue(parameters[0].ParameterType, owner, eventArgs, item, index, actionName, false, out value))
                {
                    values = new object[] { value };
                    return true;
                }
            }

            if (parameters.Length == 2)
            {
                object senderValue;
                object argsValue;
                if (TryGetActionParameterValue(parameters[0].ParameterType, owner, eventArgs, item, index, actionName, true, out senderValue) &&
                    TryGetActionParameterValue(parameters[1].ParameterType, owner, eventArgs, item, index, actionName, false, out argsValue))
                {
                    values = new object[] { senderValue, argsValue };
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetActionParameterValue(Type parameterType, object owner, object eventArgs, object item, int index, string actionName, bool senderParameter, out object value)
        {
            value = null;

            if (parameterType == null)
            {
                return false;
            }

            if (senderParameter)
            {
                value = owner;
                return IsTypeAssignable(parameterType, value);
            }

            if (eventArgs != null && IsTypeAssignable(parameterType, eventArgs))
            {
                value = eventArgs;
                return true;
            }

            if (item != null && IsTypeAssignable(parameterType, item))
            {
                value = item;
                return true;
            }

            if (owner != null && IsTypeAssignable(parameterType, owner))
            {
                value = owner;
                return true;
            }

            if (parameterType == typeof(int))
            {
                value = index;
                return true;
            }

            if (parameterType == typeof(string))
            {
                value = actionName ?? string.Empty;
                return true;
            }

            if (parameterType == typeof(object))
            {
                value = eventArgs;
                return true;
            }

            return false;
        }

        private static bool IsTypeAssignable(Type targetType, object value)
        {
            if (targetType == null || value == null)
            {
                return false;
            }

            return targetType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo());
        }
    }
}
