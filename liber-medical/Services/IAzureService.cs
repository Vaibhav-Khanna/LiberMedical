using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.DTO.Models;

namespace libermedical.Services
{
	public interface IAzureService<T> where T:BaseDTO
	{
		Task<T> AddAsync(T unit);
		Task<IEnumerable<T>> GetList();
		Task SyncTables();
	}
}