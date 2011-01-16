namespace Trinity.Encore.AccountService.Database.Implementation
{
    public abstract class AccountDatabaseRecord
    {
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
