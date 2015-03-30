namespace Raven.Documentation.Samples.FileSystem.Server
{
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.Smuggler;
	using Smuggler;

	public class ExportImport
	{
		public async Task Foo()
		{
			{
				#region smuggler_api_1

				SmugglerFilesApi smugglerApi = new SmugglerFilesApi();

				var exportOptions = new SmugglerExportOptions<FilesConnectionStringOptions>
				{
					ToFile = "dump.ravenfs",
					From = new FilesConnectionStringOptions
					{
						Url = "http://localhost:8080",
						DefaultFileSystem = "NorthwindFS"
					}
				};

				var exportResult = await smugglerApi.ExportData(exportOptions);

				#endregion
			}

			{
				#region smuggler_api_2

				SmugglerFilesApi smugglerApi = new SmugglerFilesApi();

				var importOptions = new SmugglerImportOptions<FilesConnectionStringOptions>
				{
					FromFile = "dump.ravenfs",
					To = new FilesConnectionStringOptions
					{
						Url = "http://localhost:8080",
						DefaultFileSystem = "NewNorthwindFS"
					}
				};

				await smugglerApi.ImportData(importOptions);

				#endregion
			}

			{
				#region smuggler_api_3
				// export files
				// from NorthwindFS file system
				// found on http://localhost:8080
				// and import them to NewNorthwindFS
				// found on the same server
				SmugglerFilesApi smugglerApi = new SmugglerFilesApi();

				var betweenOptions = new SmugglerBetweenOptions<FilesConnectionStringOptions>
				{
					From = new FilesConnectionStringOptions
					{
						Url = "http://localhost:8080",
						DefaultFileSystem = "NorthwindFS"
					},
					To = new FilesConnectionStringOptions
					{
						Url = "http://localhost:8080",
						DefaultFileSystem = "NewNorthwindFS"
					}
				};

				await smugglerApi.Between(betweenOptions);
				#endregion
			}
		}
	}
}