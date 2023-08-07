using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class ProximitySearch
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region proximity_1
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // Make a full-text search with search terms
                        .Search(x => x.Notes,"fluent french")
                         // Call 'Proximity' with 0 distance
                        .Proximity(0)
                        .ToList();
                    
                    // Running the above query on the Northwind sample data returns the following Employee documents:
                    // * employees/2-A
                    // * employees/5-A
                    // * employees/9-A
                    
                    // Each resulting document has the text 'fluent in French' in its 'Notes' field.
                    //
                    // The word "in" is not taken into account as it is Not part of the terms list generated
                    // by the analyzer. (Search is case-insensitive in this case).
                    //
                    // Note:
                    // A document containing text with the search terms appearing with no words in between them
                    // (e.g. "fluent french") would have also been returned.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region proximity_2
                    List<Employee> employees = await asyncSession.Advanced
                        .AsyncDocumentQuery<Employee>()
                         // Make a full-text search with search terms
                        .Search(x => x.Notes,"fluent french")
                         // Call 'Proximity' with 0 distance
                        .Proximity(0)
                        .ToListAsync();
                    
                    // Running the above query on the Northwind sample data returns the following Employee documents:
                    // * employees/2-A
                    // * employees/5-A
                    // * employees/9-A
                    
                    // Each resulting document has the text 'fluent in French' in its 'Notes' field.
                    //
                    // The word "in" is not taken into account as it is Not part of the terms list generated
                    // by the analyzer. (Search is case-insensitive in this case).
                    //
                    // Note:
                    // A document containing text with the search terms appearing with no words in between them
                    // (e.g. "fluent french") would have also been returned.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region proximity_3
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                         // Make a full-text search with search terms
                        .Search(x => x.Notes,"fluent french")
                         // Call 'Proximity' with distance 5
                        .Proximity(4)
                        .ToList();
                    
                    // Running the above query on the Northwind sample data returns the following Employee documents:
                    // * employees/2-A
                    // * employees/5-A
                    // * employees/6-A
                    // * employees/9-A
                    
                    // This time document 'employees/6-A' was added to the previous results since it contains the phrase:
                    // "fluent in Japanese and can read and write French"
                    // where the search terms are separated by a count of 4 terms.
                    //
                    // "in" & "and" are not taken into account as they are not part of the terms list generated
                    // by the analyzer.(Search is case-insensitive in this case).
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region proximity_4
                    List<Employee> employees = await asyncSession.Advanced
                        .AsyncDocumentQuery<Employee>()
                         // Make a full-text search with search terms
                        .Search(x => x.Notes,"fluent french")
                         // Call 'Proximity' with distance 5
                        .Proximity(4)
                        .ToListAsync();
                    
                    // Running the above query on the Northwind sample data returns the following Employee documents:
                    // * employees/2-A
                    // * employees/5-A
                    // * employees/6-A
                    // * employees/9-A
                    
                    // This time document 'employees/6-A' was added to the previous results since it contains the phrase:
                    // "fluent in Japanese and can read and write French"
                    // where the search terms are separated by a count of 4 terms.
                    //
                    // "in" & "and" are not taken into account as they are not part of the terms list generated
                    // by the analyzer.(Search is case-insensitive in this case).
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region syntax
            IDocumentQuery<T> Proximity(int proximity);
            #endregion
        }
    }
}
