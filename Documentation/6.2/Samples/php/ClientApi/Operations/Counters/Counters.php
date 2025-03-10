<?php

namespace RavenDB\Samples\ClientApi\Operations\Counters;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Counters\CounterBatch;
use RavenDB\Documents\Operations\Counters\CounterBatchOperation;
use RavenDB\Documents\Operations\Counters\CounterOperation;
use RavenDB\Documents\Operations\Counters\CounterOperationType;
use RavenDB\Documents\Operations\Counters\CountersDetail;
use RavenDB\Documents\Operations\Counters\DocumentCountersOperation;
use RavenDB\Documents\Operations\Counters\DocumentCountersOperationList;
use RavenDB\Documents\Operations\Counters\GetCountersOperation;

class Foo
{
    /*
    # region get_counter_constructor
    class GetCountersOperation {
        public function __construct(?string $docId, string|StringArray|array|null $counters = 
            null, bool $returnFullResults = false) { ... }
    }
    # endregion
    */

    public function examples(): void
    {
        # region get_single_counter
        $docId = "users/1";
        $counter = "likes";
        $returnFullResults = false;

        $operation = new GetCountersOperation($docId, $counter, $returnFullResults);
        # endregion

        # region get_multiple_counters
        $docId = "users/1";
        $counters = [ "likes", "score"];
        $returnFullResults = false;

        $operation = new GetCountersOperation($docId, $counters, $returnFullResults);
        # endregion

        # region get_all_counters
        $docId = "users/1";
        $returnFullResults = false;

        $operation = new GetCountersOperation($docId, null, $returnFullResults);
        # endregion
    }
}

/*
# region counter_batch
class CounterBatch
{
    private bool $replyWithAllNodesValues = false;  // A flag that indicates if the results should include a
                                                    // dictionary of counter values per database node

    private ?DocumentCountersOperationList $documents = null;

    private bool $fromEtl = false;

    // ... getter and setters
}
# endregion
*/

/*
# region counter_batch_op
class CounterBatchOperation
{
    public function __construct(CounterBatch $counterBatch) { ... }
}
# endregion
*/

/*
# region document_counters_op
class DocumentCountersOperation
{
    private ?CounterOperationList $operations = null;   // A list of counter operations to perform
    private ?string $documentId = null;                 // Id of the document that holds the counters
}
# endregion
*/

# region counter_operation

/*
class CounterOperation
{
    private ?CounterOperationType $type = null;
    private ?string $counterName = null;
    private ?int $delta = null;     // the value to increment by
}
# endregion
*/

/*
# region counter_operation_type
class CounterOperationType
{
    public function isIncrement(): bool;
    public static function increment(): CounterOperationType;

    public function isDelete(): bool;
    public static function delete(): CounterOperationType;

    public function isGet(): bool;
    public static function get(): CounterOperationType;

    public function isPut(): bool;
    public static function put(): CounterOperationType;
}
# endregion
*/

/*
# region counters_detail
class CountersDetail
{
    private ?CounterDetail $counters = null;
}
# endregion
*/

# region counter_detail
class CounterDetail
{
    private ?string $documentId = null;     // ID of the document that holds the counter
    private ?string $counterName = null;    // The counter name
    private ?int $totalValue = null;        // Total counter value
    private ?int $etag = null;              // Counter Etag
    private ?array $counterValues = [];     // A dictionary of counter values per database node

    private ?string $changeVector = null;   // Change vector of the counter

    // ... getters and setters
}
# endregion

