using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Facets;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.Facets
{
    public class Foo
    {
        #region facet_7_3
        public class Facet
        {
            public string FieldName { get; set; }

            public FacetOptions Options { get; set; }

            public Dictionary<FacetAggregation, string> Aggregations { get; set; }

            public string DisplayFieldName { get; set; }
        }

        public class Facet<T>
        {
            public Expression<Func<T, object>> FieldName { get; set; }

            public FacetOptions Options { get; set; }

            public Dictionary<FacetAggregation, string> Aggregations { get; set; }

            public string DisplayFieldName { get; set; }
        }
        #endregion

        #region facet_7_4
        public class RangeFacet
        {
            public List<string> Ranges { get; set; }

            public FacetOptions Options { get; set; }

            public Dictionary<FacetAggregation, string> Aggregations { get; set; }

            public string DisplayFieldName { get; set; }
        }

        public class RangeFacet<T>
        {
            public List<Expression<Func<T, bool>>> Ranges { get; set; }

            public FacetOptions Options { get; set; }

            public Dictionary<FacetAggregation, string> Aggregations { get; set; }

            public string DisplayFieldName { get; set; }
        }
        #endregion

        #region facet_7_5
        public enum FacetAggregation
        {
            None,
            Max,
            Min,
            Average,
            Sum
        }
        #endregion
    }
}

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToPerformFacetedSearch
    {
        private interface IFoo<T>
        {
            #region facet_1
            IAggregationQuery<T> AggregateBy<T>(FacetBase facet);

            IAggregationQuery<T> AggregateBy<T>(IEnumerable<FacetBase> facets);

            IAggregationQuery<T> AggregateBy<T>(Action<IFacetBuilder<T>> builder);

            IAggregationQuery<T> AggregateUsing<T>(string facetSetupDocumentKey);
            #endregion

            #region facet_7_1
            IFacetOperations<T> ByRanges(Expression<Func<T, bool>> path, params Expression<Func<T, bool>>[] paths);

            IFacetOperations<T> ByField(Expression<Func<T, object>> path);

            IFacetOperations<T> ByField(string fieldName);

            IFacetOperations<T> WithDisplayName(string displayName);

            IFacetOperations<T> WithOptions(FacetOptions options);

            IFacetOperations<T> SumOn(Expression<Func<T, object>> path);

            IFacetOperations<T> MinOn(Expression<Func<T, object>> path);

            IFacetOperations<T> MaxOn(Expression<Func<T, object>> path);

            IFacetOperations<T> AverageOn(Expression<Func<T, object>> path);
            #endregion
        }

        private class Foo1
        {
            #region facet_7_2
            public FacetTermSortMode TermSortMode { get; set; } = FacetTermSortMode.ValueAsc;

            public bool IncludeRemainingTerms { get; set; }

            public int Start { get; set; }

            public int PageSize { get; set; } = int.MaxValue;
            #endregion
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region facet_2_1
                    Dictionary<string, FacetResult> facets = session
                        .Query<Camera>("Camera/Costs")
                        .AggregateBy(new Facet
                        {
                            FieldName = "Manufacturer",
                            Options = new FacetOptions
                            {
                                TermSortMode = FacetTermSortMode.CountDesc
                            }
                        })
                        .AndAggregateBy(new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                camera => camera.Cost < 200m,
                                camera => camera.Cost >= 200m && camera.Cost < 400m,
                                camera => camera.Cost >= 400m && camera.Cost < 600m,
                                camera => camera.Cost >= 600m && camera.Cost < 800m,
                                camera => camera.Cost >= 800m
                            },
                            Aggregations =
                            {
                                { FacetAggregation.Average, "Cost" }
                            }
                        })
                        .AndAggregateBy(new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                camera => camera.Megapixels < 3.0,
                                camera => camera.Megapixels >= 3.0 && camera.Megapixels < 7.0,
                                camera => camera.Megapixels >= 7.0 && camera.Megapixels < 10.0,
                                camera => camera.Megapixels >= 10.0
                            }
                        })
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region facet_2_2
                    Dictionary<string, FacetResult> facets = await asyncSession
                        .Query<Camera>("Camera/Costs")
                        .AggregateBy(new Facet
                        {
                            FieldName = "Manufacturer",
                            Options = new FacetOptions
                            {
                                TermSortMode = FacetTermSortMode.CountDesc
                            }
                        })
                        .AndAggregateBy(new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                camera => camera.Cost < 200m,
                                camera => camera.Cost >= 200m && camera.Cost < 400m,
                                camera => camera.Cost >= 400m && camera.Cost < 600m,
                                camera => camera.Cost >= 600m && camera.Cost < 800m,
                                camera => camera.Cost >= 800m
                            },
                            Aggregations =
                            {
                                { FacetAggregation.Average, "Cost" }
                            }
                        })
                        .AndAggregateBy(new RangeFacet<Camera>
                        {
                            Ranges =
                            {
                                camera => camera.Megapixels < 3.0,
                                camera => camera.Megapixels >= 3.0 && camera.Megapixels < 7.0,
                                camera => camera.Megapixels >= 7.0 && camera.Megapixels < 10.0,
                                camera => camera.Megapixels >= 10.0
                            }
                        })
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region facet_3_1
                    Dictionary<string, FacetResult> facets = session
                        .Query<Camera>("Camera/Costs")
                        .AggregateBy(builder => builder
                            .ByField(x => x.Manufacturer)
                            .WithOptions(new FacetOptions
                            {
                                TermSortMode = FacetTermSortMode.CountDesc
                            }))
                        .AndAggregateBy(builder => builder
                            .ByRanges(
                                camera => camera.Cost < 200m,
                                camera => camera.Cost >= 200m && camera.Cost < 400m,
                                camera => camera.Cost >= 400m && camera.Cost < 600m,
                                camera => camera.Cost >= 600m && camera.Cost < 800m,
                                camera => camera.Cost >= 800m)
                            .AverageOn(x => x.Cost))
                        .AndAggregateBy(builder => builder
                            .ByRanges(
                                camera => camera.Megapixels < 3.0,
                                camera => camera.Megapixels >= 3.0 && camera.Megapixels < 7.0,
                                camera => camera.Megapixels >= 7.0 && camera.Megapixels < 10.0,
                                camera => camera.Megapixels >= 10.0))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region facet_3_2
                    Dictionary<string, FacetResult> facets = await asyncSession
                        .Query<Camera>("Camera/Costs")
                        .AggregateBy(builder => builder
                            .ByField(x => x.Manufacturer)
                            .WithOptions(new FacetOptions
                            {
                                TermSortMode = FacetTermSortMode.CountDesc
                            }))
                        .AndAggregateBy(builder => builder
                            .ByRanges(
                                camera => camera.Cost < 200m,
                                camera => camera.Cost >= 200m && camera.Cost < 400m,
                                camera => camera.Cost >= 400m && camera.Cost < 600m,
                                camera => camera.Cost >= 600m && camera.Cost < 800m,
                                camera => camera.Cost >= 800m)
                            .AverageOn(x => x.Cost))
                        .AndAggregateBy(builder => builder
                            .ByRanges(
                                camera => camera.Megapixels < 3.0,
                                camera => camera.Megapixels >= 3.0 && camera.Megapixels < 7.0,
                                camera => camera.Megapixels >= 7.0 && camera.Megapixels < 10.0,
                                camera => camera.Megapixels >= 10.0))
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region facet_4_1
                    session.Store(new FacetSetup
                    {
                        Facets = new List<Facet>
                            {
                                new Facet
                                {
                                    FieldName = "Manufacturer"
                                }
                            },
                        RangeFacets = new List<RangeFacet>
                            {
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
                            }
                    }, "facets/CameraFacets");

                    session.SaveChanges();

                    Dictionary<string, FacetResult> facets = session
                        .Query<Camera>("Camera/Costs")
                        .AggregateUsing("facets/CameraFacets")
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region facet_4_2
                    await asyncSession.StoreAsync(new FacetSetup
                    {
                        Facets = new List<Facet>
                            {
                                new Facet
                                {
                                    FieldName = "Manufacturer"
                                }
                            },
                        RangeFacets = new List<RangeFacet>
                            {
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
                            }
                    }, "facets/CameraFacets");

                    await asyncSession.SaveChangesAsync();

                    Dictionary<string, FacetResult> facets = await asyncSession
                        .Query<Camera>("Camera/Costs")
                        .AggregateUsing("facets/CameraFacets")
                        .ExecuteAsync();
                    #endregion
                }
            }
        }
    }
}
