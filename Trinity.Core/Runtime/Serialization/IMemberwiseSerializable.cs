namespace Trinity.Core.Runtime.Serialization
{
    public interface IMemberwiseSerializable<T>
    {
        T Serialize();
    }
}
