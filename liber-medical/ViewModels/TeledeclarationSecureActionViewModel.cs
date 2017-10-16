using System;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class TeledeclarationSecureActionViewModel : FreshBasePageModel
	{
		private IStorageService<Teledeclaration> _teledeclarationService;

		private Teledeclaration _teledeclaration;

		public Teledeclaration Teledeclaration
		{
			get { return _teledeclaration; }
			set
			{
				_teledeclaration = value;
				if (_teledeclaration != null)
					CanValidate = _teledeclaration.Status == Enums.StatusEnum.waiting ? true : false;
				RaisePropertyChanged();
			}
		}
		public TeledeclarationSecureActionViewModel()
		{
			SubscribeMessages();
		}
		public bool CanValidate { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);
		}

		public ICommand ValidateCommand
		{
			get
			{
				return new Command(async (args) =>
				{

					if (args as string == "Valid")
						Teledeclaration.Status = Enums.StatusEnum.valid;
					else
						Teledeclaration.Status = Enums.StatusEnum.refused;

					_teledeclarationService = new StorageService<Teledeclaration>();
					await _teledeclarationService.DeleteItemAsync(typeof(Teledeclaration).Name + "_" + Teledeclaration.Id);
					Teledeclaration.IsSynced = false;
					await _teledeclarationService.AddAsync(Teledeclaration);

					//TODO: Display success toast

					await CoreMethods.PopPageModel();
				});
			}
		}

		public ICommand CloseCommand
		{
			get { return new Command(async () => { await CoreMethods.PopPageModel(); }); }
		}

		private void SubscribeMessages()
		{
			MessagingCenter.Subscribe<TeledeclarationsListViewModel, Teledeclaration>(this, Events.UpdateTeledeclarationDetailsPage,
				(sender, teledeclaration) =>
				{
					if (teledeclaration != null)
					{
						Teledeclaration = teledeclaration;
					}
				});
		}

		public ICommand DocumentViewCommand
		{
			get
			{
				return new Command(async () =>
				{
					if (Teledeclaration.FilePath.Contains(".pdf"))
						await CoreMethods.PushPageModel<SecuriseBillsViewModel>(Teledeclaration, true);
					else
						await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(Teledeclaration, true);
				});
			}
		}
	}
}
