using System;
using System.Collections.Generic;

namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.IO;
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Abstractions.Util;
	using Client.FileSystem;

	public class Browse
	{
		interface IFoo
		{
			#region browse_1
			Task<FileHeader[]> BrowseAsync(int start = 0, int pageSize = 1024);
			#endregion

			#region get_1
			Task<FileHeader[]> GetAsync(string[] filenames);
			#endregion

			#region starts_with_1
			Task<FileHeader[]> StartsWithAsync(string prefix, string matches, int start, int pageSize);
			#endregion

			#region stream_file_headers_1
			Task<IAsyncEnumerator<FileHeader>> StreamFileHeadersAsync(Etag fromEtag, int pageSize = int.MaxValue);
            #endregion

            #region stream_query_1
            Task<IAsyncEnumerator<FileHeader>> StreamQueryAsync(string query, string[] sortFields = null, int start = 0, int pageSize = int.MaxValue);
            #endregion

            #region get_directories_1
            Task<string[]> GetDirectoriesAsync(string from = null, int start = 0, int pageSize = 1024);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			int start;
			int pageSize;

			#region browse_2
			start = 0;
			pageSize = 10;
			FileHeader[] fileHeaders;

			do
			{
				fileHeaders = await store
					.AsyncFilesCommands
					.BrowseAsync(start, pageSize);

				start += pageSize;

			} while (fileHeaders.Length == pageSize);
			#endregion

			#region get_2
			FileHeader[] icons = await store
				.AsyncFilesCommands
				.GetAsync(new[]
				{
					"/images/icons/small/1.png", "/images/icons/large/1.png"
				});
			#endregion

			#region starts_with_2
			FileHeader[] images = await store
				.AsyncFilesCommands
				.StartsWithAsync("/images", null, 0, 128);
			#endregion

			#region starts_with_3
			FileHeader[] jpgs = await store
				.AsyncFilesCommands
				.StartsWithAsync("/images", "*.jpg", 0, 128);
			#endregion

			#region stream_file_headers_2
			using (var reader = await store.AsyncFilesCommands.StreamFileHeadersAsync(Etag.Empty))
			{
				while (await reader.MoveNextAsync())
				{
					FileHeader header = reader.Current;
				}
			}
			#endregion

			Stream content = null;

            #region stream_query_2

            var allFilesMatchingCriteria = new List<FileHeader>();
            using (var reader = await store.AsyncFilesCommands.StreamQueryAsync("", sortFields: new string[] { "-__key" }))
            {  
                while (await reader.MoveNextAsync())
                {
                    allFilesMatchingCriteria.Add(reader.Current);
                }
            }
            #endregion

            #region stream_query_3

            using (var reader = await store.AsyncFilesCommands.StreamQueryAsync("Number:2", start: 10, pageSize: 20))
            {
                while (await reader.MoveNextAsync())
                {
                    allFilesMatchingCriteria.Add(reader.Current);
                }
            }
            #endregion

            #region get_directories_2
            await store.AsyncFilesCommands.UploadAsync("text-files/txt/a.txt", content);
			await store.AsyncFilesCommands.UploadAsync("text-files/doc/a.doc", content);
			await store.AsyncFilesCommands.UploadAsync("text-files/doc/drafts/a.doc", content);
			await store.AsyncFilesCommands.UploadAsync("image-files/a.jpg", content);

			string[] dirs;

			dirs = await store.AsyncFilesCommands.GetDirectoriesAsync(); // will return "/image-files" and "/text-files"

			dirs = await store.AsyncFilesCommands.GetDirectoriesAsync("/text-files"); // will return "/text-files/doc" and "/text-files/txt"

			dirs = await store.AsyncFilesCommands.GetDirectoriesAsync("/image-files"); // will return empty array
			#endregion
		}
	}
}