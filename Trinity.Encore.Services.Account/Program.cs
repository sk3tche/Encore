using System;

namespace Trinity.Encore.Services.Account
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            AccountApplication.Instance.Start(args);
            Console.WriteLine("Ready.");
            Console.ReadKey(); // TODO: Write an actual command console (Zor).
            AccountApplication.Instance.Stop();
        }
    }
}
