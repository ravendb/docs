<?php

use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

class CountQueryResults
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region count_3
                /** @var int $numberOfOrders */
                $numberOfOrders = $session->advanced()
                    ->documentQuery(Order::class)
                    ->whereEquals("ship_to.country", "UK")
                     // Calling 'Count' from Raven.Client.Documents.Session
                    ->Count();

                // The query returns the NUMBER of orders shipped to UK (int)
                # endregion

            } finally {
                $session->close();
            }

        } finally {
            $store->close();
        }
    }
}
