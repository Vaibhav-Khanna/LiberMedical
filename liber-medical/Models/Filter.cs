using System;
using System.Collections.Generic;
using libermedical.Enums;

namespace libermedical.Models
{
    public class Filter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<StatusEnum> Statuses { get; set; }
        public bool IsActivated { get; set; }
		public bool EnableDateSearch
		{
			get;
			set;
		}
    }
}
