using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class GetDocumentId
	{
		private class EmployeeWithoutIdProperty
		{
		}

		private interface IFoo
		{
			#region get_document_id_1
			string GetDocumentId(object entity);
			#endregion
		}

		public GetDocumentId()
		{
			using (var store = new DocumentStore())
			{
				EmployeeWithoutIdProperty employee = null;

				using (var session = store.OpenSession())
				{
					#region get_document_id_2
					var commentId = session
						.Advanced
						.GetDocumentId(employee); // e.g. employees/1
					#endregion
				}
			}
		}
	}
}