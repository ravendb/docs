package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.Constants;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.spatial.SpatialRelation;
import net.ravendb.client.documents.indexes.spatial.SpatialUnits;
import net.ravendb.client.documents.queries.spatial.*;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;
import java.util.function.Function;

public class HowToQuerySpatialIndex {

    interface IFoo<T> {
        //region spatial_1
        IDocumentQuery<T> spatial(String fieldName, Function<SpatialCriteriaFactory, SpatialCriteria> clause);

        IDocumentQuery<T> spatial(DynamicSpatialField field, Function<SpatialCriteriaFactory, SpatialCriteria> clause);
        //endregion

        /*
        //region spatial_2
        public PointField(String latitude, String longitude)

        public WktField(String wkt)
        //endregion
        */

        //region spatial_3
        SpatialCriteria relatesToShape(String shapeWkt, SpatialRelation relation);

        SpatialCriteria relatesToShape(String shapeWkt, SpatialRelation relation, double distErrorPercent);

        SpatialCriteria intersects(String shapeWkt);

        SpatialCriteria intersects(String shapeWkt, double distErrorPercent) ;

        SpatialCriteria contains(String shapeWkt);

        SpatialCriteria contains(String shapeWkt, double distErrorPercent);

        SpatialCriteria disjoint(String shapeWkt);

        SpatialCriteria disjoint(String shapeWkt, double distErrorPercent);

        SpatialCriteria within(String shapeWkt);

        SpatialCriteria within(String shapeWkt, double distErrorPercent);

        SpatialCriteria withinRadius(double radius, double latitude, double longitude);

        SpatialCriteria withinRadius(double radius, double latitude, double longitude, SpatialUnits radiusUnits);

        SpatialCriteria withinRadius(double radius, double latitude, double longitude, SpatialUnits radiusUnits, double distErrorPercent);
        //endregion

        //region spatial_6
        IDocumentQuery<T> orderByDistance(DynamicSpatialField field, double latitude, double longitude);

        IDocumentQuery<T> orderByDistance(DynamicSpatialField field, String shapeWkt);

        IDocumentQuery<T> orderByDistance(String fieldName, double latitude, double longitude);

        IDocumentQuery<T> orderByDistance(String fieldName, String shapeWkt);
        //endregion

        //region spatial_8
        IDocumentQuery<T> orderByDistanceDescending(DynamicSpatialField field, double latitude, double longitude);

        IDocumentQuery<T> orderByDistanceDescending(DynamicSpatialField field, String shapeWkt);

        IDocumentQuery<T> orderByDistanceDescending(String fieldName, double latitude, double longitude);

        IDocumentQuery<T> orderByDistanceDescending(String fieldName, String shapeWkt);
        //endregion
    }



    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region spatial_4
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                List<House> results = session
                    .query(House.class)
                    .spatial(
                        new PointField("latitude", "longitude"),
                        f -> f.withinRadius(10, 32.1234, 23.4321))
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region spatial_5
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                // this equals to WithinRadius(10, 32.1234, 23.4321)
                List<House> results = session
                    .query(House.class)
                    .spatial(
                        new PointField("latitude", "longitude"),
                        f -> f.relatesToShape("Circle(32.1234 23.4321 d=10.0000)", SpatialRelation.WITHIN)
                    )
                    .toList();

                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region spatial_7
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                List<House> results = session
                    .query(House.class)
                    .spatial(
                        new PointField("latitude", "longitude"),
                        f -> f.withinRadius(10, 32.1234, 23.4321)
                    )
                    .orderByDistance(
                        new PointField("latitude", "longtude"),
                        32.12324, 23.4321)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region spatial_9
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                List<House> results = session
                    .query(House.class)
                    .spatial(
                        new PointField("latitude", "longitude"),
                        f -> f.withinRadius(10, 32.1234, 23.4321)
                    )
                    .orderByDistanceDescending(
                        new PointField("latitude", "longtude"),
                        32.12324, 23.4321)
                    .toList();
                //endregion
            }
        }
    }

    private static class House {
        private String name;
        private double latitude;
        private double longitude;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public double getLatitude() {
            return latitude;
        }

        public void setLatitude(double latitude) {
            this.latitude = latitude;
        }

        public double getLongitude() {
            return longitude;
        }

        public void setLongitude(double longitude) {
            this.longitude = longitude;
        }
    }
}
