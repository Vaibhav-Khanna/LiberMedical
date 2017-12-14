using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;
using Akavache;
using System.Reactive.Linq;
using libermedical.Managers;
using System.Diagnostics;
using System.IO;
using System.Linq;
using libermedical.Request;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections;

namespace libermedical.Services
{

	public class StorageService<TModel> : IStorageService<TModel> where TModel : BaseDTO, new()
	{
		public async Task<bool> AddAsync(TModel item)
		{
			try
			{
				var key = typeof(TModel).Name + "_" + item.Id;
				await BlobCache.UserAccount.InsertObject(key, item);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> AddManyAsync(List<TModel> items)
		{
			try
			{
				var dic = new Dictionary<string, TModel>();

                var list_keys = await GetKeys();

				foreach (var item in items)
				{
					var key = typeof(TModel).Name + "_" + item.Id;
					item.IsSynced = true;

                    if(!list_keys.Contains(key))
					dic.Add(key, item);                   
				}

				await BlobCache.UserAccount.InsertObjects(dic);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public async Task<bool> DeleteAllAsync()
		{
			try
			{
				await BlobCache.UserAccount.InvalidateAllObjects<TModel>();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// The structure of Key: typeof(TModel).Name + "_" + item.Id
		public async Task<bool> DeleteItemAsync(string key)
		{
			try
			{
				await BlobCache.UserAccount.Invalidate(key);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// The structure of Key: typeof(TModel).Name + "_" + item.Id
		public async Task<TModel> GetItemAsync(string key)
		{
			try
			{
				return await BlobCache.UserAccount.GetObject<TModel>(key);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<IEnumerable<TModel>> GetList()
		{
			try
			{
				return await BlobCache.UserAccount.GetAllObjects<TModel>();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<bool> InvalidateSyncedItems()
		{
			try
			{
				var items = (await BlobCache.UserAccount.GetAllObjects<TModel>()).ToObservable().Where(x => x.IsSynced != false).ToEnumerable();
				System.Diagnostics.Debug.WriteLine($"{typeof(TModel).Name} count is {items.ToObservable().Count()}");
				foreach (var item in items)
				{
					var key = typeof(TModel).Name + "_" + item.Id;
					await BlobCache.UserAccount.Invalidate(key);
				}
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

        bool isSyncing = false;

		public async Task SyncTables()
		{
            if (isSyncing)
                return;

            isSyncing = true;

			await SyncPatients();
			await SyncOrdonnances();
			await SyncDocuments();
            await SyncTeledeclarations();

            isSyncing = false;
		}

        public async Task SyncTeledeclarations()
        {
            try
            {

                var items = (await BlobCache.UserAccount.GetAllObjects<Teledeclaration>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
                foreach (var item in items)
                {
                    await PushTeledeclaration(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

		public async Task SyncPatients()
		{
			try
			{

				var items = (await BlobCache.UserAccount.GetAllObjects<Patient>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
				foreach (var item in items)
				{
                    await PushPatient(item, item.UpdatedAt == null ? true : false );
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

		public async Task SyncDocuments()
		{
			try
			{

				var items = (await BlobCache.UserAccount.GetAllObjects<Document>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
				foreach (var item in items)
				{
                    await PushDocument(item, item.UpdatedAt == null ? true : false);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}

		}

        public async Task PushTeledeclaration(Teledeclaration teledeclaration)
        {
            try
            {

                var localId = teledeclaration.Id;
                var tele = await App.TeledeclarationsManager.SaveOrUpdateAsync(teledeclaration.Id, teledeclaration, false);

                if (tele != null)
                {
                    tele.IsSynced = true;
                    tele.UpdatedAt = DateTime.UtcNow;
                    await UpdateAsync(tele as TModel, typeof(Teledeclaration).Name + "_" + localId);
                }
                else
                {
                    await UpdateAsync(teledeclaration as TModel, typeof(Teledeclaration).Name + "_" + teledeclaration.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public async Task PushPatient(Patient patientObject, bool isNew)
        {
            try
            {

                var localId = patientObject.Id;
                var patient = await App.PatientsManager.SaveOrUpdateAsync(patientObject.Id, patientObject, patientObject.UpdatedAt == null);
                if (patient != null)
                {
                    patient.IsSynced = true;
                    patient.UpdatedAt = DateTime.UtcNow;
                    await UpdateAsync(patient as TModel, typeof(Patient).Name + "_" + localId);
                    var patientDocuments = (await BlobCache.UserAccount.GetAllObjects<Document>()).ToObservable().Where(x => x.PatientId == localId).ToEnumerable();
                    foreach (var document in patientDocuments.Where(x => x.IsSynced == false))
                    {
                        document.Patient = patient;
                        document.PatientId = patient.Id;
                        await PushDocument(document, document.UpdatedAt == null ? true : false);
                    }

                    var patientOrdonnances = (await BlobCache.UserAccount.GetAllObjects<Ordonnance>()).ToObservable().Where(x => x.PatientId == localId).ToEnumerable();
                    foreach (var ordonnance in patientOrdonnances.Where(x => x.IsSynced == false))
                    {
                        ordonnance.Patient = patient;
                        ordonnance.PatientId = patient.Id;
                        await PushOrdonnance(ordonnance, ordonnance.UpdatedAt == null ? true : false);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task PushDocument(Document document, bool isNew)
        {
            try
            {
                if (!document.AttachmentPath.StartsWith("PatientDocuments") && !document.AttachmentPath.StartsWith("Ordonnance"))
                {

                    var res = await FileUpload.UploadFile(document.AttachmentPath, "PatientDocuments", document.Id);
                    if (res)
                        document.AttachmentPath = $"PatientDocuments/{document.Id}/{Path.GetFileName(document.AttachmentPath)}";
                }

                var localId = document.Id;
                var doc = await App.DocumentsManager.SaveOrUpdateAsync(document.Id, document, document.UpdatedAt == null);

                if (doc != null)
                {
                    doc.IsSynced = true;
                    doc.UpdatedAt = DateTime.UtcNow;
                    await DeleteItemAsync(typeof(Document).Name + "_" + localId);
                    await UpdateAsync(doc as TModel, typeof(Document).Name + "_" + document.Id);
                }
                else
                {
                    await UpdateAsync(document as TModel, typeof(Document).Name + "_" + localId);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task PushOrdonnance(Ordonnance ordonnanceObject, bool isNew)
        {
            try
            {
                var localId = ordonnanceObject.Id;            
                var ordonnance = await App.OrdonnanceManager.SaveOrUpdateAsync(ordonnanceObject.Id, ordonnanceObject, ordonnanceObject.UpdatedAt == null );
            
                if (ordonnance != null)
                {
                    ordonnance.UpdatedAt = DateTime.UtcNow;

                    await DeleteItemAsync(typeof(Ordonnance).Name + "_" + localId);
                    await UpdateAsync(ordonnance as TModel, typeof(Ordonnance).Name + "_" + ordonnance.Id);

                    var attachments = new Dictionary<string, string>();

                    foreach (var attachment in ordonnance.Attachments)
                    {
                        if (!attachment.StartsWith("PatientDocuments") && !attachment.StartsWith("Ordonnance"))
                        {
                            var res = await FileUpload.UploadFile(attachment, "Ordonnance", ordonnance.Id);
                            if (res)
                                attachments.Add(attachment, $"Ordonnance/{ordonnance.Id}/{Path.GetFileName(attachment)}");
                        }
                    }

                    if (attachments.Keys.Count > 0)
                        foreach (var key in attachments.Keys)
                            ordonnance.Attachments[ordonnance.Attachments.IndexOf(key)] = attachments[key];

                    var ordonnanceUpdated = await App.OrdonnanceManager.SaveOrUpdateAsync(ordonnance.Id, ordonnance, false);
                   
                    if (ordonnanceUpdated != null)
                    {
                        ordonnance = ordonnanceUpdated;
                        ordonnance.IsSynced = true;
                        ordonnance.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        ordonnance.IsSynced = false;
                    }

                    await UpdateAsync(ordonnance as TModel, typeof(Ordonnance).Name + "_" + ordonnance.Id);
                    MessagingCenter.Send(this,"RefreshOrdonanceList");

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(TModel item, string id)
        {
            try
            {

                await DeleteItemAsync(id);
                await AddAsync(item as TModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


		public async Task SyncOrdonnances()
		{
			try
			{
				var items = (await BlobCache.UserAccount.GetAllObjects<Ordonnance>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
				foreach (var item in items)
				{
                    var isNew = item.UpdatedAt == null ? true : false;
					await PushOrdonnance(item, isNew);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

        public async Task<IEnumerable<Ordonnance>> SearchOrdonnance(string query)
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(100,1,searchValue: query,searchFields: "patientName", sortField: "createdAt", sortDirection: Enums.SortDirectionEnum.Desc);

                var response = await App.OrdonnanceManager.GetListAsync(request);

                if (response.rows!=null && response.rows.Any())
                {
                    return response.rows;
                }
                else
                    return new List<Ordonnance>();
            }
            else
            {
                IEnumerable<Ordonnance> list;

                try
                {
                    list = await BlobCache.UserAccount.GetAllObjects<Ordonnance>();
                }
                catch (Exception)
                {
                    list = null;
                }

                if (list != null && list.Any())
                {
                    return list.Where((arg) => arg.PatientName.ToLower().Contains(query.Trim().ToLower()));
                }
                else
                    return new List<Ordonnance>();

            }
        }


		public async Task<int> DownloadOrdonnances(int count)
		{
			if (App.IsConnected())
			{
                var request = new GetListRequest(count, 1, sortField: "createdAt", sortDirection: Enums.SortDirectionEnum.Desc);

                var response = await App.OrdonnanceManager.GetListAsync(request);
              
                if (response.rows == null)
                    return -1;
                
                var Ordonnances = new ObservableCollection<Ordonnance>(response.rows);

                //Updating records in local cache
                await InvalidateSyncedItems();
                await AddManyAsync(Ordonnances.ToList() as List<TModel>);
                return response.total;
            }
            return -1;
        }

        public async Task<int> DownloadPatients(int count)
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(count, 1);
                var response = await App.PatientsManager.GetListAsync(request);
                var patients =
                    new ObservableCollection<Patient>(response.rows);

                if (response.rows == null)
                    return -1;
                //Updating records in local cache
                await InvalidateSyncedItems();
                await AddManyAsync(patients.ToList() as List<TModel>);
                return response.total;
            }
            return -1;
        }

        public async Task<int> DownloadTeledeclarations(int count)
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(count, 1);
                var response = await App.TeledeclarationsManager.GetListAsync(request);
                var teledeclarations =
                    new ObservableCollection<Teledeclaration>(response.rows);

                if (response.rows == null)
                    return -1;
                //Updating records in local cache
                await InvalidateSyncedItems();
                await AddManyAsync(teledeclarations.ToList() as List<TModel>);
                return response.total;
            }
            return -1;
        }

        private async Task<List<string>> GetKeys()
        {
            try
            {
                var response = await BlobCache.UserAccount.GetAllKeys();
                return response.ToList();
            }
            catch(Exception ex)
            {
                return new List<string>();
            }
        }

    }
}