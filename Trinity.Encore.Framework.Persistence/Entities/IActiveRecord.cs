using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Persistence.Entities
{
    public interface IActiveRecord
    {
        void Create();

        void Update();

        void Delete();
    }
}
