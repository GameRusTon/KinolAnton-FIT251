using Xunit;
using CommandLib;
namespace task10tests;

public class PluginsTests
{
    [Fact]
    public void LoadSortPlugins_ShouldCorrectlyAssembleAndSort()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "testDir" + Guid.NewGuid());
        Directory.CreateDirectory(testDir);

        string PathA = typeof(PluginA.Plugin_A).Assembly.Location;
        string PathB = typeof(PluginB.Plugin_B).Assembly.Location;
        string PathC = typeof(PluginC.Plugin_C).Assembly.Location;

        File.Copy(PathA, Path.Combine(testDir, "PluginA.dll"));
        File.Copy(PathB, Path.Combine(testDir, "PluginB.dll"));
        File.Copy(PathC, Path.Combine(testDir, "PluginC.dll"));

        var service = new PluginService.PluginService(testDir);
        List<ICommand> result = service.LoadSortPlugins();

        Assert.Equal(3, result.Count);
        Assert.Equal("Plugin_A", result[0].GetType().Name);
        Assert.Equal("Plugin_B", result[1].GetType().Name);
        Assert.Equal("Plugin_C", result[2].GetType().Name);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void LoadSortPlugins_ShouldThrowException_WhenDependencyIsMissing()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "testDir" + Guid.NewGuid());
        Directory.CreateDirectory(testDir);

        string PathB = typeof(PluginB.Plugin_B).Assembly.Location;

        File.Copy(PathB, Path.Combine(testDir, "PluginB.dll"));

        var service = new PluginService.PluginService(testDir);

        var exception = Assert.Throws<InvalidOperationException>(() => service.LoadSortPlugins());
        Assert.Contains("требует зависимость", exception.Message);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void LoadSortPlugins_ShouldThrowException_WhenCircularDependencyDetected()
    {
        var service = new PluginService.PluginService("./FakePath");

        var cyclePlugins = new List<PluginService.PluginService.PluginMetadata>
        {
            new PluginService.PluginService.PluginMetadata 
            { 
                PluginType = typeof(FakePluginD), 
                PluginName = "PluginD", 
                Dependencies = new List<string> { "PluginF" } 
            },
            new PluginService.PluginService.PluginMetadata 
            { 
                PluginType = typeof(FakePluginF), 
                PluginName = "PluginF", 
                Dependencies = new List<string> { "PluginD" } 
            }
        };

        var DfsMethod = typeof(PluginService.PluginService)
        .GetMethod("DFS", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var targetException = Assert.Throws<System.Reflection.TargetInvocationException>(() => 
        DfsMethod?.Invoke(service, new object[] { cyclePlugins }));

        Assert.Contains("замечена циклическая зависимость", targetException.InnerException?.Message);

    }

    // Заглушки, так как чтобы отдельно не создавать дополнительные плагины, создали "фейк" версии.
    private class FakePluginD : ICommand { public void Execute() {} }
    private class FakePluginF : ICommand { public void Execute() {} }
}
