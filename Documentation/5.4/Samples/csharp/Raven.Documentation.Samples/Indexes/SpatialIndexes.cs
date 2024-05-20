using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Spatial;

namespace Raven.Documentation.Samples.Indexes
{
    #region spatial_index_1
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
                    
                    // Documents can be retrieved
                    // by making a spatial query on the 'Coordinates' index-field
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
    
    #region spatial_index_2
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

                    // Documents can be retrieved
                    // by making a spatial query on the 'WKT' index-field
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

    #region spatial_index_3
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

    #region spatial_index_4
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
    
    #region spatial_index_5
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

    public class QuerySpatialIndex
    {
        public QuerySpatialIndex()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region spatial_query_1
                    // Define a spatial query on index 'Events_ByNameAndCoordinates'
                    List<Event> employeesWithinRadius = session
                        .Query<Event, Events_ByNameAndCoordinates>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Pass the spatial index-field containing the spatial data
                            "Coordinates",
                            // Set the geographical area in which to search for matching documents
                            // Call 'WithinRadius', pass the radius and the center points coordinates  
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                        .ToList();

                    // The query returns all matching Event entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_query_2
                    // Define a spatial query on index 'Events_ByNameAndCoordinates'
                    List<Event> employeesWithinRadius = session.Advanced
                        .DocumentQuery<Event, Events_ByNameAndCoordinates>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Pass the spatial index-field containing the spatial data
                            "Coordinates",
                            // Set the geographical area in which to search for matching documents
                            // Call 'WithinRadius', pass the radius and the center points coordinates  
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                        .ToList();
                    
                    // The query returns all matching Event entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_query_3
                    // Define a spatial query on index 'EventsWithWKT_ByNameAndWKT'
                    List<EventWithWKT> employeesWithinShape = session
                        .Query<EventWithWKT, EventsWithWKT_ByNameAndWKT>()
                        // Call 'Spatial' method
                        .Spatial(
                            // Pass the spatial index-field containing the spatial data
                            "WKT",
                            // Set the geographical search criteria, call 'RelatesToShape'
                            criteria => criteria.RelatesToShape(
                                // Specify the WKT string
                                shapeWkt: @"POLYGON ((
                                               -118.6527948 32.7114894,
                                               -95.8040242 37.5929338,
                                               -102.8344151 53.3349629,
                                               -127.5286633 48.3485664,
                                               -129.4620208 38.0786067,
                                               -118.7406746 32.7853769,
                                               -118.6527948 32.7114894
                                          ))",
                                // Specify the relation between the WKT shape and the documents spatial data
                                relation: SpatialRelation.Within))
                        .ToList();
                    
                    // The query returns all matching Event entities
                    // that are located within the specified polygon.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_query_4
                    // Define a spatial query on index 'EventsWithWKT_ByNameAndWKT'
                    List<EventWithWKT> employeesWithinShape = session.Advanced
                        .DocumentQuery<EventWithWKT, EventsWithWKT_ByNameAndWKT>()
                        // Call 'Spatial' method
                        .Spatial(
                            // Pass the spatial index-field containing the spatial data
                            "WKT",
                            // Set the geographical search criteria, call 'RelatesToShape'
                            criteria => criteria.RelatesToShape(
                                // Specify the WKT string
                                shapeWkt: @"POLYGON ((
                                               -118.6527948 32.7114894,
                                               -95.8040242 37.5929338,
                                               -102.8344151 53.3349629,
                                               -127.5286633 48.3485664,
                                               -129.4620208 38.0786067,
                                               -118.7406746 32.7853769,
                                               -118.6527948 32.7114894
                                          ))",
                                // Specify the relation between the WKT shape and the documents spatial data
                                relation: SpatialRelation.Within))
                        .ToList();
                    
                    // The query returns all matching Event entities
                    // that are located within the specified polygon.
                   #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_query_5
                    // Define a spatial query on index 'Events_ByNameAndCoordinates'
                    List<Event> employeesSortedByDistance = session
                        .Query<Event, Events_ByNameAndCoordinates>()
                         // Filter results by geographical criteria
                        .Spatial(
                            "Coordinates",
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                         // Sort results, call 'OrderByDistance'
                        .OrderByDistance(
                            // Pass the spatial index-field containing the spatial data
                            "Coordinates",
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                        .ToList();

                    // Return all matching Event entities located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).

                    // Sort the results by their distance from a specified point,
                    // the closest results will be listed first.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_query_6
                    // Define a spatial query on index 'Events_ByNameAndCoordinates'
                    List<Event> employeesSortedByDistance = session.Advanced
                        .DocumentQuery<Event, Events_ByNameAndCoordinates>()
                         // Filter results by geographical criteria
                        .Spatial(
                            "Coordinates",
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                         // Sort results, call 'OrderByDistance'
                        .OrderByDistance(
                            // Pass the spatial index-field containing the spatial data
                            "Coordinates",
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                        .ToList();

                    // Return all matching Event entities located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).

                    // Sort the results by their distance from a specified point,
                    // the closest results will be listed first.
                    #endregion
                }
            }
        }
    }
    
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
