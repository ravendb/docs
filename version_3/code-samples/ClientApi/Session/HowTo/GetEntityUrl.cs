using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class GetEntityUrl
	{
		private interface IFoo
		{
			#region get_entity_url_1
			string GetDocumentUrl(object entity);
			#endregion
		}

		public GetEntityUrl()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region get_entity_url_2
					var employee = session.Load<Employee>("employees/1");
					// http://localhost:8080/databases/Northwind/docs/employees/1
					var url = session.Advanced.GetDocumentUrl(employee);
					#endregion
				}
			}
		}
	}
}