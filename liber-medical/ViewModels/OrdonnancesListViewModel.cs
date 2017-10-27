using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using libermedical.Enums;
using libermedical.Request;
using Plugin.Media;
using Plugin.Media.Abstractions;
using libermedical.Pages;

namespace libermedical.ViewModels
{
	public class OrdonnancesListViewModel : ListViewModelBase<Ordonnance>
	{

        private IStorageService<Ordonnance> _ordonnanceStorage;
		private ObservableCollection<Ordonnance> _ordonnances;
		public ObservableCollection<Ordonnance> Ordonnances
		{
			get { return _ordonnances; }
			set
			{
				_ordonnances = value;
				RaisePropertyChanged();
			}
		}

		public OrdonnancesListViewModel(IStorageService<Ordonnance> storageService) : base(storageService)
		{
			_ordonnanceStorage = storageService;
            BindData();
            DownlaodDocuments();
        }

        public async Task BindData()
		{
           await  new StorageService<Ordonnance>().DownloadOrdonnances();
           Ordonnances = new ObservableCollection<Ordonnance>(await _ordonnanceStorage.GetList());
		}

        private async Task DownlaodDocuments()
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(200, 0);
                var documents =
                    new ObservableCollection<Document>((await App.DocumentsManager.GetListAsync(request)).rows);

                //Updating records in local cache
                await new StorageService<Document>().InvalidateSyncedItems();
                await new StorageService<Document>().AddManyAsync(documents.ToList());
            }
        }
        protected override async Task TapCommandFunc(Cell cell)
		{
			var ctx = cell.BindingContext;
			await CoreMethods.PushPageModel<DetailsPatientListViewModel>(ctx, true);
		}

		public async void CreatePrescription(string filePath)
		{
			await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(filePath, true);
		}

		public ICommand SelectItemCommand => new Command(async (item) =>
		{
			await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(item,true);
        });

		public ICommand AddCommand => new Command(async () =>
		{
			var action =
				await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance rapide",
					"Ordonnance classique");

			string filePath = "";

			if (action != null && action != "Annuler")
			{
				var action2 =
					await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");
				if (action2 != null && action2 != "Annuler")
				{
					await CrossMedia.Current.Initialize();
					if (action2 == "Appareil photo")
					{
						if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
						{
							await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
							return;
						}

						var file = await CrossMedia.Current.TakePhotoAsync(
							new StoreCameraMediaOptions
							{
								AllowCropping = true
							});
						if (file != null)
						{
							filePath = file.Path;
						}
					}
					else if (action2 == "Bibliothèque photo")
					{
						var file = await CrossMedia.Current.PickPhotoAsync();
						if (file != null)
						{
							filePath = file.Path;
						}
					}

					var ordonnance = new Ordonnance
					{
						Id = Guid.NewGuid().ToString(),
						Attachments = new List<string> { filePath },
						Frequencies = new List<Frequency>()
					};

					if (action == "Ordonnance rapide")
					{
						await CoreMethods.PushPageModel<PatientListViewModel>(
							new string[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);

						MessagingCenter.Subscribe<PatientListViewModel, Patient>(this,
							Events.OrdonnancePageSetPatientForOrdonnance, async (sender, patient) =>
							{
								if (patient != null)
								{
									ordonnance.PatientId = patient.Id;

									await new StorageService<Ordonnance>().AddAsync(ordonnance);

									//display toast
									//pop to root
									Ordonnances = new ObservableCollection<Ordonnance>(await _ordonnanceStorage.GetList());
								}
							});
					}
					else if (action == "Ordonnance classique")
					{
						CreatePrescription(filePath);
					}
				}
			}
		});

		protected override async void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
            Ordonnances = new ObservableCollection<Ordonnance>(await _ordonnanceStorage.GetList());
        }
	}
}
