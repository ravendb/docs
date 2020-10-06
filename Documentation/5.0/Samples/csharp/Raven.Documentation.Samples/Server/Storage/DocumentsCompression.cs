using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Client.Http;
using Raven.Client.Json;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Documentation.Samples.Indexes.Querying;

namespace Raven.Documentation.Samples.ClientApi
{
    class DocumentsCompression
    {
        /*
        #region Syntax_0
        public class DocumentsCompressionConfiguration
        {
            public string[] Collections { get; set; }
            public bool CompressRevisions { get; set; }
        }
        #endregion
        */

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
