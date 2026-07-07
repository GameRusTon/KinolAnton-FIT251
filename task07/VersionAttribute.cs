namespace task07;
[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute: Attribute
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}