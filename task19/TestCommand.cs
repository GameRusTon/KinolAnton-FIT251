namespace task19;

public class TestCommand: ICommand
{
    private readonly int Id;
    private readonly IScheduler Scheduler;
    private int Counter = 0;
    private readonly int MaxStep = 3;

    public TestCommand(int id, IScheduler scheduler)
    {
        Id = id;
        Scheduler = scheduler;
    }

    public void Execute()
    {
        Counter++;
        Console.WriteLine($"Поток {Id} вызов {Counter}");

        if (Counter < MaxStep)
        Scheduler.Add(this);
    }
}
