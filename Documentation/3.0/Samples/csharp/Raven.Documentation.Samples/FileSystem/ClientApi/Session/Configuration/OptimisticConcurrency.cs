namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Configuration
{
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Json.Linq;

	public class OptimisticConcurrency
	{
		public async Task Foo()
		{
			using (var store = new FilesStore())
			{
				Stream content = new MemoryStream();
				Stream newContent = new MemoryStream();

				#region optimistic_concurrency_1
				using (IAsyncFilesSession session = store.OpenAsyncSession())
				{
					session.Advanced.UseOptimisticConcurrency = true;

					session.RegisterUpload("ravendb.exe", content, new RavenJObject { { "Copyright", "hibernatingrhinos.com"}});

					await session.SaveChangesAsync();

					using (IAsyncFilesSession otherSession = store.OpenAsyncSession())
					{
						FileHeader fileInOtherSession = await otherSession.LoadFileAsync("ravendb.exe");

						fileInOtherSession.Metadata["Copyright"] = "Hibernating Rhinos LTD";

						await otherSession.SaveChangesAsync();
					}

					FileHeader file = await session.LoadFileAsync("ravendb.exe");

					session.RegisterUpload(file, newContent);

					await session.SaveChangesAsync(); // will throw ConcurrencyException
				}
				#endregion

				#region optimistic_concurrency_2 
				store.Conventions.DefaultUseOptimisticConcurrency = true;

				using (IAsyncFilesSession session = store.OpenAsyncSession())
				{
					bool isSessionUsingOptimisticConcurrency = session.Advanced.UseOptimisticConcurrency; // will return true
				}
				#endregion

				#region optimistic_concurrency_3

				using (IAsyncFilesSession session = store.OpenAsyncSession())
				{
					FileHeader file = await session.LoadFileAsync("git.exe");

					session.RegisterFileDeletion(file, file.Etag);

					await session.SaveChangesAsync();
				}
				#endregion
			}
		} 
	}
}