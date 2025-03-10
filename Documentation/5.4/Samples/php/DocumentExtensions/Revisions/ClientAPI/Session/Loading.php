<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Session;

use DateInterval;
use DateTime;
use PHPUnit\Framework\TestCase;
use RavenDB\Constants\DocumentsMetadata;
use RavenDB\Documents\DocumentStore;
use RavenDB\Json\MetadataAsDictionary;
use RavenDB\Samples\Infrastructure\Orders\Order;
use RavenDB\Type\StringArray;

interface IFoo
{
    # region syntax_1
    public function getFor(?string $className, ?string $id, int $start = 0, int $pageSize = 25): array;
    # endregion

    # region syntax_2
    public function getMetadataFor(?string $id, int $start = 0, int $pageSize = 25): array;
    # endregion

    # region syntax_3
    function getBeforeDate(?string $className, ?string $id, DateTime $date): ?object;
    # endregion

    # region syntax_4
    // Get a revision(s) by its change vector(s)
    public function get(?string $className, null|string|array|StringArray $changeVectors): mixed;
    # endregion
}

class Loading extends TestCase
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region example_1_sync
                // Get revisions for document 'orders/1-A'
                // Revisions will be ordered by most recent revision first
                /** @var array<Order>  $orderRevisions */
                $orderRevisions = $session
                    ->advanced()
                    ->revisions()
                    ->getFor(Order::class, id: "orders/1-A", start: 0, pageSize: 10);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region example_2_sync
                // Get revisions' metadata for document 'orders/1-A'
                /** @var array<MetadataAsDictionary> $orderRevisionsMetadata */
                $orderRevisionsMetadata = $session
                    ->advanced()
                    ->revisions()
                    ->getMetadataFor(id: "orders/1-A", start: 0, pageSize: 10);

                // Each item returned is a revision's metadata, as can be verified in the @flags key
                $metadata = $orderRevisionsMetadata[0];
                $flagsValue = $metadata[DocumentsMetadata::FLAGS];

                $this->assertContains("Revision", $flagsValue);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region example_3_sync
                // Get a revision by its creation time
                $revisionFromLastYear = $session
                    ->advanced()
                    ->revisions()
                     // If no revision was created at the specified time,
                     // then the first revision that precedes it will be returned
                    ->getBeforeDate(Order::class, "orders/1-A", (new DateTime())->sub(new DateInterval("P1Y")));
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region example_4_sync
                // Get revisions metadata
                /** @var array<MetadataAsDictionary> $revisionsMetadata */
                $revisionsMetadata = $session
                    ->advanced()
                    ->revisions()
                    ->getMetadataFor("orders/1-A", start: 0, pageSize: 25);

                // Get the change-vector from the metadata
                $changeVector = $revisionsMetadata[0][DocumentsMetadata::CHANGE_VECTOR];

                // Get the revision by its change-vector
                $revision = $session
                    ->advanced()
                    ->revisions()
                    ->get(Order::class, $changeVector);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
