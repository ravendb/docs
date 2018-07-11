using System.Linq;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Spatial;

namespace Raven.Documentation.Samples.Indexes
{
    namespace Foo
    {
        #region spatial_search_enhancements_3
        public class SpatialOptionsFactory
        {
            public GeographySpatialOptionsFactory Geography;

            public CartesianSpatialOptionsFactory Cartesian;
        }
        #endregion

        public interface GeographySpatialOptionsFactory
        {
            #region spatial_search_enhancements_4
            // GeohashPrefixTree strategy with maxTreeLevel set to 9
            SpatialOptions Default(SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

            SpatialOptions BoundingBoxIndex(SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

            SpatialOptions GeohashPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

            SpatialOptions QuadPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);
            #endregion
        }

        public interface CartesianSpatialOptionsFactory
        {
            #region spatial_search_enhancements_5
            SpatialOptions BoundingBoxIndex();

            SpatialOptions QuadPrefixTreeIndex(int maxTreeLevel, SpatialBounds bounds);
            #endregion
        }

        public interface Foo
        {
            #region spatial_search_0
            object CreateSpatialField(double? lat, double? lng);

            object CreateSpatialField(string shapeWkt);
            #endregion
        }
    }

    #region spatial_search_2
    public class EventWithWKT
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WKT { get; set; }
    }

    public class EventsWithWKT_ByNameAndWKT : AbstractIndexCreationTask<EventWithWKT>
    {
        public EventsWithWKT_ByNameAndWKT()
        {
            Map = events => from e in events
                            select new
                            {
                                Name = e.Name,
                                WKT = CreateSpatialField(e.WKT)
                            };
        }
    }
    #endregion

    #region spatial_search_1
    public class Event
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class Events_ByNameAndCoordinates : AbstractIndexCreationTask<Event>
    {
        public Events_ByNameAndCoordinates()
        {
            Map = events => from e in events
                            select new
                            {
                                Name = e.Name,
                                Coordinates = CreateSpatialField(e.Latitude, e.Longitude)
                            };
        }
    }
    #endregion

    #region spatial_search_3
    public class Events_ByNameAndCoordinates_Custom : AbstractIndexCreationTask<Event>
    {
        public Events_ByNameAndCoordinates_Custom()
        {
            Map = events => from e in events
                            select new
                            {
                                Name = e.Name,
                                Coordinates = CreateSpatialField(e.Latitude, e.Longitude)
                            };

            Spatial("Coordinates", factory => factory.Cartesian.BoundingBoxIndex());
        }
    }
    #endregion

    public class SpatialIndexes
    {
    }
}
