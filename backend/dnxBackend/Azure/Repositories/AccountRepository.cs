namespace Backend
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountRepository : RepositoryBase<AccountEntity>
    {
        public AccountRepository(CloudContext context) : base("Account", context)
        {
        }

        internal bool Add(Account model)
        {
            return true;
        }

        public new async Task<IEnumerable<Account>> GetAll()
        {
            return (from entity in await base.GetAll()
                    select FromEntity(entity)).ToArray();
        }

        private Account FromEntity(AccountEntity entity)
        {
            return new Account
            {
                Id = entity.Id,
                Email = entity.Email,
                Password = "*****",
                Username = entity.Email,
                UserId = entity.UserId
            };
        }
    }
}