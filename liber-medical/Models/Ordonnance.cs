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
        public long First_Care_At { get; set; }
        [JsonProperty("statusChangedAt")]
        public long StatusChangedAt { get; set; }
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
        [JsonProperty("frequencies")]
        public List<Frequency> Frequencies { get; set; }
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        [JsonProperty("nurseName")]
        public string NurseName { get; set; }

        public long Reference { set; get; }
        public Patient Patient { set; get; }
        
        [JsonIgnore]
        public string StatusString => Status == StatusEnum.waiting
            ? "En attente"
            : Status == StatusEnum.valid
                ? "Traité"
                : "Refusé";

        [JsonIgnore]
        public DateTime FirstCareAt => ConvertFromUnixTimestamp(First_Care_At);

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            try
            {
                return origin.AddSeconds(timestamp);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
