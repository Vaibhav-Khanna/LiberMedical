using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using libermedical.CustomExceptions;
using libermedical.Enums;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.Request;
using libermedical.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

		public async Task<PaginationResponse<T>> GetAllDataAsyncWithParameters(
			int limit, int page, string searchValue = "", string searchFields = "",
			string sortField = "", SortDirectionEnum direction = SortDirectionEnum.Asc)
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
			if (!string.IsNullOrEmpty(sortField))
			{
                parameters += "&sortField=" + sortField + "="+direction.ToString().ToLower();
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

		public async Task<T> GetSingleDataAsyncCached(string id)
		{
			T result = default(T);
			var cache = BlobCache.UserAccount;
			var key = typeof(T).Name + "_" + id;
			var cachedPostsPromise = cache.GetAndFetchLatest(
				key,
				() => GetSingleDataAsync(id),
				offset =>
				{
                TimeSpan elapsed = DateTimeOffset.UtcNow - offset;
					return elapsed > new TimeSpan(days: 0, hours: 8, minutes: 0, seconds: 0);
				});

			cachedPostsPromise.Subscribe(subscribedPosts =>
			{
				Debug.WriteLine("Subscribed Posts ready");
				result = subscribedPosts;
			});

			result = await cachedPostsPromise.LastOrDefaultAsync();
			return result;
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

		public async Task<TokenResponse> GetLoginToken(LoginRequest login)
		{
			TokenResponse resp = null;

			var uri = new Uri(string.Format(Constants.RestUrl + "login", string.Empty));

			try
			{
				var json = JsonConvert.SerializeObject(login);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _client.PostAsync(uri, content);
				if (response.IsSuccessStatusCode)
				{
					var content2 = await response.Content.ReadAsStringAsync();
					var user = JsonConvert.DeserializeObject<User>(JObject.Parse(content2)["user"].ToString());
					Settings.CurrentUser = JsonConvert.SerializeObject(user);
					resp = JsonConvert.DeserializeObject<TokenResponse>(content2);

					Debug.WriteLine(@"				Item successfully saved.");
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0}", ex.Message);
			}

			return resp;
		}

		public async Task<TokenResponse> RegenerateLoginToken()
		{
			var uri = new Uri(string.Format(Constants.RestUrl + "regenerate" + "?" + _auth));
            var response = await _client.GetAsync(uri).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
					var resp = JsonConvert.DeserializeObject<TokenResponse>(content);
					return resp;
				case HttpStatusCode.BadRequest:
                    return null;
				default:
                    return null;
			}
		}

		public async Task RequestNewPassword(ForgotPasswordRequest request)
		{
			var uri = new Uri(string.Format(Constants.RestUrl + "forgot", string.Empty));

			try
			{
				var json = JsonConvert.SerializeObject(request);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _client.PostAsync(uri, content);
				if (!response.IsSuccessStatusCode)
				{
					throw new BadResponseException("Error retrieving password");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0}", ex.Message);
			}
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

        public async Task<Stream> DownloadBill(string filePath)
        {
            var uri = new Uri(string.Format(filePath));

            try
            {
                var response = await _client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
                return new System.IO.MemoryStream();
            }
            catch (Exception ex)
            {
                return new System.IO.MemoryStream();
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
        }

		public async Task<Stream> DownloadFile(string filePath, bool shouldSave)
		{
			var uri = new Uri(string.Format(Constants.RestUrl + "file?path=" + filePath + "&" + _auth));

			try
			{
				var response = await _client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsStreamAsync();
				}
				return new System.IO.MemoryStream();
			}
			catch (Exception ex)
			{
				return new System.IO.MemoryStream();
				Debug.WriteLine(@"				ERROR {0}", ex.Message);
			}
		}

        public async Task<Contract> GetContract()
        {
            //&sortField=field1=desc
            var uri = new Uri(string.Format(Constants.RestUrl + "contracts?limit=1&page=1"+ "&" + _auth));

            try
            {
                
                var response = await _client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();

                    var con = JsonConvert.DeserializeObject<PaginationResponse<Contract>>(resp);

                    if (con?.rows?.Count != 0)
                    {
                        return con.rows[0];
                    }
                    else
                        return null;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
        }
    }
}
