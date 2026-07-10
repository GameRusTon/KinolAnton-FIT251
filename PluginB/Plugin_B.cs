using CommandLib;
namespace PluginB;

[PluginLoad("Plugin_B", "Plugin_A")]
public class Plugin_B: ICommand
{
    public void Execute()
    {
        Console.WriteLine("Executed: B");
    }
}
