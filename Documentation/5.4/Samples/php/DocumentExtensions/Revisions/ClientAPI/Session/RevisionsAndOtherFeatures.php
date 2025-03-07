<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Session;

use RavenDB\Constants\DocumentsMetadata;
use RavenDB\Documents\DocumentStore;

class RevisionsAndOtherFeatures
{
    public function extractCountersFromRevisions(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region extract_counters
                // Use GetMetadataFor to get revisions metadata for document 'orders/1-A'
                /** @var array<MetadataAsDictionary> $revisionsMetadata */
                $revisionsMetadata = $session
                ->advanced()->revisions()->getMetadataFor("orders/1-A");

                // Extract the counters data from the metadata
                $orderCountersSnapshots = array_map(
                    fn($x) => $x[DocumentsMetadata::REVISION_COUNTERS],
                    array_filter(
                        $revisionsMetadata,
                        fn($x) => array_key_exists(DocumentsMetadata::REVISION_COUNTERS, $x)
                    )
                );
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
