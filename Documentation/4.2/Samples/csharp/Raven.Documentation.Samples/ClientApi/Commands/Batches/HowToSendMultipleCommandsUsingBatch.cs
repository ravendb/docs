using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations;
using Sparrow.Json;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.ClientApi.Commands.Batches
{
    public class BatchInterface
    {
        private class BatchCommand
        {
            #region batch_1
            public BatchCommand(DocumentConventions conventions, JsonOperationContext context, List<ICommandData> commands, BatchOptions options = null)
                #endregion
            {

            }
        }
    }

    public class HowToSendMultipleCommandsUsingBatch
    {
        #region batch_2
        public class BatchOptions
        {
            public bool WaitForReplicas { get; set; }

            //if set to true, will wait for replication to be performed on at least a majority
            //of DB instances (applies only when WaitForReplicas is set to true)
            public bool Majority { get; set; }

            public int NumberOfReplicasToWaitFor { get; set; }

            public TimeSpan WaitForReplicasTimeout { get; set; }

            public bool ThrowOnTimeoutInWaitForReplicas { get; set; }

            public bool WaitForIndexes { get; set; }

            public TimeSpan WaitForIndexesTimeout { get; set; }

            public bool ThrowOnTimeoutInWaitForIndexes { get; set; }

            public string[] WaitForSpecificIndexes { get; set; }

            public TimeSpan? RequestTimeout { get; set; }
        }
        #endregion

        public async Task Examples()
        {
            using (var documentStore = new DocumentStore())
            {
                #region batch_3
                using (var session = documentStore.OpenSession())
                {
                    var commands = new List<ICommandData>
                    {
                        new PutCommandData("users/3", null, new DynamicJsonValue
                        {
                            ["Name"] = "James"
                        }),
                        new PatchCommandData("users/1-A", null, new PatchRequest
                        {
                            Script = "this.Name = 'Nhoj';"
                        }, null),
                        new DeleteCommandData("users/2-A", null)
                    };

                    var batch = new BatchCommand(documentStore.Conventions, session.Advanced.Context, commands);
                    session.Advanced.RequestExecutor.Execute(batch, session.Advanced.Context);
                }
                #endregion

                #region batch_3_async
                using (var session = documentStore.OpenAsyncSession())
                {
                    var commands = new List<ICommandData>
                    {
                        new PutCommandData("users/3", null, new DynamicJsonValue
                        {
                            ["Name"] = "James"
                        }),
                        new PatchCommandData("users/1-A", null, new PatchRequest
                        {
                            Script = "this.Name = 'Nhoj';"
                        }, null),
                        new DeleteCommandData("users/2-A", null)
                    };

                    var batch = new BatchCommand(documentStore.Conventions, session.Advanced.Context, commands);
                    await session.Advanced.RequestExecutor.ExecuteAsync(batch, session.Advanced.Context);
                }
                #endregion
            }
        }
    }
}
