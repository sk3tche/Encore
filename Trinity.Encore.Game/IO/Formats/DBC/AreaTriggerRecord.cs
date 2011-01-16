using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class AreaTriggerRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int Map { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public float Radius { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Orientation { get; set; }
    }
}
