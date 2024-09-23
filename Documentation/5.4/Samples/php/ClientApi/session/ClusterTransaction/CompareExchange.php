<?php

use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

class CompareExchange
{
    public function runSamples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            # region open_cluster_session_sync
                $session = $store->openSession();
            # endregion
                try {
                    # region new_compare_exchange_sync
                    // The session must be first opened with cluster-wide mode
                    $session->advanced()->clusterTransaction()->createCompareExchangeValue(
                        key: "Best NoSQL Transactional Database",
                        value: "RavenDB"
                    );

                    $session->saveChanges();
                    # endregion

                    $key = "key";
                    $keys = [ "key" ];
                    $index = 0;
                    $value = null;
                    $item = $session->advanced()->clusterTransaction()->getCompareExchangeValue(null, $key);

                    # region methods_1_sync
                    $session->advanced()->clusterTransaction()->getCompareExchangeValue(null, $key);
                    # endregion

                    # region methods_2_sync
                    $session->advanced()->clusterTransaction()->getCompareExchangeValues(null, $keys);
                    # endregion

                    # region methods_3_sync
                    $session->advanced()->clusterTransaction()->createCompareExchangeValue($key, $value);
                    # endregion

                    # region methods_4_sync
                    // Delete by key & index
                    $session->advanced()->clusterTransaction()->deleteCompareExchangeValue($key, $index);

                    // Delete by compare-exchange item
                    $session->advanced()->clusterTransaction()->deleteCompareExchangeValue($item);
                    # endregion

                    //# region methods_5_sync
                    //$session->advanced()->clusterTransaction()->updateCompareExchangeValue(new CompareExchangeValue($key, $index, $value));
                    //# endregion

                    # region methods_sync_lazy_1
                    // Single value
                    $session->advanced()->clusterTransaction()->lazily()->getCompareExchangeValue(null, $key);

                    // Multiple values
                    $session->advanced()->clusterTransaction()->lazily()->getCompareExchangeValues(null, $keys);
                    # endregion
                } finally {
                    $session->close();
                }
        } finally {
            $store->close();
        }
    }
}
