using NUnit;
using task02;
namespace task02tests;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;
    
    [SetUp]
    public void StudentsServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Test]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.IsTrue(result.All(s => s.Faculty == "ФИТ"));
    }

    [Test]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.That(result, Is.EqualTo("Экономика"));
    }

    [Test]
    public void GetStudentsWithAverageGrade_BiggerThan_MinAverageGrade()
    {
        var result = _service.GetStudentsWithMinAverageGrade(5);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result, Has.Some.Matches<Student>(s => s.Name == "Петр"));
    }

    [Test]
    public void GetStudentsByName()
    {
        var result = _service.GetStudentsOrderedByName();
        Assert.That(result.Select(s => s.Name), Is.EqualTo(new [] {"Анна", "Иван", "Петр"}));
    }

    [Test]
    public void GroupStudentsByFaculty()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result["ФИТ"].Count(), Is.EqualTo(2));
        Assert.That(result["Экономика"].Count(), Is.EqualTo(1));
    }
}