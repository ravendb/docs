namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Configuration
{
	using Client.FileSystem;

	public class MaxRequests
	{
		public MaxRequests()
		{
			using (var store = new FilesStore())
			{
				using (var session = store.OpenAsyncSession())
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