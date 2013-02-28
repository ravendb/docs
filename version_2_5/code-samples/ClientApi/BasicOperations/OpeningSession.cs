namespace RavenCodeSamples.ClientApi.BasicOperations
{
	using Raven.Client;

	public class OpeningSession : CodeSampleBase
	{
		public void Open()
		{
			using (var store = NewDocumentStore())
			{
				#region open_the_session
				// Saving changes using the session API
				using (IDocumentSession session = store.OpenSession())
				{
					// Operations against session

					// Flush those changes
					session.SaveChanges();
				}

				#endregion
			}
		}
	}
}