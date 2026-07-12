namespace task04;

public class Fighter : ISpaceship
{
    public int Speed => 100;

    public int FirePower => 50;

    public double X {get; private set;}

    public double Y {get; private set;}
    
    public int DirAngle {get; private set;}

    public int Ammo {get; private set;}

    public Fighter (double x = 0, double y = 0, int StartAmmo = 10)
    {
        X = x;
        Y = y;
        DirAngle = 0;
        Ammo = StartAmmo;
    }

    public void MoveForward()
    {
        double rad = DirAngle * (Math.PI / 180.0);

        X += Speed * Math.Cos(rad);
        Y += Speed * Math.Sin(rad);
    }
    public void Rotate(int angle)
    {
        DirAngle = (DirAngle + angle) % 360;
        if (DirAngle < 0) DirAngle += 360;
    }
    public void Fire()
    {
        if (Ammo > 0)
        Ammo--;
    }
}
