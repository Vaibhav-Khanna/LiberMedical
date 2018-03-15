
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace libermedical.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RoleEnum
	{
		[EnumMember(Value ="admin")]
		admin,
		[EnumMember(Value ="nurse")]
		nurse,
		[EnumMember(Value ="employee")]
		employee
	}
}
