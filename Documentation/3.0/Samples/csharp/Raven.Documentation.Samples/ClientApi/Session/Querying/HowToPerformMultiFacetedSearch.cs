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
					FacetQuery facetQuery1 = session.Query<Camera>()
						.ToFacetQuery("facets/CameraFacets1");

					FacetQuery facetQuery2 = session.Query<Camera>()
						.ToFacetQuery("facets/CameraFacets2");

					FacetResults[] results = session
						.Advanced
						.MultiFacetedSearch(facetQuery1, facetQuery2);

					FacetResults facetResults1 = results[0];
					FacetResults facetResults2 = results[1];
					#endregion
				}
			}
		}
	}
}