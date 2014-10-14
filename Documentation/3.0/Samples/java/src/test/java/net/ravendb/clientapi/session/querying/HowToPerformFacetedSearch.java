package net.ravendb.clientapi.session.querying;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.data.Facet;
import net.ravendb.abstractions.data.FacetMode;
import net.ravendb.abstractions.data.FacetQuery;
import net.ravendb.abstractions.data.FacetResult;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.data.FacetSetup;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.tests.faceted.Camera;


public class HowToPerformFacetedSearch {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region facet_1
    public FacetResults toFacets(List<Facet> facets);

    public FacetResults toFacets(List<Facet> facets, int start);

    public FacetResults toFacets(List<Facet> facets, int start, Integer pageSize);

    public FacetResults toFacets(String facetSetupDoc);

    public FacetResults toFacets(String facetSetupDoc, int start);

    public FacetResults toFacets(String facetSetupDoc, int start, Integer pageSize);
    //endregion

    //region facet_4
    public FacetQuery toFacetQuery(List<Facet> facets);

    public FacetQuery toFacetQuery(List<Facet> facets, int start);

    public FacetQuery toFacetQuery(List<Facet> facets, int start, Integer pageSize);

    public FacetQuery toFacetQuery(String facetSetupDoc);

    public FacetQuery toFacetQuery(String facetSetupDoc, int start);

    public FacetQuery toFacetQuery(String facetSetupDoc, int start, Integer pageSize);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToPerformFacetedSearch() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region facet_2
        // passing facets directly
        Facet facet1 = new Facet();
        facet1.setName("Manufacturer");

        Facet facet2 = new Facet();
        facet2.setName("Cost_Range");
        facet2.setMode(FacetMode.RANGES);
        facet2.setRanges(Arrays.asList("[NULL TO Dx200.0]",
                                          "[Dx300.0 TO Dx400.0]",
                                          "[Dx500.0 TO Dx600.0]",
                                          "[Dx700.0 TO Dx800.0]",
                                          "[Dx900.0 TO NULL]"));

        Facet facet3 = new Facet();
        facet3.setName("Megapixels_Range");
        facet3.setMode(FacetMode.RANGES);
        facet3.setRanges(Arrays.asList( "[NULL TO Dx3.0]",
                                          "[Dx4.0 TO Dx7.0]",
                                          "[Dx8.0 TO Dx10.0]",
                                          "[Dx11.0 TO NULL]"));

        List<Facet> facets = Arrays.asList(facet1, facet2, facet3);

        FacetResults facetsResult = session.query(Camera.class).toFacets(facets);
        //TODO: duration
        Map<String, FacetResult> results = facetsResult.getResults();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region facet_3
        // using predefined facet setup
        Facet facet1 = new Facet();
        facet1.setName("Manufacturer");

        Facet facet2 = new Facet();
        facet2.setName("Cost_Range");
        facet2.setMode(FacetMode.RANGES);
        facet2.setRanges(Arrays.asList("[NULL TO Dx200.0]",
                                          "[Dx300.0 TO Dx400.0]",
                                          "[Dx500.0 TO Dx600.0]",
                                          "[Dx700.0 TO Dx800.0]",
                                          "[Dx900.0 TO NULL]"));

        Facet facet3 = new Facet();
        facet3.setName("Megapixels_Range");
        facet3.setMode(FacetMode.RANGES);
        facet3.setRanges(Arrays.asList( "[NULL TO Dx3.0]",
                                          "[Dx4.0 TO Dx7.0]",
                                          "[Dx8.0 TO Dx10.0]",
                                          "[Dx11.0 TO NULL]"));

        List<Facet> facets = Arrays.asList(facet1, facet2, facet3);
        session.store(new FacetSetup("facets/CameraFacets", facets));
        session.saveChanges();

        FacetResults facetResults = session.query(Camera.class, "Camera/Costs").toFacets("facets/CameraFacets");
        //TODO: duration
        Map<String, FacetResult> results = facetResults.getResults();
        //endregion
      }

      try(IDocumentSession session = store.openSession()) {
        //region facet_5
        FacetQuery facetQuery1 = session.query(Camera.class)
          .toFacetQuery("facets/CameraFacets1");

        FacetQuery facetQuery2 = session.query(Camera.class)
          .toFacetQuery("facets/CameraFacets2");

        FacetResults[] results = session.advanced()
          .multiFacetedSearch(facetQuery1, facetQuery2);

        FacetResults facetResults1 = results[0];
        FacetResults facetResults2 = results[1];
        //endregion
      }
    }
  }
}
