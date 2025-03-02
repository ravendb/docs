<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\CollectionStatistics;
use RavenDB\Documents\Operations\DatabaseStatistics;
use RavenDB\Documents\Operations\DetailedCollectionStatistics;
use RavenDB\Documents\Operations\DetailedDatabaseStatistics;
use RavenDB\Documents\Operations\GetCollectionStatisticsOperation;
use RavenDB\Documents\Operations\GetDetailedStatisticsOperation;
use RavenDB\Documents\Operations\GetDetailedCollectionStatisticsOperation;
use RavenDB\Documents\Operations\GetStatisticsOperation;

class GetStats
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region stats_1
            // Pass an instance of class `GetCollectionStatisticsOperation` to the store
            /** @var  CollectionStatistics $stats */
            $stats = $store->maintenance()->send((new GetCollectionStatisticsOperation());
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region stats_2
            // Pass an instance of class `GetDetailedCollectionStatisticsOperation` to the store
            /** @var DetailedCollectionStatistics $stats */
            $stats = $store->maintenance()->send(new GetDetailedCollectionStatisticsOperation());
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region stats_3
            // Pass an instance of class `GetStatisticsOperation` to the store
            /** @var DatabaseStatistics $stats */
            $stats = $store->maintenance()->send(new GetStatisticsOperation());
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region stats_4
            // Pass an instance of class `GetDetailedStatisticsOperation` to the store
            /** @var DetailedDatabaseStatistics $stats */
            $stats = $store->maintenance()->send(new GetDetailedStatisticsOperation());
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region stats_5
            // Get stats for 'AnotherDatabase':
            /** @var DatabaseStatistics $stats */
            $stats = $store->maintenance()->forDatabase("AnotherDatabase")->send(new GetStatisticsOperation());
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
# region stats_1_results
// Collection stats results:
class CollectionStatistics
{
    // Total # of documents in all collections
    private ?int $countOfDocuments = null;
    // Total # of conflicts
    private ?int $countOfConflicts = null;
    // Total # of documents per collection
    private array $collections = [];

    // ... getters and setters
}
# endregion
*/

/*
# region stats_2_results
// Detailed collection stats results:
public class DetailedCollectionStatistics
{
    // Total # of documents in all collections
    public long CountOfDocuments { get; set; }
    // Total # of conflicts
    public long CountOfConflicts { get; set; }
    // Collection details per collection
    public Dictionary<string, CollectionDetails> Collections { get; set; }
}

// Details per collection
class CollectionDetails
{
    private ?string $name = null;
    private ?int $countOfDocuments = null;
    private ?Size $size = null;
    private ?Size $documentsSize = null;
    private ?Size $tombstonesSize = null;
    private ?Size $revisionsSize = null;

    // ... getters and setters
}
# endregion
*/

/*
# region stats_3_results
// Database stats results:
class DatabaseStatistics implements ResultInterface
{
    private ?int $lastDocEtag = null;               // Last document etag in database
    private ?int $lastDatabaseEtag = null;          // Last database etag

    private ?int $countOfIndexes = null;            // Total # of indexes in database
    private ?int $countOfDocuments = null;          // Total # of documents in database
    private ?int $countOfRevisionDocuments = null;  // Total # of revision documents in database
    private ?int $countOfDocumentsConflicts = null; // Total # of documents conflicts in database
    private ?int $countOfTombstones = null;         // Total # of tombstones in database
    private ?int $countOfConflicts = null;          // Total # of conflicts in database
    private ?int $countOfAttachments = null;        // Total # of attachments in database
    private ?int $countOfUniqueAttachments = null;  // Total # of unique attachments in database
    private ?int $countOfCounterEntries = null;     // Total # of counter-group entries in database
    private ?int $countOfTimeSeriesSegments = null; // Total # of time-series segments in database

    // List of stale index names in database
    public function getStaleIndexes(): IndexInformationArray
    {
        return IndexInformationArray::fromArray(
            array_map(
                function (IndexInformation $index) {
                    return $index->isStale();
                },
            $this->indexes->getArrayCopy())
        );
    }

    // Statistics for each index in database
    private ?IndexInformationArray $indexes = null;

    private ?string $databaseChangeVector = null;           // Global change vector of the database
    private ?string $databaseId = null;                     // Database identifier
    private bool $is64Bit = false;                          // Indicates if process is 64-bit
    private ?string $pager = null;                          // Component handling the memory-mapped files
    private ?DateTimeInterface $lastIndexingTime = null;    // Last time of indexing an item
    private ?Size $sizeOnDisk = null;                       // Database size on disk
    private ?Size $tempBuffersSizeOnDisk = null;            // Temp buffers size on disk
    private ?int $numberOfTransactionMergerQueueOperations = null;

    // ... getters and setters
}
# endregion
*/

/*
# region stats_4_results
// Detailed database stats results:
class DetailedDatabaseStatistics extends DatabaseStatistics implements ResultInterface
{
    // Total # of identities in database
    private ?int $countOfIdentities = null;
    // Total # of compare-exchange items in database
    private ?int $countOfCompareExchange = null;
    // Total # of cmpXchg tombstones in database
    private ?int $countOfCompareExchangeTombstones = null;
    // Total # of TS deleted ranges values in database
    private ?int $countOfTimeSeriesDeletedRanges = null;

    // ... getters and setters
}
# endregion
*/
