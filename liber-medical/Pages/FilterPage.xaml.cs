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
                if (filter.Statuses.Contains(StatusEnum.waiting))
                {
                    attente.On = true;
                }
                if (filter.Statuses.Contains(StatusEnum.valid))
                {
                    traite.On = true;
                }
                if (filter.Statuses.Contains(StatusEnum.refused))
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
                _filter.Statuses.Add(StatusEnum.waiting);
            }
            if (traite.On)
            {
                _filter.Statuses.Add(StatusEnum.valid);
            }
            if (refuse.On)
            {
                _filter.Statuses.Add(StatusEnum.refused);
            }
            if (_filter.Statuses.Count > 1)
                _filter.IsActivated = true;

            MessagingCenter.Send(this, Events.UpdatePrescriptionFilters, _filter);
			if(_parentScreen=="Teledeclarations")
			MessagingCenter.Send(this, Events.UpdateTeledeclarationsFilters, _filter);

            await Navigation.PopModalAsync();
        }
    }
}
