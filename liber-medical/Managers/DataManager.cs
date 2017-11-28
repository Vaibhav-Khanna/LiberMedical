using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;
using libermedical.Helpers;
using libermedical.Pages;
using libermedical.Request;
using libermedical.Responses;
using Xamarin.Forms;

namespace libermedical.Managers
{
    public class DataManager<TModel> : IDataManager<TModel> where TModel : class, new()
    {
        public IRestService<TModel> _restService;
        internal bool _firstAuthFail = true;

        public DataManager(IRestService<TModel> service)
        {
            _restService = service;
        }

        public virtual async Task<PaginationResponse<TModel>> GetListAsync(GetListRequest request)
        {
            var res = new PaginationResponse<TModel>();
            try
            {
                res = (await _restService.GetAllDataAsyncWithParameters(
                    request.Limit,
                    request.Page,
                    request.SearchValue,
                    request.SearchFields,
                    request.SortField,
                    request.SortDirection));
            }
            catch (UnauthorizedAccessException)
            {
                if (!_firstAuthFail) return res;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                    await GetListAsync(request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
            }

            return res;
        }
        
        public async Task<PaginationResponse<TModel>> GetListAsync()
        {
            var res = new PaginationResponse<TModel>();
            try
            {
                res = await _restService.GetAllDataAsync();
            }
            catch (UnauthorizedAccessException e)
            {
                if (!_firstAuthFail) return res;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                    await GetListAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
            }

            return res;
        }

        public async Task<TModel> GetAsync(string id)
        {
            TModel res = null;
            try
            {
                res = await _restService.GetSingleDataAsyncCached(id);
            }
            catch (UnauthorizedAccessException e)
            {
                if (!_firstAuthFail) return res;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                    await GetAsync(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
            }

            return res;
        }

        public async Task<TModel> SaveOrUpdateAsync(string id, TModel model, bool isNew = false)
        {
            TModel res = null;
            try
            {
                res = await _restService.SaveItemAsync(model, id, isNew);
            }
            catch (UnauthorizedAccessException e)
            {
                if (!_firstAuthFail) return res;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                    await _restService.SaveItemAsync(model, id, isNew);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
            }

            return res;
        }

        public async Task DeleteItemAsync(string id)
        {

            try
            {
                await _restService.DeleteItemAsync(id);
            }
            catch (UnauthorizedAccessException e)
            {
                if (!_firstAuthFail) return;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                    await _restService.DeleteItemAsync(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
            }

        }

        private async Task<bool> RegenerateLoginToken()
        {
           
                try
                {
                    var newToken = await _restService.RegenerateLoginToken().ConfigureAwait(false);
                    Settings.Token = newToken.token;
                    Settings.TokenExpiration = newToken.tokenExpiration;
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Token has been expired and this is the error message : " + ex.Message);
                 
                // Settings.Token = string.Empty;
                   // Settings.TokenExpiration = 0;
                   // Settings.IsLoggedIn = false;

                   // Device.BeginInvokeOnMainThread(() =>
                   //{
                   //    Application.Current.MainPage = new NavigationPage(new LoginPage());
                   //});

                    return false;
                }
        }
    }
}
