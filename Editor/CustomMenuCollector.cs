using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace UIBuilderCustomMenu.Editor
{
    internal static class CustomMenuCollector
    {
        public static readonly List<UIBuilderCustomMenuCreator.Creator> Creators = new();

        public static void CollectCustomMenuCreateMethods()
        {
            Creators.Clear();

            var methods = TypeCache.GetMethodsWithAttribute<CreateUIBuilderCustomMenuAttribute>();
            foreach (var method in methods)
            {
                if (!method.IsStatic)
                {
                    UnityEngine.Debug.LogError(
                        $"{method.Name} has no static modifier {GetDeclaringPointMessage(method)}"
                    );
                    continue;
                }

                var returnType = method.ReturnType;
                if (returnType != typeof(List<CustomMenuInfo>))
                {
                    UnityEngine.Debug.LogError(
                        $"Return type of {method.Name} is not {typeof(List<CustomMenuInfo>)} {GetDeclaringPointMessage(method)}"
                    );
                    continue;
                }

                var methodParams = method.GetParameters();
                if (methodParams.Length != 0)
                {
                    UnityEngine.Debug.LogError(
                        $"Methods with {typeof(CreateUIBuilderCustomMenuAttribute)} must not have arguments {GetDeclaringPointMessage(method)}"
                    );
                    continue;
                }

                var attribute = (CreateUIBuilderCustomMenuAttribute)Attribute.GetCustomAttribute(
                    method,
                    typeof(CreateUIBuilderCustomMenuAttribute)
                );

                var creator = new UIBuilderCustomMenuCreator.Creator(
                    create: () => (List<CustomMenuInfo>)method.Invoke(null, null),
                    priority: attribute.Priority
                );

                Creators.Add(creator);
            }
        }

        private static string GetDeclaringPointMessage(MethodInfo method)
        {
            return $"At: {method.DeclaringType}.{method.Name}";
        }
    }
}