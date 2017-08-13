using FreshMvvm;
using PropertyChanged;

namespace libermedical.ViewModels.Base
{
	[AddINotifyPropertyChangedInterface] // uses fody for property changed
	public abstract class ViewModelBase : FreshBasePageModel
	{
	}
}
