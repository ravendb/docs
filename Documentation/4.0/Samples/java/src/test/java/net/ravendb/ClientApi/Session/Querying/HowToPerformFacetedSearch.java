package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.queries.facets.*;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.function.Consumer;

public class HowToPerformFacetedSearch {

    private static class Foo {
        //region facet_7_3
        public class Facet {
            private String fieldName;
            private FacetOptions options;
            private Map<FacetAggregation, String> aggregations;
            private String displayFieldName;

            public String getFieldName() {
                return fieldName;
            }

            public void setFieldName(String fieldName) {
                this.fieldName = fieldName;
            }

            public FacetOptions getOptions() {
                return options;
            }

            public void setOptions(FacetOptions options) {
                this.options = options;
            }

            public Map<FacetAggregation, String> getAggregations() {
                return aggregations;
            }

            public void setAggregations(Map<FacetAggregation, String> aggregations) {
                this.aggregations = aggregations;
            }

            public String getDisplayFieldName() {
                return displayFieldName;
            }

            public void setDisplayFieldName(String displayFieldName) {
                this.displayFieldName = displayFieldName;
            }
        }
        //endregion

        //region facet_7_4
        public class RangeFacet {
            private List<String> ranges;
            private FacetOptions options;
            private Map<FacetAggregation, String> aggregations;
            private String displayFieldName;

            public List<String> getRanges() {
                return ranges;
            }

            public void setRanges(List<String> ranges) {
                this.ranges = ranges;
            }

            public FacetOptions getOptions() {
                return options;
            }

            public void setOptions(FacetOptions options) {
                this.options = options;
            }

            public Map<FacetAggregation, String> getAggregations() {
                return aggregations;
            }

            public void setAggregations(Map<FacetAggregation, String> aggregations) {
                this.aggregations = aggregations;
            }

            public String getDisplayFieldName() {
                return displayFieldName;
            }

            public void setDisplayFieldName(String displayFieldName) {
                this.displayFieldName = displayFieldName;
            }
        }
        //endregion

        //region facet_7_5
        public enum FacetAggregation {
            NONE,
            MAX,
            MIN,
            AVERAGE,
            SUM;
        }
        //endregion
    }



    private interface IFoo<T> {

        //region facet_1
        IAggregationDocumentQuery<T> aggregateBy(Consumer<IFacetBuilder<T>> builder);

        IAggregationDocumentQuery<T> aggregateBy(FacetBase facet);

        IAggregationDocumentQuery<T> aggregateBy(Facet... facet);

        IAggregationDocumentQuery<T> aggregateUsing(String facetSetupDocumentId);
        //endregion

        //region facet_7_1
        IFacetOperations<T> byRanges(RangeBuilder range, RangeBuilder... ranges);

        IFacetOperations<T> byField(String fieldName);

        IFacetOperations<T> withDisplayName(String displayName);

        IFacetOperations<T> withOptions(FacetOptions options);

        IFacetOperations<T> sumOn(String path);
        IFacetOperations<T> minOn(String path);
        IFacetOperations<T> maxOn(String path);
        IFacetOperations<T> averageOn(String path);
        //endregion
    }

    private class Foo1 {
        //region facet_7_2
        private FacetTermSortMode termSortMode = FacetTermSortMode.VALUE_ASC;
        private boolean includeRemainingTerms;
        private int start;
        private int pageSize = Integer.MAX_VALUE;

        //getters and setters
        //endregion
    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region facet_2_1
                FacetOptions facetOptions = new FacetOptions();
                facetOptions.setTermSortMode(FacetTermSortMode.COUNT_DESC);

                Facet facet1 = new Facet();
                facet1.setFieldName("manufacturer");
                facet1.setOptions(facetOptions);

                RangeFacet facet2 = new RangeFacet();
                facet2.setRanges(Arrays.asList(
                    "cost < 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800"
                ));
                facet2.setAggregations(Collections.singletonMap(FacetAggregation.AVERAGE, "cost"));

                RangeFacet facet3 = new RangeFacet();
                facet3.setRanges(Arrays.asList(
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10"
                ));

                Map<String, FacetResult> facets = session
                    .query(Camera.class, Query.index("Camera/Costs"))
                    .aggregateBy(facet1)
                    .andAggregateBy(facet2)
                    .andAggregateBy(facet3)
                    .execute();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region facet_3_1
                FacetOptions options = new FacetOptions();
                options.setTermSortMode(FacetTermSortMode.COUNT_DESC);

                RangeBuilder<Integer> costBuilder = RangeBuilder.forPath("cost");
                RangeBuilder<Integer> megapixelsBuilder = RangeBuilder.forPath("megapixels");

                Map<String, FacetResult> facetResult = session
                    .query(Camera.class, Query.index("Camera/Costs"))
                    .aggregateBy(builder -> builder
                        .byField("manufacturer")
                        .withOptions(options))
                    .andAggregateBy(builder -> builder
                        .byRanges(
                            costBuilder.isLessThan(200),
                            costBuilder.isGreaterThanOrEqualTo(200).isLessThan(400),
                            costBuilder.isGreaterThanOrEqualTo(400).isLessThan(600),
                            costBuilder.isGreaterThanOrEqualTo(600).isLessThan(800),
                            costBuilder.isGreaterThanOrEqualTo(800))
                        .averageOn("cost"))
                    .andAggregateBy(builder -> builder
                        .byRanges(
                            megapixelsBuilder.isLessThan(3),
                            megapixelsBuilder.isGreaterThanOrEqualTo(3).isLessThan(7),
                            megapixelsBuilder.isGreaterThanOrEqualTo(7).isLessThan(10),
                            megapixelsBuilder.isGreaterThanOrEqualTo(10)
                        ))
                    .execute();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region facet_4_1
                FacetSetup facetSetup = new FacetSetup();

                Facet facetManufacturer = new Facet();
                facetManufacturer.setFieldName("manufacturer");
                facetSetup.setFacets(Arrays.asList(facetManufacturer));

                RangeFacet cameraFacet = new RangeFacet();
                cameraFacet.setRanges(Arrays.asList(
                    "cost < 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800"
                ));

                RangeFacet megapixelsFacet = new RangeFacet();
                megapixelsFacet.setRanges(Arrays.asList(
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10"
                ));

                facetSetup.setRangeFacets(Arrays.asList(cameraFacet, megapixelsFacet));

                session.store(facetSetup, "facets/CameraFacets");
                session.saveChanges();

                Map<String, FacetResult> facets = session
                    .query(Camera.class, Query.index("Camera/Costs"))
                    .aggregateUsing("facets/CameraFacets")
                    .execute();
                //endregion
            }
        }
    }

    private class Camera {
    }
}
