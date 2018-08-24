using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class HowToSubscribeToCounterChanges
    {
        private interface IFoo
        {
            #region counter_changes_1
            IChangesObservable<CounterChange> ForCounter(string counterName);
            #endregion

            #region counter_changes_2
            IChangesObservable<CounterChange> ForCounterOfDocument(string documentId, string counterName);
            #endregion

            #region counter_changes_3
            IChangesObservable<CounterChange> ForCountersOfDocument(string documentId);
            #endregion

            #region counter_changes_4
            IChangesObservable<CounterChange> ForAllCounters();
            #endregion
        }

        public HowToSubscribeToCounterChanges()
        {
            using (var store = new DocumentStore())
            {
                #region counter_changes_1_1
                IDisposable subscription = store
                    .Changes()
                    .ForCounter("Likes")
                    .Subscribe(
                        change =>
                        {
                            switch (change.Type)
                            {
                                case CounterChangeTypes.Increment:
                                    // do something
                                    break;
                            }
                        });
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region counter_changes_2_1
                IDisposable subscription = store
                    .Changes()
                    .ForCounterOfDocument("companies/1-A", "Likes")
                    .Subscribe(
                        change =>
                        {
                            switch (change.Type)
                            {
                                case CounterChangeTypes.Increment:
                                    // do something
                                    break;
                            }
                        });
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region counter_changes_3_1
                IDisposable subscription = store
                    .Changes()
                    .ForCountersOfDocument("companies/1-A")
                    .Subscribe(
                        change =>
                        {
                            switch (change.Type)
                            {
                                case CounterChangeTypes.Increment:
                                    // do something
                                    break;
                            }
                        });
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region counter_changes_4_1
                IDisposable subscription = store
                    .Changes()
                    .ForAllCounters()
                    .Subscribe(
                        change =>
                        {
                            switch (change.Type)
                            {
                                case CounterChangeTypes.Increment:
                                    // do something
                                    break;
                            }
                        });
                #endregion
            }
        }
    }
}
