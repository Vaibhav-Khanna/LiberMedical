using System.Collections.Generic;
using libermedical.CustomControls;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class FilterPage : BasePage
    {
        private Filter _filter;
		private string _parentScreen;

        public FilterPage(string parentScreen,Filter filter = null) : base(-1, 64, false)
        {
            InitializeComponent();
			_parentScreen = parentScreen;
            if (filter != null)
            {
                StartDatePicker.Date = filter.StartDate;
                EndDatePicker.Date = filter.EndDate;
                if (filter.Statuses.Contains(StatusEnum.Attente))
                {
                    attente.On = true;
                }
                if (filter.Statuses.Contains(StatusEnum.Traite))
                {
                    traite.On = true;
                }
                if (filter.Statuses.Contains(StatusEnum.Refuse))
                {
                    refuse.On = true;
                }
            }
        }

        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Reset_Tapped(object sender, System.EventArgs e)
        {
            _filter = null;

            MessagingCenter.Send(this, Events.UpdatePrescriptionFilters, _filter);

            await Navigation.PopModalAsync();
        }

        async void Search_Tapped(object sender, System.EventArgs e)
        {
            _filter = new Filter
            {
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                Statuses = new List<StatusEnum>()
            };
            if (attente.On)
            {
                _filter.Statuses.Add(StatusEnum.Attente);
            }
            if (traite.On)
            {
                _filter.Statuses.Add(StatusEnum.Traite);
            }
            if (refuse.On)
            {
                _filter.Statuses.Add(StatusEnum.Refuse);
            }

            MessagingCenter.Send(this, Events.UpdatePrescriptionFilters, _filter);
			if(_parentScreen=="Teledeclarations")
			MessagingCenter.Send(this, Events.UpdateTeledeclarationsFilters, _filter);

            await Navigation.PopModalAsync();
        }
    }
}
