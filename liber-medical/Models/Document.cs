using libermedical.Enums;
using Newtonsoft.Json;
using System;

namespace libermedical.Models
{
    public class Document : BaseDTO
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("status")]
        public DocumentStatusEnum Status { get; set; }
        [JsonProperty("attachment_path")]
        public string AttachmentPath { get; set; }
        [JsonProperty("patient_id")]
        public string PatientId { get; set; }
        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }

        public long Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }

    }
}
