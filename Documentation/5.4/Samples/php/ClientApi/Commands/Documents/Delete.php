<?php

namespace RavenDB\Samples\ClientApi\Commands\Documents;

use RavenDB\Documents\Commands\DeleteDocumentCommand;
use RavenDB\Documents\DocumentStore;

class Delete
{
    public function DeleteSamples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region delete_sample
                $command = new DeleteDocumentCommand("employees/1-A", null);
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
class DeleteDocumentCommandInterface {
    # region delete_interface
    DeleteDocumentCommand(?string $idOrCopy, ?string $changeVector = null);
    # endregion
}
*/
