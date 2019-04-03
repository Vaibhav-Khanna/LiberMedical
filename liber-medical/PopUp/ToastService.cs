using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;

namespace libermedical.PopUp
{
    public static class ToastService
    {
		
		public static async Task Show(string text)
		{
			var popupPage = new ToastPage(text);

			if (PopupNavigation.Instance.PopupStack.Count > 0)
			{
				await PopupNavigation.Instance.PopAllAsync();
			}

			await PopupNavigation.Instance.PushAsync(popupPage);
		}

    }
}
