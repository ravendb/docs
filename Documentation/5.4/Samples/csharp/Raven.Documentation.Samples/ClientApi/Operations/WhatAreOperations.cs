using System;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Counters;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide.Operations;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class WhatAreOperations
    {
        public interface ISendSyntax
        {
            #region operations_send
            // Available overloads:
            void Send(IOperation operation, SessionInfo sessionInfo = null);
            TResult Send<TResult>(IOperation<TResult> operation, SessionInfo sessionInfo = null);
            Operation Send(IOperation<OperationIdResult> operation, SessionInfo sessionInfo = null);
            
            PatchStatus Send(PatchOperation operation);
            PatchOperation.Result<TEntity> Send<TEntity>(PatchOperation<TEntity> operation);
            #endregion

            #region operations_send_async
            // Available overloads:
            Task SendAsync(IOperation operation,
                CancellationToken token = default(CancellationToken), SessionInfo sessionInfo = null);
            Task<TResult> SendAsync<TResult>(IOperation<TResult> operation,
                CancellationToken token = default(CancellationToken), SessionInfo sessionInfo = null);
            Task<Operation> SendAsync(IOperation<OperationIdResult> operation,
                CancellationToken token = default(CancellationToken), SessionInfo sessionInfo = null);
            
            Task<PatchStatus> SendAsync(PatchOperation operation,
                CancellationToken token = default(CancellationToken));
            Task<PatchOperation.Result<TEntity>> SendAsync<TEntity>(PatchOperation<TEntity> operation,
                CancellationToken token = default(CancellationToken));
            #endregion

            #region maintenance_send
            // Available overloads:
            void Send(IMaintenanceOperation operation);
            TResult Send<TResult>(IMaintenanceOperation<TResult> operation);
            Operation Send(IMaintenanceOperation<OperationIdResult> operation);
            #endregion

            #region maintenance_send_async
            // Available overloads:
            Task SendAsync(IMaintenanceOperation operation,
                CancellationToken token = default(CancellationToken));
            Task<TResult> SendAsync<TResult>(IMaintenanceOperation<TResult> operation,
                CancellationToken token = default(CancellationToken));
            Task<Operation> SendAsync(IMaintenanceOperation<OperationIdResult> operation,
                CancellationToken token = default(CancellationToken));
            #endregion

            #region server_send
            // Available overloads:
            void Send(IServerOperation operation);
            TResult Send<TResult>(IServerOperation<TResult> operation);
            Operation Send(IServerOperation<OperationIdResult> operation);
            #endregion

            #region server_send_async
            // Available overloads:
            Task SendAsync(IServerOperation operation,
                CancellationToken token = default(CancellationToken));
            Task<TResult> SendAsync<TResult>(IServerOperation<TResult> operation,
                CancellationToken token = default(CancellationToken));
            Task<Operation> SendAsync(IServerOperation<OperationIdResult> operation,
                CancellationToken token = default(CancellationToken));
            #endregion
              
            /*
            #region waitForCompletion_syntax
            // Available overloads:
            public IOperationResult WaitForCompletion(TimeSpan? timeout = null)
            public IOperationResult WaitForCompletion(CancellationToken token)
            
            public TResult WaitForCompletion<TResult>(TimeSpan? timeout = null)
                where TResult : IOperationResult
            public TResult WaitForCompletion<TResult>(CancellationToken token)
                where TResult : IOperationResult
            #endregion
            */

            /*
            #region waitForCompletion_syntax_async
            // Available overloads:
            public Task<IOperationResult> WaitForCompletionAsync(TimeSpan? timeout = null)
            public Task<IOperationResult> WaitForCompletionAsync(CancellationToken token)
            
            public async Task<TResult> WaitForCompletionAsync<TResult>(TimeSpan? timeout = null)
                where TResult : IOperationResult
            public async Task<TResult> WaitForCompletionAsync<TResult>(CancellationToken token)
                where TResult : IOperationResult    
            #endregion
            */
            
            /*
            #region kill_syntax
            // Available overloads:
            public void Kill()
            public async Task KillAsync(CancellationToken token = default)
            #endregion
            */
        }

        public void Examples()
        {
            using var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Northwind"
            };

            #region operations_ex
            // Define operation, e.g. get all counters info for a document
            IOperation<CountersDetail> getCountersOp = new GetCountersOperation("products/1-A");
            
            // Execute the operation by passing the operation to Operations.Send
            CountersDetail allCountersResult = documentStore.Operations.Send(getCountersOp);
            
            // Access the operation result
            int numberOfCounters = allCountersResult.Counters.Count;
            #endregion

            #region maintenance_ex
            // Define operation, e.g. stop an index 
            IMaintenanceOperation stopIndexOp = new StopIndexOperation("Orders/ByCompany");
                
            // Execute the operation by passing the operation to Maintenance.Send
            documentStore.Maintenance.Send(stopIndexOp);
            
            // This specific operation returns void
            // You can send another operation to verify the index running status
            IMaintenanceOperation<IndexStats> indexStatsOp = new GetIndexStatisticsOperation("Orders/ByCompany");
            IndexStats indexStats =  documentStore.Maintenance.Send(indexStatsOp);
            IndexRunningStatus status = indexStats.Status; // will be "Paused"
            #endregion

            #region server_ex
            // Define operation, e.g. get the server build number
            IServerOperation<BuildNumber> getBuildNumberOp = new GetBuildNumberOperation();
                
            // Execute the operation by passing the operation to Maintenance.Server.Send
            BuildNumber buildNumberResult = documentStore.Maintenance.Server.Send(getBuildNumberOp);

            // Access the operation result
            int version = buildNumberResult.BuildVersion;
            #endregion
            
            #region kill_ex
            // Define operation, e.g. delete all discontinued products 
            // Note: This operation implements interface: 'IOperation<OperationIdResult>'
            IOperation<OperationIdResult> deleteByQueryOp =
                new DeleteByQueryOperation("from Products where Discontinued = true");
                
            // Execute the operation
            // Send returns an 'Operation' object that can be 'killed'
            Operation operation = documentStore.Operations.Send(deleteByQueryOp);
            
            // Call 'Kill' to abort operation
            operation.Kill();
            #endregion
        }
        
        #region wait_timeout_ex
        public void WaitForCompletionWithTimout(
            TimeSpan timeout,
            DocumentStore documentStore)
        {
            // Define operation, e.g. delete all discontinued products 
            // Note: This operation implements interface: 'IOperation<OperationIdResult>'
            IOperation<OperationIdResult> deleteByQueryOp =
                new DeleteByQueryOperation("from Products where Discontinued = true");
                
            // Execute the operation
            // Send returns an 'Operation' object that can be awaited on
            Operation operation = documentStore.Operations.Send(deleteByQueryOp);
            
            try
            {
                // Call method 'WaitForCompletion' to wait for the operation to complete.
                // If a timeout is specified, the method will only wait for the specified time frame.
                BulkOperationResult result =
                    (BulkOperationResult)operation.WaitForCompletion(timeout);
                
                // The operation has finished within the specified timeframe
                long numberOfItemsDeleted = result.Total; // Access the operation result
            }
            catch (TimeoutException e)
            {
                // The operation did Not finish within the specified timeframe
            }
        }
        #endregion

        #region wait_token_ex
        public void WaitForCompletionWithCancellationToken(
            CancellationToken token,
            DocumentStore documentStore)
        {
            // Define operation, e.g. delete all discontinued products 
            // Note: This operation implements interface: 'IOperation<OperationIdResult>'
            IOperation<OperationIdResult> deleteByQueryOp =
                new DeleteByQueryOperation("from Products where Discontinued = true");
                    
            // Execute the operation
            // Send returns an 'Operation' object that can be awaited on
            Operation operation = documentStore.Operations.Send(deleteByQueryOp);
            
            try
            {
                // Call method 'WaitForCompletion' to wait for the operation to complete.
                // Pass a CancellationToken in order to stop waiting upon a cancellation request.
                BulkOperationResult result =
                    (BulkOperationResult)operation.WaitForCompletion(token);
                
                // The operation has finished, no cancellation request was made
                long numberOfItemsDeleted = result.Total; // Access the operation result
            }
            catch (TimeoutException e)
            {
                // The operation did Not finish at cancellation time
            }
        }
        #endregion

        public async Task ExamplesAsync()
        {
            using var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Northwind"
            };

            #region operations_ex_async
            // Define operation, e.g. get all counters info for a document
            IOperation<CountersDetail> getCountersOp = new GetCountersOperation("products/1-A");
                
            // Execute the operation by passing the operation to Operations.Send
            CountersDetail allCountersResult = await documentStore.Operations.SendAsync(getCountersOp);
            
            // Access the operation result
            int numberOfCounters = allCountersResult.Counters.Count;
            #endregion

            #region maintenance_ex_async
            // Define operation, e.g. stop an index 
            IMaintenanceOperation stopIndexOp = new StopIndexOperation("Orders/ByCompany");
            
            // Execute the operation by passing the operation to Maintenance.Send
            await documentStore.Maintenance.SendAsync(stopIndexOp);
            
            // This specific operation returns void
            // You can send another operation to verify the index running status
            IMaintenanceOperation<IndexStats> indexStatsOp = new GetIndexStatisticsOperation("Orders/ByCompany");
            IndexStats indexStats =  await documentStore.Maintenance.SendAsync(indexStatsOp);
            IndexRunningStatus status = indexStats.Status; // will be "Paused"
            #endregion

            #region server_ex_async
            // Define operation, e.g. get the server build number
            IServerOperation<BuildNumber> getBuildNumberOp = new GetBuildNumberOperation();
                
            // Execute the operation by passing the operation to Maintenance.Server.Send
            BuildNumber buildNumberResult = await documentStore.Maintenance.Server.SendAsync(getBuildNumberOp);
            
            // Access the operation result
            int version = buildNumberResult.BuildVersion;
            #endregion
            
            #region kill_ex_async
            // Define operation, e.g. delete all discontinued products 
            // Note: This operation implements interface: 'IOperation<OperationIdResult>'
            IOperation<OperationIdResult> deleteByQueryOp =
                new DeleteByQueryOperation("from Products where Discontinued = true");
                
            // Execute the operation
            // SendAsync returns an 'Operation' object that can be 'killed'
            Operation operation = await documentStore.Operations.SendAsync(deleteByQueryOp);
            
            // Call 'KillAsync' to abort operation
            await operation.KillAsync();
            
            // Assert that operation is no longer running
            await Assert.ThrowsAsync<TaskCanceledException>(() => 
                operation.WaitForCompletionAsync(TimeSpan.FromSeconds(30)));
            #endregion
        }
        
        #region wait_timeout_ex_async
        public async Task WaitForCompletionWithTimoutAsync(
            TimeSpan timeout,
            DocumentStore documentStore)
        {
            // Define operation, e.g. delete all discontinued products 
            // Note: This operation implements interface: 'IOperation<OperationIdResult>'
            IOperation<OperationIdResult> deleteByQueryOp =
                new DeleteByQueryOperation("from Products where Discontinued = true");
                
            // Execute the operation
            // SendAsync returns an 'Operation' object that can be awaited on
            Operation operation = await documentStore.Operations.SendAsync(deleteByQueryOp);
            
            try
            {
                // Call method 'WaitForCompletionAsync' to wait for the operation to complete.
                // If a timeout is specified, the method will only wait for the specified time frame.
                BulkOperationResult result = 
                    await operation.WaitForCompletionAsync(timeout)
                        .ConfigureAwait(false) as BulkOperationResult;
                
                // The operation has finished within the specified timeframe
                long numberOfItemsDeleted = result.Total; // Access the operation result
            }
            catch (TimeoutException e)
            {
                // The operation did Not finish within the specified timeframe
            }
        }
        #endregion

        #region wait_token_ex_async
        public async Task WaitForCompletionWithCancellationTokenAsync(
            CancellationToken token,
            DocumentStore documentStore)
        {
            // Define operation, e.g. delete all discontinued products 
            // Note: This operation implements interface: 'IOperation<OperationIdResult>'
            IOperation<OperationIdResult> deleteByQueryOp =
                new DeleteByQueryOperation("from Products where Discontinued = true");
                    
            // Execute the operation
            // SendAsync returns an 'Operation' object that can be awaited on
            Operation operation = await documentStore.Operations.SendAsync(deleteByQueryOp);
            
            try
            {
                // Call method 'WaitForCompletionAsync' to wait for the operation to complete.
                // Pass a CancellationToken in order to stop waiting upon a cancellation request.
                BulkOperationResult result = 
                    await operation.WaitForCompletionAsync(token)
                        .ConfigureAwait(false) as BulkOperationResult;
                
                // The operation has finished, no cancellation request was made
                long numberOfItemsDeleted = result.Total; // Access the operation result
            }
            catch (TimeoutException e)
            {
                // The operation did Not finish at cancellation time
            }
        }
        #endregion
    }
}
