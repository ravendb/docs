package net.ravendb.Indexes;

import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;

public class SpatialIndexes {

    /*
    //region spatial_search_enhancements_3
    public static class SpatialOptionsFactory {
        public GeographySpatialOptionsFactory geography()

        public CartesianSpatialOptionsFactory cartesian()
    }
    //endregion
    */

    /*
    //region spatial_search_enhancements_4
    public SpatialOptions defaultOptions()

    public SpatialOptions defaultOptions(SpatialUnits circleRadiusUnits)

    public SpatialOptions boundingBoxIndex()

    public SpatialOptions boundingBoxIndex(SpatialUnits circleRadiusUnits)

    public SpatialOptions geohashPrefixTreeIndex(int maxTreeLevel)

    public SpatialOptions geohashPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits)

    public SpatialOptions quadPrefixTreeIndex(int maxTreeLevel)

    public SpatialOptions quadPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits)
    //endregion

    //region spatial_search_enhancements_5
    public SpatialOptions boundingBoxIndex()

    public SpatialOptions quadPrefixTreeIndex(int maxTreeLevel, SpatialBounds bounds)
    //endregion

    //region spatial_search_0
    object CreateSpatialField(double? lat, double? lng);

    object CreateSpatialField(string shapeWkt);
    //endregion
     */

    //region spatial_search_2
    public static class EventWithWKT {
        private String id;
        private String name;
        private String wkt;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getWkt() {
            return wkt;
        }

        public void setWkt(String wkt) {
            this.wkt = wkt;
        }
    }

    public static class EventsWithWKT_ByNameAndWKT extends AbstractIndexCreationTask {
        public EventsWithWKT_ByNameAndWKT() {
            map = "docs.EventWithWKTs.Select(e => new { " +
                "    name = e.name, " +
                "    wkt = this.CreateSpatialField(e.wkt) " +
                "})";
        }
    }
    //endregion

    //region spatial_search_1
    public static class Event {
        private String id;
        private String name;
        private double latitude;
        private double longitude;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

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

    public static class Events_ByNameAndCoordinates extends AbstractIndexCreationTask {
        public Events_ByNameAndCoordinates() {
            map = "docs.Events.Select(e => new { " +
                "    name = e.name, " +
                "    coordinates = this.CreateSpatialField(((double ? ) e.latitude), ((double ? ) e.longitude)) " +
                "})";
        }
    }
    //endregion

    //region spatial_search_3
    public static class Events_ByNameAndCoordinates_Custom extends AbstractIndexCreationTask {
        public Events_ByNameAndCoordinates_Custom() {
            map = "docs.Events.Select(e => new { " +
                "    name = e.name, " +
                "    coordinates = this.CreateSpatialField(((double ? ) e.latitude), ((double ? ) e.longitude)) " +
                "})";

            spatial("coordinates", f -> f.cartesian().boundingBoxIndex());
        }
    }
    //endregion

}
