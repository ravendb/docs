using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class HowToSubscribeToTimeSeriesChanges
    {
        private interface IFoo
        {
            #region timeseries_changes_1
            IChangesObservable<TimeSeriesChange> ForTimeSeries(string timeSeriesName);
            #endregion

            #region timeseries_changes_2
            IChangesObservable<TimeSeriesChange> ForTimeSeriesOfDocument(string documentId, string timeSeriesName);
            #endregion

            #region timeseries_changes_3
            IChangesObservable<TimeSeriesChange> ForTimeSeriesOfDocument(string documentId);
            #endregion

            #region timeseries_changes_4
            IChangesObservable<TimeSeriesChange> ForAllTimeSeries();
            #endregion
        }

        public HowToSubscribeToTimeSeriesChanges()
        {
            using (var store = new DocumentStore())
            {
                #region timeseries_changes_1_1
                IDisposable subscription = store
                    .Changes()
                    .ForTimeSeries("Likes")
                    .Subscribe
                        (change =>
                        {
                            switch (change.Type)
                            {
                                case TimeSeriesChangeTypes.Delete:
                                    // do something
                                break;
                            }
                        });
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region timeseries_changes_2_1
                IDisposable subscription = store
                    .Changes()
                    .ForTimeSeriesOfDocument("companies/1-A", "Likes")
                    .Subscribe
                        (change =>
                        {
                            switch (change.Type)
                            {
                                case TimeSeriesChangeTypes.Delete:
                                    // do something
                                break;
                            }
                        });
                #endregion
            }


            using (var store = new DocumentStore())
            {
                #region timeseries_changes_3_1
                IDisposable subscription = store
                    .Changes()
                    .ForTimeSeriesOfDocument("companies/1-A")
                    .Subscribe
                        (change =>
                        {
                            switch (change.Type)
                            {
                                case TimeSeriesChangeTypes.Delete:
                                    // do something
                                break;
                            }
                        });
                    #endregion
            }

            using (var store = new DocumentStore())
            {
                #region timeseries_changes_4_1
                IDisposable subscription = store
                    .Changes()
                    .ForAllTimeSeries()
                    .Subscribe
                        (change =>
                        {
                            switch (change.Type)
                            {
                                case TimeSeriesChangeTypes.Delete:
                                    // do something
                                break;
                            }
                        });
                #endregion
            }
        }
    }
}
