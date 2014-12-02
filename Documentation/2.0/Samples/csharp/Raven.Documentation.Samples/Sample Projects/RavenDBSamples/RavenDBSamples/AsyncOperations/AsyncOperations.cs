using RavenDBSamples.BaseForSamples;
using Raven.Client.Linq;
using Raven.Client;

namespace RavenDBSamples.AsyncOperations
{
	public class AsyncOperations : SampleBase
	{
		public void CreateAsyncSession()
		{
			using (var asyncSession = DocumentStore.OpenAsyncSession())
			{
				//Do async operations
			}
		}

		public async void StoreAsync()
		{
			using (var asyncSession = DocumentStore.OpenAsyncSession())
			{
				var entity = new Company {Name = "Hibernating Rhinos", Id = "companies/1"};

				asyncSession.Store(entity);
				await asyncSession.SaveChangesAsync(); 
			}
		}

		public void LoadAsync()
		{
			using (var asyncSession = DocumentStore.OpenAsyncSession())
			{
				var company = asyncSession.LoadAsync<Company>(1);
			}
		}

		public void LoadAsyncAndWork()
		{
			using (var asyncSession = DocumentStore.OpenAsyncSession())
			{
				asyncSession.LoadAsync<Company>(1).ContinueWith(task =>
				{
					var company = task.Result;
					//Do work on company
				});
			}
		}

		public void QuaryAsync()
		{
			using (var asyncSession = DocumentStore.OpenAsyncSession())
			{
				var quary = asyncSession.Query<Company>()
				                        .Where(company => company.Name == "Hibernating Rhinos")
				                        .ToListAsync();
			}
		}
	}
}
