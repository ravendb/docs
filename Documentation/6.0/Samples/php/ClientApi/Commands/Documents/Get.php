<?php

namespace RavenDB\Samples\ClientApi\Commands\Documents;

use RavenDB\Documents\Commands\GetDocumentsCommand;
use RavenDB\Documents\Commands\GetDocumentsResult;
use RavenDB\Documents\DocumentStore;

class Get
{
    public function single(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_single
                $command = GetDocumentsCommand::forSingleDocument("orders/1-A", null, false);
                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $documentsResult */
                $documentsResult = $command->getResult();
                $order = $documentsResult->getResults()[0];
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function multiple(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_multiple
                $command = GetDocumentsCommand::forMultipleDocuments(["orders/1-A", "employees/3-A"], null, false);
                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $order = $result->getResults()[0];
                $employee = $result->getResults()[1];
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function includes(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_includes
                // Fetch employees/5-A and his boss.
                $command = GetDocumentsCommand::forSingleDocument("employees/5-A", [ "ReportsTo" ], false);
                $session->advanced()->getRequestExecutor()->execute($command);
                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $employee = $result->getResults()[0];
                if (array_key_exists("ReportsTo", $employee)) {
                    $bossId = $employee["ReportsTo"];

                    $boss = $result->getIncludes()[$bossId];
                }
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }


    public function missing(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_missing
                // Assuming that products/9999-A doesn't exist.
                $command = GetDocumentsCommand::forMultipleDocuments([ "products/1-A", "products/9999-A", "products/3-A" ], null, false);
                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $products = $result->getResults(); // products/1-A, null, products/3-A
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function paged(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_paged
                $command = GetDocumentsCommand::withStartAndPageSize(0, 128);
                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $firstDocs = $result->getResults();
                # endregion

            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function startsWith(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_startswith
                // return up to 128 documents with key that starts with 'products'
                $command = GetDocumentsCommand::withStartWith(
                    startWith: "products",
                    startAfter: null,
                    matches: null,
                    exclude: null,
                    start: 0,
                    pageSize: 128,
                    metadataOnly: false);
                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $products = $result->getResults();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function startsWithMatches(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_startswith_matches
                // return up to 128 documents with key that starts with 'products/'
                // and rest of the key begins with "1" or "2" e.g. products/10, products/25
                $command = GetDocumentsCommand::withStartWith(
                    startWith: "products",
                    startAfter: null,
                    matches: "1*|2*",
                    exclude: null,
                    start: 0,
                    pageSize: 128,
                    metadataOnly: false);
                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $products = $result->getResults();
                # endregion

            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }

    public function startsWithMatchesEnd(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region get_sample_startswith_matches_end
                // return up to 128 documents with key that starts with 'products/'
                // and rest of the key have length of 3, begins and ends with "1"
                // and contains any character at 2nd position e.g. products/101, products/1B1
                $command = GetDocumentsCommand::withStartWith(
                    startWith: "products",
                    startAfter: null,
                    matches: "1?1",
                    exclude: null,
                    start: 0,
                    pageSize: 128,
                    metadataOnly: false);

                $session->advanced()->getRequestExecutor()->execute($command);

                /** @var GetDocumentsResult $result */
                $result = $command->getResult();
                $products = $result->getResults();
                # endregion

            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }


}

/*
public class GetDocumentsCommandInterface
{
    # region get_interface_single
    public static function forSingleDocument(string $id, StringArray|array|null $includes = null, bool $metadataOnly = false): GetDocumentsCommand;
    # endregion

    # region get_interface_multiple
    public static function forMultipleDocuments(StringArray|array|null $ids, StringArray|array|null $includes, bool $metadataOnly = false): GetDocumentsCommand;
    # endregion

    # region get_interface_paged
    public static function withStartAndPageSize(int $start, int $pageSize): GetDocumentsCommand;
    # endregion

    # region get_interface_startswith
    public static function withStartWith(
        ?string $startWith,
        ?string $startAfter,
        ?string $matches,
        ?string $exclude,
        int $start,
        int $pageSize,
        bool $metadataOnly
    ): GetDocumentsCommand;
    # endregion
}
*/
