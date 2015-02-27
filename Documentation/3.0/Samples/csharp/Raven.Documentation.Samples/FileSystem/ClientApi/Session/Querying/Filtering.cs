namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client;
	using Client.FileSystem;
	using Json.Linq;

	public class Filtering
	{
		public async Task Foo()
		{
			using (var store = new FilesStore())
			{
				using (var session = store.OpenAsyncSession())
				{
					#region filtering_1
					List<FileHeader> results = await session.Query()
						.Where("__fileName:readme* AND Copyright:HR")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_2
					List<FileHeader> results = await session.Query()
						.WhereBetween(x => x.TotalSize, 1024, 1024 * 1024)
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_3
					List<FileHeader> results = await session.Query()
						.WhereBetweenOrEqual(x => x.TotalSize, 1024, 1024 * 1024)
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_4
					List<FileHeader> results = await session.Query()
						.WhereEndsWith(x => x.Name, ".txt")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_5
					List<FileHeader> results = await session.Query()
						.WhereEquals("Copyright", "HR")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_6
					List<FileHeader> results = await session.Query()
						.WhereGreaterThan(x => x.TotalSize, 1024 * 1024)
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_7
					List<FileHeader> results = await session.Query()
						.WhereGreaterThanOrEqual(x => x.TotalSize, 1024 * 1024)
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_8
					List<FileHeader> results = await session.Query()
						.WhereIn(x => x.Name, new[] { "readme.txt", "help.doc" })
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_9
					List<FileHeader> results = await session.Query()
						.WhereLessThan(x => x.TotalSize, 1024)
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_10
					List<FileHeader> results = await session.Query()
						.WhereLessThanOrEqual(x => x.TotalSize, 1024)
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_11
					List<FileHeader> results = await session.Query()
						.WhereStartsWith(x => x.FullPath, "/movies/ravenfs")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()

						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()

						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()

						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()

						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()

						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_
					List<FileHeader> results = await session.Query()

						.ToListAsync();
					#endregion
				}
			}
			
		}
	}
}