using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using libermedical.DTO.Models;
using libermedical.DTO.Requests;
using libermedical.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace libermedical.Services
{
	public class AzureService<TModel> : IAzureService<TModel> where TModel : BaseDTO
	{

		public MobileServiceClient Client { get; set; }
		IMobileServiceSyncTable<TModel> coffeeTable;
		public static bool UseAuth { get; set; } = false;

		public async Task Initialize()
		{
			if (Client?.SyncContext?.IsInitialized ?? false)
				return;


			var appUrl = "https://YOUR-BACKEND-URL-HERE.azurewebsites.net";

#if AUTH
            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace (Settings.AuthToken) && !string.IsNullOrWhiteSpace (Settings.UserId)) {
                Client.CurrentUser = new MobileServiceUser (Settings.UserId);
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
#else
			//Create our client

			Client = new MobileServiceClient(appUrl);

#endif

			//InitialzeDatabase for path
			var path = "syncstore.db";
			path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

			//setup our local sqlite store and intialize our table
			var store = new MobileServiceSQLiteStore(path);

			//Define table
			store.DefineTable<TModel>();


			//Initialize SyncContext
			try
			{
				await Client.SyncContext.InitializeAsync(store);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			//Get our sync table that will call out to azure
			coffeeTable = Client.GetSyncTable<TModel>();


		}

		public async Task SyncTables()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
					return;

				await coffeeTable.PullAsync("allCoffee", coffeeTable.CreateQuery());

				await Client.SyncContext.PushAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
			}

		}

		public async Task<IEnumerable<TModel>> GetList()
		{
			//Initialize & Sync
			await Initialize();
			await SyncTables();

			return await coffeeTable.OrderBy(c => c.CreatedAt).ToEnumerableAsync();
		}

		public async Task<TModel> AddAsync(TModel unit)
		{
			await Initialize();

			unit.CreatedAt = DateTime.UtcNow;
			await coffeeTable.InsertAsync(unit);

			await SyncTables();
			return unit;
		}



		public async Task<bool> LoginAsync()
		{

			await Initialize();

			var user = await Client.InvokeApiAsync<LoginRequest, MobileServiceUser>(
					"CustomLogin", new LoginRequest()
					{
						Username = "test",
						Password = "test"
					});


			if (user == null)
			{
				Settings.AuthToken = string.Empty;
				Settings.UserId = string.Empty;
				Device.BeginInvokeOnMainThread(async () =>
				{
					await Application.Current.MainPage.DisplayAlert("Login Error", "Unable to login, please try again", "OK");
				});
				return false;
			}
			else
			{
				Settings.AuthToken = user.MobileServiceAuthenticationToken;
				Settings.UserId = user.UserId;
			}

			return true;
		}

	}
}