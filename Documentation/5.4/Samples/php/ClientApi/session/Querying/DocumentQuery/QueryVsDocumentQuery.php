<?php

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Queries\Query;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Order;

class  Orders_ByShipToAndLines extends AbstractIndexCreationTask {

}

class QueryVsDocumentQuery
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region query_1a
                $session->query(Order::class)
                # endregion
                ;

                # region query_1b
                $session->advanced()->documentQuery(Order::class)
                # endregion
                ;

                # region query_2a
                $session->query(Order::class, Orders_ByShipToAndLines::class)
                # endregion
                ;

                # region query_2b
                $session->advanced()->documentQuery(Order::class, Orders_ByShipToAndLines::class)
                # endregion
                ;

                # region query_3a
                $session->query(Order::class, Query::index("Orders/ByShipToAndLines"))
                # endregion
                ;

                # region query_3b
                $session->advanced()->documentQuery(Order::class, "Orders/ByShipToAndLines", null, false);
                # endregion

                # region query_4a
                $session->query(Order::class, Query::collection("orders"))
                # endregion
                ;

                # region query_4b
                $session->advanced()->documentQuery(Order::class, null, "orders", false);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
