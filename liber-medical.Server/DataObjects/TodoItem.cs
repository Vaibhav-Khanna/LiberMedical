using Microsoft.WindowsAzure.Mobile.Service;

namespace liber_medical.Server.DataObjects
{
	public class TodoItem : EntityData
	{
		public string Text { get; set; }

		public bool Complete { get; set; }
	}
}