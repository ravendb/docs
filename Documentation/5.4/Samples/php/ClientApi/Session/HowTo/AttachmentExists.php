<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use RavenDB\Documents\DocumentStore;

interface ExistsInterface
{
    # region syntax
    function exists(?string $documentId, ?string $name): bool;
    # endregion
}

class AttachmentExists
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region exists
                $exists = $session
                    ->advanced()
                    ->attachments()
                    ->exists("categories/1-A", "image.jpg");

                    if ($exists)
                    {
                        // attachment 'image.jpg' exists on document 'categories/1-A'
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
