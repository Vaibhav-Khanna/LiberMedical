using System;
using libermedical.Models;

namespace libermedical.Models
{
	public class Document : BaseDTO
    {

        public int Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }

    }
}
