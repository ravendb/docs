namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Xunit;

	public class WhatIsFilesSession
	{
		public async Task Foo()
		{
			IFilesStore store = null;

			#region session_usage_1
			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				using (Stream content = File.OpenRead(@"C:\intro.avi"))
				{
					session.RegisterUpload("/movies/intro.avi", content);

					await session.SaveChangesAsync();
				}
			}

			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				FileHeader file = await session.LoadFileAsync("/movies/intro.avi");

				using (Stream content = await session.DownloadAsync(file.FullPath))
				{
					/* ... */
				}

				if (file.CreationDate < DateTime.Now.AddDays(-1))
					session.RegisterFileDeletion(file);
				
				await session.SaveChangesAsync();
			}
			#endregion


			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				#region unit_of_work_1
				Assert.Same(await session.LoadFileAsync("/movies/intro.avi"), await session.LoadFileAsync("/movies/intro.avi"));
				#endregion
			}

			#region unit_of_work_2
			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				FileHeader file = await session.LoadFileAsync("/movies/intro.avi");

				file.Metadata.Add("Owner", "James");

				await session.SaveChangesAsync(); // will sent the metadata update to the file system
			}
			#endregion
		} 
	}
}