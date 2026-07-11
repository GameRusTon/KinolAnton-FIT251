using System.Text.Json;
using task13;
using Xunit;

namespace task13tests;

public class StudentJsonTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    [Fact]
    public void Should_SerializeAndDeserializeCorrectly_WhenDataIsValid()
    {
        var student = new Student
        {
            FirstName = "Антон",
            LastName = "Киноль",
            MiddleName = "Дмитриевич",
            BirthDate = new DateTime(2008, 02, 02),
            Subjects = new List<Subject>
            {
                new Subject {Name = "Физика", Grade = 5}
            }
        };

        string json = JsonSerializer.Serialize(student, _options);
        var deserializedStudent = JsonSerializer.Deserialize<Student>(json, _options);

        Assert.NotNull(deserializedStudent);
        Assert.Equal("Антон", deserializedStudent.FirstName);
        Assert.Equal("Киноль", deserializedStudent.LastName);
        Assert.Equal("Дмитриевич", deserializedStudent.MiddleName);
        Assert.Equal(new DateTime(2008, 02, 02), deserializedStudent.BirthDate);
        Assert.Equal("Физика", deserializedStudent.Subjects[0].Name);
        Assert.Equal(5, deserializedStudent.Subjects[0].Grade);

        var exception = Record.Exception(() => StudentValidator.Validator(deserializedStudent));
        Assert.Null(exception);
    }

    [Fact]
    public void Should_IgnoreMiddleName_WhenNull()
    {
        var student = new Student
        {
            FirstName = "Антон",
            LastName = "Киноль",
            MiddleName = null,
            BirthDate = new DateTime(2008, 02, 02),
        };

        string json = JsonSerializer.Serialize(student, _options);

        Assert.DoesNotContain("middle_name", json);
    }

    [Fact]
    public void Should_ThrowException_WhenGradeIsInvalid()
    {
        var student = new Student
        {
            FirstName = "Антон",
            LastName = "Киноль",
            MiddleName = "Дмитриевич",
            BirthDate = new DateTime(2008, 02, 02),
            Subjects = new List<Subject> 
            { 
                new Subject {Name = "Физика", Grade = 6} 
            }
        };

        var exception = Assert.Throws<InvalidOperationException>(() => StudentValidator.Validator(student));

        Assert.Contains("Оценка 6 по предмету Физика находится за пределами интервала 1-5 ", exception.Message);
    }

    [Fact]
    public void Should_ThrowException_WhenBirthDateIsInFuture()
    {
        var student = new Student
        {
            FirstName = "Антон",
            LastName = "Киноль",
            MiddleName = "Дмитриевич",
            BirthDate = DateTime.Today.AddDays(1),
        };

        var exception = Assert.Throws<InvalidOperationException>(() => StudentValidator.Validator(student));

        Assert.Contains("Дата рождения студента не может быть в будущем.", exception.Message);
    }

    [Fact]
    public void Should_ThrowException_WhenBirthDateIsTooOld()
    {
        var student = new Student
        {
            FirstName = "Антон",
            LastName = "Киноль",
            MiddleName = "Дмитриевич",
            BirthDate = new DateTime(1800, 02, 02),
        };

        var exception = Assert.Throws<InvalidOperationException>(() => StudentValidator.Validator(student));

        Assert.Contains("Указана слишком старая дата рождения.", exception.Message);
    }

    [Fact]
    public void Should_ThrowException_WhenFirstNameIsEmptyOrWhiteSpaces()
    {
        var student = new Student
        {
            FirstName = " ",
            LastName = "Киноль",
            MiddleName = "Дмитриевич",
            BirthDate = new DateTime(2008, 02, 02),
        };

        var exception = Assert.Throws<InvalidOperationException>(() => StudentValidator.Validator(student));

        Assert.Contains("Имя студента не должно быть пустое.", exception.Message);
    }

    [Fact]
    public void SaveAndLoadStudent_ShouldWork()
    {
        var student = new Student
        {
            FirstName = "Антон",
            LastName = "Киноль"
        };
        string testPath = "test_student.json";

        StudentStorage.SaveToFile(testPath, student);

        var loadedStudent = StudentStorage.LoadFromFile(testPath);

        Assert.NotNull(loadedStudent);
        Assert.Equal("Антон", loadedStudent.FirstName);
        Assert.Equal("Киноль", loadedStudent.LastName);

        if (File.Exists(testPath))
        File.Delete(testPath);
    }
}
