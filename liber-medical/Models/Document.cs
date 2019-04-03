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

        [JsonProperty("refused_reason")]
        public string RefusedReason { get; set; }

        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }

        [JsonIgnore]
        public string StatusString => Status == DocumentStatusEnum.waiting.ToString()
            ? "En attente"
                                                                      : Status == DocumentStatusEnum.valid.ToString()
                ? "Traitée"
                                                                      : Status == DocumentStatusEnum.refused.ToString() ? "Refusée" : "En attente";

        [JsonIgnore]
        public string RefusedReasonString => !string.IsNullOrWhiteSpace(RefusedReason) && Status == StatusEnum.refused.ToString() ? "Motif: " + RefusedReason : "";

        public long Reference { set; get; }

        public DateTime AddDate { set; get; }

        public Patient Patient { set; get; }

    }
}
