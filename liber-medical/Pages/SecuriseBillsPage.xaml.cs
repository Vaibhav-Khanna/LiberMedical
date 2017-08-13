using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class SecuriseBillsPage : BasePage
    {
        public ObservableCollection<Teledeclaration> teledeclarations { get; set; }
        public SecuriseBillsPage() : base(-1, 0, false)
        {
            BindingContext = this;
            teledeclarations = new ObservableCollection<Teledeclaration>
            {
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 11, 08),
                    TotalAccount= 67.64,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 05, 24),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2016, 07, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 02, 11),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 03, 30),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 10, 22),
                    TotalAccount= 12.64,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 08, 02),
                    TotalAccount= 145.32,
                    Status = "Traité"
                }
            };


            InitializeComponent();
        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Bill_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new TeledeclarationSecureActionPage());
        }
    }
}
