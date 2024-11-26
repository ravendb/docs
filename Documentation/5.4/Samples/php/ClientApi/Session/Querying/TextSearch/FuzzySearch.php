<?php

use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Company;

interface FooInterface
{
    # region syntax
    public function fuzzy(float $fuzzy): DocumentQueryInterface;
    # endregion
}

class FuzzySearch
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region fuzzy_1
                /** @var array<Company> $companies */
                $companies = $session->advanced()
                    ->documentQuery(Company::class)
                    // Query with a term that is misspelled
                    ->whereEquals("Name", "Ernts Hnadel")
                    // Call 'Fuzzy'
                    // Pass the required similarity, a decimal param between 0.0 and 1.0
                    ->fuzzy(0.5)
                    ->toList();

                // Running the above query on the Northwind sample data returns document: companies/20-A
                // which contains "Ernst Handel" in its Name field.
                # endregion
            } finally {
                $session->close();
            }

        } finally {
            $store->close();
        }
    }
}
