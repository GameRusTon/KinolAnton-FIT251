using System.Collections.Concurrent;
using System.Threading;

namespace task19;

public class ServerThread
{
    private BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();
    readonly IScheduler scheduler = new RoundRobinScheduler();
    private Thread? thread;
    private bool running = true;
    public Thread? Thread => thread;
    public IScheduler Scheduler => scheduler; 

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
        while (running || scheduler.HasCommand() || commands.Count > 0)
        {
            if(!running)
            break;
            if (commands.TryTake(out var command))
            {
                try
                {
                    command.Execute();
                }
                catch (Exception exception)
                {
                    ExceptionHandler.Catching(exception, command);
                }
            }
            else if (scheduler.HasCommand())
            {
                ICommand command1 = scheduler.Select();
                try
                {
                    command1.Execute();
                }
                catch (Exception exception)
                {
                    ExceptionHandler.Catching(exception, command1);
                }
            }
            else
            {
                Thread.Sleep(1);
                if (commands.IsAddingCompleted && !scheduler.HasCommand())
                break;
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
