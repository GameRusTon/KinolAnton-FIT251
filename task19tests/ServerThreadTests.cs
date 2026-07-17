using System.Threading;
using Xunit;
using task19;
namespace task18tests;

public class ServerThreadTests
{
    private class ExampleCommand: ICommand
    {
        public bool IsExecuted = false;
        public void Execute()
        {
            IsExecuted = true;
        }
    }
    private class TestSimpleCommand: ICommand
    {
        private readonly string Name;
        private readonly List<string> Order;

        public TestSimpleCommand(string name, List<string> order)
        {
            Name = name;
            Order = order;
        }
        public void Execute()
        {
            lock (Order)
            {
                Order.Add(Name);
            }
        }
    }

    private class TestLongCommand: ICommand
    {
        private readonly int Id;
        private readonly IScheduler Scheduler;
        private readonly List<string> Order;
        private int Step = 0;
        private readonly int MaxStep;

        public TestLongCommand (int id, IScheduler scheduler, List<string> order, int maxStep)
        {
            Id = id;
            Scheduler = scheduler;
            Order = order;
            MaxStep = maxStep;
        }

        public void Execute()
        {
            Step++;

            lock(Order)
            {
                Order.Add($"ID: {Id}, Step: {Step}");
            }

            if (Step < MaxStep)
                Scheduler.Add(this);
        }
    }

    [Fact]
    public void Cycle_WhenHardStopExecuted_RejectRemainingCommands()
    {
        ServerThread server = new ServerThread();
        ExampleCommand example1 = new ExampleCommand();
        ExampleCommand example2 = new ExampleCommand();
        HardStop hardStop = new HardStop(server);

        server.Add(example1);
        server.Add(hardStop);
        server.Add(example2);

        server.StartCycle();
        Thread.Sleep(300);

        Assert.True(example1.IsExecuted);
        Assert.False(example2.IsExecuted);
    }

    [Fact]
    public void Cycle_WhenSoftStopExecuted_DoRemainingCommands()
    {
        ServerThread server = new ServerThread();
        ExampleCommand example1 = new ExampleCommand();
        ExampleCommand example2 = new ExampleCommand();
        SoftStop softStop = new SoftStop(server);

        server.Add(example1);
        server.Add(softStop);
        server.Add(example2);

        server.StartCycle();
        Thread.Sleep(300);

        Assert.True(example1.IsExecuted);
        Assert.True(example2.IsExecuted);
    }

    [Fact]
    public void HardStop_ExecuteFromWrongThread_ThrowsInvalidOperationException()
    {
        ServerThread server = new ServerThread();
        HardStop hardStop = new HardStop(server);

        server.StartCycle();

        var exception = Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
        Assert.Contains("Команда HardStop может быть выполнена только внутри целевого серверного потока.", exception.Message);

        server.HardStopHepler();
    }

    [Fact]
    public void SoftStop_ExecuteFromWrongThread_ThrowsInvalidOperationException()
    {
        ServerThread server = new ServerThread();
        SoftStop softStop = new SoftStop(server);

        server.StartCycle();

        var exception = Assert.Throws<InvalidOperationException>(() => softStop.Execute());
        Assert.Contains("Команда SoftStop может быть выполнена только внутри целевого серверного потока.", exception.Message);

        server.HardStopHepler();
    }

    [Fact]
    public void Cycle_WithSimpleAndLongCommands_ExecutesCorrectlyWithoutBlocking()
    {
        ServerThread server = new ServerThread();
        IScheduler scheduler = server.Scheduler;
        List<string> log = new List<string>();

        TestSimpleCommand command1 = new TestSimpleCommand("command1.txt", log);
        TestLongCommand testLongCommand = new TestLongCommand(1, scheduler, log, 2);
        TestSimpleCommand command2 = new TestSimpleCommand("command2.txt", log);

        server.Add(command1);
        server.Add(testLongCommand);
        server.Add(command2);

        server.StartCycle();
        Thread.Sleep(300);
        server.HardStopHepler();
        
        var ExpectedOrder = new List<string>
        {
            "command1.txt",
            "ID: 1, Step: 1",
            "command2.txt",
            "ID: 1, Step: 2"
        };

        Assert.Equal(ExpectedOrder, log);
    }

    [Fact]
    public void TestCommand_ShouldExecuteExactlyThreeTimesAndStop()
    {
        ServerThread server = new ServerThread();
        IScheduler scheduler = server.Scheduler;

        var testCommand = new TestCommand(1, scheduler);

        scheduler.Add(testCommand);

        //Всего должно быть 3 вызова, после чего команда уходит из планировщика.

        //1 вызов
        ICommand command1 = scheduler.Select();
        command1.Execute();
        Assert.True(scheduler.HasCommand());

        //2 вызов
        ICommand command2 = scheduler.Select();
        command2.Execute();
        Assert.True(scheduler.HasCommand());

        //3 вызов
        ICommand command3 = scheduler.Select();
        command3.Execute();

        //Ушла из планировщика
        Assert.False(scheduler.HasCommand());
    }
}
