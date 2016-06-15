package net.ravendb.clientapi.commands.querying;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.data.Facet;
import net.ravendb.abstractions.data.FacetMode;
import net.ravendb.abstractions.data.FacetQuery;
import net.ravendb.abstractions.data.FacetResult;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.data.FacetSetup;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class HowToWorkWithFacetQuery {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_facets_2
    public FacetResults getFacets(String index, IndexQuery query, String facetSetupDoc);

    public FacetResults getFacets(String index, IndexQuery query, String facetSetupDoc, int start);

    public FacetResults getFacets(String index, IndexQuery query, String facetSetupDoc, int start, Integer pageSize);
    //endregion

    //region get_facets_1
    public FacetResults getFacets(String index, IndexQuery query, List<Facet> facets) ;

    public FacetResults getFacets(String index, IndexQuery query, List<Facet> facets, int start) ;

    public FacetResults getFacets(String index, IndexQuery query, List<Facet> facets, int start, Integer pageSize) ;
    //endregion

    //region get_facets_5
    public FacetResults[] getMultiFacets(FacetQuery[] facetedQueries);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToWorkWithFacetQuery() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {

      //region get_facets_3
      // For the Manufacturer field look at the documents and return a count for each unique Term found
      // For the Cost field, return the count of the following ranges:
      //      Cost <= 200.0
      //      200.0 <= Cost <= 400.0
      //      400.0 <= Cost <= 600.0
      //      600.0 <= Cost <= 800.0
      //      Cost >= 800.0
      // For the Megapixels field, return the count of the following ranges:
      //      Megapixels <= 3.0
      //      3.0 <= Megapixels <= 7.0
      //      7.0 <= Megapixels <= 10.0
      //      Megapixels >= 10.0

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
      FacetResults facetResults = store.getDatabaseCommands().getFacets("Camera/Costs", new IndexQuery(), facets);
      FacetResult manufacturerResults = facetResults.getResults().get("Manufacturer");
      FacetResult costResults = facetResults.getResults().get("Cost_Range");
      FacetResult megapixelResults = facetResults.getResults().get("Megapixels_Range");
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region get_facets_4
      // For the Manufacturer field look at the documents and return a count for each unique Term found
      // For the Cost field, return the count of the following ranges:
      //      Cost <= 200.0
      //      200.0 <= Cost <= 400.0
      //      400.0 <= Cost <= 600.0
      //      600.0 <= Cost <= 800.0
      //      Cost >= 800.0
      // For the Megapixels field, return the count of the following ranges:
      //      Megapixels <= 3.0
      //      3.0 <= Megapixels <= 7.0
      //      7.0 <= Megapixels <= 10.0
      //      Megapixels >= 10.0

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
      FacetSetup facetsSetup = new FacetSetup("facets/CameraFacets", facets);
      store.getDatabaseCommands().put("facets/CameraFacets", null, RavenJObject.fromObject(facetsSetup), new RavenJObject());

      FacetResults facetResults = store.getDatabaseCommands().getFacets("Camera/Costs", new IndexQuery(), "facets/CameraFacets");
      FacetResult manufacturerResults = facetResults.getResults().get("Manufacturer");
      FacetResult costResults = facetResults.getResults().get("Cost_Range");
      FacetResult megapixelResults = facetResults.getResults().get("Megapixels_Range");
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region get_facets_6
      FacetQuery facetsQuery1 = new FacetQuery();
      facetsQuery1.setIndexName("Camera/Costs");
      facetsQuery1.setFacetSetupDoc("facets/CameraFacets1");
      facetsQuery1.setQuery(new IndexQuery());

      FacetQuery facetsQuery2 = new FacetQuery();
      facetsQuery2.setIndexName("Camera/Costs");
      facetsQuery2.setFacetSetupDoc("facets/CameraFacets2");
      facetsQuery2.setQuery(new IndexQuery());

      FacetQuery facetsQuery3 = new FacetQuery();
      facetsQuery3.setIndexName("Camera/Costs");
      facetsQuery3.setFacetSetupDoc("facets/CameraFacets3");
      facetsQuery3.setQuery(new IndexQuery());

      FacetResults[] facetResults = store.getDatabaseCommands().getMultiFacets(new FacetQuery[] {
        facetsQuery1, facetsQuery2, facetsQuery3
      });
      Map<String, FacetResult> facetResults1 = facetResults[0].getResults();
      Map<String, FacetResult> facetResults2 = facetResults[1].getResults();
      Map<String, FacetResult> facetResults3 = facetResults[2].getResults();
      //endregion
    }
  }
}
