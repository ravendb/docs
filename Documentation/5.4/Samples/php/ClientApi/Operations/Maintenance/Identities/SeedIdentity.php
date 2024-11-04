<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Identities;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Identities\SeedIdentityForOperation;
use RavenDB\Samples\Infrastructure\Entity\Company;

class SeedIdentity
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region seed_identity_1
            // Seed a higher identity value on the server:
            // ===========================================

            // Define the seed identity operation. Pass:
            //   * The collection name (can be with or without a pipe)
            //   * The new value to set
            $seedIdentityOp = new SeedIdentityForOperation("companies|", 23);

            // Execute the operation by passing it to Maintenance.Send
            // The latest value on the server will be incremented to "23"
            // and the next document created with an identity will be assigned "24"
            $seededValue = $store->maintenance()->send($seedIdentityOp);

            // Create a document with an identity ID:
            // ======================================

            $session = $store->openSession();
            try {
                $company = new Company();
                $company->setName("RavenDB");
                $session->store($company, "companies|");
                $session->saveChanges();
                // => Document "companies/24" will be created
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region seed_identity_2
            // Force a smaller identity value on the server:
            // =============================================

            // Define the seed identity operation. Pass:
            //   * The collection name (can be with or without a pipe)
            //   * The new value to set
            //   * Set 'forceUpdate' to true
            $seedIdentityOp = new SeedIdentityForOperation("companies|", 5, forceUpdate: true);

            // Execute the operation by passing it to Maintenance.Send
            // The latest value on the server will be decremented to "5"
            // and the next document created with an identity will be assigned "6"
            $seededValue = $store->maintenance()->send($seedIdentityOp);

            // Create a document with an identity ID:
            // ======================================

            $session = $store->openSession();
            try {
                $company = new Company();
                $company->setName("RavenDB");
                $session->store($company, "companies|");
                $session->saveChanges();
                // => Document "companies/6" will be created
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
    SeedIdentityForOperation(string $name, int $value, bool $forceUpdate = false)
    # endregion
}
*/
