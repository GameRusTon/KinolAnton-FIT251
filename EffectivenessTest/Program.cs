using task15;
using ScottPlot;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        //Общая информация
        Func <double, double> SIN = (double x) => Math.Sin(x);
        double a = -100.0;
        double b = 100.0;
        double [] steps = {1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6};

        //Поиск оптимального шага
        double OptimalStep = 1e-1;

        foreach (var step in steps)
        {
            double result = DefiniteIntegral.SingleThreaded_Solve(a, b, SIN, step);
            Console.WriteLine($"Шаг: {step}, результат: {result}.");
            if (Math.Abs(result - 0.0) < 1e-4 && OptimalStep == 1e-1)
            {
                OptimalStep = step;
            }
        }
        Console.WriteLine($"Минимальный шаг с оптимальным результатом: {OptimalStep}.");
        Console.WriteLine();

        //Поиск оптимального количества потоков
        int[] ThreadsNumbers = {1, 2, 4, 6, 8, 10, 12, 16};
        double[] AverageThreadsResults = new double[ThreadsNumbers.Length];
        for (int i = 0; i < ThreadsNumbers.Length; i ++)
        {
            int thread = ThreadsNumbers[i];
            double[] NumberOfLaunches = new double[5];
            for (int n = 0; n < NumberOfLaunches.Length; n++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                DefiniteIntegral.Solve(a, b, SIN, OptimalStep, thread);
                stopwatch.Stop();

                NumberOfLaunches[n] = stopwatch.Elapsed.TotalMilliseconds;
            }

            AverageThreadsResults[i] = NumberOfLaunches.Average();
            Console.WriteLine($"Количество потоков: {thread}, среднее время выполнения: {AverageThreadsResults[i]}.");
        }

        double MinResult = AverageThreadsResults.Min();
        int OptimalThreadNum = ThreadsNumbers[Array.IndexOf(AverageThreadsResults, MinResult)];

        Console.WriteLine($"Оптимальный результат: количество потоков: {OptimalThreadNum}, время выполнения: {MinResult}.");
        Console.WriteLine();

        //Поиск при однопоточности
        double[] LaunchesForSingleThread = new double[5];
        for (int l = 0; l < LaunchesForSingleThread.Length; l++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            DefiniteIntegral.SingleThreaded_Solve(a, b, SIN, OptimalStep);
            stopwatch.Stop();

            LaunchesForSingleThread[l] = stopwatch.Elapsed.TotalMilliseconds;
        }

        double MinSingleResult = LaunchesForSingleThread.Average();
        Console.WriteLine($"Время однопоточной реализации: {MinSingleResult}.");
        Console.WriteLine();

        //Сравнение
        double diffrence = (MinSingleResult - MinResult)/MinSingleResult * 100;
        Console.WriteLine($"Многопоточная реализация быстрее чем однопоточная на {diffrence} %.");
        if (diffrence < 15)
        Console.WriteLine("Эффективность использования многопоточности меньше 15 процентов, необходима доработка.");
        Console.WriteLine();

        //Построение графика
        double[] OX = AverageThreadsResults;
        double[] OY = ThreadsNumbers.Select(t => (double)t).ToArray();

        Plot plot = new();
        var scatter = plot.Add.Scatter(OX, OY);
        scatter.LineWidth = 2;
        scatter.MarkerSize = 8;
        plot.Title("Зависимость времени вычисления от количества потоков");
        plot.XLabel("Время вычисления функции Solve (мс)");
        plot.YLabel("Количество потоков");

        plot.SavePng("graph.png", 600, 400);

        string reportPath = "report.txt";
        using (StreamWriter streamWriter = new StreamWriter(reportPath))
        {
            streamWriter.WriteLine("Отчет о эффективности применения многопоточности для вычисления определенного интеграла.");
            streamWriter.WriteLine();
            streamWriter.WriteLine("ИСХОДНЫЕ ДАННЫЕ:");
            streamWriter.WriteLine("Функция: Sin");
            streamWriter.WriteLine("Интервал вычисления: [-100, 100]");
            streamWriter.WriteLine("ПРЕДУСЛОВИЕ:");
            streamWriter.WriteLine("Solve - метод для поиска определенного интеграла с помощью многопоточности.");
            streamWriter.WriteLine("SingleThreaded_Solve - метод для поиска определенного интеграла в условиях однопоточности.");
            streamWriter.WriteLine();
            streamWriter.WriteLine("ПОИСК ОПТИМАЛЬНОГО ШАГА");
            streamWriter.WriteLine($"Минимальный размер шага, дающий оптимальный результат: {OptimalStep}");
            streamWriter.WriteLine("Пояснение:");
            streamWriter.WriteLine("Необходимо было найти минимальный размер шага из предложенных вариантов:");
            streamWriter.WriteLine("(1e-1, 1e-2, 1 e-3, 1e-4, 1e-5, 1e-6)");
            streamWriter.WriteLine("Этот шаг должен обеспечить оптимальную производительность с точностью 1e-4.");
            streamWriter.WriteLine("Выбирается самый первый найденный шаг, так как остальные могут выдать тот же результат, но с большими затратами времени.");
            streamWriter.WriteLine();
            streamWriter.WriteLine("МНОГОПОТОЧНАЯ РЕАЛИЗАЦИЯ");
            streamWriter.WriteLine($"Оптимальное количество потоков, с минимальным временем выполнения: {OptimalThreadNum}");
            streamWriter.WriteLine($"Время выполнения: {MinResult}");
            streamWriter.WriteLine("Пояснение:");
            streamWriter.WriteLine("Данное количество потоков обеспечивает минимальное время выполнения алгоритма.");
            streamWriter.WriteLine("Для точности время выполнения берется усредненно. (Проверяется по 5 раз)");
            streamWriter.WriteLine();
            streamWriter.WriteLine("ОДНОПОТОЧНАЯ РЕАЛИЗАЦИЯ (без потоков)");
            streamWriter.WriteLine($"Время однопоточной реализации: {MinSingleResult}");
            streamWriter.WriteLine("Пояснение:");
            streamWriter.WriteLine("Это усредненное время выполнения для однопоточной реализации.");
            streamWriter.WriteLine();
            streamWriter.WriteLine("СРАВНЕНИЕ");
            streamWriter.WriteLine($"Время однопоточной реализации: {MinSingleResult} | Время многопоточной реализации: {MinResult}");
            streamWriter.WriteLine($"Многопоточная быстрее однопоточной на {diffrence} %.");
            streamWriter.WriteLine("Проверяется условие через if, что эффективность не менее 15 процентов иначе выводится сообщение:");
            streamWriter.WriteLine("\"Эффективность использования многопоточности меньше 15 процентов, необходима доработка.\"");

        }
    }
}
