namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.Extensions;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Json.Linq;

	public class DownloadingFiles
	{
		interface IInterface
		{
			#region download_1
			Task<Stream> DownloadAsync(string path, Reference<RavenJObject> metadata = null);
			Task<Stream> DownloadAsync(FileHeader file, Reference<RavenJObject> metadata = null);
			#endregion
		}

		public async Task Foo()
		{
			IAsyncFilesSession session = null;

			Stream localFile = null;

			#region download_2
			Reference<RavenJObject> metadata = new Reference<RavenJObject>();

			Stream content = await session.DownloadAsync("/movies/intro.avi", metadata);

			Console.WriteLine("Downloaded {0} bytes", metadata.Value.Value<long>("RavenFS-Size"));
			#endregion
		}
	}
}