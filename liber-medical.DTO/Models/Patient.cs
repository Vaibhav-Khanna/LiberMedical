namespace libermedical.DTO.Models
{
	public class Patient:BaseDTO
    {
        public string FirstName { set; get; }

		public string LastName { set; get; }

	    public string FullName => this.ToString();

		public override string ToString()
		{
			if (string.IsNullOrEmpty(FirstName+LastName))
			{
				return "Unknown User";
			}
			return $"{FirstName} {LastName}";
		}
	}
}
