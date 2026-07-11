using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests
{
    [Fact]
    public void CreateCalculator_ShouldReturnWorkingCalculator()
    {
        CalculatorInterface calculator = ClassCalculatorGenerator.CreateCalculator();

        Assert.Equal(3, calculator.Add(2, 1));
        Assert.Equal(1, calculator.Minus(2, 1));
        Assert.Equal(2, calculator.Mul(2, 1));
        Assert.Equal(3, calculator.Div(9, 3));
    }

    [Fact]
    public void DivideByZero_ShouldThrowDivideByZeroException()
    {
        CalculatorInterface calculator = ClassCalculatorGenerator.CreateCalculator();

        Assert.Throws<DivideByZeroException>(() => calculator.Div(10, 0));
    }

    [Fact]
    public void Add_ShouldHandleOverflow()
    {
        CalculatorInterface calculator = ClassCalculatorGenerator.CreateCalculator();

        Assert.Equal(int.MinValue, calculator.Add(int.MaxValue, 1));
    }
}
