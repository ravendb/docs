using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations;
using Sparrow.Json;
using Sparrow.Json.Parsing;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Commands.Batches
{
    public class SendMultipleCommands
    {
        public async Task Examples()
        {
            #region batch_1
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor()
                       .ContextPool.AllocateOperationContext(out var storeContext))
            {
                // Define the list of batch commands to execute
                var commands = new List<ICommandData>
                {
                    new PutCommandData("employees/999", null, new DynamicJsonValue
                    {
                        ["FirstName"] = "James",
                        ["@metadata"] = new DynamicJsonValue
                        {
                            ["@collection"] = "employees"
                        }
                    }),
                    
                    new PatchCommandData("employees/2-A", null, new PatchRequest
                    {
                        Script = "this.HomePhone = 'New phone number';"
                    }, null),
                    
                    new DeleteCommandData("employees/3-A", null)
                };

                // Define the SingleNodeBatchCommand command
                var batchCommand = new SingleNodeBatchCommand(store.Conventions,
                    storeContext, commands);
    
                // Execute the batch command,
                // all the 3 commands defined in the list will be executed in a single transaction
                store.GetRequestExecutor().Execute(batchCommand, storeContext);
                
                // Can access the batch command results:
                var commandResults = batchCommand.Result.Results;
                Assert.Equal(3, commandResults.Length);

                var blittable = (BlittableJsonReaderObject)commandResults[0];
                
                blittable.TryGetMember("Type", out var commandType);
                Assert.Equal("PUT", commandType.ToString());
                
                blittable.TryGetMember("@id", out var documentId);
                Assert.Equal("employees/999", documentId.ToString());
            }
            #endregion
                
            #region batch_1_async
            using (var store = new DocumentStore())
            using (store.GetRequestExecutor()
                       .ContextPool.AllocateOperationContext(out var storeContext))
            {
                // Define the list of batch commands to execute
                var commands = new List<ICommandData>
                {
                    new PutCommandData("employees/999", null, new DynamicJsonValue
                    {
                        ["FirstName"] = "James",
                        ["@metadata"] = new DynamicJsonValue
                        {
                            ["@collection"] = "employees"
                        }
                    }),
                    
                    new PatchCommandData("employees/2-A", null, new PatchRequest
                    {
                        Script = "this.HomePhone = 'New phone number';"
                    }, null),
                    
                    new DeleteCommandData("employees/3-A", null)
                };

                // Define the SingleNodeBatchCommand command
                var batchCommand = new SingleNodeBatchCommand(store.Conventions,
                    storeContext, commands);
    
                // Execute the batch command,
                // all the 3 commands defined in the list will be executed in a single transaction
                await store.GetRequestExecutor().ExecuteAsync(batchCommand, storeContext);
                
                // Can access the batch command results:
                var commandResults = batchCommand.Result.Results;
                Assert.Equal(3, commandResults.Length);

                var blittable = (BlittableJsonReaderObject)commandResults[0];
                
                blittable.TryGetMember("Type", out var commandType);
                Assert.Equal("PUT", commandType.ToString());
                
                blittable.TryGetMember("@id", out var documentId);
                Assert.Equal("employees/999", documentId.ToString());
            }
            #endregion
            
            using (var store = new DocumentStore())
            {
                #region batch_2
                using (var session = store.OpenSession())
                {
                    // Define the list of batch commands to execute
                    var commands = new List<ICommandData>
                    {
                        new PutCommandData("employees/999", null, new DynamicJsonValue
                        {
                            ["FirstName"] = "James",
                            ["@metadata"] = new DynamicJsonValue
                            {
                                ["@collection"] = "employees"
                            }
                        }),
                        
                        new PatchCommandData("employees/2-A", null, new PatchRequest
                        {
                            Script = "this.HomePhone = 'New phone number';"
                        }, null),
                        
                        new DeleteCommandData("employees/3-A", null)
                    };

                    // Define the SingleNodeBatchCommand command
                    var batchCommand = new SingleNodeBatchCommand(store.Conventions,
                        session.Advanced.Context, commands);
                    
                    // Execute the batch command,
                    // all the 3 commands defined in the list will be executed in a single transaction
                    session.Advanced.RequestExecutor.Execute(batchCommand, session.Advanced.Context);
                    
                    // Can access the batch command results:
                    var commandResults = batchCommand.Result.Results;
                    Assert.Equal(3, commandResults.Length);

                    var blittable = (BlittableJsonReaderObject)commandResults[0];
                
                    blittable.TryGetMember("Type", out var commandType);
                    Assert.Equal("PUT", commandType.ToString());
                
                    blittable.TryGetMember("@id", out var documentId);
                    Assert.Equal("employees/999", documentId.ToString());
                }
                #endregion
                
                #region batch_2_async
                using (var session = store.OpenAsyncSession())
                {
                    // Define the list of batch commands to execute
                    var commands = new List<ICommandData>
                    {
                        new PutCommandData("employees/999", null, new DynamicJsonValue
                        {
                            ["FirstName"] = "James",
                            ["@metadata"] = new DynamicJsonValue
                            {
                                ["@collection"] = "employees"
                            }
                        }),
                        
                        new PatchCommandData("employees/2-A", null, new PatchRequest
                        {
                            Script = "this.HomePhone = 'New phone number';"
                        }, null),
                        
                        new DeleteCommandData("employees/3-A", null)
                    };

                    // Define the SingleNodeBatchCommand command
                    var batchCommand = new SingleNodeBatchCommand(store.Conventions,
                        session.Advanced.Context, commands);
                    
                    // Execute the batch command,
                    // all the 3 commands defined in the list will be executed in a single transaction
                    await session.Advanced.RequestExecutor.ExecuteAsync(
                        batchCommand, session.Advanced.Context);
                    
                    // Can access the batch command results:
                    var commandResults = batchCommand.Result.Results;
                    Assert.Equal(3, commandResults.Length);

                    var blittable = (BlittableJsonReaderObject)commandResults[0];
                
                    blittable.TryGetMember("Type", out var commandType);
                    Assert.Equal("PUT", commandType.ToString());
                
                    blittable.TryGetMember("@id", out var documentId);
                    Assert.Equal("employees/999", documentId.ToString());
                }
                #endregion
            }
        }
    }
    
    public class BatchInterface
    {
        private class SingleNodeBatchCommand
        {
            #region syntax_1
            public SingleNodeBatchCommand(
                    DocumentConventions conventions,
                    JsonOperationContext context, 
                    List<ICommandData> commands,
                    BatchOptions options = null)
            #endregion
            {
            }
        }
        
        #region syntax_2
        public class BatchOptions
        {
            public TimeSpan? RequestTimeout { get; set; }
            public ReplicationBatchOptions ReplicationOptions { get; set; }
            public IndexBatchOptions IndexOptions { get; set; }
            public ShardedBatchOptions ShardedOptions { get; set; }
        }
        
        public class ReplicationBatchOptions
        {
            // If set to true,
            // will wait for replication to be performed on at least a majority of DB instances. 
            public bool WaitForReplicas { get; set; }
            
            public int NumberOfReplicasToWaitFor { get; set; }
            public TimeSpan WaitForReplicasTimeout { get; set; }
            public bool Majority { get; set; }
            public bool ThrowOnTimeoutInWaitForReplicas { get; set; }
        }
        
        public sealed class IndexBatchOptions
        {
            public bool WaitForIndexes { get; set; }
            public TimeSpan WaitForIndexesTimeout { get; set; }
            public bool ThrowOnTimeoutInWaitForIndexes { get; set; }
            public string[] WaitForSpecificIndexes { get; set; }
        }
        
        public class ShardedBatchOptions
        {
            public ShardedBatchBehavior BatchBehavior { get; set; }
        }
        #endregion
        
        #region syntax_3
        // Executing `SingleNodeBatchCommand` returns the following object:
        // ================================================================
        
        public class BatchCommandResult
        {
            public BlittableJsonReaderArray Results { get; set; }
            public long? TransactionIndex { get; set; }
        }
        
        public sealed class BlittableArrayResult
        {
            public BlittableJsonReaderArray Results { get; set; }
            public long TotalResults { get; set; }
            public string ContinuationToken { get; set; }
        }
        #endregion
    }
}
