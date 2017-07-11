using System;
namespace libermedical.Models
{
    public class Patient
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string FirstLastName { get { return FirstName + " " + LastName; } }
    }
}
