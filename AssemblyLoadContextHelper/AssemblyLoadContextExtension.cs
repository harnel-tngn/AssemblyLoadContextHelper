using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace AssemblyLoadContextHelper;

/// <summary>
/// Extension methods for AssemblyLoadContext.
/// </summary>
public static class AssemblyLoadContextExtension
{
    private static Assembly CoreLibAsembly = typeof(int).Assembly;

    /// <summary>
    /// Returns assembly with same FullName from the given AssemblyLoadContext.
    /// </summary>
    public static Assembly GetMatchingAssembly(this AssemblyLoadContext context, Assembly assembly, bool loadIfNotLoaded = false)
    {
        // Note: System.Private.CoreLib is unable to load
        if (assembly.FullName == CoreLibAsembly.FullName)
            return assembly;

        var matchedAssembly = context
            .Assemblies
            .FirstOrDefault(asm => asm.FullName == assembly.FullName);

        if (matchedAssembly == null)
        {
            if (loadIfNotLoaded)
            {
                if (assembly.IsDynamic)
                    throw new Exception("Failed to load dynamic assembly");

                return context.LoadFromAssemblyPath(assembly.Location);
            }

            throw new Exception($"Failed to find {assembly.FullName} in {context.Name}");
        }

        return matchedAssembly;
    }

    /// <summary>
    /// Returns Type with same AssemblyQualifiedName from the given AssemblyLoadContext.
    /// </summary>
    public static Type GetMatchingType(this AssemblyLoadContext context, Type type, bool loadIfNotLoaded = false)
    {
        if (type.IsGenericType && !type.IsGenericTypeDefinition)
        {
            var matchedOpenType = context.GetMatchingType(type.GetGenericTypeDefinition(), loadIfNotLoaded: loadIfNotLoaded);
            var typeArguments = type
                .GetGenericArguments()
                .Select(type => context.GetMatchingType(type, loadIfNotLoaded: loadIfNotLoaded))
                .ToArray();

            return matchedOpenType.MakeGenericType(typeArguments);
        }

        var matchedAssembly = context.GetMatchingAssembly(type.Assembly, loadIfNotLoaded: loadIfNotLoaded);
        var matchedType = matchedAssembly
            .GetTypes()
            .SingleOrDefault(t => t.AssemblyQualifiedName == type.AssemblyQualifiedName);

        if (matchedType == null)
            throw new Exception($"Failed to find {type.AssemblyQualifiedName} in {context.Name}");

        return matchedType;
    }

    /// <summary>
    /// Returns Method with matching name, generic arguments, and parameter signatures from the given AssemblyLoadContext.
    /// </summary>
    public static MethodInfo GetMatchingMethod(this AssemblyLoadContext context, MethodInfo methodInfo, bool loadIfNotLoaded = false)
    {
        if (methodInfo.DeclaringType == null)
            throw new NotImplementedException("Declaring Type is null");

        if (methodInfo.IsGenericMethod && !methodInfo.IsGenericMethodDefinition)
        {
            var matchedOpenMethodInfo = context.GetMatchingMethod(methodInfo.GetGenericMethodDefinition(), loadIfNotLoaded: loadIfNotLoaded);
            var typeArguments = methodInfo
                .GetGenericArguments()
                .Select(type => context.GetMatchingType(type, loadIfNotLoaded: loadIfNotLoaded))
                .ToArray();

            return matchedOpenMethodInfo.MakeGenericMethod(typeArguments);
        }

        var matchedType = context.GetMatchingType(methodInfo.DeclaringType, loadIfNotLoaded: loadIfNotLoaded);
        var matchedMethodInfo = matchedType
            .GetMethods()
            .SingleOrDefault(mi =>
            {
                if (mi.Name != methodInfo.Name)
                    return false;

                var lhsArguments = mi.GetGenericArguments();
                var rhsArguments = methodInfo.GetGenericArguments();
                if (lhsArguments.Length != rhsArguments.Length || !lhsArguments.Zip(rhsArguments).All(p =>
                    p.First.AssemblyQualifiedName == p.Second.AssemblyQualifiedName
                    && p.First.Attributes == p.Second.Attributes))
                {
                    return false;
                }

                var lhsParamters = mi.GetParameters();
                var rhsParamters = methodInfo.GetParameters();
                if (lhsParamters.Length != rhsParamters.Length || !lhsParamters.Zip(rhsParamters).All(p =>
                    p.First.ParameterType.AssemblyQualifiedName == p.Second.ParameterType.AssemblyQualifiedName
                    && p.First.ParameterType.Attributes == p.Second.ParameterType.Attributes
                    && p.First.Attributes == p.Second.Attributes))
                {
                    return false;
                }

                return true;
            });

        if (matchedMethodInfo == null)
            throw new Exception($"Failed to find {methodInfo.Name} in {context.Name}");

        return matchedMethodInfo;
    }
}
