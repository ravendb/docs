// -----------------------------------------------------------------------
//  <copyright file="IncludeQueryTimings.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Timings;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.Debugging
{
    public class IncludeQueryTimings
    {
        public interface IFoo
        {
            #region syntax
            IDocumentQueryCustomization Timings(out QueryTimings timings);
            #endregion
        }

        public async Task Timings()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region timing_1
                    // Define an object that will receive the query timings
                    QueryTimings timings = null;
                    
                    var results = session.Query<Product>()
                        // Use the Customize method to Call Timings, provide an out param for the timings results
                        .Customize(x => x.Timings(out timings))
                        // Define query criteria
                        // i.e. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager")
                        // Execute the query
                        .ToList();

                    // Get total query duration:
                    var totalQueryDuration = timings.DurationInMs;
                    
                    // Get specific parts duration: 
                    IDictionary<string, QueryTimings> timingsDictionary = timings.Timings;
                    
                    var optimizerDuration = timingsDictionary["Optimizer"].DurationInMs;
                    var luceneDuration = timings.Timings["Query"].Timings["Lucene"].DurationInMs;
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region timing_2
                    var results = session.Advanced.DocumentQuery<Product>()
                        // Call Timings, provide an out param for the timings results
                        .Timings(out QueryTimings timings)
                        // Define query criteria
                        // i.e. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager")
                        // Execute the query
                        .ToList();

                    // Get total query duration:
                    var totalQueryDuration = timings.DurationInMs;
                    
                    // Get specific parts duration: 
                    IDictionary<string, QueryTimings> timingsDictionary = timings.Timings;
                    
                    var optimizerDuration = timingsDictionary["Optimizer"].DurationInMs;
                    var luceneDuration = timings.Timings["Query"].Timings["Lucene"].DurationInMs;
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region timing_3
                    // Define an object that will receive the query timings
                    QueryTimings timings = null;
                    
                    var results = await asyncSession.Query<Product>()
                        // Use the Customize method to Call Timings, provide an out param for the timings results
                        .Customize(x => x.Timings(out timings))
                        // Define the search criteria
                        // Search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager")
                        // Execute the query
                        .ToListAsync();

                    // Get total query duration:
                    var totalQueryDuration = timings.DurationInMs;
                    
                    // Get specific parts duration: 
                    IDictionary<string, QueryTimings> timingsDictionary = timings.Timings;
                    
                    var optimizerDuration = timingsDictionary["Optimizer"].DurationInMs;
                    var luceneDuration = timings.Timings["Query"].Timings["Lucene"].DurationInMs;
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region timing_4
                    var results = await asyncSession.Advanced.AsyncDocumentQuery<Product>()
                        // Call Timings, provide an out param for the timings results
                        .Timings(out QueryTimings timings)
                         // Define the search criteria
                         // Search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager")
                         // Execute the query
                        .ToListAsync();

                    // Get total query duration:
                    var totalQueryDuration = timings.DurationInMs;
                    
                    // Get specific parts duration: 
                    IDictionary<string, QueryTimings> timingsDictionary = timings.Timings;
                    
                    var optimizerDuration = timingsDictionary["Optimizer"].DurationInMs;
                    var luceneDuration = timings.Timings["Query"].Timings["Lucene"].DurationInMs;
                    #endregion
                }
            }
        }
    }
}
