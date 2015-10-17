namespace Backend
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRepository : RepositoryBase<UserEntity>
    {
        public UserRepository(CloudContext context) : base("User", context)
        {
        }

        internal void Add(User model)
        {
            // Create a new customer entity. 
            var entity = ToEntity(model);

            Insert(entity.Tuple);
        }

        private User FromEntity(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                Name = entity.Name,
                ProfileImage = entity.ProfileImage,
                SendMoment = entity.SendMoment
            };
        }

        private UserEntity ToEntity(User model)
        {
            var entity = Create();
            entity.Name = model.Name;
            entity.ProfileImage = model.ProfileImage;
            entity.SendMoment = model.SendMoment;

            return entity;
        }

        public new async Task<IEnumerable<User>> GetAll()
        {
            return (from entity in await base.GetAll()
                    select FromEntity(entity)).ToArray();
        }

        public new async Task<User> Find(string partitionKey)
        {
            return FromEntity(await base.Find(partitionKey));
        }
    }
}