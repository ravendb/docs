namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class Paging
	{
		public async Task Foo()
		{
			using (var store = new FilesStore())
			{
				using (var session = store.OpenAsyncSession())
				{
					#region paging_1
					List<FileHeader> results = await session.Query()
												.Take(10)
												.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region paging_2
					List<FileHeader> results = await session.Query()
												.Skip(10)
												.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region paging_3

					const int pageSize = 10;
					int start = 0;
					List<FileHeader> results;

					do
					{
						results = await session.Query()
												.Skip(start)
												.Take(pageSize)
												.ToListAsync();

						start += pageSize;

					} while (results.Count == pageSize);
					#endregion
				}
			}
		}
	}
}