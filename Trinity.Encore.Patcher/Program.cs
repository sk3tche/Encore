using System;
using Trinity.Core.Collections;

namespace Trinity.Encore.Patcher
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var fileName = args.TryGet(0);
            var patcher = !string.IsNullOrEmpty(fileName) ? new ClientPatcher(fileName) : new ClientPatcher("Wow.exe");
            var result = patcher.Patch();

            Console.WriteLine("Patching {0}.", result ? "succeeded" : "failed");
        }
    }
}
