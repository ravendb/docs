4<?php

namespace RavenDB\Samples\ClientApi\Session;

use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Address;
use RavenDB\Samples\Infrastructure\Orders\Company;

class UpdateDocuments
{
    public function updateDocuments(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            # region load-company-and-update
            $session = $store->openSession();
            try {
                // Load a company document
                // The entity loaded from the document will be added to the Session's entities map
                /** @var Company $company */
                $company = $session->load(Company::class, "companies/1-A");

                // Update the company's PostalCode
                $address = $company->getAddress();
                $address->setPostalCode("TheNewPostalCode");
                $company->setAddress($address);

                // Apply changes
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            # region query-companies-and-update
            $session = $store->openSession();
            try {
                // Query: find companies with the specified PostalCode
                // The entities loaded from the matching documents will be added to the Session's entities map
                $query = $session->query(Company::class)
                    ->whereEquals("address.postal_code", "12345");

                $matchingCompanies = $query->toList();

                // Update the PostalCode for the resulting company documents
                for ($i = 0; $i < count($matchingCompanies); $i++) {
                    $address = $matchingCompanies[$i]->getAddress();
                    $address->setPostalCode("TheNewPostalCode");
                    $matchingCompanies[$i]->setAddress($address);
                }

                // Apply changes
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
