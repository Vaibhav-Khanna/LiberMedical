using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;

namespace libermedical.Services
{
	public class AzureService<TModel> : IAzureService<TModel> where TModel : BaseDTO, new()
	{
		public Task<TModel> AddAsync(TModel unit)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<TModel>> GetList()
		{
			await Task.Delay(200);
			return new[] {new TModel(),};
		}

		public Task SyncTables()
		{
			throw new NotImplementedException();
		}
	}
}