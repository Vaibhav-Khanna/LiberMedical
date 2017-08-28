using System.Windows.Input;
using FreshMvvm;
using Xamarin.Forms;

namespace libermedical.ViewModels.Base
{
	public abstract class ViewModelBase : FreshBasePageModel
	{

		public ICommand  NavBackCommand => new Command(async () => 
			await CoreMethods.PopPageModel(true)); 
	}
}
