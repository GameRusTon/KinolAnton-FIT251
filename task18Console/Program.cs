using task18;
using ScottPlot;
using System.Diagnostics;
namespace task18Console;

class Program
{
    private class TestLongCommand: ICommand
    {
        private readonly int Id;
        private readonly IScheduler Scheduler;
        private int Step = 0;
        private readonly int MaxStep;

        public TestLongCommand (int id, IScheduler scheduler, int maxStep)
        {
            Id = id;
            Scheduler = scheduler;
            MaxStep = maxStep;
        }

        public void Execute()
        {
            Step++;

            double mul = Math.Sqrt(Step);

            if (Step < MaxStep)
                Scheduler.Add(this);
        }
    }

    static void Main()
    {
        int [] steps = {1, 5, 10, 50, 100, 500, 1000, 2000 };
        double [] AverageResults = new double [steps.Length];

        for (int i = 0; i < steps.Length; i++)
        {
            int step = steps[i];
            double[] NumberOfLaunches = new double[5];

            for (int n = 0; n < NumberOfLaunches.Length; n++)
            {
                RoundRobinScheduler scheduler = new RoundRobinScheduler();

                for (int t = 0; t < 20; t++)
                {
                    scheduler.Add(new TestLongCommand(t, scheduler, step));
                }

                Stopwatch stopwatch = Stopwatch.StartNew();

                while (scheduler.HasCommand())
                {
                    ICommand command = scheduler.Select();
                    command.Execute();
                }
                
                stopwatch.Stop();
                NumberOfLaunches[n] = stopwatch.Elapsed.TotalMilliseconds;
            }

            AverageResults[i] = NumberOfLaunches.Average();
            Console.WriteLine($"Количество разбиений: {step} | Средний результат: {AverageResults[i]}");
        }
        Console.WriteLine();

        double MinResult = AverageResults.Min();
        int OptimalNum = steps[Array.IndexOf(AverageResults, MinResult)];
        Console.WriteLine($"Оптимальное количество разбиений: {OptimalNum} со временем {MinResult}.");

        double[] OX = AverageResults;
        double[] OY = steps.Select(t => (double)t).ToArray();

        Plot plot = new();
        var scatter = plot.Add.Scatter(OX, OY);
        scatter.LineWidth = 2;
        scatter.MarkerSize = 8;
        plot.Title("Зависимость времени вычисления от количества разбиений");
        plot.XLabel("Время вычисления (мс)");
        plot.YLabel("Количество разбиений");

        plot.SavePng("graph.png", 600, 400);

        string reportPath = "report.txt";
        using (StreamWriter streamWriter = new StreamWriter(reportPath))
        {
            streamWriter.WriteLine("Отчет о реализации планировщика и зависимости времени выполнения команд от количества разбиений");
            streamWriter.WriteLine();
            streamWriter.WriteLine("ИСХОДНЫЕ ДАННЫЕ:");
            streamWriter.WriteLine("Реализован планировщик на основе стратегии Round Robbin;");
            streamWriter.WriteLine("Реализован класс команд TestLongCommand, реализующих интерфейс ICommand,");
            streamWriter.WriteLine("при этом такие команды не могут выполнить всю работу за один вызов метода Execute;");
            streamWriter.WriteLine();
            streamWriter.WriteLine("ТЕСТИРОВАНИЕ:");
            streamWriter.WriteLine("Создаем массив с различным количеством разбиений:");
            streamWriter.WriteLine("steps = {1, 5, 10, 50, 100, 500, 1000, 2000 }");
            streamWriter.WriteLine("Для каждого отдельного количества разбиений создается 20 экземпляров команд,");
            streamWriter.WriteLine("каждая тестируется по 5 раз и выводится среднее время работы.");
            streamWriter.WriteLine("Среди всего количества разбиений находим оптимальное по времени.");
            streamWriter.WriteLine();
            streamWriter.WriteLine("РЕЗУЛЬТАТЫ:");
            streamWriter.WriteLine("Данные для каждого разбиения:");
            for (int i = 0; i < AverageResults.Length; i++)
            {
               streamWriter.WriteLine($"Количество разбиений: {steps[i]} | Средний результат: {AverageResults[i]}"); 
            }
            streamWriter.WriteLine();
            streamWriter.WriteLine("ВЫВОД:");
            streamWriter.WriteLine($"Оптимальное количество разбиений: {OptimalNum} со временем {MinResult}.");
            streamWriter.WriteLine("Пояснение: При увеличении числа разбиений (например, до 2000) резко возрастают накладные");
            streamWriter.WriteLine("расходы процессора на постоянное извлечение и добавление задач обратно в планировщик,");
            streamWriter.WriteLine("из-за чего общее время выполнения существенно увеличивается.");
        }
    }
}
