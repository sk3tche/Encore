namespace Trinity.Encore.Framework.Core.Runtime.Serialization
{
    public interface IMemberwiseSerializable<T>
    {
        T Serialize();
    }
}
