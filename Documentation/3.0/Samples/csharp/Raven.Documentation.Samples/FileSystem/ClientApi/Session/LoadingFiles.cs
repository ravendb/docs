namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class LoadingFiles
	{
		interface IInterface
		{
			#region load_file_0
			Task<FileHeader> LoadFileAsync(string path);
			#endregion

			#region load_file_1
			Task<FileHeader[]> LoadFileAsync(IEnumerable<string> paths);
			#endregion
		}

		public async Task Foo()
		{

			IAsyncFilesSession session = null;

			#region load_file_2
			FileHeader file = await session.LoadFileAsync("/movies/intro.avi");
			#endregion


			#region load_file_3

			FileHeader[] files = await session.LoadFileAsync(new[]
			{
				"non-existing-file", "/movies/intro.avi"
			}); // will return [null, FileHeader] array

			#endregion
		}
	}
}