using System.Collections.Generic;
using System.Linq;
#region spatial_2
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes.Spatial;
#endregion

using Raven.Client.Documents.Indexes;

/*
#region spatial_1
using Raven.Abstractions.Indexing;
using Raven.Client.Document;
#endregion
*/

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Spatial
    {
        private class Foo
        {
            public Foo()
            {
                /*
                #region spatial_3
                List<SpatialDoc> results = session
                    .Query<SpatialDoc, SpatialDoc_ByShapeAndPoint>()
                    .Customize(x => x
                        .RelatesToShape(
                            "Shape", 
                            "Circle(32.1234 23.4321 d=10.0000)", 
                            SpatialRelation.Within))
                    .ToList();
                #endregion
                */

                /*
                #region spatial_5
                List<SpatialDoc> results = session
                    .Query<SpatialDoc, SpatialDoc_ByShapeAndPoint>()
                    .Customize(x => x
                        .WithinRadiusOf("Shape", 10, 32.1234, 23.4321))
                    .ToList();
                #endregion
                */

                /*
                #region spatial_7
                List<SpatialDoc> results = session
                    .Query<SpatialDoc, SpatialDoc_ByShapeAndPoint>()
                    .Customize(x => x.SortByDistance())
                    .Spatial(
                        x => x.Shape, 
                        criteria => criteria
                            .WithinRadiusOf(10, 32.1234, 23.4321))
                    .ToList();
                #endregion
                */
            }

            /*
            #region spatial_index_1
            private class SpatialDoc_ByShapeAndPoint : AbstractIndexCreationTask<SpatialDoc>
            {
                public SpatialDoc_ByShapeAndPoint()
                {
                    Map = docs => from spatial in docs
                        select new
                        {
                            Shape = spatial.Shape,
                            Point = spatial.Point,
                            _ = SpatialGenerate("Coordinates", spatial.Point.Latitude, spatial.Point.Longitude),
                            _ = SpatialClustering("Clustering", spatial.Point.Latitude, spatial.Point.Longitude)
                        };

                    Spatial(x => x.Shape, options => options.Geography.Default());
                    Spatial(x => x.Point, options => options.Cartesian.BoundingBoxIndex());
                }
            }
            #endregion
            */
        }

        public Spatial()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region spatial_4
                    List<SpatialDoc> results = session
                        .Query<SpatialDoc, SpatialDoc_ByShapeAndPoint>()
                        .Spatial(
                            x => x.Shape,
                            criteria => criteria
                                .RelatesToShape(
                                    "Circle(32.1234 23.4321 d=10.0000)",
                                    SpatialRelation.Within))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_6
                    List<SpatialDoc> results = session
                        .Query<SpatialDoc, SpatialDoc_ByShapeAndPoint>()
                        .Spatial(
                            x => x.Shape,
                            criteria => criteria
                                .WithinRadius(10, 32.1234, 23.4321))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_8
                    List<SpatialDoc> results = session
                        .Query<SpatialDoc, SpatialDoc_ByShapeAndPoint>()
                        .Spatial(
                            x => x.Shape,
                            criteria => criteria
                                .WithinRadius(10, 32.1234, 23.4321))
                        .OrderByDistance(x => x.Shape, 32.1234, 23.4321)
                        .ToList();
                    #endregion
                }
            }
        }

        private class SpatialDoc
        {
            public Shape Shape { get; set; }

            public Point Point { get; set; }
        }

        public class Point
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        public class Shape
        {
            public string Wkt { get; set; }
        }

        #region spatial_index_2
        private class SpatialDoc_ByShapeAndPoint : AbstractIndexCreationTask<SpatialDoc>
        {
            public SpatialDoc_ByShapeAndPoint()
            {
                Map = docs => from spatial in docs
                              select new
                              {
                                  Shape = CreateSpatialField(spatial.Shape.Wkt),
                                  Point = CreateSpatialField(spatial.Point.Latitude, spatial.Point.Longitude),
                                  Coordinates = CreateSpatialField(spatial.Point.Latitude, spatial.Point.Longitude)
                              };

                Spatial(x => x.Shape, options => options.Geography.Default());
                Spatial(x => x.Point, options => options.Cartesian.BoundingBoxIndex());
            }
        }
        #endregion
    }
}
