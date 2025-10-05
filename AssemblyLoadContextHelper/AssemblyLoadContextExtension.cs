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
            var openType = context.GetMatchingType(type.GetGenericTypeDefinition());
            var typeArguments = new Type[type.GenericTypeArguments.Length];
            for (var idx = 0; idx < type.GenericTypeArguments.Length; idx++)
            {
                var typeArgument = context.GetMatchingType(type.GenericTypeArguments[idx]);
                typeArguments[idx] = typeArgument;
            }

            return openType.MakeGenericType(typeArguments);
        }

        var matchedAssembly = context.GetMatchingAssembly(type.Assembly);
        var matchedType = matchedAssembly
            .GetTypes()
            .SingleOrDefault(t => t.FullName == type.FullName);

        if (matchedType == null)
            throw new Exception($"Failed to find {type.FullName} in {context.Name}");

        return matchedType;
    }
}
