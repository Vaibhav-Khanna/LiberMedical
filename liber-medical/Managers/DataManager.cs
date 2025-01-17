﻿using System;
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
            var res = new PaginationResponse<TModel>()
            { 
                rows = new System.Collections.Generic.List<TModel>()
            };
           
            if(Device.RuntimePlatform== Device.Android)
            {
                res.rows = new System.Collections.Generic.List<TModel>();
            }

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
                if (!_firstAuthFail) 
                    return res;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                {
                   res = await GetListAsync(request);
                }
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
                {
                    res = await GetListAsync();
                }
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
                res = await _restService.GetSingleDataAsync(id);
            }
            catch (UnauthorizedAccessException e)
            {
                if (!_firstAuthFail) return res;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                { 
                    res = await GetAsync(id);
                }
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
                {
                    res = await _restService.SaveItemAsync(model, id, isNew);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
            }

            return res;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                await _restService.DeleteItemAsync(id);
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                if (!_firstAuthFail) return false;

                _firstAuthFail = !_firstAuthFail;

                if (await RegenerateLoginToken())
                {
                    await _restService.DeleteItemAsync(id);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"				ERROR {0}", e.Message);
                return false;
            }

            return false;
        }

        bool isOpening = false;

        private async Task<bool> RegenerateLoginToken()
        {
            if (isOpening)
                return false;

            isOpening = true;

            try
            {
                var newToken = await _restService.RegenerateLoginToken().ConfigureAwait(false);

                if (newToken != null)
                {
                    Settings.Token = newToken.token;
                    Settings.TokenExpiration = newToken.tokenExpiration;
                    isOpening = false;
                    return true;
                }
                else
                {
                    Settings.Token = string.Empty;
                    Settings.TokenExpiration = 0;
                    Settings.IsLoggedIn = false;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        App.MoveToLogin();
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new NavigationPage(new LoginPage());
                        });
                    }

                    isOpening = false;
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Token has been expired and this is the error message : " + ex.Message);

                Settings.Token = string.Empty;
                Settings.TokenExpiration = 0;
                Settings.IsLoggedIn = false;

                if (Device.RuntimePlatform == Device.Android)
                {
                    App.MoveToLogin();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                    });
                }

                isOpening = false;
                return false;
            }
        }
    }
}
