package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.counters.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;

public class Counters {

    private static class Definitions {
        private class GetCountersOperation {
        /*
        //region get_single_counter
        public GetCountersOperation(String docId, String counter)
        public GetCountersOperation(String docId, String counter, boolean returnFullResults)
        //endregion
        */

        /*
        //region get_multiple_counters
        public GetCountersOperation(String docId, String[] counters)
        public GetCountersOperation(String docId, String[] counters, boolean returnFullResults)
        //endregion
        */

        /*
        //region get_all_counters
        public GetCountersOperation(String docId)
        public GetCountersOperation(String docId, boolean returnFullResults)
        //endregion
         */
        }

        //region counter_batch
        public class CounterBatch {
            private boolean replyWithAllNodesValues;
            private List<DocumentCountersOperation> documents = new ArrayList<>();

            // getters and setters
        }
        //endregion

        private class CounterBatchOperation {
            //region counter_batch_op
            public CounterBatchOperation(CounterBatch counterBatch)
            //endregion
            {}
        }

        //region document_counters_op
        public class DocumentCountersOperation {
            private List<CounterOperation> operations;
            private String documentId;

            // getters and setters
        }
        //endregion

        //region counter_operation
        public static class CounterOperation {
            private CounterOperationType type;
            private String counterName;
            private long delta; // the value to increment by

            // getters and setters
        }
        //endregion

        //region counter_operation_type
        public enum CounterOperationType {
            NONE,
            INCREMENT,
            DELETE,
            GET,
            PUT
        }
        //endregion

        //region counters_detail
        public class CountersDetail {

            private List<CounterDetail> counters;

            // getters and setters
        }
        //endregion

        //region counter_detail
        public class CounterDetail {
            private String documentId; // ID of the document that holds the counter
            private String counterName; // The counter name
            private long totalValue; // Total counter value
            private long etag; // Counter Etag
            private Map<String, Long> counterValues; // A map of counter values per database node

            private String changeVector; // Change vector of the counter

            // getters and setters
        }
        //endregion
    }

    public class CountersExamples {
        public CountersExamples() {
            try (IDocumentStore store = new DocumentStore()) {
                {
                    //region get_counters1
                    CountersDetail operationResult = store.operations()
                        .send(new GetCountersOperation("users/1", "likes"));
                    //endregion
                }

                {
                    //region get_counters2
                    CountersDetail operationResult = store.operations()
                        .send(new GetCountersOperation("users/1", new String[]{ "likes", "dislikes" }));
                    //endregion
                }

                {
                    //region get_counters3
                    CountersDetail operationResult = store.operations()
                        .send(new GetCountersOperation("users/1"));
                    //endregion
                }

                {
                    //region get_counters4
                    CountersDetail operationResult = store.operations()
                        .send(new GetCountersOperation("users/1", "likes", true));
                    //endregion
                }

                {
                    //region counter_batch_exmpl1
                    DocumentCountersOperation operation1 = new DocumentCountersOperation();
                    operation1.setDocumentId("users/1");
                    operation1.setOperations(Arrays.asList(
                        CounterOperation.create("likes", CounterOperationType.INCREMENT, 5),
                        CounterOperation.create("dislikes", CounterOperationType.INCREMENT) // No delta specified, value will stay the same
                    ));

                    DocumentCountersOperation operation2 = new DocumentCountersOperation();
                    operation2.setDocumentId("users/2");
                    operation2.setOperations(Arrays.asList(
                        CounterOperation.create("likes", CounterOperationType.INCREMENT, 100),

                        // this will create a new counter "score", with initial value 50
                        // "score" will be added to counter-names in "users/2" metadata
                        CounterOperation.create("score", CounterOperationType.INCREMENT, 50)
                    ));

                    CounterBatch counterBatch = new CounterBatch();
                    counterBatch.setDocuments(Arrays.asList(operation1, operation2));
                    store.operations().send(new CounterBatchOperation(counterBatch));
                    //endregion
                }

                {
                    //region counter_batch_exmpl2
                    DocumentCountersOperation operation1 = new DocumentCountersOperation();
                    operation1.setDocumentId("users/1");
                    operation1.setOperations(Arrays.asList(
                        CounterOperation.create("likes", CounterOperationType.GET),
                        CounterOperation.create("downloads", CounterOperationType.GET)
                    ));

                    DocumentCountersOperation operation2 = new DocumentCountersOperation();
                    operation2.setDocumentId("users/2");
                    operation2.setOperations(Arrays.asList(
                        CounterOperation.create("likes", CounterOperationType.GET),
                        CounterOperation.create("score", CounterOperationType.GET)
                    ));

                    CounterBatch counterBatch = new CounterBatch();
                    counterBatch.setDocuments(Arrays.asList(operation1, operation2));

                    store.operations().send(new CounterBatchOperation(counterBatch));
                    //endregion
                }

                {
                    //region counter_batch_exmpl3
                    DocumentCountersOperation operation1 = new DocumentCountersOperation();
                    operation1.setDocumentId("users/1");
                    operation1.setOperations(Arrays.asList(
                        // "likes" and "dislikes" will be removed from counter-names in "users/1" metadata
                        CounterOperation.create("likes", CounterOperationType.DELETE),
                        CounterOperation.create("dislikes", CounterOperationType.DELETE)
                    ));

                    DocumentCountersOperation operation2 = new DocumentCountersOperation();
                    operation2.setDocumentId("users/2");
                    operation2.setOperations(Arrays.asList(
                        // "downloads" will be removed from counter-names in "users/2" metadata
                        CounterOperation.create("downloads", CounterOperationType.DELETE)
                    ));

                    CounterBatch counterBatch = new CounterBatch();
                    counterBatch.setDocuments(Arrays.asList(operation1, operation2));
                    store.operations().send(new CounterBatchOperation(counterBatch));
                    //endregion
                }

                {
                    //region counter_batch_exmpl4
                    DocumentCountersOperation operation1 = new DocumentCountersOperation();
                    operation1.setDocumentId("users/1");
                    operation1.setOperations(Arrays.asList(
                        CounterOperation.create("likes", CounterOperationType.INCREMENT, 30),
                        // The results will include null for this 'Get' 
                        // since we deleted the "dislikes" counter in the previous example flow
                        CounterOperation.create("dislikes", CounterOperationType.GET),
                        CounterOperation.create("downloads", CounterOperationType.DELETE)
                    ));

                    DocumentCountersOperation operation2 = new DocumentCountersOperation();
                    operation2.setDocumentId("users/2");
                    operation2.setOperations(Arrays.asList(
                        CounterOperation.create("likes", CounterOperationType.GET),
                        CounterOperation.create("dislikes", CounterOperationType.DELETE)
                    ));

                    CounterBatch counterBatch = new CounterBatch();
                    counterBatch.setDocuments(Arrays.asList(operation1, operation2));
                    store.operations().send(new CounterBatchOperation(counterBatch));
                    //endregion
                }
            }
        }
    }
}
