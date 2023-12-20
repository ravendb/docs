using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes.Spatial;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.Spatial;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class MakeSpatialQuery
    {
        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region spatial_1
                    // This query will return all matching employee entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Define a dynamic query on Employees collection
                    List<Employee> employeesWithinRadius = session
                        .Query<Employee>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            pointField => pointField.Point(
                                x => x.Address.Location.Latitude, 
                                x => x.Address.Location.Longitude),
                            // Set the geographical area in which to search for matching documents
                            // Call 'WithinRadius', pass the radius and the center points coordinates  
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_1_1
                    // This query will return all matching employee entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Define a dynamic query on Employees collection
                    List<Employee> employeesWithinRadius = await asyncSession
                        .Query<Employee>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            pointField => pointField.Point(
                                x => x.Address.Location.Latitude, 
                                x => x.Address.Location.Longitude),
                            // Set the geographical area in which to search for matching documents
                            // Call 'WithinRadius', pass the radius and the center points coordinates  
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_1_2
                    // This query will return all matching employee entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Define a dynamic query on Employees collection
                    List<Employee> employeesWithinRadius = session.Advanced
                        .DocumentQuery<Employee>()
                        // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            pointField => pointField.Point(
                                x => x.Address.Location.Latitude, 
                                x => x.Address.Location.Longitude),
                            // Set the geographical area in which to search for matching documents
                            // Call 'WithinRadius', pass the radius and the center points coordinates  
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_2
                    // This query will return all matching employee entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Define a dynamic query on Employees collection
                    List<Employee> employeesWithinShape = session
                        .Query<Employee>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            factory => factory.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude),
                            // Set the geographical search criteria, call 'RelatesToShape'
                            criteria => criteria.RelatesToShape(
                                // Specify the WKT string. Note: longitude is written FIRST
                                shapeWkt: "CIRCLE(-122.3060097 47.623473 d=20)",
                                // Specify the relation between the WKT shape and the documents spatial data
                                relation: SpatialRelation.Within,
                                // Optional: customize radius units (default is Kilometers)
                                units: SpatialUnits.Miles))  
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_2_1
                    // This query will return all matching employee entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Define a dynamic query on Employees collection
                    List<Employee> employeesWithinShape = await asyncSession
                        .Query<Employee>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            factory => factory.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude),
                            // Set the geographical search criteria, call 'RelatesToShape'
                            criteria => criteria.RelatesToShape(
                                // Specify the WKT string. Note: longitude is written FIRST
                                shapeWkt: "CIRCLE(-122.3060097 47.623473 d=20)",
                                // Specify the relation between the WKT shape and the documents spatial data
                                relation: SpatialRelation.Within,
                                // Optional: customize radius units (default is Kilometers)
                                units: SpatialUnits.Miles))  
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_2_2
                    // This query will return all matching employee entities
                    // that are located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Define a dynamic query on Employees collection
                    List<Employee> employeesWithinShape = session.Advanced
                        .DocumentQuery<Employee>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            factory => factory.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude),
                            // Set the geographical search criteria, call 'RelatesToShape'
                            criteria => criteria.RelatesToShape(
                                // Specify the WKT string. Note: longitude is written FIRST
                                shapeWkt: "CIRCLE(-122.3060097 47.623473 d=20)",
                                // Specify the relation between the WKT shape and the documents spatial data
                                relation: SpatialRelation.Within,
                                // Optional: customize radius units (default is Kilometers)
                                units: SpatialUnits.Miles))  
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_3
                    // This query will return all matching company entities
                    // that are located within the specified polygon.
                    
                    // Define a dynamic query on Companies collection
                    List<Company> companiesWithinShape = session
                        .Query<Company>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            factory => factory.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude),
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
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_3_1
                    // This query will return all matching company entities
                    // that are located within the specified polygon.
                    
                    // Define a dynamic query on Companies collection
                    List<Company> companiesWithinShape = await asyncSession
                        .Query<Company>()
                         // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            factory => factory.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude),
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
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_3_2
                    // This query will return all matching company entities
                    // that are located within the specified polygon.
                    
                    // Define a dynamic query on Companies collection
                    List<Company> companiesWithinShape = session.Advanced
                        .DocumentQuery<Company>()
                        // Call 'Spatial' method
                        .Spatial(
                            // Call 'Point'
                            // Pass the path to the document fields containing the spatial data
                            factory => factory.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude),
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
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_4
                    // Return all matching employee entities located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Sort the results by their distance from a specified point,
                    // the closest results will be listed first.

                    List<Employee> employeesSortedByDistance = session
                        .Query<Employee>()
                         // Provide the query criteria:
                        .Spatial(
                            pointField => pointField.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                         // Call 'OrderByDistance'
                        .OrderByDistance(
                            factory => factory.Point(
                                // Pass the path to the document fields containing the spatial data
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                        .ToList();
                    #endregion
                    
                    #region spatial_4_getDistance
                    // Get the distance of the results:
                    // ================================
                    
                    // Call 'GetMetadataFor', pass an entity from the resulting employees list
                    var metadata = session.Advanced.GetMetadataFor(employeesSortedByDistance[0]);

                    // The distance is available in the '@spatial' metadata property
                    var spatialResults = (IDictionary<string, object>)metadata[Constants.Documents.Metadata.SpatialResult];
                    
                    var distance = spatialResults["Distance"];   // The distance of the entity from the queried location
                    var latitude = spatialResults["Latitude"];   // The entity's longitude value
                    var longitude = spatialResults["Longitude"]; // The entity's longitude value
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_4_1
                    // Return all matching employee entities located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Sort the results by their distance from a specified point,
                    // the closest results will be listed first.
                    
                    List<Employee> employeesSortedByDistance = await asyncSession
                        .Query<Employee>()
                         // Provide the query criteria:
                        .Spatial(
                            pointField => pointField.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                         // Call 'OrderByDistance'
                        .OrderByDistance(
                            factory => factory.Point(
                                // Pass the path to the document fields containing the spatial data
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_4_2
                    // Return all matching employee entities located within 20 kilometers radius
                    // from point (47.623473 latitude, -122.3060097 longitude).
                    
                    // Sort the results by their distance from a specified point,
                    // the closest results will be listed first.

                    List<Employee> employeesSortedByDistance = session.Advanced
                        .DocumentQuery<Employee>()
                         // Provide the query criteria:
                        .Spatial(
                            pointField => pointField.Point(
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            criteria => criteria.WithinRadius(20, 47.623473, -122.3060097))
                         // Call 'OrderByDistance'
                        .OrderByDistance(
                            factory => factory.Point(
                                // Pass the path to the document fields containing the spatial data
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_5
                    // Return all employee entities sorted by their distance from a specified point.
                    // The farthest results will be listed first.

                    List<Employee> employeesSortedByDistanceDesc = session
                        .Query<Employee>()
                         // Call 'OrderByDistanceDescending'
                        .OrderByDistanceDescending(
                            factory => factory.Point(
                                // Pass the path to the document fields containing the spatial data
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            // Sort the results by their distance (descending) from this point: 
                            47.623473, -122.3060097)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_5_1
                    // Return all employee entities sorted by their distance from a specified point.
                    // The farthest results will be listed first.
                    
                    List<Employee> employeesSortedByDistanceDesc = await asyncSession
                        .Query<Employee>()
                         // Call 'OrderByDistanceDescending'
                        .OrderByDistanceDescending(
                            factory => factory.Point(
                                // Pass the path to the document fields containing the spatial data
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            // Sort the results by their distance (descending) from this point: 
                            47.623473, -122.3060097)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_5_2
                    // Return all employee entities sorted by their distance from a specified point.
                    // The farthest results will be listed first.
                    
                    List<Employee> employeesSortedByDistanceDesc = session.Advanced
                        .DocumentQuery<Employee>()
                         // Call 'OrderByDistanceDescending'
                        .OrderByDistanceDescending(
                            factory => factory.Point(
                                // Pass the path to the document fields containing the spatial data
                                x => x.Address.Location.Latitude,
                                x => x.Address.Location.Longitude
                            ),
                            // Sort the results by their distance (descending) from this point: 
                            47.623473, -122.3060097)
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_6
                    // Return all employee entities.
                    // Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
                    // A secondary sort can be applied within the 100 km range, e.g. by field LastName.

                    List<Employee> employeesSortedByRoundedDistance = session
                        .Query<Employee>()
                         // Call 'OrderByDistance'
                        .OrderByDistance(
                            factory => factory.Point(
                                    // Pass the path to the document fields containing the spatial data
                                    x => x.Address.Location.Latitude,
                                    x => x.Address.Location.Longitude)
                                 // Round up distance to 100 km 
                                .RoundTo(100),
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                         // A secondary sort can be applied
                        .ThenBy(x => x.LastName)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_6_1
                    // Return all employee entities.
                    // Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
                    // A secondary sort can be applied within the 100 km range, e.g. by field LastName.

                    List<Employee> employeesSortedByRoundedDistance = await asyncSession
                        .Query<Employee>()
                         // Call 'OrderByDistance'
                        .OrderByDistance(
                            factory => factory.Point(
                                    // Pass the path to the document fields containing the spatial data
                                    x => x.Address.Location.Latitude,
                                    x => x.Address.Location.Longitude)
                                 // Round up distance to 100 km 
                                .RoundTo(100),
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                         // A secondary sort can be applied
                        .ThenBy(x => x.LastName)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region spatial_6_2
                    // Return all employee entities.
                    // Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
                    // A secondary sort can be applied within the 100 km range, e.g. by field LastName.

                    List<Employee> employeesSortedByRoundedDistance = session.Advanced
                        .DocumentQuery<Employee>()
                         // Call 'OrderByDistance'
                        .OrderByDistance(
                            factory => factory.Point(
                                    // Pass the path to the document fields containing the spatial data
                                    x => x.Address.Location.Latitude,
                                    x => x.Address.Location.Longitude)
                                 // Round up distance to 100 km 
                                .RoundTo(100),
                            // Sort the results by their distance from this point: 
                            47.623473, -122.3060097)
                         // A secondary sort can be applied
                        .OrderBy(x => x.LastName)
                        .ToList();
                    #endregion
                }
            }
        }
        
        private interface IFoo<T>
        {
            #region spatial_7
            IRavenQueryable<T> Spatial<T>(
                Expression<Func<T, object>> path,
                Func<SpatialCriteriaFactory, SpatialCriteria> clause);

            IRavenQueryable<T> Spatial<T>(
                string fieldName,
                Func<SpatialCriteriaFactory, SpatialCriteria> clause);

            IRavenQueryable<T> Spatial<T>(
                Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> field,
                Func<SpatialCriteriaFactory, SpatialCriteria> clause);

            IRavenQueryable<T> Spatial<T>(
                DynamicSpatialField field,
                Func<SpatialCriteriaFactory, SpatialCriteria> clause);
            #endregion

            #region spatial_8
            PointField Point(
                Expression<Func<T, object>> latitudePath,
                Expression<Func<T, object>> longitudePath);

            WktField Wkt(Expression<Func<T, object>> wktPath);
            #endregion

            #region spatial_9
            SpatialCriteria RelatesToShape(
                string shapeWkt,
                SpatialRelation relation,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria RelatesToShape(
                string shapeWkt,
                SpatialRelation relation,
                SpatialUnits units,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Intersects(
                string shapeWkt,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Intersects(
                string shapeWkt,
                SpatialUnits units,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Contains(
                string shapeWkt,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Contains(
                string shapeWkt,
                SpatialUnits units,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Disjoint(
                string shapeWkt,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Disjoint(
                string shapeWkt,
                SpatialUnits units,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Within(
                string shapeWkt,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria Within(
                string shapeWkt,
                SpatialUnits units,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);

            SpatialCriteria WithinRadius(
                double radius,
                double latitude,
                double longitude,
                SpatialUnits? radiusUnits = null,
                double distErrorPercent = Constants.Documents.Indexing.Spatial.DefaultDistanceErrorPct);
            #endregion
            
            #region spatial_10
            // From point
            IOrderedQueryable<T> OrderByDistance<T>(
                Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> field,
                double latitude,
                double longitude);

            IOrderedQueryable<T> OrderByDistance<T>(
                DynamicSpatialField field,
                double latitude,
                double longitude);

            IOrderedQueryable<T> OrderByDistance<T>(
                Expression<Func<T, object>> path,
                double latitude,
                double longitude);

            IOrderedQueryable<T> OrderByDistance<T>(
                string fieldName,
                double latitude,
                double longitude);

            // From center of WKT shape
            IOrderedQueryable<T> OrderByDistance<T>(
                Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> field,
                string shapeWkt);

            IOrderedQueryable<T> OrderByDistance<T>(
                DynamicSpatialField field,
                string shapeWkt);

            IOrderedQueryable<T> OrderByDistance<T>(
                Expression<Func<T, object>> path,
                string shapeWkt);

            IOrderedQueryable<T> OrderByDistance<T>(
                string fieldName,
                string shapeWkt);

            // Rounding
            IOrderedQueryable<T> OrderByDistance<T>(
                Expression<Func<T, object>> path,
                double latitude,
                double longitude,
                double roundFactor);

            IOrderedQueryable<T> OrderByDistance<T>(
                string fieldName,
                double latitude,
                double longitude,
                double roundFactor);
            
            IOrderedQueryable<T> OrderByDistance<T>(
                Expression<Func<T, object>> path,
                string shapeWkt,
                double roundFactor);

            IOrderedQueryable<T> OrderByDistance<T>(
                string fieldName,
                string shapeWkt,
                double roundFactor);
            #endregion

            #region spatial_11
            // From point
            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> field,
                double latitude,
                double longitude);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                DynamicSpatialField field,
                double latitude,
                double longitude);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                Expression<Func<T, object>> path,
                double latitude,
                double longitude);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                string fieldName,
                double latitude,
                double longitude);

            // From center of WKT shape
            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                Func<DynamicSpatialFieldFactory<T>, DynamicSpatialField> field,
                string shapeWkt);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                DynamicSpatialField field,
                string shapeWkt);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                Expression<Func<T, object>> path,
                string shapeWkt);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                string fieldName,
                string shapeWkt);

            // Rounding
            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                Expression<Func<T, object>> path,
                double latitude,
                double longitude,
                double roundFactor);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                string fieldName,
                double latitude,
                double longitude,
                double roundFactor);
            
            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                Expression<Func<T, object>> path,
                string shapeWkt,
                double roundFactor);

            IOrderedQueryable<T> OrderByDistanceDescending<T>(
                string fieldName,
                string shapeWkt,
                double roundFactor);
            #endregion
        }
    }
}
