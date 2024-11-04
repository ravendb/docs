<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Identities;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Identities\GetIdentitiesOperation;
use RavenDB\Samples\Infrastructure\Entity\Company;

class GetIdentities
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region get_identities
            // Create a document with an identity ID:
            // ======================================

            $session = $store->openSession();
            try {
                // Request the server to generate an identity ID for the new document. Pass:
                //   * The entity to store
                //   * The collection name with a pipe (|) postfix
                $company = new Company();
                $company->setName("RavenDB");
                $session->store($company, "companies|");

                // If this is the first identity created for this collection,
                // and if the identity value was not customized
                // then a document with an identity ID "companies/1" will be created
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Get identities information:
            // ===========================

            // Define the get identities operation
            $getIdentitiesOp = new GetIdentitiesOperation();

            // Execute the operation by passing it to Maintenance.Send
            /** @var array $identities */
            $identities = $store->maintenance()->send($getIdentitiesOp);

            // Results
            $latestIdentityValue = $identities["companies|"]; // => value will be 1
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
    GetIdentitiesOperation();
    # endregion
}
*/
