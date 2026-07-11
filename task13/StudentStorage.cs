using System.Text.Json;

namespace task13;

//Загрузка в файл и из файла.
public static class StudentStorage
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public static void SaveToFile(string filePath, Student student)
    {
        string jsonLine = JsonSerializer.Serialize(student, _options);

        File.WriteAllText(filePath, jsonLine);
    }

    public static Student? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        throw new FileNotFoundException($"Файл по пути: {filePath} не найден.");

        string jsonLine = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<Student>(jsonLine, _options);
    }
}
