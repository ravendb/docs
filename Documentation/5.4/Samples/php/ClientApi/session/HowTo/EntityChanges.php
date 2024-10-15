<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use Employee;
use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Session\DocumentsChanges;
use RavenDB\Documents\Session\DocumentsChangesArray;

interface FooInterface
{
    # region syntax_1
    public function hasChanged(?object $entity): bool;
    # endregion
}

interface Foo2Interface
{
    # region syntax_2
    // WhatChangedFor
    public function whatChangedFor(object $entity): DocumentsChangesArray;
    # endregion
}

# region syntax_3
class DocumentsChanges
{
    private mixed $fieldOldValue = null;            // Previous field value
    private mixed $fieldNewValue = null;            // Current field value
    private ChangeType $change;                     // Type of change that occurred
    private ?string $fieldName = null;              // Name of field on which the change occurred
    private ?string $fieldPath = null;              // Path of field on which the change occurred

    public function getFieldFullName(): ?string;    // Path + Name of field on which the change occurred

    // ... getters and setters
}

class ChangeType
{
    public function isDocumentDeleted(): bool;
    public function isDocumentAdded(): bool;
    public function isFieldChanged(): bool;
    public function isNewField(): bool;
    public function isRemovedField(): bool;
    public function isArrayValueChanged(): bool;
    public function isArrayValueAdded(): bool;
    public function isArrayValueRemoved(): bool;

}
# endregion


class EntityChanges extends TestCase
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            # region changes_1
            $session = $store->openSession();
            try {
                // Store a new entity within the session
                // =====================================

                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");
                $session->store($employee, "employees/1-A");

                // '$hasChanged' will be TRUE
                $hasChanged = $session->advanced()->hasChanged($employee);

                // 'HasChanged' will reset to FALSE after saving changes
                $session->saveChanges();
                $hasChanged = $session->advanced()->hasChanged($employee);

                // Load & modify entity within the session
                // =======================================

                $employee = $session->load(Employee::class, "employees/1-A");
                $hasChanged = $session->advanced()->hasChanged($employee); // FALSE

                $employee->setLastName("Brown");
                $hasChanged = $session->advanced()->hasChanged($employee); // TRUE

                $session->saveChanges();
                $hasChanged = $session->advanced()->hasChanged($employee); // FALSE

            } finally {
                $session->close();
            }
            # endregion

            # region changes_2
            $session = $store->openSession();
            try {
                // Store (add) a new entity, it will be tracked by the session
                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");
                $session->store($employee, "employees/1-A");

                // Get the changes for the entity in the session
                // Call 'WhatChangedFor', pass the entity object in the param
                $changesForEmployee = $session->advanced()->whatChangedFor($employee);
                $this->assertCount($changesForEmployee, 1); // a single change for this entity (adding)

                // Get the change type
                $changeType = $changesForEmployee[0]->getChange();
                $this->assertTrue($changeType->isDocumentAdded());

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

            # region changes_3
            $session = $store->openSession();
            try {

                // Load the entity, it will be tracked by the session
                $employee = $session->load(Employee::class, "employees/1-A");

                // Modify the entity
                $employee->setFirstName("Jim");
                $employee->LastName("Brown");

                // Get the changes for the entity in the session
                // Call 'WhatChangedFor', pass the entity object in the param
                $changesForEmployee = $session->advanced()->whatChangedFor($employee);

                $this->assertEquals("FirstName", $changesForEmployee[0]->getFieldName());// Field name
                $this->assertEquals("Jim", $changesForEmployee[0]->getFieldNewValue());  // New value
                $this->assertTrue($changesForEmployee[0]->getChange()->isFieldChanged());         // Change type

                $this->assertEquals("LastName", $changesForEmployee[1]->getFieldName());
                $this->assertEquals("Brown", $changesForEmployee[1]->getFieldNewValue());
                $this->assertTrue($changesForEmployee[1]->getChange()->isFieldChanged());

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
