// -----------------------------------------------------------------------
//  <copyright file="IncludeExplanations.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Explanation;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Debugging
{
    public class IncludeExplanations
    {
        public interface IFoo<T>
        {
            #region explain_1
            IDocumentQuery<T> IncludeExplanations(out Explanations explanations);
            #endregion
        }

        public async Task Explain()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region explain_2
                    var syrups = session.Advanced.DocumentQuery<Product>()
                        .IncludeExplanations(out Explanations explanations)
                        .Search(x => x.Name, "Syrup")
                        .ToList();

                    string[] scoreDetails = explanations.GetExplanations(syrups[0].Id);
                    #endregion
                }

                using (var session = store.OpenAsyncSession())
                {
                    #region explain_3
                    var syrups = await session.Advanced.AsyncDocumentQuery<Product>()
                        .IncludeExplanations(out Explanations explanations)
                        .Search(x => x.Name, "Syrup")
                        .ToListAsync();

                    string[] scoreDetails = explanations.GetExplanations(syrups[0].Id);
                    #endregion
                }
            }
        }
    }
}
