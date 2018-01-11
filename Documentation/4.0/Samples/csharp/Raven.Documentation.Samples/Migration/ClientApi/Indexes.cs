using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Spatial;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Migration.ClientApi
{
    public class Indexes
    {
        public Indexes()
        {
            /*
            #region indexes_1
            Sort(x => x.Month, SortOptions.Int);
            #endregion
            */
        }

        /*
        #region indexes_2
        public class Index : AbstractIndexCreationTask
        {
            public override IndexDefinition CreateIndexDefinition()
            {
                return new IndexDefinition()
                {
                    Map = @"from item in docs.Items select new 
                            {
                                Field = item.Field 
                            }",
                    Stores = { { "Field", FieldStorage.Yes } },
                    Indexes = { { "Field", FieldIndexing.NotAnalyzed } },
                    SuggestionsOptions = { "Field" },
                    SpatialIndexes = { { "Field", new SpatialOptions() } },
                    TermVectors = { { "Field", FieldTermVector.WithOffsets } }
                };
            }
        }
        #endregion
         */

        #region indexes_3
        public class Index : AbstractIndexCreationTask
        {
            public override IndexDefinition CreateIndexDefinition()
            {
                return new IndexDefinition()
                {
                    Maps =
                    {
                        @"from item in docs.Items select new 
                            {
                                Field = item.Field 
                            }"
                    },
                    Fields =
                    {
                        {
                            "Field", new IndexFieldOptions
                            {
                                Storage = FieldStorage.Yes,
                                Indexing = FieldIndexing.Exact,
                                Suggestions = true,
                                Spatial = new SpatialOptions(),
                                TermVector = FieldTermVector.WithOffsets
                            }
                        }
                    }
                };
            }
        }
        #endregion
    }
}
