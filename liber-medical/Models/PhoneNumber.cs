using Newtonsoft.Json;

namespace libermedical.Models
{
	public class PhoneNumber : BaseDTO
	{
		public string Number { get; set; }
        [JsonIgnore]
		public string PatientId { get; set; }
	}
}