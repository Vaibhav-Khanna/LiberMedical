using System;

namespace libermedical.Models
{
	public class Ordonnance:BaseDTO
    {

        public long Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }
        public string Status { set; get; }

    }
}
