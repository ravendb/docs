using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetMetadata
	{
		private interface IFoo
		{
            #region get_metadata_1
		    IMetadataDictionary GetMetadataFor<T>(T instance);
			#endregion
		}

		public GetMetadata()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region get_metadata_2
					Employee employee = session.Load<Employee>("employees/1-A");
					IMetadataDictionary metadata = session.Advanced.GetMetadataFor(employee);
					#endregion
				}
			}
		}
	}
}
