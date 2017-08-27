using Newtonsoft.Json;

namespace libermedical.Models
{
	public class Profile:BaseDTO
	{
		public string FirstName { get; set; } 
		public string LastName{ get; set; } 
		public string PhoneNumber{ get; set; } 
		public string EmailAddress{ get; set; }

		[JsonIgnore]
		public string FullName => $"{FirstName} {LastName}";

		public override string ToString()
		{
			return FullName;
		}
	}
}