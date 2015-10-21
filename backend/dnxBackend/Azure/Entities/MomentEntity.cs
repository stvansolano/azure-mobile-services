namespace Backend
{
    using System;

    public class MomentEntity : StorageEntity
    {
        public MomentEntity()
        {
            AllocateProperty("MomentUrl", string.Empty);
            AllocateProperty("SenderUserId", string.Empty);
            AllocateProperty("SenderProfileImage", string.Empty);
            AllocateProperty("RecipientUserId", string.Empty);
            AllocateProperty("DisplayTime", (int?)null);
            AllocateProperty("TimeSent", (DateTime?)null);

            Map = (storage, entity) => {
                var account = (MomentEntity)entity;

                account.MomentUrl = FromStoredPropertyString(storage, "MomentUrl");
                account.SenderUserId = FromStoredPropertyString(storage, "SenderUserId");
                //account.SenderName = FromStoredPropertyString(storage, "SenderName");
                account.SenderProfileImage = FromStoredPropertyString(storage, "SenderProfileImage");
                account.RecipientUserId = FromStoredPropertyString(storage, "RecipientUserId");
                account.DisplayTime = FromStoredPropertyInt32(storage, "DisplayTime").GetValueOrDefault(0);
                account.TimeSent = FromStoredPropertyDateTime(storage, "TimeSent").GetValueOrDefault(DateTime.Now);
            };
        }

        public string Id
        {
            get { return Tuple.RowKey; }
        }

        private string _momentUrl;
        public string MomentUrl
        {
            get { return _momentUrl; }
            set
            {
                _momentUrl = value;
                SetValue("MomentUrl", value);
            }
        }

        private string _senderUserId;
        public string SenderUserId
        {
            get { return _senderUserId; }
            set
            {
                _senderUserId = value;
                SetValue("SenderUserId", value);
            }
        }

        private string _senderProfileImage;
        public string SenderProfileImage
        {
            get { return _senderProfileImage; }
            set
            {
                _senderProfileImage = value;
                SetValue("SenderProfileImage", value);
            }
        }

        private string _recipientUserId;
        public string RecipientUserId
        {
            get { return _recipientUserId; }
            set
            {
                _recipientUserId = value;
                SetValue("RecipientUserId", value);
            }
        }

        private int _displayTime;
        public int DisplayTime
        {
            get { return _displayTime; }
            set
            {
                _displayTime = value;
                SetValue("DisplayTime", value);
            }
        }

        private DateTime _timeSent;
        public DateTime TimeSent
        {
            get { return _timeSent; }
            set
            {
                _timeSent = value;
                SetValue("TimeSent", value);
            }
        }
    }
}