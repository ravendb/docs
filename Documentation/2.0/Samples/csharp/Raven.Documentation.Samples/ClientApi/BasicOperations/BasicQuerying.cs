namespace RavenCodeSamples.ClientApi.BasicOperations
{
	using System.Linq;

	using Raven.Client.Linq;

	public class BasicQuerying : CodeSampleBase
	{
		public void Basic()
		{
			using (var store = this.NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region basic_querying_1
					var results = from blog in session.Query<BlogPost>()
								  where blog.Category == "RavenDB"
								  select blog;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basic_querying_2
					var results = session.Query<BlogPost>()
						.Where(x => x.Comments.Length >= 10)
						.ToList();
					#endregion
				}
			}
		}
	}
}