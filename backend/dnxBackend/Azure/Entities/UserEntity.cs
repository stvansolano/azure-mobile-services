namespace Backend
{
    public class UserEntity : StorageEntity
    {
        public UserEntity()
        {
            AllocateProperty("Name", string.Empty);
            AllocateProperty("ProfileImage", string.Empty);
            AllocateProperty("SendMoment", false);

            Map = (storage, entity) => {
                var user = (UserEntity)entity;

                user.Name = FromStoredPropertyString(storage, "Name");
                user.ProfileImage = FromStoredPropertyString(storage, "ProfileImage");
                user.SendMoment = FromStoredPropertyBoolean(storage, "SendMoment").GetValueOrDefault(false);
            };
        }

        public string Id
        {
            get { return Tuple.RowKey; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                SetValue("Name", value);
            }
        }

        private string _profileImage;
        public string ProfileImage
        {
            get { return _profileImage; }
            set
            {
                _profileImage = value;
                SetValue("ProfileImage", value);
            }
        }

        private bool _sendMoment;
        public bool SendMoment
        {
            get { return _sendMoment; }
            set
            {
                _sendMoment = value;
                SetValue("SendMoment", value);
            }
        }
    }
}