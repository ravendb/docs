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

namespace Documentation.Samples.DocumentExtensions.TimeSeries
{
    public class TcpCompression
    {
        public void DisableTcpCompressionForStore()
        {
            #region DisableTcpCompression
            using (var store = new DocumentStore())
            {
                var DocumentConventions = new DocumentConventions
                {
                    // Disable TCP Compression 
                    DisableTcpCompression = true
                };
            }
            #endregion
        }
        
   


        private struct Downloads
        {
            [TimeSeriesValue(0)] public double DownloadsCount;
        }


    }
}


