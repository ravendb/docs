using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToPerformMultiFacetedSearch
	{
		private interface IFoo
		{
			#region multi_facet_1
			FacetResults[] MultiFacetedSearch(params FacetQuery[] queries);
			#endregion
		}

		public HowToPerformMultiFacetedSearch()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region multi_facet_2
					var facetQuery1 = session.Query<Camera>()
						.ToFacetQuery("facets/CameraFacets1");

					var facetQuery2 = session.Query<Camera>()
						.ToFacetQuery("facets/CameraFacets2");

					var results = session
						.Advanced
						.MultiFacetedSearch(facetQuery1, facetQuery2);

					var facetResults1 = results[0];
					var facetResults2 = results[1];
					#endregion
				}
			}
		}
	}
}