using System;
using System.Collections.Generic;
using System.IO;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Operations.Counters;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class HowToSubscribeToOperationChanges
    {
        // Changes overload that allows subscribing to operations changes
        private interface IFoo
        {
            #region changes-definition
            ISingleNodeDatabaseChanges Changes(string database, string nodeTag);
            #endregion
        }

        public HowToSubscribeToOperationChanges()
        {
            // Subscribing operations with Changes requires opening Changes with DB name AND node Tag
            using (var store = new DocumentStore())
            {
                var operationId = 1;
                string database = "database";
                string nodeTag = "node";
                #region changes_ForOperationId
                IDisposable subscription = store
                    .Changes(database, nodeTag)
                    .ForOperationId(operationId)
                    .Subscribe(
                        change =>
                        {
                            switch (change.State.Status)
                            {
                                case OperationStatus.InProgress:
                                    //Do Something
                                    break;
                                case OperationStatus.Completed:
                                    //Do Something
                                    break;
                                case OperationStatus.Faulted:
                                    //Do Something
                                    break;
                                case OperationStatus.Canceled:
                                    //Do Something
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        });
                #endregion
            };

            // CounterBatchOperation that is not passed a Delta increments counters by 1
            using (var store = new DocumentStore())
            {
                #region CounterBatchOperation
                var operationResult = store.Operations.Send(new CounterBatchOperation(new CounterBatch
                {
                    Documents = new List<DocumentCountersOperation>
                    {
                        new DocumentCountersOperation
                        {
                            DocumentId = "users/1",
                            Operations = new List<CounterOperation>
                            {
                                new CounterOperation
                                {
                                    Type = CounterOperationType.Increment,
                                    CounterName = "likes",
                                    Delta = 5
                                },
                                new CounterOperation
                                {
                                    // No delta specified, value will be incremented by 1
                                    Type = CounterOperationType.Increment,
                                    CounterName = "dislikes"
                                }
                            }
                        }
                    }
                }));
                #endregion
            }

            // Compare exchange can be created only with index set to 0
            using (var store = new DocumentStore())
            {
                #region CmpXchg
                // This will fail since the initial index value is not 0 but 123
                CompareExchangeResult<string> compareExchangeResult
                    = store.Operations.Send(
                        new PutCompareExchangeValueOperation<string>("key", "value", 123));

                // This will succeed since the item is created with an initial index of 0
                compareExchangeResult = store.Operations.Send(
                        new PutCompareExchangeValueOperation<string>("key", "value", 0));

                // This will succeed since the item already exists and we just update its value
                compareExchangeResult
                    = store.Operations.Send(
                        new PutCompareExchangeValueOperation<string>("key", "value", 123));

                #endregion
            }
        }
    }
}
