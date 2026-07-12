using task04;
using Xunit;
namespace task04tests;
public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Cruiser_ShouldHaveMoreFirePowerThanFighter()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }

    //Переделал методы под больший функционал => методы проверки тоже изменились.
    [Fact]
    public void MoveForward_ShouldChangeCoordinates()
    {
        var fighter = new Fighter(x: 0, y: 0);
        var cruiser = new Cruiser(x: 0, y: 0);

        fighter.MoveForward();
        cruiser.MoveForward();

        Assert.Equal(100, fighter.X, 0.001);
        Assert.Equal(0, fighter.Y, 0.001);
        Assert.Equal(50, cruiser.X, 0.001);
        Assert.Equal(0, cruiser.Y, 0.001);
    }

    [Fact]
    public void Rotate_ShouldChangeAngle()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        fighter.Rotate(90);
        cruiser.Rotate(-90);

        Assert.Equal(90, fighter.DirAngle);
        Assert.Equal(270, cruiser.DirAngle);
    }

    [Fact]
    public void Fire_ShouldDicreaseAmmo()
    {
        var fighter = new Fighter(StartAmmo: 0);
        var cruiser = new Cruiser();

        fighter.Fire();
        cruiser.Fire();

        Assert.Equal(0, fighter.Ammo);
        Assert.Equal(9, cruiser.Ammo);
    }
}
