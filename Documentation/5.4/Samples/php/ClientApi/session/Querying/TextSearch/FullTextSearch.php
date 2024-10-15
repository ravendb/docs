<?php

use RavenDB\Documents\Queries\SearchOperator;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Company;

interface Foo
{
    # region syntax
    public function search(string $fieldName, string $searchTerms, ?SearchOperator $operator = null): DocumentQueryInterface;
    # endregion
}

class FullTextSearch
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            // Search for single term
            // ======================
            $session = $store->openSession();
            try {
                # region fts_1
                /** @var array<Employee> $employees */
                $employees = $session
                    // Make a dynamic query on Employees collection
                    ->query(Employee::class)
                    // * Call 'Search' to make a Full-Text search
                    // * Search is case-insensitive
                    // * Look for documents containing the term 'University' within their 'Notes' field
                    ->search("Notes", "University")
                    ->toList();

                // Results will contain Employee documents that have
                // any case variation of the term 'university' in their 'Notes' field.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_3
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    // Make a dynamic DocumentQuery on Employees collection
                    ->documentQuery(Employee::class)
                    // * Call 'Search' to make a Full-Text search
                    // * Search is case-insensitive
                    // * Look for documents containing the term 'University' within their 'Notes' field
                    ->search("Notes", "University")
                    ->toList();

                // Results will contain Employee documents that have
                // any case variation of the term 'university' in their 'Notes' field.
                # endregion
            } finally {
                $session->close();
            }

            // Search for multiple terms - string
            // ==================================
            $session = $store->openSession();
            try {
                # region fts_4
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    // * Pass multiple terms in a single string, separated by spaces.
                    // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                    //   within their 'Notes' field
                    ->search("Notes", "University Sales Japanese")
                    ->toList();

                // * Results will contain Employee documents that have at least one of the specified terms.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_6
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // * Pass multiple terms in a single string, separated by spaces.
                    // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                    //   within their 'Notes' field
                    ->search("Notes", "University Sales Japanese")
                    ->toList();

                // * Results will contain Employee documents that have at least one of the specified terms.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            // Search for multiple terms - list
            // ==================================
            $session = $store->openSession();
            try {
                # region fts_7
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    // * Pass terms in array<string>.
                    // * Look for documents containing either 'University' OR 'Sales' OR 'Japanese'
                    //   within their 'Notes' field
                    ->search("Notes", ["University", "Sales", "Japanese"])
                    ->toList();

                // * Results will contain Employee documents that have at least one of the specified terms.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            // Search in multiple fields
            // =========================
            $session = $store->openSession();
            try {
                # region fts_9
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    // * Look for documents containing:
                    //   'French' in their 'Notes' field OR 'President' in their 'Title' field
                    ->search("Notes", "French")
                    ->search("Title", "President")
                    ->toList();

                // * Results will contain Employee documents that have
                //   at least one of the specified fields with the specified terms.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_11
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // * Look for documents containing:
                    //   'French' in their 'Notes' field OR 'President' in their 'Title' field
                    ->search("Notes", "French")
                    ->search("Title", "President")
                    ->toList();

                // * Results will contain Employee documents that have
                //   at least one of the specified fields with the specified terms.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            // Search in complex object
            // ========================
            $session = $store->openSession();
            try {
                # region fts_12
                /** @var array<Company> $companies */
                $companies = $session
                    ->query(Company::class)
                    // * Look for documents that contain:
                    //   the term 'USA' OR 'London' in any field within the complex 'Address' object
                    ->search("Address", "USA London")
                    ->toList();

                // * Results will contain Company documents that are located either in 'USA' OR in 'London'.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_14
                /** @var array<Company> $companies */
                $companies = $session->advanced()
                    ->documentQuery(Company::class)
                    // * Look for documents that contain:
                    //   the term 'USA' OR 'London' in any field within the complex 'Address' object
                    ->search("Address", "USA London")
                    ->toList();

                // * Results will contain Company documents that are located either in 'USA' OR in 'London'.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            // Search operators - AND
            // ======================
            $session = $store->openSession();
            try {
                # region fts_15
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    // * Pass operator with SearchOperator::and()
                    ->search("Notes", "College German", SearchOperator::and())
                    ->toList();

                // * Results will contain Employee documents that have BOTH 'College' AND 'German'
                //   in their 'Notes' field.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_17
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // * Pass operator with SearchOperator::and()
                    ->search("Notes", "College German", SearchOperator::and())
                    ->toList();

                // * Results will contain Employee documents that have BOTH 'College' AND 'German'
                //   in their 'Notes' field.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            // Search operators - OR
            // =====================
            $session = $store->openSession();
            try {
                # region fts_18
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    // * Pass operator with SearchOperator::or()
                    ->search("Notes", "College German", SearchOperator::or())
                    ->toList();

                // * Results will contain Employee documents that have EITHER 'College' OR 'German'
                //   in their 'Notes' field.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_20
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // * Pass operator with SearchOperator::or()
                    ->search("Notes", "College German", SearchOperator::or())
                    ->toList();

                // * Results will contain Employee documents that have EITHER 'College' OR 'German'
                //   in their 'Notes' field.
                // * Search is case-insensitive.
                # endregion
            } finally {
                $session->close();
            }

            // Search options - Not
            // ====================
            $session = $store->openSession();
            try {
                # region fts_21
                /** @var array<Company> $companies */
                $companies = $session
                    ->query(Company::class)
                    # Call 'not()' to negate the next search call
                    ->not()
                    ->search("Address", "USA")
                    ->toList();

                // * Results will contain Company documents are NOT located in 'USA'
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_23
                /** @var array<Company> $companies */
                $companies = $session->advanced()
                    ->documentQuery(Company::class)
                    ->openSubclause()
                    // Call 'not()' to negate the next search call
                    ->not()
                    ->search("Address", "USA")
                    ->closeSubclause()
                    ->toList();

                // * Results will contain Company documents are NOT located in 'USA'
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            // Search options - Default
            // ========================
            $session = $store->openSession();
            try {
                # region fts_24
                /** @var array<Company> $companies */
                $companies = $session
                    ->query(Company::class)
                    ->whereEquals("Contact.Title", "Owner")
                    // Operator AND will be used with previous 'Where' predicate
                    ->search("Address.Country", "France")
                    // Operator OR will be used between the two 'Search' calls by default
                    ->search("Name", "Markets")
                    ->toList();

                // * Results will contain Company documents that have:
                //   ('Owner' as the 'Contact.Title')
                //   AND
                //   (are located in 'France' OR have 'Markets' in their 'Name' field)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_26
                /** @var array<Company> $companies */
                $companies = $session->advanced()
                    ->documentQuery(Company::class)
                    ->whereEquals("Contact.Title", "Owner")
                    // Operator AND will be used with previous 'Where' predicate
                    // Call 'openSubclause()' to open predicate block
                    ->openSubclause()
                    ->search("Address.Country", "France")
                    // Operator OR will be used between the two 'Search' calls by default
                    ->search("Name", "Markets")
                    // Call 'closeSubclause()' to close predicate block
                    ->closeSubclause()
                    ->toList();

                // * Results will contain Company documents that have:
                //   ('Owner' as the 'Contact.Title')
                //   AND
                //   (are located in 'France' OR have 'Markets' in their 'Name' field)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            // Search options - AND
            // ====================
            $session = $store->openSession();
            try {
                # region fts_27
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    ->search("Notes", "French")
                    // Call 'AndAlso' so that operator AND will be used with previous 'search' call
                    ->andAlso()
                    ->search("Title", "Manager")
                    ->toList();

                // * Results will contain Employee documents that have:
                //   ('French' in their 'Notes' field)
                //   AND
                //   ('Manager' in their 'Title' field)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_29
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    ->search("Notes", "French")
                    // Call 'andAlso()' so that operator AND will be used with previous 'search' call
                    ->andAlso()
                    ->search("Title", "Manger")
                    ->toList();

                // * Results will contain Employee documents that have:
                //   ('French' in their 'Notes' field)
                //   AND
                //   ('Manager' in their 'Title' field)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            // Search options - Flags
            // ======================
            $session = $store->openSession();
            try {
                # region fts_30
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    ->search("Notes", "French")
                    // Call 'andAlso()' so that operator AND will be used with previous 'search' call
                    ->andAlso()
                    ->openSubclause()
                    // Call 'not()' to negate the next search call
                    ->not()
                    ->search("Title", "Manager")
                    ->closeSubclause()
                    ->toList();

                // * Results will contain Employee documents that have:
                //   ('French' in their 'Notes' field)
                //   AND
                //   (do NOT have 'Manager' in their 'Title' field)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_32
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    ->search("Notes", "French")
                    // Call 'andAlso()' so that operator AND will be used with previous 'search' call
                    ->andAlso()
                    ->openSubclause()
                    // Call 'not()' to negate the next search call
                    ->not()
                    ->search("Title", "Manager")
                    ->closeSubclause()
                    ->toList();

                // * Results will contain Employee documents that have:
                //   ('French' in their 'Notes' field)
                //   AND
                //   (do NOT have 'Manager' in their 'Title' field)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            // Using wildcards
            // ===============
            $session = $store->openSession();
            try {
                # region fts_33
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employee::class)
                    // Use '*' to replace one or more characters
                    ->search("Notes", "art*")
                    ->search("Notes", "*logy")
                    ->search("Notes", "*mark*")
                    ->ToList();

                // Results will contain Employee documents that have in their 'Notes' field:
                // (terms that start with 'art')  OR
                // (terms that end with 'logy') OR
                // (terms that have the text 'mark' in the middle)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fts_35
                /** @var array<Employee> $employees */
                $employees = $session->advanced()
                    ->documentQuery(Employee::class)
                    // Use '*' to replace one ore more characters
                    ->search("Notes", "art*")
                    ->search("Notes", "*logy")
                    ->search("Notes", "*mark*")
                    ->toList();

                // Results will contain Employee documents that have in their 'Notes' field:
                // (terms that start with 'art')  OR
                // (terms that end with 'logy') OR
                // (terms that have the text 'mark' in the middle)
                //
                // * Search is case-insensitive
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
