// -----------------------------------------------------------------------
//  <copyright file="IncludeExplanations.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Explanation;
using Raven.Client.Documents.Queries.Timings;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Debugging
{
    public class IncludeQueryTimings
    {
        public interface IFoo<T>
        {
            #region timing_1
            IDocumentQuery<T> Timings(out QueryTimings timings);
            #endregion
        }

        public async Task Explain()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region timing_2
                    var resultWithTimings = session.Advanced.DocumentQuery<Product>()
                        .Timings(out QueryTimings timings)
                        .Search(x => x.Name, "Syrup")
                        .ToList();

                    IDictionary<string, QueryTimings> timingsDictionary = timings.Timings;
                    #endregion
                }

                using (var session = store.OpenAsyncSession())
                {
                    #region timing_3
                    var resultWithTimings = await session.Advanced.AsyncDocumentQuery<Product>()
                        .Timings(out QueryTimings timings)
                        .Search(x => x.Name, "Syrup")
                        .ToListAsync();

                    IDictionary<string, QueryTimings> timingsDictionary = timings.Timings;
                    #endregion
                }
            }
        }
    }
}
