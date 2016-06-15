package net.ravendb.clientapi.session.querying;

import net.ravendb.abstractions.data.FacetQuery;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.Camera;


public class HowToPerformMultiFacetedSearch {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region multi_facet_1
    public FacetResults[] multiFacetedSearch(FacetQuery... queries);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToPerformMultiFacetedSearch() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region multi_facet_2
        FacetQuery facetQuery1 = session.query(Camera.class)
          .toFacetQuery("facets/CameraFacets1");
        FacetQuery facetQuery2 = session.query(Camera.class)
          .toFacetQuery("facets/CameraFacets2");

        FacetResults[] results = session.advanced().multiFacetedSearch(facetQuery1, facetQuery2);
        FacetResults facetResults1 = results[0];
        FacetResults facetResults2 = results[1];
        //endregion
      }
    }
  }
}
