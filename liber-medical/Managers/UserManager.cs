using System;
using libermedical.Models;
using System.Threading.Tasks;

namespace libermedical.Managers
{
	public class UserManager: DataManager<User>, IUserManager
	{
		public UserManager(IRestService<User> service) : base(service)
        {
		}

        public async Task<Contract> GetContract()
        {
            var response = await _restService.GetContract();
            return response;
        }
    }

	public interface IUserManager : IDataManager<User>
	{
        Task<Contract> GetContract();   
	}
}
