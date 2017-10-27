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
				foreach (var item in items)
				{
					var key = typeof(TModel).Name + "_" + item.Id;
					item.IsSynced = true;
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

		public async Task SyncTables()
		{
			await SyncPatients();
			await SyncOrdonnances();
			await SyncDocuments();
		}

		public async Task SyncPatients()
		{
			try
			{

				var items = (await BlobCache.UserAccount.GetAllObjects<Patient>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
				foreach (var item in items)
				{
					await PushPatient(item, item.CreatedAt == item.UpdatedAt);
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
					await PushDocument(item, item.CreatedAt == item.UpdatedAt);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}

		}

		public async Task PushPatient(Patient patientObject, bool isNew)
		{
			try
			{

				var localId = patientObject.Id;
				var patient = await App.PatientsManager.SaveOrUpdateAsync(patientObject.Id, patientObject, isNew);
				if (patient != null)
				{
					await DeleteItemAsync(typeof(Patient).Name + "_" + localId);
					patient.IsSynced = true;
					await AddAsync(patient as TModel);
					var patientDocuments = (await BlobCache.UserAccount.GetAllObjects<Document>()).ToObservable().Where(x => x.PatientId == localId).ToEnumerable();
					foreach (var document in patientDocuments.Where(x => x.IsSynced == false))
					{
						document.Patient = patient;
						document.PatientId = patient.Id;
						await PushDocument(document, document.CreatedAt == document.UpdatedAt);
					}

					var patientOrdonnances = (await BlobCache.UserAccount.GetAllObjects<Ordonnance>()).ToObservable().Where(x => x.PatientId == localId).ToEnumerable();
					foreach (var ordonnance in patientOrdonnances.Where(x => x.IsSynced == false))
					{
						ordonnance.Patient = patient;
						ordonnance.PatientId = patient.Id;
						await PushOrdonnance(ordonnance, ordonnance.CreatedAt == ordonnance.UpdatedAt);
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
				var res = await FileUpload.UploadFile(document.AttachmentPath, "PatientDocuments", document.Id);
				if (res)
				{
					document.AttachmentPath = $"PatientDocuments/{document.Id}/{Path.GetFileName(document.AttachmentPath)}";
					var localId = document.Id;
					var doc = await App.DocumentsManager.SaveOrUpdateAsync(document.Id, document, isNew);
					if (doc != null)
					{
						await DeleteItemAsync(typeof(Document).Name + "_" + localId);
						doc.IsSynced = true;
						await AddAsync(doc as TModel);
					}
					else
					{
						await DeleteItemAsync(typeof(Document).Name + "_" + localId);
						await AddAsync(document as TModel);
					}
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
				var ordonnance = await App.OrdonnanceManager.SaveOrUpdateAsync(ordonnanceObject.Id, ordonnanceObject, isNew);
				if (ordonnance != null)
				{
					await DeleteItemAsync(typeof(Ordonnance).Name + "_" + localId);
					ordonnance.IsSynced = true;
					await AddAsync(ordonnance as TModel);
					var attachments = new Dictionary<string, string>();
					foreach (var attachment in ordonnance.Attachments)
					{
						var res = await FileUpload.UploadFile(attachment, "Ordonnance", ordonnance.Id);
						if (res)
							attachments.Add(attachment, $"Ordonnance/{ordonnance.Id}/{Path.GetFileName(attachment)}");
					}
					if (attachments.Keys.Count > 0)
						foreach (var key in attachments.Keys)
						{
							ordonnance.Attachments[ordonnance.Attachments.IndexOf(key)] = attachments[key];

						}
					var ordonnanceUpdated = await App.OrdonnanceManager.SaveOrUpdateAsync(ordonnance.Id, ordonnance, false);
					if (ordonnanceUpdated != null)
						return;
					else
					{
						await DeleteItemAsync(typeof(Ordonnance).Name + "_" + ordonnance.Id);
						ordonnance.IsSynced = false;
						await AddAsync(ordonnance as TModel);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public async Task SyncOrdonnances()
		{
			try
			{
				var items = (await BlobCache.UserAccount.GetAllObjects<Ordonnance>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
				foreach (var item in items)
				{
					var isNew = item.CreatedAt == item.UpdatedAt ? true : false;
					await PushOrdonnance(item, isNew);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

	}
}
