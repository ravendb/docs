namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Database.FileSystem.Storage.Voron.Impl;

	public class Configurations
	{
		interface IInterface
		{
			#region set_key_1
			Task SetKeyAsync<T>(string key, T data);
			#endregion

			#region get_key_1
			Task<T> GetKeyAsync<T>(string key);
			#endregion

			#region delete_key_1
			Task DeleteKeyAsync(string key);
			#endregion

			#region get_key_names_1
			Task<string[]> GetKeyNamesAsync(int start = 0, int pageSize = 25);
			#endregion

			#region search_1
			Task<ConfigurationSearchResults> SearchAsync(string prefix, int start = 0, int pageSize = 25);
			#endregion
		}

		#region set_key_2
		public class FileDescription
		{
			 /* ... */
		}
		#endregion

		public async Task Foo()
		{
			IFilesStore store = null;

			#region set_key_3
			await store
				.AsyncFilesCommands
				.Configuration
				.SetKeyAsync("descriptions/intro.avi", new FileDescription { /* ... */ });
			#endregion

			#region get_key_2

			FileDescription desc = await store
				.AsyncFilesCommands
				.Configuration
				.GetKeyAsync<FileDescription>("descriptions/intro.avi");
			#endregion

			#region delete_key_2

			await store
				.AsyncFilesCommands
				.Configuration
				.DeleteKeyAsync("descriptions/intro.avi");
			#endregion

			{
				#region get_key_names_2

				string[] names = await store
					.AsyncFilesCommands
					.Configuration
					.GetKeyNamesAsync();

				#endregion
			}

			{
				#region search_2

				int start = 0;
				int pageSize = 10;

				do
				{
					ConfigurationSearchResults results = await store
						.AsyncFilesCommands
						.Configuration
						.SearchAsync("descriptions/", start, pageSize);

					IList<string> names = results.ConfigNames;

					start += pageSize;
					
					if(names.Count < pageSize)
						break;

				} while (true);
				#endregion
			}
		}
	}
}