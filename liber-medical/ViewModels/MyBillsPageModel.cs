using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class MyBillsPageModel : ViewModelBase
    {

        public bool NoBillsFound { get; set; } = false;

        public ObservableCollection<Invoice> Bills { get; set; } = new ObservableCollection<Invoice>();

        public bool IsLoading { get; set; } = true;


        public Command BackCommand => new Command(async() =>
       {
            await CoreMethods.PopPageModel(false, true);
       });

        public MyBillsPageModel()
        {
            GetBills();
        }


        public Command OpenBill => new Command(async (obj) =>
        {
            if (((Invoice)obj).FilePath.Contains(".pdf"))
                await CoreMethods.PushPageModel<SecuriseBillsViewModel>(((Invoice)obj), true);
            else
                await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(((Invoice)obj), true);
        });


        async Task GetBills()
        {
            var list_response = await App.BillsManager.GetListAsync(new Request.GetListRequest(100, 0, sortDirection: Enums.SortDirectionEnum.Desc));

            IsLoading = false;

            if (list_response != null && list_response.rows != null && list_response.rows.Any())
            {
                NoBillsFound = false;
                Bills = new ObservableCollection<Invoice>(list_response.rows);
            }
            else
            {
                NoBillsFound = true;
            }
        }
    }
}
