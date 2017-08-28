using System.Text.RegularExpressions;

namespace libermedical.Utility
{
	public static class EmailValidator
	{
		private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();

		/// <summary>
		///     Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
		/// </summary>
		/// <returns></returns>
		private static Regex CreateValidEmailRegex()
		{
			var validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
			                        + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
			                        + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

			return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
		}

		internal static bool EmailIsValid(string emailAddress)
		{
			var isValid = ValidEmailRegex.IsMatch(emailAddress);

			return isValid;
		}
	}
}