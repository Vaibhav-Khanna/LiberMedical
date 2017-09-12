using System.Threading.Tasks;
using libermedical.Enums;
using libermedical.Request;
using libermedical.Responses;

namespace libermedical.Managers
{
    public interface IRestService<T>
    {
        Task<PaginationResponse<T>> GetAllDataAsync();
        
        Task<PaginationResponse<T>> GetAllDataAsyncWithParameters(int limit, int page, string searchValue = "", string searchFields = "", string sortField = "", SortDirectionEnum direction = SortDirectionEnum.Asc);

        Task<T> GetSingleDataAsync(string id);

        Task<PaginationResponse<T>> GetAdditionalDataAsStringAsync(string otherType, string otherId);

        Task<TokenResponse> GetLoginToken(LoginRequest login);

        Task<TokenResponse> RegenerateLoginToken();

        Task RequestNewPassword(ForgotPasswordRequest request);

        Task<T> SaveItemAsync(T item, string id = "", bool isNewItem = false);

        Task DeleteItemAsync(string id);
    }
}
