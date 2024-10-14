<?php

use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

class BoostSearchResults
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region boost_1
                /** @var array<Employee> $employees */
                $employees = $session
                    // Make a dynamic full-text search Query on 'Employees' collection
                    ->query(Employee::class)
                    // This search predicate will use the default boost value of 1
                    ->search("Notes", "English")
                    // * This search predicate will use a boost value of 10
                    ->search("Notes", "Italian")
                    // Call 'boost()' to set the boost value of the previous 'search()' call
                    ->boost(10)
                    ->toList();

                // * Results will contain all Employee documents that have
                //   EITHER 'English' OR 'Italian' in their 'Notes' field (case-insensitive).
                //
                // * Matching documents that contain 'Italian' will get a HIGHER score
                //   than those that contain 'English'.
                //
                // * Unless configured otherwise, the resulting documents will be ordered by their score.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region boost_3
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    // Make a dynamic full-text search DocumentQuery on 'Employees' collection
                    ->documentQuery(Employee::class)
                    // This search predicate will use the default boost value of 1
                    ->search("Notes", "English")
                    // * This search predicate will use a boost value of 10
                    ->search("Notes", "Italian")
                    // Call 'boost()' to set the boost value of the previous 'search()' call
                    ->boost(10)
                    ->toList();

                // * Results will contain all Employee documents that have
                //   EITHER 'English' OR 'Italian' in their 'Notes' field (case-insensitive).
                //
                // * Matching documents that contain 'Italian' will get a HIGHER score
                //   than those that contain 'English'.
                //
                // * Unless configured otherwise, the resulting documents will be ordered by their score.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region boost_4
                /** @var array<Company> $companies */
                $companies = $session->advanced()
                    // Make a dynamic DocumentQuery on 'Companies' collection
                    ->documentQuery(Company::class)
                    // Define a 'Where' condition
                    ->WhereStartsWith("Name", "O")
                    // Call 'Boost' to set the boost value of the previous 'Where' predicate
                    ->boost(10)
                    // Call 'OrElse' so that OR operator will be used between statements
                    ->orElse()
                    ->whereStartsWith("Name", "P")
                    ->boost(50)
                    ->orElse()
                    ->whereEndsWith("Name", "OP")
                    ->boost(90)
                    ->toList();

                // * Results will contain all Company documents that either
                //   (start-with 'O') OR (start-with 'P') OR (end-with 'OP') in their 'Name' field (case-insensitive).
                //
                // * Matching documents that end-with 'OP' will get the HIGHEST scores.
                //   Matching documents that start-with 'O' will get the LOWEST scores.
                //
                // * Unless configured otherwise, the resulting documents will be ordered by their score.
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
