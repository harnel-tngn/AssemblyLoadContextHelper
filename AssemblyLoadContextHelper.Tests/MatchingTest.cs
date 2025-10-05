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

    [Theory]
    public void TestTypeMatching(Type type)
    {
        var matchedType = _context.GetMatchingType(type);
        Assert.NotNull(matchedType);
        Assert.Equal(type.AssemblyQualifiedName, matchedType.AssemblyQualifiedName);
    }

    public Task DisposeAsync()
    {
        _context.Unload();
        return Task.CompletedTask;
    }
}
