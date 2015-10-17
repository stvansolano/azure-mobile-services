namespace Backend
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MomentRepository : RepositoryBase<MomentEntity>
    {
        public MomentRepository(CloudContext context) : base("Moment", context)
        {
        }

        public new async Task<IEnumerable<Moment>> GetAll()
        {
            return (from entity in await base.GetAll()
                    select FromEntity(entity)).ToArray();
        }

        private Moment FromEntity(MomentEntity entity)
        {
            return new Moment
            {
                DisplayTime = entity.DisplayTime,
                Id = entity.Id,
                MomentUrl = entity.MomentUrl,
                //SenderName = entity.SenderName,
                SenderUserId = entity.SenderUserId,
                RecipientUserId = entity.RecipientUserId,
                SenderProfileImage = entity.SenderProfileImage,
                TimeSent = entity.TimeSent
            };
        }

        internal async Task<IEnumerable<Moment>> FindSentTo(string userId)
        {
            var entities = await Find("RecipientUserId", userId).ConfigureAwait(false);

            return entities.Select(entity => FromEntity(entity)).ToArray();
        }

        private MomentEntity ToEntity(Moment model)
        {
            var entity = Create();

            entity.DisplayTime = model.DisplayTime;
            entity.MomentUrl = model.MomentUrl;
            //entity.SenderName = model.SenderName;
            entity.SenderUserId = model.SenderUserId;
            entity.RecipientUserId = model.RecipientUserId;
            entity.SenderProfileImage = model.SenderProfileImage;
            entity.TimeSent = model.TimeSent;

            return entity;
        }

        internal void Add(Moment model)
        {
            var entity = ToEntity(model);

            Insert(entity.Tuple);
        }
    }
}