using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class HowToSubscribeToIndexChanges
    {
        private interface IFoo
        {
            #region index_changes_1
            IChangesObservable<IndexChange> ForIndex(string indexName);
            #endregion

            #region index_changes_3
            IChangesObservable<IndexChange> ForAllIndexes();
            #endregion
        }

        public HowToSubscribeToIndexChanges()
        {
            using (var store = new DocumentStore())
            {
                #region index_changes_2
                IDisposable subscription = store
                    .Changes()
                    .ForIndex("Orders/All")
                    .Subscribe(
                        change =>
                        {
                            switch (change.Type)
                            {
                                case IndexChangeTypes.None:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.BatchCompleted:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexAdded:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexRemoved:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexDemotedToIdle:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexPromotedFromIdle:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexDemotedToDisabled:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexMarkedAsErrored:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.SideBySideReplace:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.IndexPaused:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.LockModeChanged:
                                    //Do someting
                                    break;
                                case IndexChangeTypes.PriorityChanged:
                                    //Do someting
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        });
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region index_changes_4
                IDisposable subscription = store
                    .Changes()
                    .ForAllIndexes()
                    .Subscribe(change => Console.WriteLine("{0} on index {1}", change.Type, change.Name));
                #endregion
            }
        }
    }
}
