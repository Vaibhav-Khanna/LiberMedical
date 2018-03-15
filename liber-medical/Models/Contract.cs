using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace libermedical.Models
{
   

    public partial class Contract
    {
        [JsonProperty("attachment_path")]
        public string AttachmentPath { get; set; }

        [JsonProperty("lastUpdatedAt")]
        public DateTime LastUpdatedAt { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }
    }

    public partial class Contract
    {
        public static Contract FromJson(string json) => JsonConvert.DeserializeObject<Contract>(json, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
