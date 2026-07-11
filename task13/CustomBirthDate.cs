using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

//Кастомное форматирование даты.
public class CustomDateTime: JsonConverter<DateTime>
{
    private const string Format = "dd.MM.yyyy";

    //Дисериализация.
    public override DateTime Read(ref Utf8JsonReader utfReader, Type type, JsonSerializerOptions options)
    {
        string? dateLine = utfReader.GetString();

        if (string.IsNullOrEmpty(dateLine))
        throw new JsonException("Строка даты пустая или некорректная.");

        if (DateTime.TryParseExact(dateLine, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        return date;

        throw new JsonException($"Неверный формат даты: {dateLine}. Ожидается формат: {Format}");
    }

    //Сериализация.
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}
