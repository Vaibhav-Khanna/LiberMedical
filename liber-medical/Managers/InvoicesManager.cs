using libermedical.Models;

namespace libermedical.Managers
{
    public class InvoicesManager : DataManager<Invoice>, IInvoicesManager
    {
        public InvoicesManager(IRestService<Invoice> service) : base(service)
        {
        }
    }

    public interface IInvoicesManager : IDataManager<Invoice>
    {

    }
}
