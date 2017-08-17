using System;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using libermedical.Utility;
using Plugin.Media;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class OrdonnancesListPage : BasePage
    {
        public ObservableCollection<Ordonnance> ordonnances { get; set; }

        public OrdonnancesListPage() : base(0, 0)
        {

            BindingContext = this;
            ordonnances = new ObservableCollection<Ordonnance>
            {
                new Ordonnance {
                    Reference= 1,
                    Patient= new Patient {FirstName= "Fred",LastName="Pearson"},
                    AddDate= new DateTime(2017, 04, 02),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 2,
                    Patient= new Patient {FirstName= "Jonanthan",LastName="Vaughn"},
                    AddDate= new DateTime(2017, 11, 10),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 3,
                    Patient= new Patient {FirstName= "Alexander",LastName="Zimmerman"},
                    AddDate= new DateTime(2017, 02, 06),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 4,
                    Patient= new Patient {FirstName= "Keith",LastName="Kelley"},
                    AddDate= new DateTime(2017, 09, 17),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 5,
                    Patient= new Patient {FirstName= "Justin",LastName="Sims"},
                    AddDate= new DateTime(2017, 03, 04),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 6,
                    Patient= new Patient {FirstName= "Franklin",LastName="Howard"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 7,
                    Patient= new Patient {FirstName= "Franklin",LastName="Howard"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 8,
                    Patient= new Patient {FirstName= "Mickael",LastName="Green"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 9,
                    Patient= new Patient {FirstName= "Paul",LastName="Howard"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 10,
                    Patient= new Patient {FirstName= "Justin",LastName="Howard"},
                    AddDate= new DateTime(2017, 10, 29),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 11,
                    Patient= new Patient {FirstName= "John",LastName="Obama"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "En attente"
                }
            };


            InitializeComponent();
        }


        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new OrdonnanceDetailPage()));
        }

        async void Filter_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new FilterPage());
        }
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        async void Add_Clicked(object sender, System.EventArgs e)
        {

            var action = await DisplayActionSheet(null, "Annuler", null, "Ordonnance rapide", "Ordonnance classique");
            var typeDoc = "ordonnance";
            string typeNavigation = "";

            if ((action != null) && (action != "Annuler"))
            {
                //init du plugin photo 
                await CrossMedia.Current.Initialize();

                if (UtilityClass.CameraAvailable())
                {

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                    {
                        AllowCropping = true
                    });

                    if (file != null)
                    {

                        var profilePicture = ImageSource.FromStream(() => file.GetStream());

                        if (action == "Ordonnance rapide") { typeNavigation = "fast"; }
                        if (action == "Ordonnance classique") { typeNavigation = "normal"; }

                        await Navigation.PushModalAsync(new NavigationPage(new PatientListPage(typeNavigation, typeDoc)));

                    }
                }
                else
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                }
            }
            else
            {
                return;
            }
        }
    }
}
