using System;
using Newtonsoft.Json;

namespace libermedical.Models
{
	public interface IDTO
	{
		[JsonProperty(PropertyName = "id")]
		string Id { get; set; }


		//[JsonProperty(PropertyName = "version")]
		//byte[] Version { get; set; }


		[JsonProperty(PropertyName = "createdAt")]
		DateTimeOffset? CreatedAt { get; set; }


		[JsonProperty(PropertyName = "updatedAt")]
		DateTimeOffset? UpdatedAt { get; set; }


		[JsonProperty(PropertyName = "deleted")]
		bool Deleted { get; set; }
	}
}