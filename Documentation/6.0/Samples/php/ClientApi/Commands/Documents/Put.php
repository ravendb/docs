<?php

namespace RavenDB\Samples\ClientApi\Commands\Documents;

use RavenDB\Documents\Commands\PutDocumentCommand;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\DocumentInfo;
use RavenDB\Samples\Infrastructure\Orders\Category;

class Put
{
    public function PutSamples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region put_sample
                // Create a new document
                $doc = new Category();
                $doc->setName("My category");
                $doc->setDescription("My category description");

                // Create metadata on the document
                $docInfo = new DocumentInfo();
                $docInfo->setCollection("Categories");

                // Convert your entity to a BlittableJsonReaderObject
                $jsonDoc = $session->advanced()->getEntityToJson()->convertEntityToJson($doc, $docInfo);

                // The Put command (parameters are document ID, changeVector check is null, the document to store)
                $command = new PutDocumentCommand("categories/999", null, $jsonDoc);
                // RequestExecutor sends the command to the server
                $session->advanced()->getRequestExecutor()->execute($command);
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
class PutDocumentCommandInterface {
    # region put_interface
    PutDocumentCommand(string $idOrCopy, ?string $changeVector, array $document);
    # endregion
}
*/
