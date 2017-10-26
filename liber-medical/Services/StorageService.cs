using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;
using Akavache;
using System.Reactive.Linq;
using libermedical.Managers;
using System.Diagnostics;
using System.IO;

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
        }

        public async Task SyncPatients()
        {
            try
            {
                var items = (await BlobCache.UserAccount.GetAllObjects<Patient>()).ToObservable().Where(x => x.IsSynced == false).ToEnumerable();
                foreach (var item in items)
                {
                    var isNew = item.CreatedAt == item.UpdatedAt ? true : false;
                    var localId = item.Id;
                    var patient = await App.PatientsManager.SaveOrUpdateAsync(item.Id, item, isNew);
                    if (patient != null)
                    {
                        await DeleteItemAsync(typeof(Patient).Name + "_" + localId);
                        patient.IsSynced = true;
                        await AddAsync(patient as TModel);
                        var patientDocuments = (await BlobCache.UserAccount.GetAllObjects<Document>()).ToObservable().Where(x => x.PatientId == localId).ToEnumerable();
                        foreach (var document in patientDocuments)
                        {
                            await FileUpload.UploadFile(document.AttachmentPath, "PatientDocuments", document.Id);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
                    var localId = item.Id;
                    var ordonnance = await App.OrdonnanceManager.SaveOrUpdateAsync(item.Id, item, isNew);
                    if (ordonnance != null)
                    {
                        await DeleteItemAsync(typeof(Ordonnance).Name + "_" + localId);
                        ordonnance.IsSynced = true;
                        await AddAsync(ordonnance as TModel);
                        foreach (var attachment in ordonnance.Attachments)
                        {
                            var res = await FileUpload.UploadFile(attachment, "Ordonnance", ordonnance.Id);
                            if (res)
                                ordonnance.Attachments[ordonnance.Attachments.IndexOf(attachment)] = $"Ordonnance/{ordonnance.Id}/{Path.GetFileName(attachment)}";
                        }
                        var ordonnanceUpdated = await App.OrdonnanceManager.SaveOrUpdateAsync(item.Id, item, false);
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
