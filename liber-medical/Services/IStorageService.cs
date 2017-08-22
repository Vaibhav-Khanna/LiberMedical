using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;

namespace libermedical.Services
{
	public interface IStorageService<T> where T:BaseDTO
	{
		Task<T> AddAsync(T unit);
		Task<IEnumerable<T>> GetList();
		Task SyncTables();
        Task<T> GetItemAsync(string key);
        Task<bool> DeleteItemAsync(string key);
        Task<bool> DeleteAllAsync();
    }
}