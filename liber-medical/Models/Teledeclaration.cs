using System;
using libermedical.Enums;
using Newtonsoft.Json;

namespace libermedical.Models
{
    public class Teledeclaration : BaseDTO
    {

        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("file_path")]
        public string FilePath { get; set; }
        [JsonProperty("patient_id")]
        public string PatientId { get; set; }
        [JsonProperty("nurse_id")]
        public string NurseId { get; set; }
        [JsonProperty("employee_id")]
        public string EmployeeId { get; set; }

        [JsonIgnore]
        public string StatusString => Status == StatusEnum.waiting.ToString()
                    ? "En attente de validation"
                                                              : Status == StatusEnum.valid.ToString()
                                                          ? "En attente de télétransmission"
                                                              : Status == StatusEnum.refused.ToString() ? "Refusée" : Status == "sent" ? "Télétransmise" : "";
	}
}
