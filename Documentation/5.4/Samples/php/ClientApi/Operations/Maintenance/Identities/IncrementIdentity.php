<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Identities;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Identities\NextIdentityForOperation;
use RavenDB\Samples\Infrastructure\Entity\Company;

class IncrementIdentity
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region increment_identity
            // Create a document with an identity ID:
            // ======================================

            $session = $store->openSession();
            try {
                // Pass a collection name that ends with a pipe '|' to create an identity ID
                $company = new Company();
                $company->setName("RavenDB");
                $session->store($company, "companies|");
                $session->saveChanges();
                // => Document "companies/1" will be created
            } finally {
                $session->close();
            }

            // Increment the identity value on the server:
            // ===========================================

            // Define the next identity operation
            // Pass the collection name (can be with or without a pipe)
            $nextIdentityOp = new NextIdentityForOperation("companies|");

            // Execute the operation by passing it to Maintenance.Send
            // The latest value will be incremented to "2"
            // and the next document created with an identity will be assigned "3"
            $incrementedValue = $store->maintenance()->send($nextIdentityOp);

            // Create another document with an identity ID:
            // ============================================

            $session = $store->openSession();
            try {
                $company = new Company();
                $company->setName("RavenDB");
                $session->store($company, "companies|");
                $session->saveChanges();
                // => Document "companies/3" will be created
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region syntax
    NextIdentityForOperation(?string $name);
    # endregion
}
*/
