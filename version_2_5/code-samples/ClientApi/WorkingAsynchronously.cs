namespace RavenCodeSamples.ClientApi
{
	using Raven.Client;
	using Raven.Client.Linq;
	using Raven.Client.Extensions;

	public class WorkingAsynchronously : CodeSampleBase
	{
		public void WorkingAsync()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region async1
				var entity = new Company { Name = "Async Company #2", Id = "companies/2" };
				using (var session = documentStore.OpenAsyncSession())
				{
					var company = session.LoadAsync<Company>(1); // loading an entity asynchronously

					session.Store(entity); // in-memory operations are committed asynchronously when calling SaveChangesAsync
					session.SaveChangesAsync(); // returns a task that completes asynchronously

					var query = session.Query<Company>()
						.Where(x => x.Name == "Async Company #1")
						.ToListAsync(); // returns a task that will execute the query
				}

				#endregion
			}
		}
	}
}