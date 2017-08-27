using System.Windows.Input;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace libermedical.ViewModels.Base
{
	[AddINotifyPropertyChangedInterface] // uses fody for property changed
	public abstract class ViewModelBase : FreshBasePageModel
	{

		public ICommand  NavBackCommand => new Command(async () => 
			await CoreMethods.PopPageModel(true)); 
	}
}
