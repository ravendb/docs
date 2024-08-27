using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;
using Raven.Client.Documents.Operations;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class HowToSubscribeToOperationChanges1
    {
        private interface IFoo
        {
            #region operation_changes_1
            IChangesObservable<OperationStatusChange> ForOperationId(long operationId);
            #endregion

            #region operation_changes_3
            IChangesObservable<OperationStatusChange> ForAllOperations();
            #endregion
        }

        public HowToSubscribeToOperationChanges1()
        {
            using (var store = new DocumentStore())
            {
                var dbName = "sampleDB";
                var nodeTag = "A";
                var operationId = 7;
                #region operation_changes_2
                IDisposable subscription = store
                    .Changes(dbName, nodeTag)
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
            }

            using (var store = new DocumentStore())
            {
                var dbName = "sampleDB";
                var nodeTag = "A";
                #region operation_changes_4
                IDisposable subscription = store
                    .Changes(dbName, nodeTag)
                    .ForAllOperations()
                    .Subscribe(change => Console.WriteLine("Operation #{1} reports progress: {0}", change.State.Progress.ToJson(), change.OperationId));
                #endregion
            }
        }
    }
}
