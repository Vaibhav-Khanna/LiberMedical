using System.Collections.Generic;
using libermedical.Enums;
using Newtonsoft.Json;

namespace libermedical.Models
{
	public class User : BaseDTO
	{
		[JsonProperty("firstname")]
		public string Firstname { get; set; }
		[JsonProperty("lastname")]
		public string Lastname { get; set; }
		[JsonProperty("phone")]
		public string Phone { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }
		[JsonProperty("password")]
		public string Password { get; set; }
		[JsonProperty("reference")]
		public string Reference { get; set; }
		[JsonProperty("role")]
		public RoleEnum Role { get; set; }
		[JsonProperty("nursesInCharge")]
		public List<string> NursesInCharge { get; set; }

        [JsonProperty("oneSignalId")]
        public string OneSignalId { get; set; }

		[JsonIgnore]
		public string Fullname => $"{Firstname} {Lastname}";

		public override string ToString()
		{
			return Fullname;
		}
	}
}
