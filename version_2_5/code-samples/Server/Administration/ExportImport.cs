namespace RavenCodeSamples.Server.Administration
{
	using System.Net;

	using Raven.Abstractions.Data;
	using Raven.Abstractions.Smuggler;
	using Raven.Smuggler;

	public class ExportImport : CodeSampleBase
	{
		public void Sample()
		{
			#region smuggler-api
			var smugglerOptions = new SmugglerOptions { };

			var connectionStringOptions = new RavenConnectionStringOptions
			{
				ApiKey = "ApiKey",
				Credentials = new NetworkCredential("username", "password", "domain"),
				DefaultDatabase = "database",
				Url = "http://localhost:8080",
			};

			var smugglerApi = new SmugglerApi(smugglerOptions, connectionStringOptions);
			smugglerApi.ExportData(new SmugglerOptions { BackupPath = "dump.raven", OperateOnTypes = ItemType.Documents | ItemType.Indexes | ItemType.Attachments | ItemType.Transformers }, incremental: false);
			smugglerApi.ImportData(new SmugglerOptions { BackupPath = "dump.raven", OperateOnTypes = ItemType.Documents | ItemType.Indexes });

			#endregion
		}
	}
}