using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Counters;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class Counters
    {
        private class GetCountersOperation
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

        #region counter_batch

        public class CounterBatch
        {
            public bool ReplyWithAllNodesValues; // A flag that indicates if the results should include a
                                                 // dictionary of counter values per database node
            public List<DocumentCountersOperation> Documents = new List<DocumentCountersOperation>();
        }

        #endregion

        private class CounterBatchOperation
        {
            #region counter_batch_op
            public CounterBatchOperation(CounterBatch counterBatch)
            #endregion
            { }
        }

        #region document_counters_op

        public class DocumentCountersOperation
        {
            public string DocumentId; // Id of the document that holds the counters
            public List<CounterOperation> Operations; // A list of counter operations to perform
        }

        #endregion

        #region counter_operation

        public class CounterOperation
        {
            public CounterOperationType Type;
            public string CounterName;
            public long Delta; // the value to increment by
        }

        #endregion

        #region counter_operation_type

        public enum CounterOperationType
        {
            Increment,
            Delete,
            Get
        }

        #endregion

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
            public Dictionary<string, long> CounterValues; // A dictionary of counter values per database node
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
                {
                    #region counter_batch_exmpl1

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
                                        // No Delta specified, value will be incremented by 1
                                        // (From RavenDB 6.2 on, the default Delta is 1)

                                        Type = CounterOperationType.Increment,
                                        CounterName = "dislikes"
                                    }
                                }
                            },
                            new DocumentCountersOperation
                            {
                                DocumentId = "users/2",
                                Operations = new List<CounterOperation>
                                {
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Increment,
                                        CounterName = "likes",
                                        Delta = 100
                                    },
                                    new CounterOperation
                                    {
                                        // this will create a new counter "score", with initial value 50
                                        // "score" will be added to counter-names in "users/2" metadata

                                        Type = CounterOperationType.Increment,
                                        CounterName = "score", 
                                        Delta = 50
                                    }
                                }
                            }
                        }
                    }));
                    #endregion
                }
                {
                    #region counter_batch_exmpl2

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
                                        Type = CounterOperationType.Get,
                                        CounterName = "likes"
                                    },
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Get,
                                        CounterName = "downloads"
                                    }
                                }
                            },
                            new DocumentCountersOperation
                            {
                                DocumentId = "users/2",
                                Operations = new List<CounterOperation>
                                {
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Get,
                                        CounterName = "likes"
                                    },
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Get,
                                        CounterName = "score"
                                    }
                                }
                            }
                        }
                    }));
                    #endregion
                }
                {
                    #region counter_batch_exmpl3

                    var operationResult = store.Operations.Send(new CounterBatchOperation(new CounterBatch
                    {
                        Documents = new List<DocumentCountersOperation>
                        {
                            new DocumentCountersOperation
                            {
                                DocumentId = "users/1",
                                Operations = new List<CounterOperation>
                                {
                                    // "likes" and "dislikes" will be removed from counter-names in "users/1" metadata
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Delete,
                                        CounterName = "likes"
                                    },
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Delete,
                                        CounterName = "dislikes"
                                    }
                                }
                            },
                            new DocumentCountersOperation
                            {
                                DocumentId = "users/2",
                                Operations = new List<CounterOperation>
                                {
                                    // "downloads" will be removed from counter-names in "users/2" metadata

                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Delete,
                                        CounterName = "downloads"
                                    }
                                }
                            }
                        }
                    }));
                    #endregion
                }

                {
                    #region counter_batch_exmpl4

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
                                        Delta = 30
                                    },
                                    new CounterOperation
                                    {
                                        // The results will include null for this 'Get'
                                        // since we deleted the "dislikes" counter in the previous example flow
                                        Type = CounterOperationType.Get,
                                        CounterName = "dislikes"
                                    },
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Delete,
                                        CounterName = "downloads"
                                    }
                                }
                            },
                            new DocumentCountersOperation
                            {
                                DocumentId = "users/2",
                                Operations = new List<CounterOperation>
                                {
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Get,
                                        CounterName = "likes"
                                    },
                                    new CounterOperation
                                    {
                                        Type = CounterOperationType.Delete,
                                        CounterName = "dislikes"
                                    }
                                }
                            }
                        }
                    }));
                    #endregion
                }
            }
        }
    }

}
