using System.Collections.Generic;
using libermedical.Enums;
using Newtonsoft.Json;

namespace libermedical.Models
{
    public class Frequency
    {
        [JsonProperty("period")]
        public PeriodEnum Period { get; set; }
        [JsonProperty("quotations")]
        public List<string> Quotations { get; set; }
        [JsonProperty("increase")]
        public IncreaseEnum Increase { get; set; }
        [JsonProperty("movement")]
        public string Movement { get; set; } = "Non";
        [JsonProperty("night")]
        public bool Night { get; set; }
    }
}
