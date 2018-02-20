using System.IO;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;


namespace Raven.Documentation.Samples.Indexes
{
    public class Analyzers
    {
        private class SnowballAnalyzer
        {
        }

        #region analyzers_1
        public class BlogPosts_ByTagsAndContent : AbstractIndexCreationTask<BlogPost>
        {
            public BlogPosts_ByTagsAndContent()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   post.Tags,
                                   post.Content
                               };

                Analyzers.Add(x => x.Tags, "SimpleAnalyzer");
                Analyzers.Add(x => x.Content, typeof(SnowballAnalyzer).AssemblyQualifiedName);
            }
        }
        #endregion

        #region analyzers_3
        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            public Employees_ByFirstAndLastName()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                       LastName = employee.LastName,
                                       FirstName = employee.FirstName
                                   };

                Indexes.Add(x => x.FirstName, FieldIndexing.Exact);
            }
        }
        #endregion

        #region analyzers_4
        public class BlogPosts_ByContent : AbstractIndexCreationTask<BlogPost>
        {
            public BlogPosts_ByContent()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   Title = post.Title,
                                   Content = post.Content
                               };

                Indexes.Add(x => x.Content, FieldIndexing.Search);
            }
        }
        #endregion

        #region analyzers_5
        public class BlogPosts_ByTitle : AbstractIndexCreationTask<BlogPost>
        {
            public BlogPosts_ByTitle()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   Title = post.Title,
                                   Content = post.Content
                               };

                Indexes.Add(x => x.Content, FieldIndexing.No);
                Stores.Add(x => x.Content, FieldStorage.Yes);
            }
        }
        #endregion

        public Analyzers()
        {
            using (var store = new DocumentStore())
            {
                #region analyzers_2
                store.Maintenance.Send(new PutIndexesOperation(new IndexDefinitionBuilder<BlogPost>("BlogPosts/ByTagsAndContent")
                {
                    Map = posts => from post in posts
                                   select new
                                   {
                                       post.Tags,
                                       post.Content
                                   },
                    Analyzers =
                    {
                        {x => x.Tags, "SimpleAnalyzer"},
                        {x => x.Content, typeof(SnowballAnalyzer).AssemblyQualifiedName}
                    }
                }.ToIndexDefinition(store.Conventions)));
                #endregion
            }
        }
    }

    /*
    #region analyzers_6
    public class MyAnalyzer : Lucene.Net.Analysis.Analyzer
    {
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            throw new CodeOmitted();
        }
    }
    #endregion
    */
}
