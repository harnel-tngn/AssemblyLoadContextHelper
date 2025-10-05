using System.Reflection;
using System.Runtime.Loader;

namespace AssemblyLoadContextHelper;

public static class AssemblyLoadContextExtension
{
    private static Assembly CoreLibAsembly = typeof(int).Assembly;

    public static Assembly GetMatchingAssembly(this AssemblyLoadContext context, Assembly assembly)
    {
        // Note: System.Private.CoreLib is unable to load
        if (assembly.FullName == CoreLibAsembly.FullName)
            return assembly;

        var matchedAssembly = context
            .Assemblies
            .FirstOrDefault(asm => asm.FullName == assembly.FullName);

        if (matchedAssembly == null)
            throw new Exception($"Failed to find {assembly.FullName} in {context.Name}");

        return matchedAssembly;
    }

    public static Type GetMatchingType(this AssemblyLoadContext context, Type type)
    {
        if (type.IsGenericType && !type.IsGenericTypeDefinition)
        {
            var matchedOpenType = context.GetMatchingType(type.GetGenericTypeDefinition());
            var typeArguments = type
                .GetGenericArguments()
                .Select(context.GetMatchingType)
                .ToArray();

            return matchedOpenType.MakeGenericType(typeArguments);
        }

        var matchedAssembly = context.GetMatchingAssembly(type.Assembly);
        var matchedType = matchedAssembly
            .GetTypes()
            .SingleOrDefault(t => t.FullName == type.FullName);

        if (matchedType == null)
            throw new Exception($"Failed to find {type.FullName} in {context.Name}");

        return matchedType;
    }

    public static MethodInfo GetMatchingMethod(this AssemblyLoadContext context, MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType == null)
            throw new NotImplementedException("Declaring Type is null");

        if (methodInfo.IsGenericMethod && !methodInfo.IsGenericMethodDefinition)
        {
            var matchedOpenMethodInfo = context.GetMatchingMethod(methodInfo.GetGenericMethodDefinition());
            var typeArguments = methodInfo
                .GetGenericArguments()
                .Select(context.GetMatchingType)
                .ToArray();

            return matchedOpenMethodInfo.MakeGenericMethod(typeArguments);
        }

        var matchedType = context.GetMatchingType(methodInfo.DeclaringType);
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
