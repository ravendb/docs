namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class RenamingFiles
	{
		interface IInterface
		{
			#region rename_1
			void RegisterRename(string sourceFile, string destinationFile, Etag etag = null);
			void RegisterRename(FileHeader sourceFile, string destinationFile, Etag etag = null);
			#endregion
		}

		public async Task Foo()
		{
			IAsyncFilesSession session = null;

			#region rename_2
			session.RegisterRename("/movies/intro.avi", "/movies/introduction-to-ravenfs.avi");

			await session.SaveChangesAsync();
			#endregion
		}
	}
}