<?php

use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;

interface FooInterface {
    # region stats_1
    public function statistics(QueryStatistics &$stats): DocumentQueryInterface;
    # endregion
}

# region stats_2
class QueryStatistics
{
    private bool $isStale = false;
    private int $durationInMs = 0;
    private int $totalResults = 0;
    private int $longTotalResults = 0;
    private int $skippedResults = 0;
    private ?int $scannedResults = null;
    private ?DateTimeInterface $timestamp = null;
    private ?string $indexName = null;
    private ?DateTimeInterface $indexTimestamp = null;
    private ?DateTimeInterface $lastQueryTime = null;
    private ?int $resultEtag = null;
    private ?string $nodeTag = null;
}
# endregion

class HowToGetQueryStatistics
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {

            $session = $store->openSession();
            try {
                # region stats_3
                $stats = new QueryStatistics();

                $employees = $session->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->statistics($stats)
                    ->toList();

                $totalResults = $stats->getTotalResults();
                $durationInMs = $stats->getDurationInMs();
                # endregion

            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
