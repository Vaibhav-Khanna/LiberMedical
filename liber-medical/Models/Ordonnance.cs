using System;
using System.Collections.Generic;
using libermedical.Enums;

namespace libermedical.Models
{
    public class Ordonnance : BaseDTO
    {
        public string Id { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime FirstCareAt { get; set; }
        public string Comments { get; set; }
        public string PatientId { get; set; }
        public string NurseId { get; set; }
        public string RefusedReason { get; set; }
        public long Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }
		public List<string> Attachments { get; set; }

        public string StatusString => Status == StatusEnum.Attente
            ? "En attente"
            : Status == StatusEnum.Traite
                ? "Traité"
                : "Refusé";
    }
}
