<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Session;

use RavenDB\Documents\DocumentStore;

interface IFoo
{
    # region syntax
    function getCountFor(?string $id): int;
    # endregion
}

class Counting
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region getCount
                // Get the number of revisions for document 'companies/1-A"
                $revisionsCount = $session->advanced()->revisions()->getCountFor("companies/1-A");
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
