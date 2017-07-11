using System;
namespace libermedical.Models
{
    public class Teledeclaration
    {
        public int Reference { set; get; }
        public DateTime AddDate { set; get; }
        public double TotalAccount { set; get; }
        public string Status { set; get; }
    }
}
