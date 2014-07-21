using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class GetEtag
	{
		private interface IFoo
		{
			#region get_etag_1
			Etag GetEtagFor<T>(T instance);
			#endregion
		}

		public GetEtag()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region get_etag_2
					var person = session.Load<Person>("people/1");
					var etag = session.Advanced.GetEtagFor(person);
					#endregion
				}
			}
		}
	}
}