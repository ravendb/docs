using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;

namespace Raven.Documentation.CodeSamples.Indexes
{
	#region boosting_2
	public class Users_ByName : AbstractIndexCreationTask<User>
	{
		public Users_ByName()
		{
			Map = users => from user in users
						   select new
								{
									FirstName = user.FirstName.Boost(10),
									LastName = user.LastName
								};
		}
	}

	#endregion

	public class Boosting
	{
		#region boosting_1
		public class User
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }
		}
		#endregion

		public Boosting()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region boosting_3
					session.Query<User, Users_ByName>()
						   .Where(x => x.FirstName == "Bob" || x.LastName == "Bob")
						   .ToList();

					#endregion
				}
			}
		}
	}
}