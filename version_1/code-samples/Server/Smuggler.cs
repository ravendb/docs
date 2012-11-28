using System.Net;
using Raven.Abstractions.Data;
using Raven.Abstractions.Smuggler;
using Raven.Smuggler;

namespace RavenCodeSamples.Server
{
	public class Smuggler
	{
		public Smuggler()
		{
			#region smuggler-api
			var connectionStringOptions = new RavenConnectionStringOptions
			{
				ApiKey = "ApiKey",
				Credentials = new NetworkCredential("username", "password", "domain"),
				DefaultDatabase = "database",
				Url = "http://localhost:8080",
			};
			var smugglerApi = new SmugglerApi(connectionStringOptions);
			smugglerApi.ExportData(new SmugglerOptions { File = "dump.raven", OperateOnTypes = ItemType.Documents | ItemType.Indexes | ItemType.Attachments });
			smugglerApi.ImportData(new SmugglerOptions { File = "dump.raven", OperateOnTypes = ItemType.Documents | ItemType.Indexes });
			#endregion
		}
	}
}