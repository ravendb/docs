<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Operations;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Revisions\GetRevisionsOperation;
use RavenDB\Documents\Operations\Revisions\GetRevisionsOperationParameters;
use RavenDB\Documents\Operations\Revisions\RevisionsResult;
use RavenDB\Samples\Infrastructure\Orders\Company;

class GetRevisions
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region getAllRevisions
            // Define the get revisions operation, pass the document id
            $getRevisionsOp = new GetRevisionsOperation(Company::class, "Companies/1-A");

            // Execute the operation by passing it to Operations.Send
            /** @var RevisionsResult $revisions */
            $revisions = $documentStore->operations()->send($getRevisionsOp);

            // The revisions info:
            /** @var array<Company> $allRevisions */
            $allRevisions = $revisions->getResults(); // All the revisions
            $revisionsCount = $revisions->getTotalResults();    // Total number of revisions
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region getRevisionsWithPaging
            $start = 0;
            $pageSize = 100;

            while (true)
            {
                // Execute the get revisions operation
                // Pass the document id, start & page size to get
                /** @var RevisionsResult $revisions */
                $revisions = $documentStore->operations()->send(
                    new GetRevisionsOperation(Company::class, "Companies/1-A", $start, $pageSize));

                // Process the retrieved revisions here

                if (count($revisions->getResults()) < $pageSize)
                    break; // No more revisions to retrieve

                // Increment 'start' by page-size, to get the "next page" in next iteration
                $start += $pageSize;
            }
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region getRevisionsWithPagingParams
            $parameters = new GetRevisionsOperationParameters();
            $parameters->setId("Companies/1-A");
            $parameters->setStart(0);
            $parameters->setPageSize(100);

            /** @var RevisionsResult $revisions */
            $revisions = $documentStore->operations->send(
                new GetRevisionsOperation(Company::class, $parameters));
            # endregion
        } finally {
            $store->close();
        }
    }
}
