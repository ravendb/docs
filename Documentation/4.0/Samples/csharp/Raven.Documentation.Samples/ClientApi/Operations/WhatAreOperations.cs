using System;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Attachments;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class WhatAreOperations
    {
        public interface IInterface
        {
            #region Client_Operations_api
            void Send(IOperation operation, SessionInfo sessionInfo = null);

            TResult Send<TResult>(IOperation<TResult> operation, SessionInfo sessionInfo = null);

            Operation Send(IOperation<OperationIdResult> operation, SessionInfo sessionInfo = null);

            PatchStatus Send(PatchOperation operation);

            PatchOperation.Result<TEntity> Send<TEntity>(PatchOperation<TEntity> operation);
            #endregion

            #region Client_Operations_api_async
            Task SendAsync(IOperation operation, CancellationToken token = default(CancellationToken), SessionInfo sessionInfo = null);

            Task<TResult> SendAsync<TResult>(IOperation<TResult> operation, CancellationToken token = default(CancellationToken), SessionInfo sessionInfo = null);

            Task<Operation> SendAsync(IOperation<OperationIdResult> operation, CancellationToken token = default(CancellationToken), SessionInfo sessionInfo = null);

            Task<PatchStatus> SendAsync(PatchOperation operation, CancellationToken token = default(CancellationToken));

            Task<PatchOperation.Result<TEntity>> SendAsync<TEntity>(PatchOperation<TEntity> operation, CancellationToken token = default(CancellationToken));
            #endregion

            #region Maintenance_Operations_api
            void Send(IMaintenanceOperation operation);

            TResult Send<TResult>(IMaintenanceOperation<TResult> operation);

            Operation Send(IMaintenanceOperation<OperationIdResult> operation);
            #endregion

            #region Maintenance_Operations_api_async
            Task SendAsync(IMaintenanceOperation operation, CancellationToken token = default(CancellationToken));

            Task<TResult> SendAsync<TResult>(IMaintenanceOperation<TResult> operation, CancellationToken token = default(CancellationToken));

            Task<Operation> SendAsync(IMaintenanceOperation<OperationIdResult> operation, CancellationToken token = default(CancellationToken));
            #endregion

            #region Server_Operations_api
            void Send(IServerOperation operation);

            TResult Send<TResult>(IServerOperation<TResult> operation);

            Operation Send(IServerOperation<OperationIdResult> operation);
            #endregion

            #region Server_Operations_api_async
            Task SendAsync(IServerOperation operation, CancellationToken token = default(CancellationToken));

            Task<TResult> SendAsync<TResult>(IServerOperation<TResult> operation, CancellationToken token = default(CancellationToken));

            Task<Operation> SendAsync(IServerOperation<OperationIdResult> operation, CancellationToken token = default(CancellationToken));
            #endregion

        }

        public async Task Examples()
        {
            using (var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Northwind"
            })
            {

                #region Client_Operations_1
                using (AttachmentResult fetchedAttachment = documentStore.Operations.Send(new GetAttachmentOperation("users/1", "file.txt", AttachmentType.Document, null)))
                {
                    //do stuff with the attachment stream --> fetchedAttachment.Stream                                       
                }
                #endregion

                #region Client_Operations_1_async
                using (AttachmentResult fetchedAttachment = await documentStore.Operations.SendAsync(new GetAttachmentOperation("users/1", "file.txt", AttachmentType.Document, null)))
                {
                    //do stuff with the attachment stream --> fetchedAttachment.Stream                                       
                }
                #endregion

                #region Maintenance_Operations_1                
                documentStore.Maintenance.Send(new StopIndexOperation("Orders/ByCompany"));
                #endregion

                #region Maintenance_Operations_1_async                
                await documentStore.Maintenance.SendAsync(new StopIndexOperation("Orders/ByCompany"));
                #endregion

                #region Server_Operations_1                               
                var getBuildNumberResult = documentStore.Maintenance.Server.Send(new GetBuildNumberOperation());
                Console.WriteLine(getBuildNumberResult.BuildVersion);
                #endregion

                #region Server_Operations_1_async                               
                var buildNumber = await documentStore.Maintenance.Server.SendAsync(new GetBuildNumberOperation());
                Console.WriteLine(buildNumber.BuildVersion);
                #endregion

            }
        }
    }
}
