using System;
using System.Linq;
using Raven.Client.Documents;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using Raven.Client.Documents.Operations.TimeSeries;
using Raven.Client.Documents.Commands.Batches;
using PatchRequest = Raven.Client.Documents.Operations.PatchRequest;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client;
using System.Threading.Tasks;
using Raven.Client.Documents.Session.TimeSeries;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Linq;
using static Raven.Client.Documents.BulkInsert.BulkInsertOperation;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Queries.TimeSeries;
using Raven.Client.Documents.Indexes.TimeSeries;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Documents.Session.Loaders;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;
using System.IO.Compression;

namespace Documentation.Samples.DocumentExtensions.TimeSeries
{
    public class migration
    {
        public void SwitchCompressionAlgorithm()
        {
            using (var store = new DocumentStore())
            {
                #region SwitchCompressionAlgorithm
                var DocumentConventions = new DocumentConventions
                {
                    // Switch HTTP compresion algorithm
                    HttpCompressionAlgorithm = HttpCompressionAlgorithm.Gzip
                };
                #endregion
            }
        }



        public void switchBulkInsertState()
        {
            using (var store = new DocumentStore())
            {
                #region switchBulkInsertState
                using (var bulk = store.BulkInsert(new BulkInsertOptions
                {
                    // Disable bulk-insert compression
                    CompressionLevel = CompressionLevel.NoCompression
                }));
                #endregion
            }
        }

    }
}


