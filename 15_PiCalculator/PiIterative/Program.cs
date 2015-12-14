using System;
using System.Diagnostics;

namespace PiIterative
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Calculating PI -- please be patient...");

            const int million   = 1000 * 1000;
            const int rangeFrom = 0;
            const int rangeTo   = 200 * million;

            var stopWatch = Stopwatch.StartNew();

            double sum = 0;
            for (int n = rangeFrom; n <= rangeTo; n++)
                sum += Math.Pow(-1, n) / (2 * n + 1);

            stopWatch.Stop();

            Console.WriteLine("Pi = {0}, Elapsed: {1:F1}s", 4 * sum, stopWatch.ElapsedMilliseconds / 1000.0);
        }
    }
}
