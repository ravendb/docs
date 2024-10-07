using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Analysis;
using Raven.Client.Documents.Operations.Analyzers;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Searching
    {
        #region index_1
        public class Employees_ByNotes :
            AbstractIndexCreationTask<Employee, Employees_ByNotes.IndexEntry>
        {
            // The IndexEntry class defines the index-fields
            public class IndexEntry
            {
                public string EmployeeNotes { get; set; }
            }

            public Employees_ByNotes()
            {
                // The 'Map' function defines the content of the index-fields
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        EmployeeNotes = employee.Notes[0]
                    };

                // Configure the index-field for FTS:
                // Set 'FieldIndexing.Search' on index-field 'EmployeeNotes'
                Index(x => x.EmployeeNotes, FieldIndexing.Search);
                
                // Optionally: Set your choice of analyzer for the index-field.
                // Here the text from index-field 'EmployeeNotes' will be tokenized by 'WhitespaceAnalyzer'.
                Analyze(x => x.EmployeeNotes, "WhitespaceAnalyzer");

                // Note:
                // If no analyzer is set then the default 'RavenStandardAnalyzer' is used.
            }
        }
        #endregion
        
        #region index_2
        public class Employees_ByEmployeeData : 
            AbstractIndexCreationTask<Employee, Employees_ByEmployeeData.IndexEntry>
        {
            public class IndexEntry
            {
                public object[] EmployeeData { get; set; }
            }

            public Employees_ByEmployeeData()
            {
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        EmployeeData = new object[]
                        {
                            // Multiple document-fields can be indexed
                            // into the single index-field 'EmployeeData' 
                            employee.FirstName,
                            employee.LastName,
                            employee.Title,
                            employee.Notes
                        }
                    };

                // Configure the index-field for FTS:
                // Set 'FieldIndexing.Search' on index-field 'EmployeeData'
                Index(x => x.EmployeeData, FieldIndexing.Search);
                
                // Note:
                // Since no analyzer is set then the default 'RavenStandardAnalyzer' is used.
            }
        }
        #endregion
        
        #region index_3
        public class Employees_ByNotes_usingDefaultAnalyzer :
            AbstractIndexCreationTask<Employee, Employees_ByNotes_usingDefaultAnalyzer.IndexEntry>
        {
            public class IndexEntry
            {
                public string EmployeeNotes { get; set; }
            }

            public Employees_ByNotes_usingDefaultAnalyzer()
            {
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        EmployeeNotes = employee.Notes[0]
                    };

                // Configure the index-field for FTS:
                Index(x => x.EmployeeNotes, FieldIndexing.Search);

                // Since no analyzer is explicitly set
                // then the default 'RavenStandardAnalyzer' will be used at indexing time.
                
                // However, when making a search query with wildcards,
                // the 'LowerCaseKeywordAnalyzer' will be used to process the search terms
                // prior to sending them to the search engine. 
            }
        }
        #endregion
        
        #region index_4
        public class Employees_ByNotes_usingCustomAnalyzer :
            AbstractIndexCreationTask<Employee, Employees_ByNotes_usingCustomAnalyzer.IndexEntry>
        {
            public class IndexEntry
            {
                public string EmployeeNotes { get; set; }
            }

            public Employees_ByNotes_usingCustomAnalyzer()
            {
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        EmployeeNotes = employee.Notes[0]
                    };

                // Configure the index-field for FTS:
                Index(x => x.EmployeeNotes, FieldIndexing.Search);
                
                // Set a custom analyzer for the index-field:
                Analyze(x => x.EmployeeNotes, "CustomAnalyzers.RemoveWildcardsAnalyzer");
            }
        }
        #endregion
        
        #region index_5
        public class Employees_ByFirstName_usingExactAnalyzer :
            AbstractIndexCreationTask<Employee, Employees_ByFirstName_usingExactAnalyzer.IndexEntry>
        {
            public class IndexEntry
            {
                public string FirstName { get; set; }
            }
        
            public Employees_ByFirstName_usingExactAnalyzer()
            {
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        FirstName = employee.FirstName
                    };
                
                // Set the Exact analyzer for the index-field:
                // (The field will not be tokenized)
                Indexes.Add(x => x.FirstName, FieldIndexing.Exact);
            }
        }
        #endregion

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_1
                    List<Employee> employees = session
                         // Query the index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Call 'Search':
                         // pass the index field that was configured for FTS and the term to search for.
                        .Search(x => x.EmployeeNotes, "French")
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                    //
                    // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                    //   which preserves casing.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_2
                    List<Employee> employees = await asyncSession
                         // Query the index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Call 'Search':
                         // pass the index field that was configured for FTS and the term to search for.
                        .Search(x => x.EmployeeNotes, "French")
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                    // 
                    // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                    //   which preserves casing.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_3
                    List<Employee> employees = session.Advanced
                         // Query the index
                        .DocumentQuery<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Call 'Search':
                         // pass the index field that was configured for FTS and the term to search for.
                        .Search(x => x.EmployeeNotes, "French")
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                    // 
                    // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                    //   which preserves casing.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_4
                    List<Employee> employees = session
                         // Query the static-index
                        .Query<Employees_ByEmployeeData.IndexEntry, Employees_ByEmployeeData>()
                         // A logical OR is applied between the following two Search calls:
                        .Search(x => x.EmployeeData, "Manager")
                         // A logical AND is applied between the following two terms: 
                        .Search(x => x.EmployeeData, "French Spanish", @operator: SearchOperator.And)
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have:
                    //   ('Manager' in any of the 4 document-fields that were indexed)
                    //   OR 
                    //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                    //
                    // * Search is case-insensitive since the default analyzer is used
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_5
                    List<Employee> employees = await asyncSession
                         // Query the static-index
                        .Query<Employees_ByEmployeeData.IndexEntry, Employees_ByEmployeeData>()
                         // A logical OR is applied between the following two Search calls:
                        .Search(x => x.EmployeeData, "Manager")
                         // A logical AND is applied between the following two terms: 
                        .Search(x => x.EmployeeData, "French Spanish", @operator: SearchOperator.And)
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // * Results will contain all Employee documents that have:
                    //   ('Manager' in any of the 4 document-fields that were indexed)
                    //   OR 
                    //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                    //
                    // * Search is case-insensitive since the default analyzer is used
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_6
                    List<Employee> employees = session.Advanced
                         // Query the static-index
                        .DocumentQuery<Employees_ByEmployeeData.IndexEntry, Employees_ByEmployeeData>()
                        .OpenSubclause()
                         // A logical OR is applied between the following two Search calls:
                        .Search(x => x.EmployeeData, "Manager")
                         // A logical AND is applied between the following two terms: 
                        .Search(x => x.EmployeeData, "French Spanish", @operator: SearchOperator.And)
                        .CloseSubclause()
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have:
                    //   ('Manager' in any of the 4 document-fields that were indexed)
                    //   OR 
                    //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                    //                                                                                                                                                                                                                                                                                                                  
                    // * Search is case-insensitive since the default analyzer is used
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_7
                    List<Employee> employees = session
                        .Query<Employees_ByNotes_usingDefaultAnalyzer.IndexEntry,
                            Employees_ByNotes_usingDefaultAnalyzer>()
                        
                         // If you request to include explanations,
                         // you can see the exact term that was sent to the search engine.
                        .ToDocumentQuery()
                        .IncludeExplanations(out var explanations)
                        .ToQueryable()
                        
                         // Provide a term with a wildcard to the Search method:
                        .Search(x => x.EmployeeNotes, "*rench")
                        .OfType<Employee>()
                        .ToList();
                    
                    // Results will contain all Employee documents that have terms that end with 'rench'
                    // (e.g. French). 
                    
                    // Checking the explanations, you can see that the search term 'rench'
                    // was sent to the search engine WITH the leading wildcard, i.e. '*rench'
                    // since the 'LowerCaseKeywordAnalyzer' is used in this case. 
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"EmployeeNotes:*rench", explanation);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_8
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByNotes_usingDefaultAnalyzer.IndexEntry,
                            Employees_ByNotes_usingDefaultAnalyzer>()
                        
                         // If you request to include explanations,
                         // you can see the exact term that was sent to the search engine.
                        .ToDocumentQuery()
                        .IncludeExplanations(out var explanations)
                        .ToQueryable()
                        
                         // Provide a term with a wildcard to the Search method:
                        .Search(x => x.EmployeeNotes, "*rench")
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // Results will contain all Employee documents that have terms that end with 'rench'
                    // (e.g. French). 
                    
                    // Checking the explanations, you can see that the search term 'rench'
                    // was sent to the search engine WITH the leading wildcard, i.e. '*rench'
                    // since the 'LowerCaseKeywordAnalyzer' is used in this case. 
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"EmployeeNotes:*rench", explanation);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_9
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employees_ByNotes_usingDefaultAnalyzer.IndexEntry,
                            Employees_ByNotes_usingDefaultAnalyzer>()
    
                         // If you request to include explanations,
                         // you can see the exact term that was sent to the search engine.
                        .IncludeExplanations(out var explanations)
    
                         // Provide a term with a wildcard to the Search method:
                        .Search(x => x.EmployeeNotes, "*rench")
                        .OfType<Employee>()
                        .ToList();

                    // Results will contain all Employee documents that have terms that end with 'rench'
                    // (e.g. French). 

                    // Checking the explanations, you can see that the search term 'rench'
                    // was sent to the search engine WITH the leading wildcard, i.e. '*rench'
                    // since the 'LowerCaseKeywordAnalyzer' is used in this case. 
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"EmployeeNotes:*rench", explanation);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_10
                    List<Employee> employees = session
                        .Query<Employees_ByNotes_usingCustomAnalyzer.IndexEntry,
                            Employees_ByNotes_usingCustomAnalyzer>()
                        
                        .ToDocumentQuery()
                        .IncludeExplanations(out var explanations)
                        .ToQueryable()
                        
                         // Provide a term with wildcards to the Search method:
                        .Search(x => x.EmployeeNotes, "*French*")
                        .OfType<Employee>()
                        .ToList();
                    
                    // Even though a wildcard was provided,
                    // the results will contain only Employee documents that contain the exact term 'French'.
                    
                    // The search term was sent to the search engine WITHOUT the wildcard,
                    // as the custom analyzer's logic strips them out.
                    
                    // This can be verified by checking the explanations:
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"EmployeeNotes:french", explanation);
                    Assert.DoesNotContain($"EmployeeNotes:*french", explanation);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_11
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByNotes_usingCustomAnalyzer.IndexEntry,
                            Employees_ByNotes_usingCustomAnalyzer>()
                        
                        .ToDocumentQuery()
                        .IncludeExplanations(out var explanations)
                        .ToQueryable()
                        
                         // Provide a term with wildcards to the Search method:
                        .Search(x => x.EmployeeNotes, "*French*")
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // Even though a wildcard was provided,
                    // the results will contain only Employee documents that contain the exact term 'French'.
                    
                    // The search term was sent to the search engine WITHOUT the wildcard,
                    // as the custom analyzer's logic strips them out.
                    
                    // This can be verified by checking the explanations:
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"EmployeeNotes:french", explanation);
                    Assert.DoesNotContain($"EmployeeNotes:*french", explanation);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_12
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employees_ByNotes_usingCustomAnalyzer.IndexEntry,
                            Employees_ByNotes_usingCustomAnalyzer>()
                        .IncludeExplanations(out var explanations)
                         // Provide a term with wildcards to the Search method:
                        .Search(x => x.EmployeeNotes, "*French*")
                        .OfType<Employee>()
                        .ToList();
                    
                    // Even though a wildcard was provided,
                    // the results will contain only Employee documents that contain the exact term 'French'.
                    
                    // The search term was sent to the search engine WITHOUT the wildcard,
                    // as the custom analyzer's logic strips them out.
                    
                    // This can be verified by checking the explanations:
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"EmployeeNotes:french", explanation);
                    Assert.DoesNotContain($"EmployeeNotes:*french", explanation);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_13
                    List<Employee> employees = session
                        .Query<Employees_ByFirstName_usingExactAnalyzer.IndexEntry,
                            Employees_ByFirstName_usingExactAnalyzer>()
                        
                        .ToDocumentQuery()
                        .IncludeExplanations(out var explanations)
                        .ToQueryable()
                        
                         // Provide a term with a wildcard to the Search method:
                        .Search(x => x.FirstName, "Mich*")
                        .OfType<Employee>()
                        .ToList();
                    
                    // Results will contain all Employee documents with FirstName that starts with 'Mich'
                    // (e.g. Michael).
                    
                    // The search term, 'Mich*', is sent to the search engine
                    // exactly as was provided to the Search method, WITH the wildcard.
                    
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"FirstName:Mich*", explanation);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_14
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByFirstName_usingExactAnalyzer.IndexEntry,
                            Employees_ByFirstName_usingExactAnalyzer>()
                        
                        .ToDocumentQuery()
                        .IncludeExplanations(out var explanations)
                        .ToQueryable()
                        
                         // Provide a term with a wildcard to the Search method:
                        .Search(x => x.FirstName, "Mich*")
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // Results will contain all Employee documents with FirstName that starts with 'Mich'
                    // (e.g. Michael).
                    
                    // The search term, 'Mich*', is sent to the search engine
                    // exactly as was provided to the Search method, WITH the wildcard.
                    
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"FirstName:Mich*", explanation);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_15
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employees_ByFirstName_usingExactAnalyzer.IndexEntry,
                            Employees_ByFirstName_usingExactAnalyzer>()
                        .IncludeExplanations(out var explanations)
                         // Provide a term with a wildcard to the Search method:
                        .Search(x => x.FirstName, "Mich*")
                        .OfType<Employee>()
                        .ToList();
                    
                    // Results will contain all Employee documents with FirstName that starts with 'Mich'
                    // (e.g. Michael).
                    
                    // The search term, 'Mich*', is sent to the search engine
                    // exactly as was provided to the Search method, WITH the wildcard.
                    
                    var explanation = explanations.GetExplanations(employees[0].Id)[0];
                    Assert.Contains($"FirstName:Mich*", explanation);
                    #endregion
                }

                #region analyzer_1
                // The custom analyzer:
                // ====================
                
                const string RemoveWildcardsAnalyzer =
                    @"
                      using System.IO;
                      using Lucene.Net.Analysis; 
                      using Lucene.Net.Analysis.Standard;
                      namespace CustomAnalyzers
                      {
                          public class RemoveWildcardsAnalyzer : StandardAnalyzer
                          {
                              public RemoveWildcardsAnalyzer() : base(Lucene.Net.Util.Version.LUCENE_30)
                              {
                              }

                              public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
                              {
                                   // Read input stream and remove wildcards (*)
                                  string text = reader.ReadToEnd();
                                  string processedText = RemoveWildcards(text);
                                  StringReader newReader = new StringReader(processedText);
                                  
                                  return base.TokenStream(fieldName, newReader);
                              }

                              private string RemoveWildcards(string input)
                              {
                                  // Replace wildcard characters with an empty string
                                  return input.Replace(""*"", """");
                              }
                          }
                      }";

                // Deploying the custom analyzer:
                // ==============================
                
                store.Maintenance.Send(new PutAnalyzersOperation(new AnalyzerDefinition()
                {
                    Name = "CustomAnalyzers.RemoveWildcardsAnalyzer",
                    Code = RemoveWildcardsAnalyzer,
                }));
                #endregion
            }
        }
    }
}
