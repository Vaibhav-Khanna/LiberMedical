using System;
using libermedical.Converters;
using Newtonsoft.Json;
using PropertyChanged;

namespace libermedical.Models
{
    [AddINotifyPropertyChangedInterface]
    public class BaseDTO : IDTO
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

         [JsonProperty("lastUpdatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool Deleted { get; set; }

		[JsonConverter(typeof(BoolSyncConverter))]	
		public bool IsSynced { get; set; } 
    }
}
