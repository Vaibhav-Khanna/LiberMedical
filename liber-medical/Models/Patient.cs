﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace libermedical.Models
{
	public class Patient:BaseDTO
	{

		public string FirstName { set; get; }

		public string LastName { set; get; }

		public List<PhoneNumber> PhoneNumbers { get; set; }

		[JsonIgnore]
		public string FullName => this.ToString();

		public override string ToString()
		{
			if (string.IsNullOrEmpty(FirstName + LastName))
			{
				return "Unknown User";
			}
			return $"{FirstName} {LastName}";
		}
	}
}
