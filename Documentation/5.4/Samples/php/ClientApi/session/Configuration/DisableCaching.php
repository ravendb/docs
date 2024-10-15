<?php

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\SessionOptions;

class DisableCaching
{
    public function example(): void
    {
        $store = new DocumentStore();

        try {
            # region disable_caching
            $sessionOptions = new SessionOptions();
            $sessionOptions->setNoCaching(true);

            $session = $store->openSession($sessionOptions);
            try {
                // code here
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();

        }
    }
}
