<?php

use RavenDB\Documents\Queries\Timings\QueryTimings;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Product;

interface FooInterface
{
    # region syntax
    function timings(QueryTimings &$timings): DocumentQueryInterface;
    # endregion
}

class IncludeQueryTimings
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region timing_2
                $timings = new QueryTimings();

                /** @var array<Product> $resultsWithTimings */
                $resultsWithTimings = $session->advanced()->documentQuery(Product::class)
                    ->timings($timings)
                    ->search("Name", "Syrup")
                    ->toList();

                /** @var array<QueryTimings> $timingsMap */
                $timingsMap = $timings->getTimings();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
