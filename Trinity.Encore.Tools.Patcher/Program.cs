using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Tools.Patcher
{
    public static class Program
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
