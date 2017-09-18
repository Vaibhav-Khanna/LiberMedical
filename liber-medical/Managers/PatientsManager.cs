using libermedical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libermedical.Managers
{
    public class PatientsManager : DataManager<Patient>, IPatientsManager
    {
        public PatientsManager(IRestService<Patient> service) : base(service)
        {
        }
    }

    public interface IPatientsManager : IDataManager<Patient>
    {

    }
}
