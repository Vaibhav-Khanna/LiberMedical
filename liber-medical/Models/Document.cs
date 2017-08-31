using System;

namespace libermedical.Models
{
    public class Document : BaseDTO
    {

        public long Reference { set; get; }
        public DateTime AddDate { set; get; }
        public Patient Patient { set; get; }
        public string FilePath { get; set; }

    }
}
