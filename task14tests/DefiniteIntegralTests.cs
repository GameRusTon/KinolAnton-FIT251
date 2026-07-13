using task14;
namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_WhenEverythingIsCorrect()
    {
        Func <double, double> X = (double x) => x;
        Func <double, double> SIN = (double x) => Math.Sin(x);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);

        //В данном тесте в задании изначально была ошибка в результате. Была исправлена.
        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
    }

    [Fact]
    public void Solve_WhenFunctionIsNull_ThrowsArgumentNullException()
    {
        Func <double, double> X = (double x) => x;
        Assert.Throws<ArgumentNullException>(() => DefiniteIntegral.Solve(-1, 1, null!, 1e-4, 2));
    }

    [Fact]
    public void Solve_WhenStepIsZeroOrNegative_ThrowsArgumentException()
    {
        Func <double, double> X = (double x) => x;
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(-1, 1, X, 0, 2));
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(-1, 1, X, -1, 2));
    }

    [Fact]
    public void Solve_WhenThreadsNumberIsZeroOrNegative_ThrowsArgumentException()
    {
        Func <double, double> X = (double x) => x;
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(-1, 1, X, 1e-4, 0));
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(-1, 1, X, 1e-4, -1));
    }
}
