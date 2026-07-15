using System.Collections.Concurrent;
using System.Threading;

namespace task17;

public class ServerThread
{
    private BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();
    private Thread? thread;
    private bool running = true;
    public Thread? Thread => thread;

    public void StartCycle()
    {
        thread = new Thread(Cycle);
        thread.Start();
    }

    public void Add(ICommand command)
    {
        if (commands.IsAddingCompleted)
        return;
        commands.Add(command);
    }

    private void Cycle()
    {
        foreach (var command in commands.GetConsumingEnumerable())
        {
            if (!running)
            break;
            try
            {
                command.Execute();
            }
            catch (Exception exception)
            {
                ExceptionHandler.Catching(exception, command);
            }
        }
    }

    public void HardStopHepler()
    {
        running = false;
        commands.CompleteAdding();
    }

    public void SoftStopHelper()
    {
        commands.CompleteAdding();
    }
}
