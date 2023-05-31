using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Spatial;

namespace Raven.Documentation.Samples.Indexes
{
    #region spatial_1
    // Define an index with a spatial field
    public class Events_ByNameAndCoordinates : AbstractIndexCreationTask<Event>
    {
        public Events_ByNameAndCoordinates()
        {
            Map = events => from e in events
                select new
                {
                    Name = e.Name,
                    // Call 'CreateSpatialField' to create a spatial index-field
                    // Field 'Coordinates' will be composed of lat & lng supplied from the document
                    Coordinates = CreateSpatialField(e.Latitude, e.Longitude)
                    
                    // Documents can be retrieved by making a spatial query on this index-field
                };
        }
    }

    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    #endregion
    
    #region spatial_2
    // Define an index with a spatial field
    public class EventsWithWKT_ByNameAndWKT : AbstractIndexCreationTask<EventWithWKT>
    {
        public EventsWithWKT_ByNameAndWKT()
        {
            Map = events => from e in events
                select new
                {
                    Name = e.Name,
                    // Call 'CreateSpatialField' to create a spatial index-field
                    // Field 'WKT' will be composed of the WKT string supplied from the document
                    WKT = CreateSpatialField(e.WKT)

                    // Documents can be retrieved by making a spatial query on this index-field
                };
        }
    }

    public class EventWithWKT
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WKT { get; set; }
    }
    #endregion

    #region spatial_3
    public class Events_ByNameAndCoordinates_JS : AbstractJavaScriptIndexCreationTask
    {
        public Events_ByNameAndCoordinates_JS()
        {
            Maps = new HashSet<string>
            {
                @"map('events', function (e) {
                        return { 
                            Name: e.Name,
                            Coordinates: createSpatialField(e.Latitude, e.Longitude)
                        };
                })"
            };
        }
    }
    #endregion

    #region spatial_4
    public class Events_ByNameAndCoordinates_Custom : AbstractIndexCreationTask<Event>
    {
        public Events_ByNameAndCoordinates_Custom()
        {
            Map = events => from e in events
                            select new
                            {
                                Name = e.Name,
                                // Define a spatial index-field
                                Coordinates = CreateSpatialField(e.Latitude, e.Longitude)
                            };

            // Set the spatial indexing strategy for the spatial field 'Coordinates' 
            Spatial("Coordinates", factory => factory.Cartesian.BoundingBoxIndex());
        }
    }
    #endregion
    
    #region spatial_5
    public class Events_ByNameAndCoordinates_Custom_JS : AbstractJavaScriptIndexCreationTask
    {
        public Events_ByNameAndCoordinates_Custom_JS()
        {
            // Define index fields
            Maps = new HashSet<string>
            {
                @"map('events', function (e) {
                        return { 
                            Name: e.Name,
                            Coordinates: createSpatialField(e.Latitude, e.Longitude)
                        };
                })"
            };
            
            // Customize index fields
            Fields = new Dictionary<string, IndexFieldOptions>
            {
                ["Coordinates"] = new IndexFieldOptions
                {
                    Spatial = new SpatialOptions
                    {
                        Type = SpatialFieldType.Cartesian,
                        Strategy = SpatialSearchStrategy.BoundingBox
                    }
                }
            };
        }
    }
    #endregion
    
    namespace Foo
    {
        public interface Foo
        {
            #region spatial_syntax_1
            object CreateSpatialField(double? lat, double? lng); // Latitude/Longitude coordinates
            object CreateSpatialField(string shapeWkt);          // Shape in WKT string format
            #endregion
        }
        
        #region spatial_syntax_2
        public class SpatialOptionsFactory
        {
            public GeographySpatialOptionsFactory Geography;
            public CartesianSpatialOptionsFactory Cartesian;
        }
        #endregion

        public interface GeographySpatialOptionsFactory
        {
            #region spatial_syntax_3
            // Default is GeohashPrefixTree strategy with maxTreeLevel set to 9
            SpatialOptions Default(SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);
            
            SpatialOptions BoundingBoxIndex(SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);
            
            SpatialOptions GeohashPrefixTreeIndex(int maxTreeLevel,
                SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);
            
            SpatialOptions QuadPrefixTreeIndex(int maxTreeLevel,
                SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);
            #endregion
        }

        public interface CartesianSpatialOptionsFactory
        {
            #region spatial_syntax_4
            SpatialOptions BoundingBoxIndex();
            SpatialOptions QuadPrefixTreeIndex(int maxTreeLevel, SpatialBounds bounds);

            public class SpatialBounds
            {
                public double MinX;
                public double MaxX;
                public double MinY;
                public double MaxY;
            }
            #endregion
        }
    }
}
