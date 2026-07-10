using CommandLib;
namespace PluginC;

[PluginLoad("Plugin_C", "Plugin_B")]
public class Plugin_C: ICommand
{
    public void Execute()
    {
        Console.WriteLine("Executed: C");
    }
}
