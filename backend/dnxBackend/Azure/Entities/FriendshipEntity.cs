namespace Backend
{
    public class FriendshipEntity : StorageEntity
    {
        public FriendshipEntity()
        {
            AllocateProperty("UserId", string.Empty);
            AllocateProperty("FriendId", string.Empty);
            AllocateProperty("Accepted", false);

            Map = (storage, entity) => {
                var friendship = (FriendshipEntity)entity;

                friendship.UserId = FromStoredPropertyString(storage, "UserId");
                friendship.FriendId = FromStoredPropertyString(storage, "FriendId");
                friendship.Accepted = FromStoredPropertyBoolean(storage, "Accepted").GetValueOrDefault(false);
            };
        }

        public string Id
        {
            get { return Tuple.RowKey; }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                SetValue("UserId", value);
            }
        }

        private string _friendId;
        public string FriendId
        {
            get { return _friendId; }
            set
            {
                _friendId = value;
                SetValue("FriendId", value);
            }
        }

        private bool _accepted;
        public bool Accepted
        {
            get { return _accepted; }
            set
            {
                _accepted = value;
                SetValue("Accepted", value);
            }
        }
    }
}