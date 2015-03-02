namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Json.Linq;
	using Samples.ClientApi.Commands.Documents;

	public class Search
	{
		interface IFoo
		{
			#region search_1
			Task<SearchResults> SearchAsync(string query, string[] sortFields = null, int start = 0, int pageSize = 1024);
			#endregion

			#region search_on_directory_1
			Task<SearchResults> SearchOnDirectoryAsync(string folder, FilesSortOptions options = FilesSortOptions.Default, 
													string fileNameSearchPattern = "", int start = 0, int pageSize = 1024);
			#endregion

			#region get_search_fields_1
			Task<string[]> GetSearchFieldsAsync(int start = 0, int pageSize = 1024);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			{ 
				#region search_2
				SearchResults results = await store
					.AsyncFilesCommands
					.SearchAsync("AllowRead:Everyone", new []{ "__key"});
				#endregion
			}

			{
				#region search_3
				SearchResults results = await store
					.AsyncFilesCommands
					.SearchAsync(
						"AllowRead:Everyone", 
						new[] { "__key", "-__fileName" } // sort ascending by full path, then by file name in descending order
					);
				#endregion
			}

			MemoryStream stream = null;

			{
				

				#region search_on_directory_2

				await store.AsyncFilesCommands.UploadAsync("1.doc", stream);
				await store.AsyncFilesCommands.UploadAsync("2.txt", stream);
				await store.AsyncFilesCommands.UploadAsync("3.toc", stream);
				await store.AsyncFilesCommands.UploadAsync("/backups/1.doc", stream);

				SearchResults result = await store
					.AsyncFilesCommands
					.SearchOnDirectoryAsync(
						"/", 
						FilesSortOptions.Name | FilesSortOptions.Desc,
						"*.?oc"
					); // will return 3.toc and 1.doc

				#endregion
			}

			#region get_search_fields_2
			await store.AsyncFilesCommands.UploadAsync("intro.txt", stream, new RavenJObject() { { "Owner", "James" } });

			var terms = await store.AsyncFilesCommands.GetSearchFieldsAsync();
			#endregion
		}
	}
}