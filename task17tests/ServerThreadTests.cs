using System.Threading;
using Xunit;
using task17;
namespace task17tests;

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
}
