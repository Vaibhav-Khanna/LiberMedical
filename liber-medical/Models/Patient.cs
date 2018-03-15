using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace libermedical.Models
{
	public class Patient:BaseDTO
	{
        [JsonProperty("firstname")]
		public string FirstName { set; get; }
        [JsonProperty("lastname")]
        public string LastName { set; get; }
        [JsonProperty("phones")]
        public List<string> PhoneNumbers { get; set; }
        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }

        [JsonIgnore]
		public string Fullname => this.ToString();

		public override string ToString()
		{
			if (string.IsNullOrEmpty(FirstName + LastName))
			{
				return "Unknown User";
			}
			return $"{FirstName} {LastName}";
		}
		
	}

	public class GroupedItem<T> : ObservableCollection<BaseDTO>
	{
		public string HeaderKey
		{
			get;
			set;
		}

	}
}
