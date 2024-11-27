<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Samples\Infrastructure\Orders\Employee;

interface FooInterface
{
    # region syntax_1
    public function hasChanges(): bool;
    # endregion

    # region syntax_2
    public function whatChanged(): array; // array<string, DocumentsChangesArray>
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

class SessionChanges extends TestCase
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            # region changes_1
            $session = $store->openSession();
            try {
                // No changes made yet - 'hasChanges' will be FALSE
                $this->assertFalse($session->advanced()->hasChanges());

                // Store a new entity within the session
                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");

                $session->store($employee, "employees/1-A");

                // 'hasChanges' will now be TRUE
                $this->assertTrue($session->advanced()->hasChanges());

                // 'HasChanges' will reset to FALSE after saving changes
                $session->saveChanges();
                $this->assertFalse($session->advanced()->hasChanges());

            } finally {
                $session->close();
            }
            # endregion

            # region changes_2
            $session = $store->openSession();
            try {
                // Store (add) new entities, they will be tracked by the session
                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");
                $session->store($employee, "employees/1-A");

                $employee = new Employee();
                $employee->setFirstName("Jane");
                $employee->setLastName("Doe");
                $session->store($employee, "employees/2-A");

                // Call 'WhatChanged' to get all changes in the session
                $changes = $session->advanced()->whatChanged();

                $this->assertCount(2, $changes); // 2 entities were added

                // Get the change details for an entity, specify the entity ID
                $changesForEmployee = $changes["employees/1-A"];
                $this->assertCount(1, $changesForEmployee); // a single change for this entity (adding)

                // Get the change type
                $change = $changes[0]->getChange(); // ChangeType::DOCUMENT_ADDED
                $this->assertTrue($change->isDocumentAdded());

                $session->saveChanges();

            } finally {
                $session->close();
            }
            # endregion

            # region changes_3
            $session = $store->openSession();
            try {
                // Load the entities, they will be tracked by the session
                $employee1 = $session->load(Employee::class, "employees/1-A");// 'Joe Doe'
                $employee2 = $session->load(Employee::class, "employees/2-A");// 'Jane Doe'

                // Modify entities
                $employee1->setFirstName("Jim");
                $employee1->setLastName("Brown");
                $employee2->setLastName("Smith");

                // Delete an entity
                $session->delete($employee2);

                // Call 'WhatChanged' to get all changes in the session
                $changes = $session->advanced()->whatChanged();

                // Get the change details for an entity, specify the entity ID
                $changesForEmployee = $changes["employees/1-A"];

                $this->assertEquals("FirstName", $changesForEmployee[0]->getFieldName()); // Field name
                $this->assertEquals("Jim", $changesForEmployee[0]->getFieldNewValue());   // New value
                $this->assertTrue($changesForEmployee[0]->getChange()->isFieldChange());  // Change type

                $this->assertEquals("LastName", $changesForEmployee[1]->getFieldName());  // Field name
                $this->assertEquals("Brown", $changesForEmployee[1]->getFieldNewValue()); // New value
                $this->assertTrue($changesForEmployee[1]->getChange()->isFieldChange());  // Change type

                // Note: for employee2 - even though the LastName was changed to 'Smith',
                // the only reported change is the latest modification, which is the delete action.
                $changesForEmployee = $changes["employees/2-A"];
                $this->assertTrue($changesForEmployee[0]->getChange()->isDocumentDeleted());

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
