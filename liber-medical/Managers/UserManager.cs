using System;
using libermedical.Models;

namespace libermedical.Managers
{
	public class UserManager: DataManager<User>, IUserManager
	{
		public UserManager(IRestService<User> service) : base(service)
        {
		}
	}

	public interface IUserManager : IDataManager<User>
	{

	}
}
