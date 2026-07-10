using CommandLib;
namespace PluginA;

[PluginLoad("Plugin_A")]
public class Plugin_A: ICommand
{
    public void Execute()
    {
        Console.WriteLine("Executed: A");
    }
}
