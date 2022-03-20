using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi
{
    class DocumentsCompressionConfigExample
    {
        #region Syntax_0
        public class DocumentsCompressionConfiguration
        {
            public string[] Collections { get; set; }
            public bool CompressRevisions { get; set; }
            public bool CompressAllCollections { get; set; }
        }
        #endregion
    }
    class DocumentsCompression
    {
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
        }
    }
}
