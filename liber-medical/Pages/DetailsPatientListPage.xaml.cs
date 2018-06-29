using System;
using System.Collections.ObjectModel;
using libermedical.Enums;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class DetailsPatientListPage
    {
        public DetailsPatientListPage() : base(-1, 64, false)
        {
            InitializeComponent();

        }

        // SWipe to delete the document item
        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            var item = (sender as MenuItem).CommandParameter as Document;

            if (item != null && item.Status != DocumentStatusEnum.valid.ToString())
            {
                (BindingContext as DetailsPatientListViewModel).DeleteDocument.Execute(item.Id);
            }
        }

        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Edit_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PatientDetailModify());
        }

        void Handle_ItemSelected1(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (stackOrdonnance.SelectedItem != null)
                stackOrdonnance.SelectedItem = null;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var item = (sender as MenuItem).CommandParameter as Ordonnance;

            if (item != null)
            {
                if (item.Status != Enums.StatusEnum.valid.ToString())
                {
                    (BindingContext as DetailsPatientListViewModel).DeleteOrdo.Execute(item.Id);
                }
            }
        }

        void Handle_BindingContextChanged(object sender, System.EventArgs e)
        {
            var item = sender as ViewCell;
            var context = item.BindingContext as Ordonnance;

            if (item != null && context != null)
            {
                if (context.Status == StatusEnum.valid.ToString())
                {
                    item.ContextActions.Clear();
                }
            }
        }
    }
}
