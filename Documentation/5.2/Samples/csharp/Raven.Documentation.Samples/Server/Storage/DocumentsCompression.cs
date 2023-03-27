using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.Server.Storage
{
    class DocumentsCompression
    {
        public DocumentsCompression()
        {
            using (var store = new DocumentStore())
            {
                #region comparess_all
                // Compression is configured by setting the database record 
                
                // Retrieve the database record
                var dbrecord = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
                
                // Set compression on ALL collections
                dbrecord.DocumentsCompression.CompressAllCollections = true;

                // Update the the database record
                store.Maintenance.Server.Send(new UpdateDatabaseOperation(dbrecord, dbrecord.Etag));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region comparess_specific
                // Retrieve the database record
                var dbrecord = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));

                // Turn on compression for the specific collections
                dbrecord.DocumentsCompression.Collections = new[] { "Orders", "Employees" };
                
                // Turn off compression for all revisions, on all collections
                dbrecord.DocumentsCompression.CompressRevisions = false;

                // Update the the database record
                store.Maintenance.Server.Send(new UpdateDatabaseOperation(dbrecord, dbrecord.Etag));
                #endregion
            }
        }

        public async Task DocumentsCompressionAsync()
        {
            using (var store = new DocumentStore())
            {
                #region comparess_all_async
                // Compression is configured by setting the database record 
                
                // Retrieve the database record
                var dbrecord = await store.Maintenance.Server.SendAsync(new GetDatabaseRecordOperation(store.Database));
                
                // Set compression on ALL collections
                dbrecord.DocumentsCompression.CompressAllCollections = true;

                // Update the the database record
                await store.Maintenance.Server.SendAsync(new UpdateDatabaseOperation(dbrecord, dbrecord.Etag));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region comparess_specific_async
                // Retrieve the database record
                var dbrecord = await store.Maintenance.Server.SendAsync(new GetDatabaseRecordOperation(store.Database));

                // Turn on compression for specific collection
                dbrecord.DocumentsCompression.Collections = new[] { "Orders", "Employees" };
                
                // Turn off compression for all revisions, on all collections
                dbrecord.DocumentsCompression.CompressRevisions = false;

                // Update the the database record
                await store.Maintenance.Server.SendAsync(new UpdateDatabaseOperation(dbrecord, dbrecord.Etag));
                #endregion
            }
        }
    }

    #region syntax
    public class DocumentsCompressionConfiguration 
    {
        public string[] Collections { get; set; }
        public bool CompressRevisions { get; set; }
        public bool CompressAllCollections { get; set; }
    }
    #endregion
}
