namespace task15;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (function == null)
        throw new ArgumentNullException("Функция не должна быть пустой.");

        if (step <= 0)
        throw new ArgumentException("Шаг вычислений должен быть положителен.");

        if (threadsnumber <= 0)
        throw new ArgumentException("Количество потоков не может быть отрицательным.");

        double Width = (b - a) / threadsnumber;

        Thread[] threads = new Thread[threadsnumber];

        double result = 0.0;
        
        using (Barrier barrier = new Barrier(participantCount: threadsnumber + 1))
        {
            for (int i = 0; i < threadsnumber; i++)
            {
                double start = a + i * Width;
                double end = (i + 1 != threadsnumber) ? start + Width : b;

                double localStart = start;
                double localEnd = end;

                threads[i] = new Thread(() =>
                {
                    double ThreadResult = SegmentCalculation(localStart, localEnd, step, function);

                    InterlockedSum(ref result, ThreadResult);

                    barrier.SignalAndWait();
                });

                threads[i].Start();
            }

            barrier.SignalAndWait();
        }

        return result;
    }

    //Подсчет результатов в потоках.
    private static double SegmentCalculation(double start, double end, double step, Func<double, double> function)
    {
        double ThreadResult = 0.0;

        double StepStart_X = start;
        
        while (StepStart_X < end)
        {
            double StepEnd_X = StepStart_X + step;
            if (StepEnd_X > end)
            StepEnd_X = end;

            double StepStart_Y = function(StepStart_X);

            double StepEnd_Y = function(StepEnd_X);

            ThreadResult += (StepStart_Y + StepEnd_Y)/2 * (StepEnd_X - StepStart_X);

            StepStart_X = StepEnd_X;
        }
        
        return ThreadResult;
    }

    //Метод для безопасного суммирования результатов с использованием Interlocked операции.
    private static void InterlockedSum(ref double result, double ThreadResult)
    {
        double currentResult = result;

        while(true)
        {
            double modifiedResult = currentResult;
            double ThreadResultPlus = modifiedResult + ThreadResult;

            currentResult = Interlocked.CompareExchange(ref result, ThreadResultPlus, modifiedResult);

            if (modifiedResult == currentResult)
            break;
        }
    }
    //Метод для однопоточной реализации.
    public static double SingleThreaded_Solve(double a, double b, Func<double, double> function, double step)
    {
        if (function == null)
        throw new ArgumentNullException("Функция не должна быть пустой.");

        if (step <= 0)
        throw new ArgumentException("Шаг вычислений должен быть положителен.");

        double result = 0.0;

        double StepStart_X = a;
        
        while (StepStart_X < b)
        {
            double StepEnd_X = StepStart_X + step;
            if (StepEnd_X > b)
            StepEnd_X = b;

            double StepStart_Y = function(StepStart_X);

            double StepEnd_Y = function(StepEnd_X);

            result += (StepStart_Y + StepEnd_Y)/2 * (StepEnd_X - StepStart_X);

            StepStart_X = StepEnd_X;
        }
        
        return result;
    }
}
