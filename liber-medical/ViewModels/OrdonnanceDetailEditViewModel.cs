using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceDetailEditViewModel : ViewModelBase
    {
        public Ordonnance Ordonnance { get; set; }

        public ObservableCollection<Cotation> cotation { get; set; }

        public OrdonnanceDetailEditViewModel()
        {
            Ordonnance = new Ordonnance
            {
                CreatedAt = DateTime.Today
            };

            cotation = new ObservableCollection<Cotation>
            {
                new Cotation {
                    FirstCode=1,
                    SecondCode= "AMI",
                    ThirdCode= 2
                },
                new Cotation {
                    FirstCode=2,
                    SecondCode= "AMI",
                    ThirdCode= 1
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                }
            };

            MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance, async (sender, patient) => {

                if (patient != null)
                {
                    Ordonnance.PatientId = patient.Id;
                }
            });
        }

        public ICommand SelectPatientCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);
        });
    }
}
