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

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToQuerySpatialIndex
    {
        private interface IFoo<T>
        {
            #region spatial_1
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

            #region spatial_2
            PointField Point(
                Expression<Func<T, object>> latitudePath,
                Expression<Func<T, object>> longitudePath);

            WktField Wkt(Expression<Func<T, object>> wktPath);
            #endregion

            #region spatial_3
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

            #region spatial_6
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
            #endregion

            #region spatial_8
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
            #endregion
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region spatial_4
                    // return all matching entities
                    // within 10 kilometers radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    List<House> results = session
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(10, 32.1234, 23.4321))
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_4_1
                    // return all matching entities
                    // within 10 kilometers radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    List<House> results = await asyncSession
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(10, 32.1234, 23.4321))
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_5
                    // return all matching entities
                    // within 10 miles radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    // this equals to WithinRadius(10, 32.1234, 23.4321)
                    List<House> results = session
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.RelatesToShape(
                                "Circle(32.1234 23.4321 d=10.0000)",
                                SpatialRelation.Within,
                                SpatialUnits.Miles))
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_5_1
                    // return all matching entities
                    // within 10 miles radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    // this equals to WithinRadius(10, 32.1234, 23.4321)
                    List<House> results = await asyncSession
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.RelatesToShape(
                                "Circle(32.1234 23.4321 d=10.0000)",
                                SpatialRelation.Within,
                                SpatialUnits.Miles))
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_7
                    // return all matching entities
                    // within 10 kilometers radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                    List<House> results = session
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(10, 32.1234, 23.4321))
                        .OrderByDistance(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude), 32.1234, 23.4321)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_7_1
                    // return all matching entities
                    // within 10 kilometers radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                    List<House> results = await asyncSession
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(10, 32.1234, 23.4321))
                        .OrderByDistance(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude), 32.1234, 23.4321)
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_9
                    // return all matching entities
                    // within 10 kilometers radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                    List<House> results = session
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(10, 32.1234, 23.4321))
                        .OrderByDistanceDescending(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude), 32.1234, 23.4321)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region spatial_9_1
                    // return all matching entities
                    // within 10 kilometers radius
                    // from 32.1234 latitude and 23.4321 longitude coordinates
                    // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                    List<House> results = await asyncSession
                        .Query<House>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(10, 32.1234, 23.4321))
                        .OrderByDistanceDescending(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude), 32.1234, 23.4321)
                        .ToListAsync();
                    #endregion
                }
            }
        }

        private class House
        {
            public string Name { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }
    }
}
