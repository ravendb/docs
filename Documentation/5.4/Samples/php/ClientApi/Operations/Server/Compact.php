<?php

namespace RavenDB\Samples\ClientApi\Operations\Server;

use RavenDB\Constants\PhpClient;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\GetIndexNamesOperation;
use RavenDB\Documents\Operations\Operation;
use RavenDB\Documents\Operations\OperationIdResult;
use RavenDB\Documents\Operations\CompactDatabaseOperation;
use RavenDB\ServerWide\CompactSettings;
use RavenDB\ServerWide\DatabaseRecordWithEtag;
use RavenDB\ServerWide\GetDatabaseRecordOperation;
use RavenDB\Type\StringArrayResult;

class Compact
{
    public function samples(): void
    {
        $documentStore = new DocumentStore();
        try {
            # region compact_0

            // Define the compact settings
            $settings = new CompactSettings();
            $settings->setDatabaseName("Northwind");
            // Set 'Documents' to true to compact all documents in database
            // Indexes are not set and will not be compacted
            $settings->setDocuments(true);


            // Define the compact operation, pass the settings
            /** @var OperationIdResult  $compactOp */
            $compactOp = new CompactDatabaseOperation($settings);

            // Execute compaction by passing the operation to Maintenance.Server.Send
            /** @var Operation $operation */
            $operation = $documentStore->maintenance()->server()->send($compactOp);

            // Wait for operation to complete, during compaction the database is offline
            $operation->waitForCompletion();
            # endregion
        } finally {
            $documentStore->close();
        }

        $documentStore = new DocumentStore();
        try {
            # region compact_1
            // Define the compact settings
            $settings = new CompactSettings();

            // Database to compact
            $settings->setDatabaseName("Northwind");

            // Setting 'Documents' to false will compact only the specified indexes
            $settings->setDocuments(false);

            // Specify which indexes to compact
            $settings->setIndexes([ "Orders/Totals", "Orders/ByCompany" ]);

            // Optimize indexes is Lucene's feature to gain disk space and efficiency
            // Set whether to skip this optimization when compacting the indexes
            $settings->setSkipOptimizeIndexes(false);


            // Define the compact operation, pass the settings
            /** @var OperationIdResult $compactOp */
            $compactOp = new CompactDatabaseOperation($settings);

            // Execute compaction by passing the operation to Maintenance.Server.Send
            /** @var Operation $operation */
            $operation = $documentStore->maintenance()->server()->send($compactOp);
            // Wait for operation to complete
            $operation->waitForCompletion();
            # endregion
            } finally {
                $documentStore->close();
            }


        $documentStore = new DocumentStore();
        try {
            # region compact_2
            // Get all indexes names in the database using the 'GetIndexNamesOperation' operation
            // Use 'ForDatabase' if the target database is different than the default database defined on the store
            /** @var StringArrayResult $allIndexNames */
            $allIndexNames = $documentStore->maintenance()->forDatabase("Northwind")
                    ->send(new GetIndexNamesOperation(0, PhpClient::INT_MAX_VALUE));

            // Define the compact settings
            $settings = new CompactSettings();
            $settings->setDatabaseName("Northwind");    // Database to compact
            $settings->setDocuments(true);              // Compact all documents
            $settings->setIndexes($allIndexNames->getArrayCopy());      // All indexes will be compacted
            $settings->setSkipOptimizeIndexes(true);    // Skip Lucene's indexes optimization

            // Define the compact operation, pass the settings
            /** @var OperationIdResult $compactOp */
            $compactOp = new CompactDatabaseOperation($settings);

            // Execute compaction by passing the operation to Maintenance.Server.Send
            /** @var Operation $operation */
            $operation = $documentStore->maintenance()->server()->send($compactOp);

            // Wait for operation to complete
            $operation->waitForCompletion();
            # endregion
        } finally {
            $documentStore->close();
        }

        $documentStore = new DocumentStore();
        try {
            # region compact_3
            // Get all member nodes in the database-group using the 'GetDatabaseRecordOperation' operation
            /** @var DatabaseRecordWithEtag $databaseRecord */
            $databaseRecord = $documentStore->maintenance()->server()->send(new GetDatabaseRecordOperation("Northwind"));

            $allMemberNodes = $databaseRecord->getTopology()->getMembers();

            // Define the compact settings as needed
            $settings = new CompactSettings();

            $settings->setDatabaseName("Northwind");
            $settings->setDocuments(true); //Compact all documents in database

            // Execute the compact operation on each member node
            foreach ($allMemberNodes as $nodeTag) {
                // Define the compact operation, pass the settings
                /** @var OperationIdResult $compactOp */
                $compactOp = new CompactDatabaseOperation($settings);

                // Execute the operation on a specific node
                // Use `ForNode` to specify the node to operate on
                /** @var Operation $operation */
                $operation = $documentStore->maintenance()->server()->forNode($nodeTag)->send($compactOp);
                // Wait for operation to complete
                $operation->waitForCompletion();
            }
            # endregion
        } finally {
            $documentStore->close();
        }
    }
}

/*
interface IFoo
{
    # region syntax
    CompactDatabaseOperation(?CompactSettings $compactSettings)
    # endregion
}
*/
