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

        #region setting_analyzers_1
        // The index definition
        public class BlogPosts_ByTagsAndContent : AbstractIndexCreationTask<BlogPost>
        {
            public class IndexEntry
            {
               public string[] Tags { get; set; }
               public string Content { get; set; }
            }
            
            public BlogPosts_ByTagsAndContent()
            {
                Map = posts => from post in posts
                               select new IndexEntry()
                               {
                                   Tags = post.Tags,
                                   Content = post.Content
                               };

                // Field 'Tags' will be tokenized by Lucene's SimpleAnalyzer
                Analyzers.Add(x => x.Tags, "SimpleAnalyzer");

                // Field 'Content' will be tokenized by the Custom analyzer SnowballAnalyzer
                Analyzers.Add(x => x.Content,
                    typeof(SnowballAnalyzer).AssemblyQualifiedName);
            }
        }
        #endregion

        #region use_search_analyzer
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

                // Set the Indexing Behavior:
                // ==========================
                
                // Set 'FieldIndexing.Search' on index-field 'Content'
                Indexes.Add(x => x.Content, FieldIndexing.Search);
                
                // => Index-field 'Content' will be processed by the "Default Search Analyzer"
                //    since no other analyzer is set.
            }
        }
        #endregion
        
        #region use_exact_analyzer
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

                // Set the Indexing Behavior:
                // ==========================
                
                // Set 'FieldIndexing.Exact' on index-field 'FirstName'
                Indexes.Add(x => x.FirstName, FieldIndexing.Exact);
                
                // => Index-field 'FirstName' will be processed by the "Default Exact Analyzer"
            }
        }
        #endregion

        #region no_indexing
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
                
                // Set the Indexing Behavior:
                // ==========================
                
                // Set 'FieldIndexing.No' on index-field 'Content'
                Indexes.Add(x => x.Content, FieldIndexing.No);
                
                // Set 'FieldStorage.Yes' to store the original content of field 'Content'
                // in the index
                Stores.Add(x => x.Content, FieldStorage.Yes);
                
                // => No analyzer will process field 'Content',
                //    it will only be stored in the index.
            }
        }
        #endregion

        #region use_default_analyzer
        public class Employees_ByFirstName : AbstractIndexCreationTask<Employee>
        {
            public Employees_ByFirstName()
            {
                Map = employees => from employee in employees
                    select new
                    {
                        LastName = employee.LastName
                    };

                // Index-field 'LastName' will be processed by the "Default Analyzer"
                // since:
                // * No analyzer was specified
                // * No Indexing Behavior was specified (neither Exact nor Search)
            }
        }
        #endregion

        public Analyzers()
        {
            using (var store = new DocumentStore())
            {
                #region setting_analyzers_2
                // The index definition
                var indexDefinition = new IndexDefinitionBuilder<BlogPost>("BlogPosts/ByTagsAndContent")
                {
                    Map = posts => from post in posts
                        select new {post.Tags, post.Content},
                    
                    Analyzers =
                    {
                        // Field 'Tags' will be tokenized by Lucene's SimpleAnalyzer
                        {x => x.Tags, "SimpleAnalyzer"},
                        
                        // Field 'Content' will be tokenized by the Custom analyzer SnowballAnalyzer
                        {x => x.Content, typeof(SnowballAnalyzer).AssemblyQualifiedName}
                    }
                }.ToIndexDefinition(store.Conventions);
                
                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
        }
        
        private class PutAnalyzersOperation
        {
            #region put_analyzers_1
            public PutAnalyzersOperation(params AnalyzerDefinition[] analyzersToAdd)
            #endregion
            {
            }
        }
        
        private class PutServerWideAnalyzersOperation
        {
            #region put_analyzers_2
            public PutServerWideAnalyzersOperation(params AnalyzerDefinition[] analyzersToAdd)
            #endregion
            {
            }
        }
    }

    #region put_analyzers_3
    public class AnalyzerDefinition
    {
        // Name of the analyzer
        public string Name { get; set; }
        
        // C# source-code of the analyzer
        public string Code { get; set; }
    }
    #endregion

    public class BlogPost
    {
        public string[] Tags { get; set; }
        public string Content { get; set; }
        public string Title { get; internal set; }
    }

    /*
        #region my_custom_analyzer
        public class MyAnalyzer : Lucene.Net.Analysis.Analyzer
        {
            public override TokenStream TokenStream(string fieldName, TextReader reader)
            {
                // Implement your analyzer's logic
                throw new CodeOmitted();
            }
        }
        #endregion
    */
    
    /*
        #region my_custom_analyzer_example
        // Define the put analyzer operation:
        var putAnalyzerOp = new PutAnalyzersOperation(new AnalyzerDefinition
        {
            // The name must be same as the analyzer's class name
            Name = "MyAnalyzer", 
                        
            Code = @"
                using System.IO;
                using Lucene.Net.Analysis;
                using Lucene.Net.Analysis.Standard;

                namespace MyAnalyzer
                {
                    public class MyAnalyzer : Lucene.Net.Analysis.Analyzer
                    {
                        public override TokenStream TokenStream(string fieldName, TextReader reader)
                        {
                            throw new CodeOmitted();
                        }
                    }
                }"
        });
        
        // Deploy the analyzer:
        store.Maintenance.ForDatabase("MyDatabase").Send(putAnalyzerOp);
        #endregion
    */
}
