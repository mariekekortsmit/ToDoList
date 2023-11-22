namespace LearningAsync
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Step 1 - {DateTime.Now}");
            Step1();
            Console.WriteLine($"Step 2 - {DateTime.Now}");
            Step2();
            Console.WriteLine($"Step 3 - {DateTime.Now}");
            Step3();
            Console.WriteLine($"Step 4 - {DateTime.Now}");
            await Step4();
        }

        static void Step1()
        {
            Task.Delay(2000).Wait();
            Console.WriteLine($"Inside Step 1 - {DateTime.Now}");
        }

        static Task Step2()
        {
            Task.Delay(2000);
            Console.WriteLine($"Inside Step 2 - {DateTime.Now}");
            return Task.CompletedTask;
        }

        static void Step3()
        {
            var task = Task.Delay(2000);
            Console.WriteLine($"Inside Step 3 - {DateTime.Now}");
            task.Wait();
        }

        static async Task Step4()
        {
            await Task.Delay(2000);
            Console.WriteLine($"Inside Step 4 - {DateTime.Now}");
        }
    }
}