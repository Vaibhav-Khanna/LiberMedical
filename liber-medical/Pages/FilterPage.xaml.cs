using System;
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

		public FilterPage(string parentScreen, Filter filter = null) : base(-1, 64, false)
		{
			InitializeComponent();
			_parentScreen = parentScreen;
			if (filter != null)
			{
				_filter = filter;				
				if (filter.EnableDateSearch)
				{
                    StartDatePicker.Date = filter.StartDate.Value;
                    EndDatePicker.Date = filter.EndDate.Value;
                    EndDate.Text = _filter.EndDate.Value.ToString("MM-dd-yyyy");
					StartDate.Text = _filter.StartDate.Value.ToString("MM-dd-yyyy");
				}
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
			else
			{
				_filter = new Filter
				{
					StartDate = StartDatePicker.Date,
					EndDate = EndDatePicker.Date,
					Statuses = new List<StatusEnum>()
				};
			}
		}

		async void Back_Tapped(object sender, System.EventArgs e)
		{
			_filter = null;
			MessagingCenter.Send(this, Events.UpdatePrescriptionFilters, _filter);
			await Navigation.PopModalAsync();
		}

		async void Reset_Tapped(object sender, System.EventArgs e)
		{
			attente.On = traite.On = refuse.On = false;
			StartDate.Text = EndDate.Text = string.Empty;
            _filter = null;

        }

		void HandleEndDateTapped(object sender, System.EventArgs e)
		{
			EndDatePicker.Focus();
		}

		public void Handle_EndDateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
		{
			_filter.IsActivated = true;
			_filter.EnableDateSearch = true;
			_filter.EndDate = ((DatePicker)sender).Date;
			EndDate.Text = _filter.EndDate.Value.ToString("MM-dd-yyyy");
		}

		void EndDateUnfocused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			_filter.IsActivated = true;
			_filter.EnableDateSearch = true;
			_filter.EndDate = ((DatePicker)sender).Date;
			EndDate.Text = _filter.EndDate.Value.ToString("MM-dd-yyyy");
		}

		public void Handle_StartDateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
		{
			_filter.IsActivated = true;
			_filter.EnableDateSearch = true;
			_filter.StartDate = ((DatePicker)sender).Date;
		}

		void StartDateUnfocused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			_filter.IsActivated = true;
			_filter.EnableDateSearch = true;
			_filter.StartDate = ((DatePicker)sender).Date;
			StartDate.Text = _filter.StartDate.Value.ToString("MM-dd-yyyy");
		}

		void HandleStartDateTapped(object sender, System.EventArgs e)
		{
			StartDatePicker.Focus();
		}
		async void Search_Tapped(object sender, System.EventArgs e)
		{
            if (_filter!=null)
            {
                _filter.Statuses = new List<StatusEnum>();

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
                if (_filter.Statuses.Count >= 1)
                    _filter.IsActivated = true;
                else
                    _filter.IsActivated = false; 
            }

			if (_parentScreen == "Teledeclarations")
				MessagingCenter.Send(this, Events.UpdateTeledeclarationsFilters, _filter);
            else
                MessagingCenter.Send(this, Events.UpdatePrescriptionFilters, _filter);


            await Navigation.PopModalAsync();
		}
	}
}
