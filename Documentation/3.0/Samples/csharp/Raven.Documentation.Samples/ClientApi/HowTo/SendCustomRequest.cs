using Raven.Client.Connection;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.HowTo
{
	public class SendCustomRequest
	{
		public SendCustomRequest()
		{
			using (var store = new DocumentStore())
			{
				#region custom_request_1
				var key = "employees/1";

				// http://localhost:8080/databases/Northwind/docs/employees/1
				var url = store.Url // http://localhost:8080
					.ForDatabase("Northwind") // /databases/Northwind
					.Doc(key); // /docs/employees/1

				var commands = store.DatabaseCommands;
				var request = store
					.JsonRequestFactory
					.CreateHttpJsonRequest(new CreateHttpJsonRequestParams(commands, url, "GET", commands.PrimaryCredentials, store.Conventions));

				var json = request.ReadResponseJson();
				var jsonDocument = SerializationHelper.DeserializeJsonDocument(key, json, request.ResponseHeaders, request.ResponseStatusCode);
				#endregion
			}
		}
	}
}