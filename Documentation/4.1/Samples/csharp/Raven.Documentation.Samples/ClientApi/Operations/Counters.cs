using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Counters;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class Counters
    {
        public class GetCountersOperation
        {
            #region get_single_counter
            public GetCountersOperation(string docId, string counter, bool returnFullResults = false)
            #endregion
            { }

            #region get_multiple_counters
            public GetCountersOperation(string docId, string[] counters, bool returnFullResults = false)
            #endregion
            { }

            #region get_all_counters
            public GetCountersOperation(string docId, bool returnFullResults = false)
            #endregion
            { }

        }

        #region counters_detail
        public class CountersDetail
        {
            public List<CounterDetail> Counters;
        }
        #endregion

        #region counter_detail
        public class CounterDetail
        {
            public string DocumentId; // ID of the document that holds the counter
            public string CounterName; // The counter name
            public long TotalValue; // Total counter value
            public Dictionary<string, long> CounterValues; // A dictionary of counter value per database node
            public long Etag; // Counter Etag
            public string ChangeVector; // Change vector of the counter
        }
        #endregion
    }

    public class CountersExamples
    {
        public CountersExamples()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_counters1

                    var operationResult = store.Operations
                        .Send(new GetCountersOperation("users/1", "likes"));

                    #endregion
                }

                {
                    #region get_counters2

                    var operationResult = store.Operations
                        .Send(new GetCountersOperation("users/1", new []{"likes", "dislikes" }));

                    #endregion
                }

                {
                    #region get_counters3

                    var operationResult = store.Operations
                        .Send(new GetCountersOperation("users/1"));

                    #endregion
                }

                {
                    #region get_counters4

                    var operationResult = store.Operations
                        .Send(new GetCountersOperation("users/1", "likes", true));

                    #endregion
                }
            }
        }
    }

}
