using Microsoft.WindowsAzure.Mobile.Service.Tables;

namespace liber_medical.Server.DataObjects
{
	public class Patient: libermedical.DTO.Models.Patient, ITableData
	{
		public byte[] Version { get; set; }
	}
}