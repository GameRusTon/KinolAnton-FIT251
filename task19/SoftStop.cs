using System.Threading;

namespace task18;

public class SoftStop: ICommand
{
    private readonly ServerThread _server;

    public SoftStop(ServerThread server)
    {
        _server = server;
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _server.Thread)
        throw new InvalidOperationException("Команда SoftStop может быть выполнена только внутри целевого серверного потока.");

        _server.SoftStopHelper();
    }
}
