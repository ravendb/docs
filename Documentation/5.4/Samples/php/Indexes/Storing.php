<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldStorage;
use RavenDB\Documents\Indexes\IndexDefinition;
use RavenDB\Documents\Indexes\IndexFieldOptions;
use RavenDB\Documents\Operations\Indexes\PutIndexesOperation;

# region storing_1
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =  "docs.Employees.Select(employee => new {" .
            "    FirstName = employee.FirstName," .
            "    LastName = employee.LastName" .
            "})";

        $this->store('FirstName', FieldStorage::yes());
        $this->store('LastName', FieldStorage::yes());
    }
}
# endregion

class Storing
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region storing_2
            $indexDefinition = new IndexDefinition();
            $indexDefinition->setName("Employees_ByFirstAndLastName");
            $indexDefinition->setMaps([
                "docs.Employees.Select(employee => new {" .
                "    FirstName = employee.FirstName," .
                "    LastName = employee.LastName" .
                "})"
            ]);

            $fields = [];

            $firstNameOptions = new IndexFieldOptions();
            $firstNameOptions->setStorage(FieldStorage::yes());
            $fields['FirstName'] = $firstNameOptions;

            $lastNameOptions = new IndexFieldOptions();
            $lastNameOptions->setStorage(FieldStorage::yes());
            $fields['LastName'] = $lastNameOptions;

            $indexDefinition->setFields($fields);

            $store->maintenance()->send(new PutIndexesOperation($indexDefinition));
            # endregion
        } finally {
            $store->close();
        }
    }
}
