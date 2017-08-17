using System;
using libermedical.Converters;
using Newtonsoft.Json;
using PropertyChanged;

namespace libermedical.Models
{
	[AddINotifyPropertyChangedInterface]
	public class BaseDTO : IDTO
	{
		public string Id { get; set; }

		public DateTimeOffset? CreatedAt { get; set; }

		public DateTimeOffset? UpdatedAt { get; set; }

		[JsonConverter(typeof(BoolConverter))]
		public bool Deleted { get; set; }
	}
}