class Counters
{
    public function examples(): void
    {
        $store = new DocumentStore();
        try {


            # region get_counters1
            /** @var CountersDetail  $operationResult */
            $operationResult = $store
                ->operations()
                ->send(new GetCountersOperation("users/1", "likes"));
            # endregion


            # region get_counters2
            /** @var CountersDetail $operationResult */
            $operationResult = $store
                ->operations()
                ->send(new GetCountersOperation("users/1", [ "likes", "dislikes" ]));
            # endregion

            # region get_counters3
            /** @var CountersDetail $operationResult */
            $operationResult = $store->operations()
                ->send(new GetCountersOperation("users/1"));
            # endregion

            # region get_counters4
            /** @var CountersDetail $operationResult */
            $operationResult = $store
                ->operations()
                ->send(new GetCountersOperation("users/1", "likes", true));
            # endregion

            # region counter_batch_exmpl1
            $operation1 = new DocumentCountersOperation();
            $operation1->setDocumentId("users/1");
            $operation1->setOperations([
                CounterOperation::create("likes", CounterOperationType::increment(), 5),
                CounterOperation::create("dislikes", CounterOperationType::increment()) // No delta specified, value will stay the same
            ]);

            $operation2 = new DocumentCountersOperation();
            $operation2->setDocumentId("users/2");
            $operation2->setOperations([
                CounterOperation::create("likes", CounterOperationType::increment(), 100),

                // this will create a new counter "score", with initial value 50
                // "score" will be added to counter-names in "users/2" metadata
                CounterOperation::create("score", CounterOperationType::increment(), 50)
            ]);

            $counterBatch = new CounterBatch();
            $counterBatch->setDocuments([$operation1, $operation2]);
            $store->operations()->send(new CounterBatchOperation($counterBatch));
            # endregion



            # region counter_batch_exmpl2
            $operation1 = new DocumentCountersOperation();
            $operation1->setDocumentId("users/1");
            $operation1->setOperations([
                CounterOperation::create("likes", CounterOperationType::get()),
                CounterOperation::create("downloads", CounterOperationType::get())
            ]);

            $operation2 = new DocumentCountersOperation();
            $operation2->setDocumentId("users/2");
            $operation2->setOperations([
                CounterOperation::create("likes", CounterOperationType::get()),
                CounterOperation::create("score", CounterOperationType::get())
            ]);

            $counterBatch = new CounterBatch();
            $counterBatch->setDocuments([$operation1, $operation2]);

            $store->operations()->send(new CounterBatchOperation($counterBatch));
            # endregion


            # region counter_batch_exmpl3
            $operation1 = new DocumentCountersOperation();
            $operation1->setDocumentId("users/1");
            $operation1->setOperations([
                // "likes" and "dislikes" will be removed from counter-names in "users/1" metadata
                CounterOperation::create("likes", CounterOperationType::delete()),
                CounterOperation::create("dislikes", CounterOperationType::delete())
            ]);

            $operation2 = new DocumentCountersOperation();
            $operation2->setDocumentId("users/2");
            $operation2->setOperations([
                // "downloads" will be removed from counter-names in "users/2" metadata
                CounterOperation::create("downloads", CounterOperationType::delete())
            ]);

            $counterBatch = new CounterBatch();
            $counterBatch->setDocuments([$operation1, $operation2]);
            $store->operations()->send(new CounterBatchOperation($counterBatch));
            # endregion

            # region counter_batch_exmpl4
            $operation1 = new DocumentCountersOperation();
            $operation1->setDocumentId("users/1");
            $operation1->setOperations([
                CounterOperation::create("likes", CounterOperationType::increment(), 30),
                // The results will include null for this 'Get'
                // since we deleted the "dislikes" counter in the previous example flow
                CounterOperation::create("dislikes", CounterOperationType::get()),
                CounterOperation::create("downloads", CounterOperationType::delete())
            ]);

            $operation2 = new DocumentCountersOperation();
            $operation2->setDocumentId("users/2");
            $operation2->setOperations([
                CounterOperation::create("likes", CounterOperationType::get()),
                CounterOperation::create("dislikes", CounterOperationType::delete())
            ]);

            $counterBatch = new CounterBatch();
            $counterBatch->setDocuments([$operation1, $operation2]);
            $store->operations()->send(new CounterBatchOperation($counterBatch));
            # endregion

        } finally {
            $store->close();
        }
    }
}
