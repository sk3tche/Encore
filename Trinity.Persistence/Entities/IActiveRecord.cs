namespace Trinity.Persistence.Entities
{
    public interface IActiveRecord
    {
        void Create();

        void Update();

        void Delete();
    }
}
