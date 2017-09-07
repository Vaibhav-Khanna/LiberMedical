using System;
using libermedical.Enums;

namespace libermedical.Models
{
    public class Teledeclaration
    {
        public int Reference { set; get; }
        public DateTime AddDate { set; get; }
        public double TotalAccount { set; get; }
        public StatusEnum Status { set; get; }

		public string StatusString => Status == StatusEnum.waiting
			? "En attente"
			: Status == StatusEnum.valid
				? "Traité"
				: "Refusé";
    }
}
