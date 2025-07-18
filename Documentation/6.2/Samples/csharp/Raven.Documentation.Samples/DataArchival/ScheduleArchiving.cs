using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Util;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DataArchival
{
    public class ScheduleArchiving
    {
        public async Task Examples()
        {
            // Schedule single document:
            // =========================
            
            using (var store = new DocumentStore())
            {
                #region schedule_document
                using (var session = store.OpenSession())
                {
                    // Load the document to schedule for archiving
                    var company = session.Load<Company>("companies/91-A");
                    
                    // Access the document's metadata
                    var metadata = session.Advanced.GetMetadataFor(company);
                    
                    // Set the future archival date (in UTC)
                    var archiveDate = SystemTime.UtcNow.AddDays(1);
                    metadata["@archive-at"] = archiveDate;
                    
                    // Save the changes
                    session.SaveChanges();
                }
                #endregion
                
                #region schedule_document_async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Load the document to schedule for archiving
                    var company = await asyncSession.LoadAsync<Company>("companies/91-A");
                    
                    // Access the document's metadata
                    var metadata = asyncSession.Advanced.GetMetadataFor(company);

                    // Set the future archival date (in UTC)
                    var archiveDate = SystemTime.UtcNow.AddDays(1);
                    metadata["@archive-at"] = archiveDate;
                    
                    // Save the changes
                    await asyncSession.SaveChangesAsync();
                }
                #endregion
            }
            
            // Schedule MULTIPLE documents:
            // ============================
            
            using (var store = new DocumentStore())
            {
                #region schedule_documents
                var archiveDate = SystemTime.UtcNow.AddDays(1);
                string archiveDateString = archiveDate.ToString("o");
                
                var oldDate = SystemTime.UtcNow.AddYears(-1);
                string oldDateString = oldDate.ToString("o");
                
                // Define the patch query string
                // Request to archive all Orders older than one year
                string patchByQuery = $@"
                    // The patch query:
                    // ================
                    from Orders
                    where OrderedAt < '{oldDateString}'
                    update {{
                        // The patch script - schedule for archival:
                        // =========================================
                        archived.archiveAt(this, '{archiveDateString}')
                    }}";
                
                // Define the patch operation, pass the patch query string
                var patchByQueryOp = new PatchByQueryOperation(patchByQuery);
    
                // Execute the operation by passing it to Operations.Send
                store.Operations.Send(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region schedule_documents_async
                var archiveDate = SystemTime.UtcNow.AddDays(1);
                string archiveDateString = archiveDate.ToString("o");
                
                var oldDate = SystemTime.UtcNow.AddYears(-1);
                string oldDateString = oldDate.ToString("o");
                
                // Define the patch query string
                // Request to archive all Orders older than one year
                string patchByQuery = $@"
                    from Orders
                    where OrderedAt < '{oldDateString}'
                    update {{
                        archived.archiveAt(this, '{archiveDateString}')
                    }}";
                
                // Define the patch operation, pass the patch query string
                var patchByQueryOp = new PatchByQueryOperation(patchByQuery);
    
                // Execute the operation by passing it to Operations.SendAsync
                await store.Operations.SendAsync(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region schedule_documents_overload
                var archiveDate = SystemTime.UtcNow.AddDays(1);
                string archiveDateString = archiveDate.ToString("o");
                
                var oldDate = SystemTime.UtcNow.AddYears(-1);
                string oldDateString = oldDate.ToString("o");
                
                // Define the patch string
                // Request to archive all Orders older than one year
                string patchByQuery = $@"
                    from Orders
                    where OrderedAt < $p0 
                    update {{
                        archived.archiveAt(this, $p1)
                    }}";
                
                // Define the patch operation, pass the patch query
                var patchByQueryOp = new PatchByQueryOperation(new IndexQuery()
                {
                    Query = patchByQuery,
                    QueryParameters = new Parameters()
                    {
                        { "p0", oldDateString },
                        { "p1", archiveDateString }
                    }
                });
    
                // Execute the operation by passing it to Operations.Send
                store.Operations.Send(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region schedule_documents_overload_async
                var archiveDate = SystemTime.UtcNow.AddDays(1);
                string archiveDateString = archiveDate.ToString("o");
                
                var oldDate = SystemTime.UtcNow.AddYears(-1);
                string oldDateString = oldDate.ToString("o");
                
                // Define the patch string
                // Request to archive all Orders older than one year
                string patchByQuery = $@"
                    from Orders
                    where OrderedAt < $p0 
                    update {{
                        archived.archiveAt(this, $p1)
                    }}";
                
                // Define the patch operation, pass the patch query
                var patchByQueryOp = new PatchByQueryOperation(new IndexQuery()
                {
                    Query = patchByQuery,
                    QueryParameters = new Parameters()
                    {
                        { "p0", oldDateString },
                        { "p1", archiveDateString }
                    }
                });
    
                // Execute the operation by passing it to Operations.SendAsync
                await store.Operations.SendAsync(patchByQueryOp);
                #endregion
            }
        }
    
        private interface IFoo
        {
            /*
            #region syntax
            archived.archiveAt(doc, utcDateTimeString)
            #endregion
            */
        }
    }
}

