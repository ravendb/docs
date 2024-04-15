// -----------------------------------------------------------------------
//  <copyright file="IncludeExplanations.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Explanation;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.Debugging
{
    public class IncludeExplanations
    {
        public interface IFoo<T>
        {
            #region syntax
            IDocumentQuery<T> IncludeExplanations(out Explanations explanations);
            #endregion

            #region syntax_2
            // Call GetExplanations on the resulting Explanations object
            string[] GetExplanations(string docId);
            #endregion
        }

        public async Task Explain()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region explain
                    // Query with `DocumentQuery`
                    var results = session.Advanced.DocumentQuery<Product>()
                        
                         // Call IncludeExplanations, provide an out param for the explanations results
                        .IncludeExplanations(out Explanations explanations)
                         // Define query criteria
                         // i.e. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager")
                         // Execute the query
                        .ToList();

                    // Get the score details for a specific document from the results
                    // Call GetExplanations on the resulting Explanations object
                    string[] scoreDetails = explanations.GetExplanations(results[0].Id);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region explain_async
                    // Query with `AsyncDocumentQuery`
                    var results = await asyncSession.Advanced.AsyncDocumentQuery<Product>()
                        
                         // Call IncludeExplanations, provide an out param for the explanations results
                        .IncludeExplanations(out Explanations explanations)
                         // Define query criteria
                         // i.e. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager")
                         // Execute the query
                        .ToListAsync();

                    // Get the score details for a specific document from the results
                    // Call GetExplanations on the resulting Explanations object
                    string[] scoreDetails = explanations.GetExplanations(results[0].Id);
                    #endregion
                }
            }
        }
    }
}
