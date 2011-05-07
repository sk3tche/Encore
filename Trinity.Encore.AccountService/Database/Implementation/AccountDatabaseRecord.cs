using Trinity.Persistence.Entities;

namespace Trinity.Encore.AccountService.Database.Implementation
{
    public abstract class AccountDatabaseRecord : IActiveRecord
    {
        public virtual void Create()
        {
            AccountApplication.Instance.AccountDbContext.PostAsync(x => x.Add(this));
        }

        public virtual void Update()
        {
            AccountApplication.Instance.AccountDbContext.PostAsync(x => x.Update(this));
        }

        public virtual void Delete()
        {
            AccountApplication.Instance.AccountDbContext.PostAsync(x => x.Delete(this));
        }
    }
}
