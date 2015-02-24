namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.Extensions;
	using Client.FileSystem;
	using Json.Linq;

	public class Files
	{
		private interface IFoo
		{
			#region upload_1
			Task UploadAsync(string filename, Stream source, RavenJObject metadata = null, long? size = null);
			#endregion

			#region download_1
			Task<Stream> DownloadAsync(string filename, Reference<RavenJObject> metadata = null, long? from = null, long? to = null);
			#endregion

			#region rename_1
			Task RenameAsync(string currentName, string newName);
			#endregion

			#region delete_1
			Task DeleteAsync(string filename);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			#region upload_2
			await store
				.AsyncFilesCommands
				.UploadAsync(
					"/movies/intro.avi",
					File.OpenRead(@"C:\intro.avi"),
					new RavenJObject
					{
						{
							"AllowRead", "Everyone"
						}
					}
				);
			#endregion
			{
			#region download_2
			var metadata = new Reference<RavenJObject>();

			var data = await store
				.AsyncFilesCommands
				.DownloadAsync(
					"/movies/intro.avi",
					metadata,
					from: 0,
					to: 200);
			#endregion
			}

			#region rename_2
			await store
				.AsyncFilesCommands
				.RenameAsync(
					"/movies/intro.avi",
					"/movies/introduction.avi"
				);
			#endregion

			#region rename_3
			await store
				.AsyncFilesCommands
				.RenameAsync(
					"/movies/intro.avi",
					"/movies/examples/intro.avi"
				);
			#endregion

			#region delete_2

			await store.AsyncFilesCommands.DeleteAsync("/movies/intro.avi");
			#endregion
		} 
	}
}