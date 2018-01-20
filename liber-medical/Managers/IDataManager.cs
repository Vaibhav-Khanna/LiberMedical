using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using libermedical.Request;
using libermedical.Responses;

namespace libermedical.Managers
{
    public interface IDataManager<TModel> where TModel : class, new()
    {
        Task<PaginationResponse<TModel>> GetListAsync(GetListRequest request);
        
        Task<PaginationResponse<TModel>> GetListAsync();

        Task<TModel> GetAsync(string id);

        Task<TModel> SaveOrUpdateAsync(string id, TModel model, bool isNew = false);

        Task<bool> DeleteItemAsync(string id);
    }
}
