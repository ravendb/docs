using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
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
					var employee = session.Load<Employee>("employees/1");
					var etag = session.Advanced.GetEtagFor(employee);
					#endregion
				}
			}
		}
	}
}