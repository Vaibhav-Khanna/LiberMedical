using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;

namespace libermedical.Services
{
	public interface IStorageService<T> where T : BaseDTO
	{
		Task<bool> AddAsync(T unit);
		Task<bool> AddManyAsync(List<T> items);
		Task<IEnumerable<T>> GetList();
		Task SyncTables();
		Task<T> GetItemAsync(string key);
		Task<bool> DeleteItemAsync(string key);
		Task<bool> DeleteAllAsync();
		Task<bool> InvalidateSyncedItems();
        Task<IEnumerable<Ordonnance>> SearchOrdonnance(string query);
	}
}
