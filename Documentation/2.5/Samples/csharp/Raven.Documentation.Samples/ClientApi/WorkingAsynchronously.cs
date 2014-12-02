namespace RavenCodeSamples.ClientApi
{
	using Raven.Client;
	using Raven.Client.Linq;

	public class WorkingAsynchronously : CodeSampleBase
	{
		public async void WorkingAsync()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region async1
				var entity = new Company {Name = "Async Company #2", Id = "companies/2"};
				using (var session = documentStore.OpenAsyncSession())
				{
					var company = await session.LoadAsync<Company>(1); // loading an entity asynchronously

					await session.StoreAsync(entity); // in-memory operations are committed asynchronously when calling SaveChangesAsync
					await session.SaveChangesAsync(); // returns a task that completes asynchronously

					var query = session.Query<Company>()
					                   .Where(x => x.Name == "Async Company #1")
					                   .ToListAsync(); // returns a task that will execute the query
				}

				#endregion
			}
		}
	}
}