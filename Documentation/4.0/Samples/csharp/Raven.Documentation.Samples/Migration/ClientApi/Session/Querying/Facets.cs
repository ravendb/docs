using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Facets;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Facets
    {
        private class Foo
        {
            public Foo()
            {
                /*
                #region facets_1_0
                List<Facet> facets = new List<Facet>
                {
                    new Facet
                    {
                        Name = "Manufacturer"
                    },
                    new Facet<Camera>
                    {
                        Name = x => x.Cost,
                        Ranges =
                        {
                            x => x.Cost < 200m,
                            x => x.Cost > 200m && x.Cost < 400m,
                            x => x.Cost > 400m && x.Cost < 600m,
                            x => x.Cost > 600m && x.Cost < 800m,
                            x => x.Cost > 800m
                        }
                    },
                    new Facet<Camera>
                    {
                        Name = x => x.Megapixels,
                        Ranges =
                        {
                            x => x.Megapixels < 3.0,
                            x => x.Megapixels > 3.0 && x.Megapixels < 7.0,
                            x => x.Megapixels > 7.0 && x.Megapixels < 10.0,
                            x => x.Megapixels > 10.0
                        }
                    }
                };

                session.Store(new FacetSetup { Id = "facets/CameraFacets", Facets = facets });
                #endregion
                */

                /*
                #region facets_2_0
                Dictionary<string, FacetResult> facetResults = session
                    .Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                    .Where(x => x.Cost >= 100 && x.Cost <= 300)
                    .AggregateBy(facets)
                    .Execute();
                #endregion
                */

                /*
                #region facets_3_0
                FacetResults facetResults = session
                    .Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                    .Where(x => x.Cost >= 100 && x.Cost <= 300)
                    .ToFacets("facets/CameraFacets");
                #endregion
                */
            }
        }

        public Facets()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region facets_1_1
                    List<Facet> facets = new List<Facet>
                    {
                        new Facet
                        {
                            FieldName = "Manufacturer"
                        }
                    };

                    List<RangeFacet> rangeFacets = new List<RangeFacet>
                    {
                        new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                x => x.Cost < 200m,
                                x => x.Cost > 200m && x.Cost < 400m,
                                x => x.Cost > 400m && x.Cost < 600m,
                                x => x.Cost > 600m && x.Cost < 800m,
                                x => x.Cost > 800m
                            }
                        },
                        new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                x => x.Megapixels < 3.0,
                                x => x.Megapixels > 3.0 && x.Megapixels < 7.0,
                                x => x.Megapixels > 7.0 && x.Megapixels < 10.0,
                                x => x.Megapixels > 10.0
                            }
                        }
                    };

                    session.Store(new FacetSetup { Id = "facets/CameraFacets", Facets = facets, RangeFacets = rangeFacets });
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    List<FacetBase> facets = new List<FacetBase>();

                    #region facets_2_1
                    Dictionary<string, FacetResult> facetResults = session
                        .Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                        .Where(x => x.Cost >= 100 && x.Cost <= 300)
                        .AggregateBy(facets)
                        .Execute();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region facets_3_1
                    Dictionary<string, FacetResult> facetResults = session
                        .Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                        .Where(x => x.Cost >= 100 && x.Cost <= 300)
                        .AggregateUsing("facets/CameraFacets")
                        .Execute();
                    #endregion
                }
            }
        }

        private class Cameras_ByManufacturerModelCostDateOfListingAndMegapixels : AbstractIndexCreationTask<Camera>
        {
            public Cameras_ByManufacturerModelCostDateOfListingAndMegapixels()
            {
                Map = cameras => from camera in cameras
                                 select new
                                 {
                                     camera.Manufacturer,
                                     camera.Model,
                                     camera.Cost,
                                     camera.DateOfListing,
                                     camera.Megapixels
                                 };
            }
        }
    }
}
