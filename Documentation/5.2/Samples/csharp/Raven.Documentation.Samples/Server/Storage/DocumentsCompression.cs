using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;

namespace Raven.Documentation.Samples.ClientApi
{
    class DocumentsCompression
    {

/*        #region Syntax_0
        public class DocumentsCompressionConfiguration
        {
            public string[] Collections { get; set; }
            public bool CompressRevisions { get; set; }
            public bool CompressAllCollections { get; set; }
        }
        #endregion*/


        public void Example()
        {
            #region Example_0
            using (var store = new DocumentStore())
            {
                // Retrieve database record
                var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));

                // Enable compression on collection Orders
                // Enable compression of revisions on all 
                // collections
                record.DocumentsCompression = new DocumentsCompressionConfiguration(compressRevisions: true, "Orders");

                // Update the server
                store.Maintenance.Server.Send(new UpdateDatabaseOperation(record, record.Etag));
            }
            #endregion

            #region CompressAllCollections
            using (var store = new DocumentStore())
            {
                // Retrieve database record
                var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));

                // To configure compression on all collections for new or edited documents
                var dbrecord = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
                dbrecord.DocumentsCompression.CompressAllCollections = true;

                // Update the server
                store.Maintenance.Server.Send(new UpdateDatabaseOperation(record, record.Etag));
            }
            #endregion
        }
    }
}
