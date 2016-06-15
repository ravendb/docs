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
						.WhereBetween(x => x.TotalSize, 1024 * 1024, 5 * 1024 * 1024) // size has to be > 1KB but < 5KB
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_3
					List<FileHeader> results = await session.Query()
						.WhereBetweenOrEqual("NumberOfDownloads", 5, 10)
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
					Stream stream = null;

					#region filtering_6

					session.RegisterUpload("test.file", stream);
					session.RegisterUpload("test.fil", stream);
					session.RegisterUpload("test.fi", stream);
					session.RegisterUpload("test.f", stream);

					await session.SaveChangesAsync();

					List<FileHeader> results = await session.Query()
						.WhereGreaterThan(x => x.Name, "test.fi") // will return 'test.fil' and 'test.file'
						.ToListAsync();

					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_7
					List<FileHeader> results = await session.Query()
						.WhereGreaterThanOrEqual("Download-Ratio", 7.3)
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
						.WhereLessThanOrEqual("Downloaded", 5)
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
					Stream content = null;

					#region filtering_12
					session.RegisterUpload("git.bin", content, new RavenJObject()
					{
						{"Attributes", new RavenJArray(new object[]{ "r", "w" }) }
					});

					session.RegisterUpload("svn.bin", content, new RavenJObject()
					{
						{"Attributes", new RavenJArray(new object[]{ "w", "x" }) }
					});

					await session.SaveChangesAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_13
					List<FileHeader> results = await session.Query()
						.ContainsAll("Attributes", new[] { "r", "w" }) // will return git.bin
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_14
					List<FileHeader> results = await session.Query()
						.ContainsAny("Attributes", new[] { "r", "x" }) // will return git.bin and svn.bin
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_15
					List<FileHeader> results = await session.Query()
						.WhereStartsWith(x => x.Name, "readme")
						.WhereEquals("Copyright", "HR")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_16
					List<FileHeader> results = await session.Query()
						.WhereStartsWith(x => x.Name, "readme")
						.AndAlso()
						.WhereEquals("Copyright", "HR")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_17
					List<FileHeader> results = await session.Query()
						.WhereIn(x => x.Name, new[] { "help.txt", "documentation.doc" })
						.OrElse()
						.WhereStartsWith(x => x.Name, "readme")
						.ToListAsync();
					#endregion
				}

				using (var session = store.OpenAsyncSession())
				{
					#region filtering_18
					List<FileHeader> results = await session.Query()
						.OnDirectory("/documents/wallpapers", recursive: true)
						.WhereEndsWith(x => x.Name, "1920x1080.jpg")
						.ToListAsync();
					#endregion
				}
			}
			
		}
	}
}