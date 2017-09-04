using System;
using System.Collections.Generic;
using libermedical.Enums;
using Newtonsoft.Json;

namespace libermedical.Models
{
    public class Ordonnance : BaseDTO
    {
        [JsonProperty("status")]
        public StatusEnum Status { get; set; }
        [JsonProperty("first_care_at")]
        public DateTime FirstCareAt { get; set; }
        [JsonProperty("comments")]
        public string Comments { get; set; }
        [JsonProperty("patient_id")]
        public string PatientId { get; set; }
        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }
        [JsonProperty("refused_reason")]
        public string RefusedReason { get; set; }
        [JsonProperty("attachments")]
        public List<string> Attachments { get; set; }

        public long Reference { set; get; }
        public Patient Patient { set; get; }
        
        public string StatusString => Status == StatusEnum.waiting
            ? "En attente"
            : Status == StatusEnum.valid
                ? "Traité"
                : "Refusé";
    }
}
