<?php

namespace RavenDB\Samples\ClientApi\Session;

use PHPUnit\Framework\TestCase;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Company;

class WhatIsSession extends TestCase
{
    public function testSamples()
    {
        // A client-side copy of the document ID.
        $companyId = "companies/1-A";

        $store = DocumentStoreHolder::getStore();

        try {
            # region session_usage_1
            // Obtain a Session from your Document Store
            $session = $store->openSession();
            try {
                // Create a new entity
                $entity = new Company();
                $entity->setName("Company");

                // Store the entity in the Session's internal map
                $session->store($entity);
                // From now on, any changes that will be made to the entity will be tracked by the Session.
                // However, the changes will be persisted to the server only when saveChanges() is called.

                $session->saveChanges();
                // At this point the entity is persisted to the database as a new document.
                // Since no database was specified when opening the Session, the Default Database is used.
            } finally {
                $session->close();
            }
            # endregion

            # region session_usage_2
            // Open a session
            $session = $store->openSession();
            try {
                // Load an existing document to the Session using its ID
                // The loaded entity will be added to the session's internal map
                $entity = $session->load(Company::class, $companyId);

                // Edit the entity, the Session will track this change
                $entity->setName("NewCompanyName");

                $session->saveChanges();
                // At this point, the change made is persisted to the existing document in the database
            } finally {
                $session->close();
            }
            # endregion

            $session = $store->openSession();
            try {
                # region session_usage_3
                // A document is fetched from the server
                $entity1 = $session->load(Company::class, $companyId);

                // Loading the same document will now retrieve its entity from the Session's map
                $entity2 = $session->load(Company::class, $companyId);

                // This command will Not throw an exception
                $this->assertSame($entity1, $entity2);
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
