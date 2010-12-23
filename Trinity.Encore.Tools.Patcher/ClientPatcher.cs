using System;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Encore.Framework.Game.IO;

namespace Trinity.Encore.Tools.Patcher
{
    public sealed class ClientPatcher
    {
        /// <summary>
        /// The search pattern to find the location to patch the connection index selection.
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

        /// <summary>
        /// The search pattern to find the location to patch the Grunt/Battle.net selection.
        /// </summary>
        /// <remarks>
        /// The code to be patched looks like this in client version 4.0.3.13329:
        /// <code>
        ///.text:004D5F5D 50C 74 05                     jz      short loc_4D5F64
        ///.text:004D5F5F 50C BE 01 00 00 00            mov     esi, 1
        ///.text:004D5F64                               loc_4D5F64:
        ///.text:004D5F64 50C 8B 0D 84 3F CB 00         mov     ecx, dword_CB3F84
        ///.text:004D5F6A 50C 85 C9                     test    ecx, ecx
        ///.text:004D5F6C 50C 74 56                     jz      short loc_4D5FC4
        ///.text:004D5F6E 50C 8B 01                     mov     eax, [ecx]
        /// </code>
        /// </remarks>
        private static readonly byte?[] _emailCheckPattern =
            {
                0x74, null, 0xBE, 0x01, 0x00, 0x00, 0x00, 0x8B, 0x0D, null,
                null, null, null, 0x85, 0xC9, 0x74, null, 0x8B, 0x01, 0x8B,
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
        /// <param name="patchName">Name of the patch to be applied.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <param name="replacementBytes">The bytes to insert at the found location.</param>
        /// <returns>A <see>Boolean</see> value indicating whether or not the patching succeeded.</returns>
        private bool Patch(string patchName, byte?[] pattern, byte[] replacementBytes)
        {
            Contract.Requires(!string.IsNullOrEmpty(patchName));
            Contract.Requires(pattern != null);
            Contract.Requires(replacementBytes != null);

            var offset = _scanner.Find(pattern);

            if (offset == null)
            {
                Console.WriteLine("{0}: Offset not found.", patchName);
                return false;
            }

            var ofs = (long)offset;

            Console.WriteLine("{0}: Offset found at: 0x{1}", patchName, ofs.ToString("X8"));

            try
            {
                var stream = File.Open(_fileName, FileMode.Open, FileAccess.Write, FileShare.None);
                using (var writer = new BinaryWriter(stream))
                {
                    Contract.Assume(ofs >= 0);
                    stream.Position = ofs;
                    writer.Write(replacementBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: {1}", patchName, ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to patch the client executable.
        /// </summary>
        /// <returns>A <see>Boolean</see> value indicating whether or not the patching succeeded.</returns>
        public bool Patch()
        {
            if (!Patch("Connection index selection", _connectionIndexPattern, new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00 }))
                return false;

            if (!Patch("Grunt/Battle.net selection", _emailCheckPattern, new byte[] { 0xEB }))
                return false;

            return true;
        }
    }
}
