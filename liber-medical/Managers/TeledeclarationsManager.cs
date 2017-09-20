using System;
using libermedical.Models;

namespace libermedical.Managers
{
	public class TeledeclarationsManager : DataManager<Teledeclaration>, ITeledeclarationsManager
	{
		public TeledeclarationsManager(IRestService<Teledeclaration> service) : base(service)
		{
		}
	}
	public interface ITeledeclarationsManager : IDataManager<Teledeclaration>
	{

	}

}
