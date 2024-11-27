<?php

use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Documents\Session\TransactionMode;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

class Overview
{
    public function howToQuery(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {

            # region open_cluster_session_sync
            $sessionOptions = new SessionOptions();

            // Set mode to be cluster-wide
            $sessionOptions->setTransactionMode(TransactionMode::clusterWide());

            // Session will be single-node when either:
            //   * Mode is not specified
            //   * Explicitly set TransactionMode.SingleNode

            $session = $store->openSession($sessionOptions);
            try {
                //
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }
}
