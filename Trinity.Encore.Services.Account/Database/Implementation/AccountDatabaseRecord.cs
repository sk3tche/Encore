using Trinity.Encore.Framework.Core.Threading.Actors;
using Trinity.Encore.Framework.Persistence.Entities;

namespace Trinity.Encore.Services.Account.Database.Implementation
{
    public abstract class AccountDatabaseRecord<TRecord> : ChildActor<TRecord>, IActiveRecord
        where TRecord : AccountDatabaseRecord<TRecord>
    {
        protected AccountDatabaseRecord()
            : base(AccountApplication.Instance.AccountDbContext)
        {
        }

        public virtual void Create()
        {
            AccountApplication.Instance.AccountDbContext.Post(x => x.Add(this));
        }

        public virtual void Update()
        {
            AccountApplication.Instance.AccountDbContext.Post(x => x.Update(this));
        }

        public virtual void Delete()
        {
            AccountApplication.Instance.AccountDbContext.Post(x => x.Delete(this));
        }
    }
}
