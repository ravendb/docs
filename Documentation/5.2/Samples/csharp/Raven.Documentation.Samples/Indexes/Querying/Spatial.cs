using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Spatial;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Spatial
    {
        public class Event
        {
            public string Name { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        #region spatial_3_2
        public class Events_ByCoordinates : AbstractIndexCreationTask<Event>
        {
            public Events_ByCoordinates()
            {
                Map = events => from e in events
                                select new
                                {
                                    Coordinates = CreateSpatialField(e.Latitude, e.Longitude)
                                };
            }
        }
        #endregion

        public Spatial()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region spatial_1_0
                    List<Event> results = session
                        .Query<Event>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(500, 30, 30))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_1_1
                    List<Event> results = session
                        .Advanced
                        .DocumentQuery<Event>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.WithinRadius(500, 30, 30))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_2_0
                    List<Event> results = session
                        .Query<Event>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.RelatesToShape(
                                shapeWkt: "Circle(30 30 d=500.0000)",
                                relation: SpatialRelation.Within))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_2_1
                    List<Event> results = session
                        .Advanced
                        .DocumentQuery<Event>()
                        .Spatial(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude),
                            criteria => criteria.RelatesToShape(
                                shapeWkt: "Circle(30 30 d=500.0000)",
                                relation: SpatialRelation.Within))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_3_0
                    List<Event> results = session
                        .Query<Event, Events_ByCoordinates>()
                        .Spatial(
                            "Coordinates",
                            criteria => criteria.WithinRadius(500, 30, 30))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region spatial_3_1
                    List<Event> results = session
                        .Advanced
                        .DocumentQuery<Event>()
                        .Spatial(
                            "Coordinates",
                            criteria => criteria.WithinRadius(500, 30, 30))
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
