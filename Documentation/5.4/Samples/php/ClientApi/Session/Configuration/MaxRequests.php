<?php

use RavenDB\Documents\DocumentStore;

class MaxRequests
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region max_requests_1
                $session->advanced()->setMaxNumberOfRequestsPerSession(50);
                # endregion
            } finally {
                $session->close();
            }

            # region max_requests_2
            $store->getConventions()->setMaxNumberOfRequestsPerSession(100);
            # endregion
        } finally {
            $store->close();
        }
    }
}
