using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class BoostResults
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region boost_1
                    List<Employee> employees = session
                         // Make a dynamic full-text search Query on 'Employees' collection
                        .Query<Employee>()
                         // This search predicate will use the default boost value of 1
                        .Search(x => x.Notes, "English")
                         // * Pass the boost value using the 'boost' parameter
                         // * This search predicate will use a boost value of 10
                        .Search(x => x.Notes, "Italian", boost: 10)
                        .ToList();
                    
                    // * Results will contain all Employee documents that have
                    //   EITHER 'English' OR 'Italian' in their 'Notes' field.
                    //
                    // * Matching documents with 'Italian' will be listed FIRST in the results,
                    //   before those with 'English'.
                    // 
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region boost_2
                    List<Employee> employees = await asyncSession
                         // Make a dynamic full-text search Query on 'Employees' collection
                        .Query<Employee>()
                         // This search predicate will use the default boost value of 1
                        .Search(x => x.Notes, "English")
                         // * Pass the boost value using the 'boost' parameter
                         // * This search predicate will use a boost value of 10
                        .Search(x => x.Notes, "Italian", boost: 10)
                        .ToListAsync();
                    
                    // * Results will contain all Employee documents that have
                    //   EITHER 'English' OR 'Italian' in their 'Notes' field.
                    //
                    // * Matching documents with 'Italian' will be listed FIRST in the results,
                    //   before those with 'English'.
                    // 
                    // * Search is case-insensitive.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region boost_3
                    List<Employee> employees = session.Advanced
                         // Make a dynamic full-text search DocumentQuery on 'Employees' collection
                        .DocumentQuery<Employee>()
                         // This search predicate will use the default boost value of 1
                        .Search(x => x.Notes, "English")
                         // This search predicate will use a boost value of 10 
                        .Search(x => x.Notes, "Italian")
                         // Call 'Boost' to set the boost value of the previous 'Search' call
                        .Boost(10)
                        .ToList();
                    
                    // * Results will contain all Employee documents that have
                    //   EITHER 'English' OR 'Italian' in their 'Notes' field.
                    //
                    // * Matching documents with 'Italian' will be listed FIRST in the results,
                    //   before those with 'English'.
                    // 
                    // * Search is case-insensitive.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region boost_4
                    List<Company> companies = session.Advanced
                         // Make a dynamic DocumentQuery on 'Companies' collection
                        .DocumentQuery<Company>()
                         // Define a 'Where' condition
                        .WhereStartsWith(x => x.Name, "O")
                         // Call 'Boost' to set the boost value of the previous 'Where' predicate
                        .Boost(10)
                         // Call 'OrElse' so that OR operator will be used between statements
                        .OrElse()
                        .WhereStartsWith(x => x.Name, "P")
                        .Boost(50)
                        .OrElse()
                        .WhereEndsWith(x => x.Name, "OP")
                        .Boost(90)
                        .ToList();
                    
                    // * Results will contain all Company documents that either
                    //   (start-with 'O') OR (start-with 'P') OR (end-with 'OP') in their 'Name' field.
                    //
                    // * Matching documents the end-with 'OP' will be listed FIRST.
                    //   Matching documents that start-with 'P' will then be listed.
                    //   Matching documents that start-with 'O' will be listed LAST.
                    // 
                    // * Search is case-insensitive.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region boost_5
                    List<Company> companies = await asyncSession.Advanced
                         // Make a dynamic DocumentQuery on 'Companies' collection
                        .AsyncDocumentQuery<Company>()
                         // Define a 'Where' condition
                        .WhereStartsWith(x => x.Name, "O")
                         // Call 'Boost' to set the boost value of the previous 'Where' predicate
                        .Boost(10)
                         // Call 'OrElse' so that OR operator will be used between statements
                        .OrElse()
                        .WhereStartsWith(x => x.Name, "P")
                        .Boost(50)
                        .OrElse()
                        .WhereEndsWith(x => x.Name, "OP")
                        .Boost(90)
                        .ToListAsync();
                    
                    // * Results will contain all Company documents that either
                    //   (start-with 'O') OR (start-with 'P') OR (end-with 'OP') in their 'Name' field.
                    //
                    // * Matching documents the end-with 'OP' will be listed FIRST.
                    //   Matching documents that start-with 'P' will then be listed.
                    //   Matching documents that start-with 'O' will be listed LAST.
                    // 
                    // * Search is case-insensitive.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region boost_6
                    // Make a query:
                    // =============
                    
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Search(x => x.Notes, "English")
                        .Search(x => x.Notes, "Italian", boost: 10)
                        .ToList();
                    
                    // Get the score:
                    // ==============
                    
                    // Call 'GetMetadataFor', pass an entity from the resulting employees list
                    var metadata = session.Advanced.GetMetadataFor(employees[0]);
                    
                    // Score is available in the '@index-score' metadata property
                    var score = metadata[Constants.Documents.Metadata.IndexScore];
                    #endregion
                }
            }
        }
    }
}
