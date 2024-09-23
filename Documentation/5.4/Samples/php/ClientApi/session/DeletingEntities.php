<?php

namespace RavenDB\Samples\ClientApi\Session;

use RavenDB\Documents\Commands\Batches\DeleteCommandData;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;

interface FooInterface {
    # region deleting_1
    public function delete(?object $entity): void;

    public function delete(?string $id): void;

    public function delete(?string $id, ?string $expectedChangeVector): void;
    # endregion
}

class DeletingEntities
{
    public function testSamples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region deleting_2
                $employee = $session->load(Employee::class, "employees/1");

                $session->delete($employee);
                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region deleting_3
                $session->delete("employees/1");
                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region deleting_4
                $session->delete("employees/1");
                # endregion

                # region deleting_5
                $session->advanced()->defer(new DeleteCommandData("employees/1", null));
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
