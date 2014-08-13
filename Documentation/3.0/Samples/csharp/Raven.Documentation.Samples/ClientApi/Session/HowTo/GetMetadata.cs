using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class GetMetadata
	{
		private interface IFoo
		{
			#region get_metadata_1
			RavenJObject GetMetadataFor<T>(T instance);
			#endregion
		}

		public GetMetadata()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region get_metadata_2
					var person = session.Load<Person>("people/1");
					var metadata = session.Advanced.GetMetadataFor(person);
					#endregion
				}
			}
		}
	}
}