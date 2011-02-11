namespace Trinity.Encore.AccountService.Database.Implementation
{
    public abstract class AccountDatabaseRecord
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
