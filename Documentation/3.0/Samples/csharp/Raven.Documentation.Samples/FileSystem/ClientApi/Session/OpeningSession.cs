namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System.Threading.Tasks;
	using Client.Document;
	using Client.FileSystem;

	public class OpeningSession
	{
		private IFilesStore store = null;

		interface IInterface
		{
			#region open_session_1
			// Open a session for a 'default' file system configured in 'FilesStore'
			IAsyncFilesSession OpenAsyncSession();

			// Open a session for a specified file system
			IAsyncFilesSession OpenAsyncSession(string filesystem);

			// Open a session for a specified file system with custom API Key / Credentials
			IAsyncFilesSession OpenAsyncSession(OpenFilesSessionOptions sessionOptions);
			#endregion
		}

		public async Task Foo()
		{
			FilesStore store = null;

			#region open_session_2
			store.OpenAsyncSession(new OpenFilesSessionOptions()
			{
				FileSystem = store.DefaultFileSystem
			});
			#endregion

			string filesystem = null;

			#region open_session_3
			store.OpenAsyncSession(new OpenFilesSessionOptions()
			{
				FileSystem = filesystem
			});
			#endregion

			#region open_session_4
			using (IAsyncFilesSession session = store.OpenAsyncSession())
			{
				// code here
			}
			#endregion
		}
	}
}