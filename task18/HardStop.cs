using System.Threading;
using System.Windows.Input;
using task18;

namespace task18;

public class HardStop: ICommand
{
    private readonly ServerThread _server;

    public HardStop(ServerThread server)
    {
        _server = server;
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _server.Thread)
        throw new InvalidOperationException("Команда HardStop может быть выполнена только внутри целевого серверного потока.");

        _server.HardStopHepler();
    }
}
