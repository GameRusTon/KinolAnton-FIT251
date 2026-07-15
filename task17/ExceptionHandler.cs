namespace task17;

public static class ExceptionHandler
{
    public static void Catching(Exception exception, ICommand command)
    {
        Console.WriteLine($"Была перехвачена ошибка в команде: {command.GetType().Name}. Ошибка: {exception.Message}");
    }
}
