<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Session;

use DateInterval;
use DateTime;
use RavenDB\Constants\DocumentsMetadata;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\Loaders\IncludeBuilderInterface;
use RavenDB\Json\MetadataAsDictionary;
use RavenDB\Samples\Infrastructure\Orders\Order;

interface IFoo
{
    # region syntax
    // Include a single revision by Time
    public function includeRevisionsBefore(DateTime $before): IncludeBuilderInterface;

    // Include a single revision by Change Vector path(s)
    public function includeRevisions(string $changeVectorPaths): IncludeBuilderInterface;
    # endregion
}

class Including
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region include_1
                // The revision creation time
                // For example - looking for a revision from last month
                $creationTime = (new DateTime())->sub(new DateInterval("P1M"));

                // Load a document:
                $order = $session->load(Order::class, "orders/1-A", function($builder) use ($creationTime) {
                        return $builder
                            // Pass the revision creation time to 'IncludeRevisionsBefore'
                            // The revision will be 'loaded' to the session along with the document
                            ->includeRevisionsBefore($creationTime);
                    });

                // Get the revision by creation time - it will be retrieved from the SESSION
                // No additional trip to the server is made
                $revision = $session
                    ->advanced()->revisions()->get(Order::class, "orders/1-A", $creationTime);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region include_2
                // Load a document:
                $contract = $session->load(Contract::class, "contracts/1-A", function($builder) {
                    return $builder

                        // Pass the path to the document property that contains the revision change vector(s)
                        // The revision(s) will be 'loaded' to the session along with the document
                        ->includeRevisions("RevisionChangeVector")
                        ->includeRevisions("RevisionChangeVectors");
                });

                // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                // No additional trip to the server is made
                $revision = $session->advanced()->revisions()->get(Contract::class, "RevisionChangeVector");
                $revisions = $session->advanced()->revisions()->get(Contract::class, "RevisionChangeVectors");
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region include_3
                // The revision creation time
                // For example - looking for revisions from last month
                $creationTime = (new DateTime())->sub(new DateInterval("P1M");

                // Query for documents:
                $orderDocuments = $session->query(Order::class)
                    ->whereEquals("ShipTo.Country",  "Canada")
                     // Pass the revision creation time to 'IncludeRevisionsBefore'
                    ->include(function($builder) use ($creationTime) { return $builder->includeRevisionsBefore($creationTime); })
                     // For each document in the query results,
                     // the matching revision will be 'loaded' to the session along with the document
                    ->toList();

                // Get a revision by its creation time for a document from the query results
                // It will be retrieved from the SESSION - no additional trip to the server is made
                $revision = $session
                    ->advanced()->revisions()->getBeforeDate(Order::class, $orderDocuments[0]->getId(), $creationTime);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region include_4
                // Query for documents:
                $orderDocuments = $session->query(Contract::class)
                     // Pass the path to the document property that contains the revision change vector(s)
                    ->include(function($builder) {
                         return $builder
                             ->includeRevisions("getRevisionChangeVector")   // Include a single revision
                             ->includeRevisions("getRevisionChangeVectors"); // Include multiple revisions
                     })
                     // For each document in the query results,
                     // the matching revisions will be 'loaded' to the session along with the document
                    ->toList();

                // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                // No additional trip to the server is made
                $revision = $session
                    ->advanced()->revisions()->get(Contract::class, $orderDocuments[0]->getRevisionChangeVector());
                $revisions = $session
                    ->advanced()->revisions()->get(Contract::class, $orderDocuments[0]->getRevisionChangeVectors());
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region include_5
                // The revision creation time
                // For example - looking for revisions from last month
                $creationTime = (new DateTime())->sub(new DateInterval("P1M"));

                // Query for documents with Raw Query:
                $orderDocuments = $session->advanced()
                     // Use 'include revisions' in the RQL
                    ->rawQuery(Order::class, "from Orders include revisions(\$p0)")
                     // Pass the revision creation time
                    ->addParameter("p0", $creationTime)
                     // For each document in the query results,
                     // the matching revision will be 'loaded' to the session along with the document
                    ->toList();

                // Get a revision by its creation time for a document from the query results
                // It will be retrieved from the SESSION - no additional trip to the server is made
                $revision = $session
                    ->advanced()->revisions()->getBeforeDate(Order::class, $orderDocuments[0]->getId(), $creationTime);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region include_6
                // Query for documents with Raw Query:
                $orderDocuments = $session->advanced()
                     // Use 'include revisions' in the RQL
                    ->rawQuery(Contract::class, "from Contracts include revisions(\$p0, \$p1)")
                     // Pass the path to the document properties containing the change vectors
                    ->addParameter("p0", "RevisionChangeVector")
                    ->addParameter("p1", "RevisionChangeVectors")
                     // For each document in the query results,
                     // the matching revisions will be 'loaded' to the session along with the document
                    ->toList();

                // Get the revision(s) by change vectors - it will be retrieved from the SESSION
                // No additional trip to the server is made
                $revision = $session
                    ->advanced()->revisions()->get(Contract::class, $orderDocuments[0]->getRevisionChangeVector());
                $revisions = $session
                    ->advanced()->revisions()->get(Contract::class, $orderDocuments[0]->getRevisionChangeVectors());
                # endregion
            } finally {
                $session->close();
            }

            # region include_7
            $session = $store->openSession();
            try {
                // Get the revisions' metadata for document 'contracts/1-A'
                /** @var array<MetadataAsDictionary> $contractRevisionsMetadata */
                $contractRevisionsMetadata =
                    $session->advanced()->revisions()->getMetadataFor("contracts/1-A");

                // Get a change vector from the metadata
                $changeVector = $contractRevisionsMetadata[array_key_first($contractRevisionsMetadata)]->getString(DocumentsMetadata::CHANGE_VECTOR);

                // Patch the document - add the revision change vector to a specific document property
                $session->advanced()
                    ->patch( "contracts/1-A", "RevisionChangeVector", $changeVector);

                // Save your changes
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }
}

# region sample_document
// Sample Contract document
class Contract
{
    private ?string $id = null;
    private ?string $name = null;
    private ?string $revisionChangeVector = null; // A single change vector
    private ?array $revisionChangeVectors = null; // A list of change vectors

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getRevisionChangeVector(): ?string
    {
        return $this->revisionChangeVector;
    }

    public function setRevisionChangeVector(?string $revisionChangeVector): void
    {
        $this->revisionChangeVector = $revisionChangeVector;
    }

    public function getRevisionChangeVectors(): ?array
    {
        return $this->revisionChangeVectors;
    }

    public function setRevisionChangeVectors(?array $revisionChangeVectors): void
    {
        $this->revisionChangeVectors = $revisionChangeVectors;
    }
}
# endregion
