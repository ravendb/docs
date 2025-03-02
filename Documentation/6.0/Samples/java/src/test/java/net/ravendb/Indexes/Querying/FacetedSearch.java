package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.queries.facets.*;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;

public class FacetedSearch {

    public static class Camera {
    }

    //region step_2
    public class Cameras_ByManufacturerModelCostDateOfListingAndMegapixels extends AbstractIndexCreationTask {
        public Cameras_ByManufacturerModelCostDateOfListingAndMegapixels() {
            map = "from camera in docs.Cameras " +
                "select new {" +
                "   camera.manufacturer," +
                "   camera.model," +
                "   camera.cost," +
                "   camera.dateOfListing," +
                "   camera.megapixels" +
                "} ";
        }
    }
    //endregion

    public void step1() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region step_1
                Facet facet1 = new Facet();
                facet1.setFieldName("manufacturer");

                RangeFacet facet2 = new RangeFacet();
                facet2.setRanges(Arrays.asList(
                    "cost <= 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800"
                ));

                RangeFacet facet3 = new RangeFacet();
                facet3.setRanges(Arrays.asList(
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10"
                ));

                List<Facet> facets = Arrays.asList(facet1);
                List<RangeFacet> rangeFacets = Arrays.asList(facet2, facet3);

                //endregion

                //region step_4_0
                FacetSetup facetSetup = new FacetSetup();
                facetSetup.setFacets(facets);
                facetSetup.setRangeFacets(rangeFacets);

                session.store(facetSetup, "facets/CameraFacets");
                //endregion
            }
        }
    }

    public void step3() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                Facet[] facets = new Facet[0];

                //region step_3_0
                Map<String, FacetResult> facetResults = session
                    .query(Camera.class, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels.class)
                    .whereBetween("cost", 100, 300)
                    .aggregateBy(facets)
                    .execute();
                //endregion
            }
        }
    }

    public void step4() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region step_4_1
                Map<String, FacetResult> facetResults = session
                    .query(Camera.class, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels.class)
                    .whereBetween("cost", 100, 300)
                    .aggregateUsing("facets/CameraFacets")
                    .execute();
                //endregion
            }
        }
    }
}
