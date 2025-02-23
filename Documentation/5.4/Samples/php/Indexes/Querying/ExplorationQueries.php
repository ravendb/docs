<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Samples\Infrastructure\Orders\Employee;

class ExplorationQueries
{
    public function explorationQueries(): void
    {
        $store = new DocumentStore();
        try {
            // filter in a collection query
            $session = $store->openSession();
            try {

                # region exploration-queries_1.1
                $result = $session->query(Employee::class)
                    ->filter(function($f) { return $f->equals("Address.Country", "USA"); }, 500)
                    ->singleOrDefault();
                # endregion

                # region exploration-queries_1.2
                $result = $session->advanced()->documentQuery(Employee::class)
                    ->filter(function($p) { return $p->equals("Address.Country", "USA"); }, limit: 500)
                    ->singleOrDefault();
                # endregion

                # region exploration-queries_1.3
                $result = $session->advanced()
                        ->rawQuery(
                            Employee::class,
                            "from Employees as e " .
                               "filter e.Address.Country = 'USA' " .
                               "filter_limit 500")
                ->singleOrDefault();
                # endregion
            } finally {
                $session->close();
            }

            // filter in an index query
            $session = $store->openSession();
            try {
            # region exploration-queries_2.1
            $emp = $session->query(Employee::class)
                ->whereEquals("Title", "Sales Representative")
                ->filter(function($f) { return $f->equals("Address.Country", "USA"); }, 500)
                ->singleOrDefault();
            # endregion

            # region exploration-queries_2.2
            $emp = $session->advanced()->documentQuery(Employee::class)
                  ->whereEquals("Title", "Sales Representative")
                  ->filter(function($p) { return $p->equals("Address.Country", "USA"); }, limit: 500)
                  ->singleOrDefault();
            # endregion

            # region exploration-queries_2.3
            $emp = $session->advanced()->rawQuery(Employee::class,
                "from Employees as e" .
                 "where e.Title = \$title" .
                 "filter e.Address.Country = \$country" .
                 "filter_limit \$limit")
                ->addParameter("title", "Sales Representative")
                ->addParameter("country", "USA")
                ->addParameter("limit", 500)
                ->singleOrDefault();
            # endregion
            } finally {
                $session->close();
            }

            // filter and projection
            $session = $store->openSession();
            try {
            # region exploration-queries_3.1
            $emp1 = $session
                ->query(Employee::class)
                ->filter(function($f) { return $f->equals("Address.Country", "USA"); }, 500)
                ->selectFields(null,  "Name", "Address.City", "Address.Country")
                ->toList();

                // Results:
                // ========

                // * Results include all companies with country = 'USA'
                //   Each resulting object contains only the selected fields.
                //
                // * No auto-index is created.
            # endregion

            # region exploration-queries_3.3
            $emp3 = $session->advanced()->rawQuery(
                Employee::class,
                "from Companies " .
                    "filter Address.Country == 'USA'" .
                    "select Name, Address.City, Address.Count`"
            );
            # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
