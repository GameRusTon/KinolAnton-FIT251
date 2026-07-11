namespace task13;

//Валидация данных(проверка на ошибки).
public static class StudentValidator
{
    public static void Validator(Student? student)
    {
        if (student == null)
        throw new ArgumentNullException(nameof(student), "Объект типа - Студент не может быть null.");

        if (string.IsNullOrWhiteSpace(student.FirstName))
        throw new InvalidOperationException("Имя студента не должно быть пустое.");

        if (string.IsNullOrWhiteSpace(student.LastName))
        throw new InvalidOperationException("Фамилия студента не должна быть пустой.");

        if (student.BirthDate > DateTime.Today)
        throw new InvalidOperationException("Дата рождения студента не может быть в будущем.");

        if (student.BirthDate < DateTime.Today.AddYears(-73))
        throw new InvalidOperationException("Указана слишком старая дата рождения.");

        if (student.Subjects != null)
        {
            foreach (var subject in student.Subjects)
            {
                if (string.IsNullOrWhiteSpace(subject.Name))
                throw new InvalidOperationException("Название предмета не может быть пустым.");

                if (subject.Grade < 1 || subject.Grade > 5)
                throw new InvalidOperationException($"Оценка {subject.Grade} по предмету {subject.Name} находится за пределами интервала 1-5 ");
            }
        }
    }
}
