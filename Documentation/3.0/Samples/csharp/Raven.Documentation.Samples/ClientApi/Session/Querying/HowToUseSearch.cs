using System.Collections.Generic;
using System.Linq;

using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToUseSearch
	{
		private interface IFoo
		{
			/*
			#region search_1
			IRavenQueryable<T> Search<T>(
				this IQueryable<T> self,
				Expression<Func<T, object>> fieldSelector,
				string searchTerms,
				decimal boost = 1,
				SearchOptions options = SearchOptions.Guess,
				EscapeQueryOptions escapeQueryOptions = EscapeQueryOptions.EscapeAll) { ... }
			#endregion
			*/
		}

		private class User
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public byte Age { get; set; }

			public ICollection<string> Hobbies { get; set; }
		}

		public HowToUseSearch()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_2
					var users = session
						.Query<User>("Users/ByNameAndHobbies")
						.Search(x => x.Name, "Adam")
						.Search(x => x.Hobbies, "sport")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_3
					var users = session
						.Query<User>("Users/ByHobbies")
						.Search(x => x.Hobbies, "I love sport", boost: 10)
						.Search(x => x.Hobbies, "but also like reading books", boost: 5)
						.ToList();
					#endregion
				}
			}
		}
	}
}