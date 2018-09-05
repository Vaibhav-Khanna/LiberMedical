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
        public string Status { get; set; } = DocumentStatusEnum.waiting.ToString();

        [JsonProperty("attachment_path")]
        public string AttachmentPath { get; set; }

        [JsonProperty("patient_id")]
        public string PatientId { get; set; }

        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }

        [JsonIgnore]
        public string StatusString => Status == DocumentStatusEnum.waiting.ToString()
            ? "En attente"
                                                                      : Status == DocumentStatusEnum.valid.ToString()
                ? "Traité"
                                                                      : Status == DocumentStatusEnum.refused.ToString() ? "Refusé" : "En attente";

        public long Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }

    }
}
