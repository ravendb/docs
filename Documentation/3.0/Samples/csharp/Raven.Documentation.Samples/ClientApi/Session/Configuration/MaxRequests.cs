using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Configuration
{
	public class MaxRequests
	{
		public MaxRequests()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region max_requests_1
					session.Advanced.MaxNumberOfRequestsPerSession = 50;
					#endregion
				}

				#region max_requests_2
				store.Conventions.MaxNumberOfRequestsPerSession = 100;
				#endregion
			}
		}
	}
}