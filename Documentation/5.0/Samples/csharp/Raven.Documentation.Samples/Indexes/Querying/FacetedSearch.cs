using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Facets;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class FacetedSearch
    {
        #region step_2
        public class Cameras_ByManufacturerModelCostDateOfListingAndMegapixels : AbstractIndexCreationTask<Camera>
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
        #endregion

        public void Step1()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region step_1
                    List<FacetBase> facets = new List<FacetBase>
                    {
                        new Facet
                        {
                            FieldName = "Manufacturer"
                        },
                        new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                camera => camera.Cost < 200m,
                                camera => camera.Cost >= 200m && camera.Cost < 400m,
                                camera => camera.Cost >= 400m && camera.Cost < 600m,
                                camera => camera.Cost >= 600m && camera.Cost < 800m,
                                camera => camera.Cost >= 800m
                            }
                        },
                        new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                camera => camera.Megapixels < 3.0,
                                camera => camera.Megapixels >= 3.0 && camera.Megapixels < 7.0,
                                camera => camera.Megapixels >= 7.0 && camera.Megapixels < 10.0,
                                camera => camera.Megapixels >= 10.0
                            }
                        }
                    };
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    var facets = new List<Facet>();
                    var rangeFacets = new List<RangeFacet>();

                    #region step_4_0
                    session.Store(new FacetSetup { Id = "facets/CameraFacets", Facets = facets, RangeFacets = rangeFacets });
                    #endregion
                }
            }
        }

        public void Step3()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    List<Facet> facets = new List<Facet>();

                    #region step_3_0
                    Dictionary<string, FacetResult> facetResults = session
                        .Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                        .Where(x => x.Cost >= 100 && x.Cost <= 300)
                        .AggregateBy(facets)
                        .Execute();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    List<Facet> facets = new List<Facet>();

                    #region step_3_1
                    Dictionary<string, FacetResult> facetResults = session
                        .Advanced
                        .DocumentQuery<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                        .WhereBetween(x => x.Cost, 100, 300)
                        .AggregateBy(facets)
                        .Execute();
                    #endregion
                }
            }
        }

        public void Step4()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    List<Facet> facets = new List<Facet>();

                    #region step_4_1
                    Dictionary<string, FacetResult> facetResults = session
                        .Query<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                        .Where(x => x.Cost >= 100 && x.Cost <= 300)
                        .AggregateUsing("facets/CameraFacets")
                        .Execute();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    List<Facet> facets = new List<Facet>();

                    #region step_4_2
                    Dictionary<string, FacetResult> facetResults = session
                        .Advanced
                        .DocumentQuery<Camera, Cameras_ByManufacturerModelCostDateOfListingAndMegapixels>()
                        .WhereBetween(x => x.Cost, 100, 300)
                        .AggregateUsing("facets/CameraFacets")
                        .Execute();
                    #endregion
                }
            }
        }
    }
}
