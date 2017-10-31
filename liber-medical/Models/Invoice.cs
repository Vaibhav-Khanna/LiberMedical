using Newtonsoft.Json;

namespace libermedical.Models
{
    public class Invoice : BaseDTO
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("file_path")]
        public string FilePath { get; set; }

        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }
    }
}
