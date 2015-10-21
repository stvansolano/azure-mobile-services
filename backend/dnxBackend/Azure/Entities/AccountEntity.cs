namespace Backend
{
    public class AccountEntity : StorageEntity
    {
        public AccountEntity()
        {
            AllocateProperty("Username", string.Empty);
            AllocateProperty("Password", string.Empty);
            AllocateProperty("Email", string.Empty);
            AllocateProperty("UserId", string.Empty);

            Map = (storage, entity) => {
                var account = (AccountEntity)entity;

                account.Username = FromStoredPropertyString(storage, "Username");
                account.Password = FromStoredPropertyString(storage, "Password");
                account.Email = FromStoredPropertyString(storage, "Email");
                account.UserId = FromStoredPropertyString(storage, "UserId");
            };
        }

        public string Id
        {
            get { return Tuple.RowKey; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                SetValue("Username", value);
            }
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

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                SetValue("Password", value);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                SetValue("Email", value);
            }
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
    }
}