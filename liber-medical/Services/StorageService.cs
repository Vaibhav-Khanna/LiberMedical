using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;
using Akavache;
using System.Reactive.Linq;

namespace libermedical.Services
{
    public class StorageService<TModel> : IStorageService<TModel> where TModel : BaseDTO, new()
    {
        public async Task<TModel> AddAsync(TModel item)
        {
            try
            {
                await BlobCache.UserAccount.InsertObject(item.Id, item);
                return item;
            }
            catch (KeyNotFoundException ex)
            {
                return item;
            }
        }

        public async Task<bool> DeleteAllAsync()
        {
            try
            {
                await BlobCache.UserAccount.InvalidateAllObjects<TModel>();
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(string key)
        {
            try
            {
                await BlobCache.UserAccount.Invalidate(key);
                return true;
            }
            catch (KeyNotFoundException ex)
            {
                return false;
            }
        }

        public async Task<TModel> GetItemAsync(string key)
        {
            try
            {
                return await BlobCache.UserAccount.GetObject<TModel>(key);
            }
            catch (KeyNotFoundException ex)
            {
                return new TModel();
            }
        }

        public async Task<IEnumerable<TModel>> GetList()
        {
            try
            {
                return await BlobCache.UserAccount.GetAllObjects<TModel>();
            }
            catch (KeyNotFoundException ex)
            {
                return new List<TModel>();
            }
        }

        public Task SyncTables()
        {
            throw new NotImplementedException();
        }
    }
}