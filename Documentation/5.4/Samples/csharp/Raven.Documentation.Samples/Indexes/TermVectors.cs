using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.Indexes
{
    namespace Foo
    {
        #region term_vectors_3
        public enum FieldTermVector
        {
            /// <summary>
            /// Do not store term vectors
            /// </summary>
            No,

            /// <summary>
            /// Store the term vectors of each document. A term vector is a list of the document's
            /// terms and their number of occurrences in that document.
            /// </summary>
            Yes,

            /// <summary>
            /// Store the term vector + token position information
            /// </summary>
            WithPositions,

            /// <summary>
            /// Store the term vector + Token offset information
            /// </summary>
            WithOffsets,

            /// <summary>
            /// Store the term vector + Token position and offset information
            /// </summary>
            WithPositionsAndOffsets
        }
        #endregion
    }

    public class TermVectors
    {
        #region term_vectors_1
        public class BlogPosts_ByTagsAndContent : AbstractIndexCreationTask<BlogPost>
        {
            public BlogPosts_ByTagsAndContent()
            {
                Map = users => from doc in users
                               select new
                               {
                                   doc.Tags,
                                   doc.Content
                               };

                Indexes.Add(x => x.Content, FieldIndexing.Search);
                TermVectors.Add(x => x.Content, FieldTermVector.WithPositionsAndOffsets);
            }
        }
        #endregion

        public TermVectors()
        {
            using (var store = new DocumentStore())
            {
                #region term_vectors_2
                IndexDefinitionBuilder<BlogPost> indexDefinitionBuilder =
                    new IndexDefinitionBuilder<BlogPost>("BlogPosts/ByTagsAndContent")
                    {
                        Map = users => from doc in users
                                       select new
                                       {
                                           doc.Tags,
                                           doc.Content
                                       },
                        Indexes =
                        {
                            { x => x.Content, FieldIndexing.Search }
                        },
                        TermVectors =
                        {
                            { x => x.Content, FieldTermVector.WithPositionsAndOffsets }
                        }
                    };

                IndexDefinition indexDefinition = indexDefinitionBuilder
                    .ToIndexDefinition(store.Conventions);

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
        }
    }
}
