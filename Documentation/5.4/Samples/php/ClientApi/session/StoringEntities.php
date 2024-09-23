<?php

namespace RavenDB\Samples\ClientApi\Session;

use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;

interface FooInterface {
    # region store_entities_1
    public function store(?object $entity): void;
    # endregion

    # region store_entities_2
    public function store(?object $entity, ?string $id): void;
    # endregion

    # region store_entities_3
    public function store(?object $entity, ?string $id, ?string $changeVector): void;
    # endregion

}


class StoringEntities
{

    public function storeEntities(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region store_entities_5
                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");

                // generate Id automatically
                $session->store($employee);

                // send all pending operations to server, in this case only `Put` operation
                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
