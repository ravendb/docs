using Raven.Client.Document;

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
					var person = session.Load<Person>("people/1");
					// http://localhost:8080/databases/DB1/docs/people/1
					var url = session.Advanced.GetDocumentUrl(person);
					#endregion
				}
			}
		}
	}
}