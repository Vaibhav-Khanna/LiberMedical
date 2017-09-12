using libermedical.Models;

namespace libermedical.Managers
{
    public class OrdonnanceManager : DataManager<Ordonnance>, IOrdonnanceManager
    {
        public OrdonnanceManager(IRestService<Ordonnance> service) : base(service)
        {
        }
    }
}
