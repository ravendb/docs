package net.ravendb.indexes.querying;

import java.util.ArrayList;
import java.util.List;

import net.ravendb.abstractions.data.Facet;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.data.FacetSetup;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.Camera;
import net.ravendb.samples.QCamera;


public class FacetedSearch {

  //region step_2
  public static class Cameras_ByManufacturerModelCostDateOfListingAndMegapixels extends AbstractIndexCreationTask {
    public Cameras_ByManufacturerModelCostDateOfListingAndMegapixels() {
      map =
       " from camera in docs.Cameras  " +
       " select new                   " +
       " {                            " +
       "     camera.Manufacturer,     " +
       "     camera.Model,            " +
       "     camera.Cost,             " +
       "     camera.DateOfListing,    " +
       "     camera.Megapixels        " +
       " }; ";
    }
  }
  //endregion

  @SuppressWarnings("boxing")
  public void step1() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region step_1
        QCamera c = QCamera.camera;

        List<Facet> facets = new ArrayList<>();
        Facet f1 = new Facet();
        f1.setName(c.manufacturer);
        facets.add(f1);

        Facet f2 = new Facet();
        f2.setName(c.cost);
        f2.setRanges(c.cost.lt(200),
            c.cost.gt(200).and(c.cost.lt(400)),
            c.cost.gt(400).and(c.cost.lt(600)),
            c.cost.gt(600).and(c.cost.lt(800)),
            c.cost.gt(800));
        facets.add(f2);

        Facet f3 = new Facet();
        f3.setName(c.megapixels);
        f3.setRanges(c.megapixels.lt(3),
            c.megapixels.gt(3).and(c.megapixels.lt(7)),
            c.megapixels.gt(7).and(c.megapixels.lt(10)),
            c.megapixels.gt(10));
        facets.add(f3);
        //endregion

        //region step_4_0
        session.store(new FacetSetup("facets/CameraFacets", facets));
        //endregion
      }
    }
  }

  @SuppressWarnings({"unused", "boxing"})
  public void step3() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        List<Facet> facets = new ArrayList<>();
        //region step_3_0
        QCamera c = QCamera.camera;
        FacetResults facetResults = session
          .query(Camera.class, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels.class)
          .where(c.cost.goe(100).and(c.cost.loe(300)))
          .toFacets(facets);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        List<Facet> facets = new ArrayList<>();
        //region step_3_1
        QCamera c = QCamera.camera;
        FacetResults facetResults = session
          .advanced()
          .documentQuery(Camera.class, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels.class)
          .whereBetweenOrEqual(c.cost, 100.0, 300.0)
          .toFacets(facets);
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      List<Facet> facets = new ArrayList<>();
      //region step_3_2
      FacetResults facetResults = store
        .getDatabaseCommands()
        .getFacets("Cameras/ByManufacturerModelCostDateOfListingAndMegapixels",
          new IndexQuery("Cost_Range:[Dx100 TO Dx300]"),
          facets);
      //endregion
    }
  }

  @SuppressWarnings({"unused", "boxing"})
  public void step4() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region step_4_1
        QCamera c = QCamera.camera;
        FacetResults facetResults = session
          .query(Camera.class, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels.class)
          .where(c.cost.goe(100).and(c.cost.loe(300)))
          .toFacets("facets/CameraFacets");
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region step_4_2
        QCamera c = QCamera.camera;
        FacetResults facetResults = session
          .advanced()
          .documentQuery(Camera.class, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels.class)
          .whereBetweenOrEqual(c.cost, 100.0, 300.0)
          .toFacets("facets/CameraFacets");
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region step_4_3
      FacetResults facetResults = store
        .getDatabaseCommands()
        .getFacets("Cameras/ByManufacturerModelCostDateOfListingAndMegapixels",
          new IndexQuery("Cost_Range:[Dx100 TO Dx300]"),
          "facets/CameraFacets");
      //endregion
    }
  }
}
