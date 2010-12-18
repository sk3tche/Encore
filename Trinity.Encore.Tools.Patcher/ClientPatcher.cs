using System;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Encore.Framework.Game.IO;

namespace Trinity.Encore.Tools.Patcher
{
    public sealed class ClientPatcher
    {
        /// <summary>
        /// The pattern to search for when finding the location to patch.
        /// </summary>
        /// <remarks>
        /// The code to be patched looks like this in client version 4.0.3.13329:
        /// <code>
        /// .text:004906A9 018 E8 C2 E5 FF FF           call    ClientServices__GetConnectionIndex
        /// .text:004906AE 018 8B C8                    mov     ecx, eax
        /// .text:004906B0 018 83 C4 04                 add     esp, 4
        /// .text:004906B3 014 83 E1 01                 and     ecx, 1
        /// .text:004906B6 014 80 BC 31 60 46 00 00 00  cmp     byte ptr [ecx+esi+4660h], 0
        /// </code>
        /// </remarks>
        private static readonly byte?[] _connectionIndexPattern =
            {
                0xE8, null, null, null, null, 0x8B, 0xC8, 0x83, 0xC4, 0x04,
                0x83, 0xE1, 0x01, 0x80, 0xBC, 0x31, null, null, 0x00, 0x00,
            };

        private readonly PatternScanner _scanner;

        private readonly string _fileName;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_scanner != null);
            Contract.Invariant(!string.IsNullOrEmpty(_fileName));
        }

        public ClientPatcher(string fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            _fileName = fileName;
            var data = File.ReadAllBytes(fileName);
            _scanner = new PatternScanner(data);
        }

        /// <summary>
        /// Attempts to patch the client executable.
        /// </summary>
        /// <returns>A <see>Boolean</see> value indicating whether or not the patching succeeded.</returns>
        public bool Patch()
        {
            var offset = _scanner.Find(_connectionIndexPattern);

            if (offset == null)
            {
                Console.WriteLine("Offset not found.");
                return false;
            }

            var ofs = (long)offset;

            Console.WriteLine("Offset found at: 0x{0}", ofs.ToString("X8"));

            try
            {
                var stream = File.Open(_fileName, FileMode.Open, FileAccess.Write, FileShare.None);
                using (var writer = new BinaryWriter(stream))
                {
                    Contract.Assume(ofs >= 0);
                    stream.Position = ofs;
                    writer.Write(new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00 });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return false;
            }

            return true;
        }
    }
}
