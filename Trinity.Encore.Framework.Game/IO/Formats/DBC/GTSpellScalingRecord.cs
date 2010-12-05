namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class GTSpellScalingRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public float Coefficient { get; set; }
    }
}
