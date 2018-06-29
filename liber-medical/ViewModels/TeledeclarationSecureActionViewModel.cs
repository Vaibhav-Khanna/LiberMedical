using System;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using libermedical.PopUp;

namespace libermedical.ViewModels
{
	public class TeledeclarationSecureActionViewModel : FreshBasePageModel
	{
        
		private StorageService<Teledeclaration> _teledeclarationService;

		private Teledeclaration _teledeclaration;

		public Teledeclaration Teledeclaration
		{
			get { return _teledeclaration; }
			set
			{
				_teledeclaration = value;
				if (_teledeclaration != null)
                    CanValidate = _teledeclaration.Status == Enums.StatusEnum.waiting.ToString() ? true : false;
				RaisePropertyChanged();
			}
		}

		public TeledeclarationSecureActionViewModel()
		{
			SubscribeMessages();
		}

        public bool CanValidate { get; set; } = false;

		public override void Init(object initData)
		{
			base.Init(initData);

            if (initData is Teledeclaration)
            {
                Teledeclaration = ((Teledeclaration)initData);
            }
		}

		public ICommand ValidateCommand
		{
			get
			{
				return new Command(async (args) =>
				{
                    UserDialogs.Instance.ShowLoading("Chargement...");
                    if (args as string == "Valid")
                        Teledeclaration.Status = Enums.StatusEnum.valid.ToString();
					else
                        Teledeclaration.Status = Enums.@StatusEnum.refused.ToString();

					_teledeclarationService = new StorageService<Teledeclaration>();
					await _teledeclarationService.DeleteItemAsync(typeof(Teledeclaration).Name + "_" + Teledeclaration.Id);
					Teledeclaration.IsSynced = false;
					await _teledeclarationService.AddAsync(Teledeclaration);

                    if (App.IsConnected())
                    {
                        await _teledeclarationService.PushTeledeclaration(Teledeclaration);
                    }
                    //TODO: Display success toast

                    await ToastService.Show("Votre télédéclaration vient d’être envoyée !");

                 

                    await CoreMethods.PopPageModel(true,true);
                    UserDialogs.Instance.HideLoading();
                });
			}
		}


		public ICommand CloseCommand
		{
			get { return new Command(async () => {
                
                await CoreMethods.PopPageModel(true,true); });               
            }
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

        bool isOpening = false;

		public ICommand DocumentViewCommand
		{
			get
			{
				return new Command(async () =>
				{
                    if (isOpening)
                        return;

                    isOpening = true;

					if (Teledeclaration.FilePath.Contains(".pdf"))
						await CoreMethods.PushPageModel<SecuriseBillsViewModel>(Teledeclaration, true);
					else
						await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(Teledeclaration, true);
                    
                    isOpening = false;
				});
			}
		}
	}
}
