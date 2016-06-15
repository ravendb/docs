namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Json.Linq;

	public class UploadingFiles
	{
		interface IInterface
		{
			#region register_upload_1
			void RegisterUpload(string path, Stream stream, RavenJObject metadata = null, Etag etag = null);
			void RegisterUpload(string path, long fileSize, Action<Stream> write, RavenJObject metadata = null, Etag etag = null);
			#endregion

			#region register_upload_2
			void RegisterUpload(FileHeader file, Stream stream, Etag etag = null);
			void RegisterUpload(FileHeader file, long fileSize, Action<Stream> write, Etag etag = null);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				#region register_upload_3
				using (Stream content = File.OpenRead(@"C:\intro.avi"))
				{
					session.RegisterUpload("/movies/intro.avi", content);

					await session.SaveChangesAsync();
				}
				#endregion
			}

			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				#region register_upload_4
				session.RegisterUpload("random.bin", 128, stream =>
				{
					var bytes = new byte[128];
					new Random().NextBytes(bytes);

					stream.Write(bytes, 0, 128);
				});

				await session.SaveChangesAsync();
				#endregion
			}
			
			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				#region register_upload_5
				FileHeader ravenFile = await session.LoadFileAsync("/movies/intro.avi");

				string localFile = @"C:\intro.avi";

				if (ravenFile == null || new FileInfo(localFile).LastWriteTime - ravenFile.LastModified > TimeSpan.FromHours(1))
				{
					using (Stream content = File.OpenRead(localFile))
					{
						session.RegisterUpload(ravenFile, content);

						await session.SaveChangesAsync();
					}
				}
				#endregion
			}
		}
	}
}