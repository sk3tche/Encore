using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using Trinity.Core;
using Trinity.Core.IO;
using Trinity.Core.Reflection;

namespace Trinity.Encore.Game.IO
{
    [ContractClass(typeof(ClientDbReaderContracts<>))]
    public abstract class ClientDbReader<T> : BinaryFileReader
        where T : class, IClientDbRecord, new()
    {
        protected ClientDbReader(string fileName)
            : base(fileName, System.Text.Encoding.UTF8)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            StringTable = new Dictionary<int, string>();
            Entries = new Dictionary<int, T>();

            ReadFile();
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(StringTable != null);
            Contract.Invariant(Entries != null);
        }

        public virtual StringReadMode StringReadMode
        {
            get { return StringReadMode.StringTable1; }
        }

        public virtual int? HeaderMagic
        {
            get { return null; }
        }

        public int RecordCount { get; protected set; }

        public int StringTableSize { get; protected set; }

        public Dictionary<int, string> StringTable { get; private set; }

        public Dictionary<int, T> Entries { get; private set; }

        protected override void Read(BinaryReader reader)
        {
            if (HeaderMagic != null)
                if (reader.ReadInt32() != HeaderMagic)
                    throw new ClientDbException("Invalid client DB header magic number.");

            var data = ReadData(reader);

            if (StringReadMode != StringReadMode.Direct)
                ReadStringTable(reader);

            MapRecords(data);
        }

        protected abstract byte[] ReadData(BinaryReader reader);

        private void MapRecords(byte[] data)
        {
            Contract.Requires(data != null);

            using (var reader = new BinaryReader(new MemoryStream(data), Encoding))
            {
                for (var i = 0; i < RecordCount; i++)
                {
                    var obj = new T();
                    ReadValuesToClass(obj, reader);
                    Entries.Add(obj.Id, obj);
                }
            }
        }

        private void ReadValuesToClass(object obj, BinaryReader reader)
        {
            Contract.Requires(obj != null);
            Contract.Requires(reader != null);

            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Instance))
            {
                Contract.Assume(prop != null);

                if (prop.GetCustomAttribute<SkipPropertyAttribute>() != null)
                    continue; // Skip this property.

                var value = ReadValueToProperty(prop, reader);
                prop.SetValue(obj, value, null);
            }
        }

        private object ReadValueToProperty(PropertyInfo prop, BinaryReader reader)
        {
            Contract.Requires(prop != null);
            Contract.Requires(reader != null);
            Contract.Ensures(Contract.Result<object>() != null);

            var realAttr = prop.GetCustomAttribute<RealTypeAttribute>();
            var type = realAttr != null ? realAttr.RealType : prop.PropertyType;
            object value;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    value = Activator.CreateInstance(type);
                    ReadValuesToClass(value, reader);
                    break;
                case TypeCode.Boolean:
                    value = reader.ReadBoolean();
                    break;
                case TypeCode.SByte:
                    value = reader.ReadSByte();
                    break;
                case TypeCode.Byte:
                    value = reader.ReadByte();
                    break;
                case TypeCode.Int16:
                    value = reader.ReadInt16();
                    break;
                case TypeCode.UInt16:
                    value = reader.ReadUInt16();
                    break;
                case TypeCode.Int32:
                    value = reader.ReadInt32();
                    break;
                case TypeCode.UInt32:
                    value = reader.ReadUInt32();
                    break;
                case TypeCode.Int64:
                    value = reader.ReadInt64();
                    break;
                case TypeCode.UInt64:
                    value = reader.ReadUInt64();
                    break;
                case TypeCode.Single:
                    value = reader.ReadSingle();
                    break;
                case TypeCode.String:
                    var str = StringReadMode == StringReadMode.Direct ? reader.ReadCString() : StringTable[reader.ReadInt32()];
                    Contract.Assume(str != null);
                    value = str;
                    break;
                default:
                    throw new ClientDbException("Unsupported field type {0} encountered.".Interpolate(type));
            }

            // If we have a RealTypeAttribute, cast the value into the type used in code.
            if (realAttr != null)
                value = value.Cast(prop.PropertyType);

            if (type.IsEnum)
                value = Enum.ToObject(type, value);

            return value;
        }

        protected void ReadStringTable(BinaryReader reader)
        {
            Contract.Requires(reader != null);

            var stream = reader.BaseStream;
            var stringTableStart = stream.Position;
            var stringTableEnd = stringTableStart + StringTableSize;

            while (stream.Position != stringTableEnd)
            {
                var stringIndex = (int)(stream.Position - stringTableStart);

                if (StringReadMode == StringReadMode.StringTable2)
                    reader.ReadInt16(); // String length.

                StringTable[stringIndex] = reader.ReadCString();
            }
        }
    }

    [ContractClassFor(typeof(ClientDbReader<>))]
    public abstract class ClientDbReaderContracts<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        protected ClientDbReaderContracts(string fileName)
            : base(fileName)
        {
        }

        protected override byte[] ReadData(BinaryReader reader)
        {
            Contract.Requires(reader != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            return null;
        }
    }
}
