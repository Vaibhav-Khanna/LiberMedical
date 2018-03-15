using System;
using libermedical.DTO.Converters;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using PropertyChanged;

namespace libermedical.DTO.Models
{
	[AddINotifyPropertyChangedInterface]
	public class BaseDTO : IDTO
	{
		public string Id { get; set; }

		[CreatedAt]
		public DateTimeOffset? CreatedAt { get; set; }

		[UpdatedAt]
		public DateTimeOffset? UpdatedAt { get; set; }

		[Deleted]
		[JsonConverter(typeof(BoolConverter))]
		public bool Deleted { get; set; }
	}
}