using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Raven.Client.Documents;
using Raven.Client.Documents.Attachments;
using Raven.Client.Documents.Linq.Indexing;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class WhatAreOperations
    {
        public WhatAreOperations()
        {

            using (var documentStore = new DocumentStore
            {
                Urls = new[] {"http://localhost:8080"},
                Database = "Northwind"
            })
            {
                #region Client_Operations
                //this will address the default database
                using (var fetchedAttachment = documentStore.Operations.Send(new GetAttachmentOperation("users/1", "file.txt", AttachmentType.Document, null)))
                {
                    //do stuff with the attachment stream --> fetchedAttachment.Stream                                       
                }                                               
                
                //this will address the specified database
                using (var fetchedAttachment = documentStore.Operations.ForDatabase("TestDB").Send(new GetAttachmentOperation("users/1", "file.txt", AttachmentType.Document, null)))
                {
                    //do stuff with the attachment stream --> fetchedAttachment.Stream                                       
                }
                #endregion

                #region Maintenance_Operations                
                //this will address the default database
                documentStore.Maintenance.Send(new StopIndexOperation("Orders/ByCompany"));               

                //this will address the specified database
                documentStore.Maintenance.ForDatabase("TestDB").Send(new StopIndexOperation("Orders/ByCompany"));               
                #endregion
                
                #region Server_Operations                               
                var getBuildNumberResult = documentStore.Maintenance.Server.Send(new GetBuildNumberOperation());
                Console.WriteLine(getBuildNumberResult.BuildVersion);
                #endregion
            }

        }     
    }
}
