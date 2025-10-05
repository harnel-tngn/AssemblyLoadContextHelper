using System.Reflection;
using System.Runtime.Loader;

namespace AssemblyLoadContextHelper.Tests;



public partial class MatchingTest : IAsyncLifetime
{
    private AssemblyLoadContext _context = default!;

    public Task InitializeAsync()
    {
        _context = new AssemblyLoadContext(nameof(MatchingTest), true);
        _context.LoadFromAssemblyPath(typeof(MatchingTest).Assembly.Location);
        return Task.CompletedTask;
    }

    public static IEnumerable<object[]> TestTypeMatchingParameters()
    {
        yield return [typeof(int)];
        yield return [typeof(Guid)];
        yield return [typeof(string)];
        yield return [typeof(object)];

        yield return [typeof(List<int>)];
        yield return [typeof(List<Guid>)];
        yield return [typeof(List<string>)];
        yield return [typeof(List<object>)];

        foreach (var type in typeof(SimpleClass).Assembly.GetTypes())
            if (type.GetCustomAttribute<TestTypeAttribute>() != null)
            {
                yield return [type];

                if (type.IsGenericTypeDefinition)
                {
                    foreach (var typeArgument in new[] { typeof(int), typeof(string), typeof(List<int>) })
                    {
                        var closedType = type.MakeGenericType([..
                            Enumerable.Repeat(
                                typeArgument,
                                type.GetGenericArguments().Length)]);

                        yield return [closedType];
                    }
                }
            }
    }

    [Theory]
    [MemberData(nameof(TestTypeMatchingParameters))]
    public void TestTypeMatching(Type type)
    {
        var matchedType = _context.GetMatchingType(type);
        Assert.NotNull(matchedType);
        Assert.Equal(type.AssemblyQualifiedName, matchedType.AssemblyQualifiedName);
    }

    public static IEnumerable<object[]> TestMethodMatchingParameters()
    {
        foreach (var arr in TestTypeMatchingParameters())
        {
            if (arr.Length != 1 || arr[0] is not Type type)
                continue;

            foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                if (methodInfo.GetCustomAttribute<TestMethodAttribute>() != null)
                {
                    yield return [methodInfo];

                    if (methodInfo.IsGenericMethodDefinition)
                    {
                        foreach (var typeArgument in new[] { typeof(int), typeof(string), typeof(List<int>) })
                        {
                            var closedMethod = methodInfo.MakeGenericMethod([..
                            Enumerable.Repeat(
                                typeArgument,
                                methodInfo.GetGenericArguments().Length)]);

                            yield return [closedMethod];
                        }
                    }
                }
        }
    }

    [Theory]
    [MemberData(nameof(TestMethodMatchingParameters))]
    public void TestMethodMatching(MethodInfo methodInfo)
    {
        var matchedMethod = _context.GetMatchingMethod(methodInfo);
        Assert.NotNull(matchedMethod);

        if (methodInfo.DeclaringType != null)
        {
            Assert.NotNull(matchedMethod.DeclaringType);
            Assert.Equal(matchedMethod.DeclaringType.AssemblyQualifiedName, methodInfo.DeclaringType.AssemblyQualifiedName);
        }

        foreach (var (p1, p2) in methodInfo.GetParameters().Zip(matchedMethod.GetParameters()))
        {
            Assert.Equal(p1.Name, p2.Name);
            Assert.Equal(p1.ParameterType.AssemblyQualifiedName, p2.ParameterType.AssemblyQualifiedName);
        }

        if (!(methodInfo.DeclaringType?.IsGenericTypeDefinition ?? false)
            && !methodInfo.IsGenericMethodDefinition
            && !methodInfo.GetParameters().Any(p => p.ParameterType.IsByRefLike))
        {
            var parameter = methodInfo
                .GetParameters()
                .Select(p => p.ParameterType.IsClass || p.ParameterType.IsInterface ? null : Activator.CreateInstance(p.ParameterType))
                .ToArray();

            var res1 = methodInfo.Invoke(null, parameter);
            var res2 = matchedMethod.Invoke(null, parameter);
            Assert.Equal(res1, res2);
        }
    }

    public Task DisposeAsync()
    {
        _context.Unload();
        return Task.CompletedTask;
    }
}
