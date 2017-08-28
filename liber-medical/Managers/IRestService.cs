using System.Threading.Tasks;
using libermedical.Responses;

namespace libermedical.Managers
{
    public interface IRestService<T>
    {
        Task<PaginationResponse<T>> GetAllDataAsync();

        Task<PaginationResponse<T>> GetAllDataAsyncWithParameters(int limit, int page, string searchValue = "", string searchFields = "");
        
        Task<T> GetSingleDataAsync(string id);

        Task<PaginationResponse<T>> GetAdditionalDataAsStringAsync(string otherType, string otherId);
        
        Task<T> SaveItemAsync(T item, string id = "", bool isNewItem = false);

        Task DeleteItemAsync(string id);
    }
}
