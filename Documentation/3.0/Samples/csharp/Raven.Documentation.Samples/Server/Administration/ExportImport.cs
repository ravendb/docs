using Raven.Abstractions.Data;
using Raven.Abstractions.Smuggler;
using Raven.Client.Embedded;
using Raven.Database.Smuggler;
using Raven.Smuggler;

namespace Raven.Documentation.Samples.Server.Administration
{
	public class ExportImport
	{
		public async void Sample()
		{
			#region smuggler_api_1
			// export Documents, Indexes, Attachments and Transformers
			// to dump.raven file
			// from Northwind database
			// found on http://localhost:8080 server
			var smugglerApi = new SmugglerDatabaseApi(new SmugglerDatabaseOptions
			{
				OperateOnTypes =
					ItemType.Documents | ItemType.Indexes | ItemType.Attachments | ItemType.Transformers,
				Incremental = false
			});

			var exportOptions = new SmugglerExportOptions<RavenConnectionStringOptions>
			{
				ToFile = "dump.raven",
				From = new RavenConnectionStringOptions
				{
					DefaultDatabase = "Northwind",
					Url = "http://localhost:8080"
				}
			};

			await smugglerApi.ExportData(exportOptions);
			#endregion
		}

		public async void Sample1()
		{
			#region smuggler_api_2
			// import only Documents
			// from dump.raven file
			// to NewNorthwind database (must exist)
			// found on http://localhost:8080 server
			var smugglerApi = new SmugglerDatabaseApi(new SmugglerDatabaseOptions
			{
				OperateOnTypes = ItemType.Documents,
				Incremental = false
			});

			var importOptions = new SmugglerImportOptions<RavenConnectionStringOptions>
			{
				FromFile = "dump.raven",
				To = new RavenConnectionStringOptions
				{
					DefaultDatabase = "NewNorthwind",
					Url = "http://localhost:8080"
				}
			};

			await smugglerApi.ImportData(importOptions, null);
			#endregion
		}

		public async void Sample2()
		{
			#region smuggler_api_3
			// export Documents and Indexes
			// from Northwind database
			// found on http://localhost:8080
			// and import them to NewNorthwind
			// found on the same server
			var smugglerApi = new SmugglerDatabaseApi(new SmugglerDatabaseOptions
			{
				OperateOnTypes = ItemType.Documents | ItemType.Indexes,
				Incremental = false
			});

			await smugglerApi.Between(
				new SmugglerBetweenOptions<RavenConnectionStringOptions>
				{
					From = new RavenConnectionStringOptions
					{
						Url = "http://localhost:8080",
						DefaultDatabase = "Northwind"
					},
					To = new RavenConnectionStringOptions
					{
						Url = "http://localhost:8080",
						DefaultDatabase = "NewNorthwind"
					}
				});
			#endregion
		}

		public async void Sample3()
		{
			#region smuggler_api_4
			using (var store = new EmbeddableDocumentStore
								   {
									   DefaultDatabase = "Northwind"
								   })
			{
				store.Initialize();

				var dataDumper = new DatabaseDataDumper(
					store.DocumentDatabase,
					new SmugglerDatabaseOptions
					{
						OperateOnTypes = ItemType.Documents | ItemType.Indexes | ItemType.Attachments | ItemType.Transformers,
						Incremental = false
					});

				var exportOptions = new SmugglerExportOptions<RavenConnectionStringOptions>
				{
					From = new EmbeddedRavenConnectionStringOptions(),
					ToFile = "dump.raven"
				};

				await dataDumper.ExportData(exportOptions);
			}
			#endregion
		}
	}
}