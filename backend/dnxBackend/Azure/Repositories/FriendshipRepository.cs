using System;
using System.Collections.Generic;

namespace Backend
{
    public class FriendshipRepository : RepositoryBase<FriendshipEntity>
    {
        public FriendshipRepository(CloudContext context) : base("Friendship", context)
        {
        }

        internal IEnumerable<FriendshipEntity> Where(Predicate<FriendshipEntity> predicate)
        {
            return new FriendshipEntity[0];
        }
    }
}