using Newtonsoft.Json;

namespace Backend
{
	public class User
	{
        public string Id { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("profileImage")]
		public string ProfileImage { get; set; }

		[JsonIgnore]
		public bool SendMoment { get; set; }

        // TODO: Fill those properties through the application
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}