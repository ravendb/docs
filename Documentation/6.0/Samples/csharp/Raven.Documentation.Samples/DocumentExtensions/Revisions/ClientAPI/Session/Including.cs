using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Session
{
    public class IncludeRevisions
    {
        public IncludeRevisions()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region include_1
                    // The revision creation time
                    // For example - looking for a revision from last month
                    var creationTime = DateTime.Now.AddMonths(-1);
                    
                    // Load a document:
                    var order = session.Load<Order>("orders/1-A", builder => builder
                        // Pass the revision creation time to 'IncludeRevisions'
                        // The revision will be 'loaded' to the session along with the document
                        .IncludeRevisions(creationTime));

                    // Get the revision by creation time - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = session
                        .Advanced.Revisions.Get<Order>("orders/1-A", creationTime);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region include_2
                    // Load a document:
                    var contract = session.Load<Contract>("contracts/1-A", builder => builder
                         // Pass the path to the document property that contains the revision change vector(s)
                         // The revision(s) will be 'loaded' to the session along with the document
                        .IncludeRevisions(x => x.RevisionChangeVector)    // Include a single revision
                        .IncludeRevisions(x => x.RevisionChangeVectors)); // Include multiple revisions
                    
                    // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = session
                        .Advanced.Revisions.Get<Contract>(contract.RevisionChangeVector);
                    var revisions = session
                        .Advanced.Revisions.Get<Contract>(contract.RevisionChangeVectors);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region include_3
                    // The revision creation time
                    // For example - looking for revisions from last month
                    var creationTime = DateTime.Now.AddMonths(-1);
                    
                    // Query for documents:
                    var orderDocuments = session.Query<Order>()
                        .Where(x => x.ShipTo.Country == "Canada")
                         // Pass the revision creation time to 'IncludeRevisions'
                        .Include(builder => builder.IncludeRevisions(creationTime))
                         // For each document in the query results,
                         // the matching revision will be 'loaded' to the session along with the document
                        .ToList();
                    
                    // Get a revision by its creation time for a document from the query results
                    // It will be retrieved from the SESSION - no additional trip to the server is made
                    var revision = session
                        .Advanced.Revisions.Get<Order>(orderDocuments[0].Id, creationTime);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region include_4
                    // Query for documents:
                    var orderDocuments = session.Query<Contract>()
                         // Pass the path to the document property that contains the revision change vector(s)
                        .Include(builder => builder
                            .IncludeRevisions(x => x.RevisionChangeVector)   // Include a single revision
                            .IncludeRevisions(x => x.RevisionChangeVectors)) // Include multiple revisions
                         // For each document in the query results,
                         // the matching revisions will be 'loaded' to the session along with the document
                        .ToList();

                    // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = session.
                        Advanced.Revisions.Get<Contract>(orderDocuments[0].RevisionChangeVector);
                    var revisions = session
                        .Advanced.Revisions.Get<Contract>(orderDocuments[0].RevisionChangeVectors);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region include_5
                    // The revision creation time
                    // For example - looking for revisions from last month
                    var creationTime = DateTime.Now.AddMonths(-1);
                    
                    // Query for documents with Raw Query:
                    var orderDocuments = session.Advanced
                         // Use 'include revisions' in the RQL   
                        .RawQuery<Order>("from Orders include revisions($p0)")
                         // Pass the revision creation time 
                        .AddParameter("p0", creationTime)
                         // For each document in the query results,
                         // the matching revision will be 'loaded' to the session along with the document
                        .ToList();

                    // Get a revision by its creation time for a document from the query results
                    // It will be retrieved from the SESSION - no additional trip to the server is made
                    var revision = session
                        .Advanced.Revisions.Get<Order>(orderDocuments[0].Id, creationTime);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region include_6
                    // Query for documents with Raw Query:
                    var orderDocuments = session.Advanced
                         // Use 'include revisions' in the RQL   
                        .RawQuery<Contract>("from Contracts include revisions($p0, $p1)")
                         // Pass the path to the document properties containing the change vectors
                        .AddParameter("p0", "RevisionChangeVector")
                        .AddParameter("p1", "RevisionChangeVectors")
                         // For each document in the query results,
                         // the matching revisions will be 'loaded' to the session along with the document
                        .ToList();

                    // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = session.
                        Advanced.Revisions.Get<Contract>(orderDocuments[0].RevisionChangeVector);
                    var revisions = session
                        .Advanced.Revisions.Get<Contract>(orderDocuments[0].RevisionChangeVectors);
                    #endregion
                }

                #region include_7
                using (var session = store.OpenSession())
                {
                    // Get the revisions' metadata for document 'contracts/1-A'
                    List<MetadataAsDictionary> contractRevisionsMetadata =
                        session.Advanced.Revisions.GetMetadataFor("contracts/1-A");

                    // Get a change vector from the metadata
                    string changeVector = 
                        contractRevisionsMetadata.First().GetString(Constants.Documents.Metadata.ChangeVector);

                    // Patch the document - add the revision change vector to a specific document property
                    session.Advanced
                        .Patch<Contract, string>("contracts/1-A", x => x.RevisionChangeVector, changeVector);

                    // Save your changes
                    session.SaveChanges();
                }
                #endregion
            }
        }

        public async Task IncludeRevisionsAsync()
        {
            using (var store = new DocumentStore())
            {
                using (var asyncSession  = store.OpenAsyncSession())
                {
                    #region include_1_async
                    // The revision creation time
                    // For example - looking for a revision from last month
                    var creationTime = DateTime.Now.AddMonths(-1);

                    // Load a document:
                    var order = await asyncSession.LoadAsync<Order>("orders/1-A", builder => builder
                        // Pass the revision creation time to 'IncludeRevisions'
                        // The revision will be 'loaded' to the session along with the document
                        .IncludeRevisions(creationTime));

                    // Get the revision by creation time - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = await asyncSession
                        .Advanced.Revisions.GetAsync<Order>("orders/1-A", creationTime);
                    #endregion
                }
                
                using (var asyncSession  = store.OpenAsyncSession())
                {
                    #region include_2_async
                    // Load a document:
                    var contract = await asyncSession.LoadAsync<Contract>("contracts/1-A",builder => builder
                        // Pass the path to the document property that contains the revision change vector(s)
                        // The revision(s) will be 'loaded' to the session along with the document
                        .IncludeRevisions(x => x.RevisionChangeVector)    // Include a single revision
                        .IncludeRevisions(x => x.RevisionChangeVectors)); // Include multiple revisions
                    
                    // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = await asyncSession
                        .Advanced.Revisions.GetAsync<Contract>(contract.RevisionChangeVector);
                    var revisions = await asyncSession
                        .Advanced.Revisions.GetAsync<Contract>(contract.RevisionChangeVectors);
                    #endregion
                }
                
                using (var asyncSession  = store.OpenAsyncSession())
                {
                    #region include_3_async
                    // The revision creation time
                    // For example - looking for revisions from last month
                    var creationTime = DateTime.Now.AddMonths(-1);
                    
                    // Query for documents:
                    var orderDocuments = await asyncSession.Query<Order>()
                        .Where(x => x.ShipTo.Country == "Canada")
                         // Pass the revision creation time to 'IncludeRevisions'
                        .Include(builder => builder.IncludeRevisions(creationTime))
                         // For each document in the query results,
                         // the matching revision will be 'loaded' to the session along with the document
                        .ToListAsync();
                    
                    // Get a revision by its creation time for a document from the query results
                    // It will be retrieved from the SESSION - no additional trip to the server is made
                    var revision = await asyncSession
                        .Advanced.Revisions.GetAsync<Order>(orderDocuments[0].Id, creationTime);
                    #endregion
                }
                
                using (var asyncSession  = store.OpenAsyncSession())
                {
                    #region include_4_async
                    // Query for documents:
                    var orderDocuments = await asyncSession.Query<Contract>()
                        // Pass the path to the document property that contains the revision change vector(s)
                        .Include(builder => builder
                            .IncludeRevisions(x => x.RevisionChangeVector)   // Include a single revision
                            .IncludeRevisions(x => x.RevisionChangeVectors)) // Include multiple revisions
                        // For each document in the query results,
                        // the matching revisions will be 'loaded' to the session along with the document
                        .ToListAsync();

                    // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = await asyncSession.
                        Advanced.Revisions.GetAsync<Contract>(orderDocuments[0].RevisionChangeVector);
                    var revisions = await asyncSession
                        .Advanced.Revisions.GetAsync<Contract>(orderDocuments[0].RevisionChangeVectors);
                    #endregion
                }
                
                using (var asyncSession  = store.OpenAsyncSession())
                {
                    #region include_5_async
                    // The revision creation time
                    // For example - looking for revisions from last month
                    var creationTime = DateTime.Now.AddMonths(-1);
                    
                    // Query for documents with Raw Query:
                    var orderDocuments = await asyncSession.Advanced
                        // Use 'include revisions' in the RQL   
                        .AsyncRawQuery<Order>("from Orders include revisions($p0)")
                        // Pass the revision creation time 
                        .AddParameter("p0", creationTime)
                        // For each document in the query results,
                        // the matching revision will be 'loaded' to the session along with the document
                        .ToListAsync();

                    // Get a revision by its creation time for a document from the query results
                    // It will be retrieved from the SESSION - no additional trip to the server is made
                    var revision = await asyncSession
                        .Advanced.Revisions.GetAsync<Order>(orderDocuments[0].Id, creationTime);
                    #endregion
                }
                
                using (var asyncSession  = store.OpenAsyncSession())
                {
                    #region include_6_async
                    // Query for documents with Raw Query:
                    var orderDocuments = await asyncSession.Advanced
                        // Use 'include revisions' in the RQL   
                        .AsyncRawQuery<Contract>("from Contracts include revisions($p0, $p1)")
                        // Pass the path to the document properties containing the change vectors
                        .AddParameter("p0", "RevisionChangeVector")
                        .AddParameter("p1", "RevisionChangeVectors")
                        // For each document in the query results,
                        // the matching revisions will be 'loaded' to the session along with the document
                        .ToListAsync();

                    // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                    // No additional trip to the server is made
                    var revision = await asyncSession.
                        Advanced.Revisions.GetAsync<Contract>(orderDocuments[0].RevisionChangeVector);
                    var revisions = await asyncSession
                        .Advanced.Revisions.GetAsync<Contract>(orderDocuments[0].RevisionChangeVectors);
                    #endregion
                }
                
                #region include_7_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Get the revisions' metadata for document 'contracts/1-A'
                    List<MetadataAsDictionary> contractRevisionsMetadata =  
                        await asyncSession.Advanced.Revisions.GetMetadataForAsync("contracts/1-A");

                    // Get a change vector from the metadata
                    string changeVector = 
                        contractRevisionsMetadata.First().GetString(Constants.Documents.Metadata.ChangeVector);

                    // Patch the document - add the revision change vector to a specific document property
                    asyncSession.Advanced
                        .Patch<Contract, string>("contracts/1-A", x => x.RevisionChangeVector, changeVector);

                    // Save your changes
                    await asyncSession.SaveChangesAsync();
                }
                #endregion
            }
        }
        
        #region sample_document
        // Sample Contract document
        private class Contract
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string RevisionChangeVector { get; set; }        // A single change vector
            public List<string> RevisionChangeVectors { get; set; } // A list of change vectors
        }
        #endregion
        
        private interface IFoo
        {
            public interface IRevisionIncludeBuilder<T, out TBuilder>
            {
                #region syntax
                // Include a single revision by Time
                TBuilder IncludeRevisions(DateTime before);
                
                // Include a single revision by Change Vector
                TBuilder IncludeRevisions(Expression<Func<T, string>> path);
                
                // Include an array of revisions by Change Vectors
                TBuilder IncludeRevisions(Expression<Func<T, IEnumerable<string>>> path);
                #endregion
            }
        }
    }
}
