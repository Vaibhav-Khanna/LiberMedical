using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using libermedical.Helpers;
using libermedical.Responses;
using Newtonsoft.Json;

namespace libermedical.Managers
{
    public class RestService<T> : IRestService<T>
    {
        private readonly HttpClient _client;
        private readonly string _type;
        private static string _auth => string.Concat("token=", Settings.Token);

        public PaginationResponse<T> Items { get; private set; }

        public RestService(string type)
        {
            _type = type;
            _client = new HttpClient();
        }

        public async Task<PaginationResponse<T>> GetAllDataAsync()
        {
            Items = new PaginationResponse<T>();

            var uri = new Uri(string.Format(Constants.RestUrl + _type + "?" + _auth, string.Empty));

            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Items = JsonConvert.DeserializeObject<PaginationResponse<T>>(content);
                    break;
                case HttpStatusCode.BadRequest:
                    var r = JsonConvert.DeserializeObject<ErrorResponse>(content);
                    if (r.Error == "Unauthorized")
                        throw new UnauthorizedAccessException(r.Error);
                    else
                        throw new Exception(r.Error);
                case HttpStatusCode.InternalServerError:
                    throw new Exception();
                default:
                    throw new Exception();
            }

            return Items;
        }

        public async Task<PaginationResponse<T>> GetAllDataAsyncWithParameters(int limit, int page, string searchValue = "", string searchFields = "")
        {
            var parameters = "?limit=" + limit + "&page=" + page;
            if (!string.IsNullOrEmpty(searchValue))
            {
                parameters += "&searchValue=" + searchValue;
            }
            if (!string.IsNullOrEmpty(searchFields))
            {
                parameters += "&searchFields=" + searchFields;
            }

            parameters += "&" + _auth;

            var uri = new Uri(string.Format(Constants.RestUrl + _type + parameters, string.Empty));
            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = JsonConvert.DeserializeObject<PaginationResponse<T>>(content);
                    return result;
                case HttpStatusCode.BadRequest:
                    var r = JsonConvert.DeserializeObject<ErrorResponse>(content);
                    if (r.Error == "Unauthorized")
                        throw new UnauthorizedAccessException(r.Error);
                    else
                        throw new Exception(r.Error);
                case HttpStatusCode.InternalServerError:
                    throw new Exception();
                default:
                    throw new Exception();
            }
        }

        public async Task<T> GetSingleDataAsync(string id)
        {
            T item = default(T);

            var uri = new Uri(string.Format(Constants.RestUrl + _type + "/" + id + "?" + _auth, string.Empty));
            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = JsonConvert.DeserializeObject<T>(content);
                    return result;
                case HttpStatusCode.BadRequest:
                    var r = JsonConvert.DeserializeObject<ErrorResponse>(content);
                    if (r.Error == "Unauthorized")
                        throw new UnauthorizedAccessException(r.Error);
                    else
                        throw new Exception(r.Error);
                case HttpStatusCode.InternalServerError:
                    throw new Exception();
                default:
                    throw new Exception();
            }
        }

        public async Task<PaginationResponse<T>> GetAdditionalDataAsStringAsync(string otherType, string otherId)
        {
            Items = new PaginationResponse<T>();

            var uri = new Uri(string.Format(
                Constants.RestUrl + _type + "?" + otherType + "=" + otherId + "&" + _auth, string.Empty));

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<PaginationResponse<T>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Items;
        }

        public async Task<T> SaveItemAsync(T item, string id = "", bool isNewItem = false)
        {
            T resp = default(T);

            var uri = isNewItem
                ? new Uri(string.Format(Constants.RestUrl + _type + "?" + _auth, string.Empty))
                : new Uri(string.Format(Constants.RestUrl + _type + "/" + id + "?" + _auth));

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (isNewItem)
                {
                    response = await _client.PostAsync(uri, content);
                }
                else
                {
                    response = await _client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    var content2 = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<T>(content2);
                    Debug.WriteLine(@"				Item successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return resp;
        }

        public async Task DeleteItemAsync(string id)
        {
            var uri = new Uri(string.Format(Constants.RestUrl + _type + "/" + id + "?" + _auth));

            try
            {
                var response = await _client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				Item successfully deleted.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }
    }
}
