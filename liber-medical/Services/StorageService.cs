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

        public Task SyncTables()
        {
            throw new NotImplementedException();
        }
    }
}
