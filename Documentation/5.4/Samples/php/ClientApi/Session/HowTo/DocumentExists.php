<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use RavenDB\Documents\DocumentStore;

interface ExistsInterface
{
    # region exists_1
    public function exists(?string $id): bool;
    # endregion
}


class DocumentExists
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region exists_2
                $exists = $session->advanced()->exists("employees/1-A");

                if ($exists)
                {
                    // document 'employees/1-A exists
                }
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
