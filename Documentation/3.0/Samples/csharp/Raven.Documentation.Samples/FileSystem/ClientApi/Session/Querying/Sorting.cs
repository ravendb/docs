namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class Sorting
	{
		public async Task Foo()
		{
			using (var store = new FilesStore())
			{
				using (var session = store.OpenAsyncSession())
				{
					#region sorting_1
					List<FileHeader> results = await session.Query()
												.OrderBy(x => x.Name)
												.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region sorting_2
					List<FileHeader> results = await session.Query()
												.OrderByDescending("Owner") // order descending by custom "Owner" metadata 
												.ToListAsync();
					#endregion
				}
			}
		}
	}
}