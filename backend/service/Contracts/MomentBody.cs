namespace Backend
{
    using System.Linq;

    public class MomentBody
    {
        public string Attached { get; set; }
        public int DisplayTime { get; set; }
        public string[] Recipients { get; set; }
        public string SenderId { get;  set; }
        public string SenderProfileImage { get; set; }
        public string Url { get; set; }

        internal bool IsValid()
        {
            if (Recipients == null)
            {
                return false;
            }
            if (Recipients.Any() == false)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SenderId))
            {
                return false;
            }
            return true;
        }

        public bool ContainsAttachedContent
        {
            get
            {
                return string.IsNullOrEmpty(Url) && string.IsNullOrEmpty(Attached) == false;
            }
        }

        internal string[] SanitizeRecipients()
        {
            if (Recipients == null)
            {
                return new string[0];
            }
            if (Recipients.Any() == false)
            {
                return new string[0];
            }
            return Recipients.Where(item => IsValid(item)).ToArray();
        }

        private bool IsValid(string item)
        {
            return string.IsNullOrEmpty(item) == false && string.IsNullOrEmpty(item.Trim()) == false;
        }
    }
}