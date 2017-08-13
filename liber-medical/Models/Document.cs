using System;
using libermedical.DTO.Models;

namespace libermedical.Models
{
	public class Document
    {

        public int Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }

    }
}
