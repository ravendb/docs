using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Session
{
	public class OpeningSession
	{
		private interface IFoo
		{
			#region open_session_1
			// Open session for a 'default' database configured in 'DocumentStore'
			IDocumentSession OpenSession();

			// Open session for a specified database
			IDocumentSession OpenSession(string database);

			IDocumentSession OpenSession(OpenSessionOptions options);
			#endregion
		}

		public OpeningSession()
		{
			string databaseName = "DB1";

			using (var store = new DocumentStore())
			{
				#region open_session_2
				store.OpenSession(new OpenSessionOptions());
				#endregion

				#region open_session_3
				store.OpenSession(new OpenSessionOptions
									  {
										  Database = databaseName
									  });
				#endregion

				#region open_session_4
				using (IDocumentSession session = store.OpenSession())
				{
					// code here
				}
				#endregion

				#region open_session_5
				using (IAsyncDocumentSession session = store.OpenAsyncSession())
				{
					// async code here
				}
				#endregion
			}
		}
	}
}