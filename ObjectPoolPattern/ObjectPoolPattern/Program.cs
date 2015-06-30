using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectPoolPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            // Create an opportunity for the user to cancel.
            Task.Run(() =>
            {
                if (Console.ReadKey().KeyChar == 'c' || Console.ReadKey().KeyChar == 'C')
                    cts.Cancel();
            });

            ObjectPool<TestClass> pool = new ObjectPool<TestClass>(() => new TestClass());

            // Create a high demand for MyClass objects.
            Parallel.For(0, 1000000, (i, loopState) =>
            {

                using(TestClass mc = pool.GetResource())
                {
                    Console.CursorLeft = 0;
                    // This is the bottleneck in our application. All threads in this loop 
                    // must serialize their access to the static Console class.
                    Console.WriteLine("{0:####.####}", mc.GetValue(i));
                    // pool.PoolResource(mc); alternative to implementing repool in the dispose method
                }

                if (cts.Token.IsCancellationRequested)
                    loopState.Stop();

            });
            Console.WriteLine("Press the Enter key to exit.");
            Console.ReadLine();
            cts.Dispose();
        }
    }
}
