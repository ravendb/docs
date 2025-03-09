<?php

namespace RavenDB\Samples\DocumentExtensions\Attachments;

use RavenDB\Documents\DocumentStore;
use RavenDB\Samples\Infrastructure\Orders\Employee;

interface IFoo
{
    # region copy_0
    function copy(object|string $sourceIdOrEntity, ?string $sourceName, object|string $destinationIdOrEntity, ?string $destinationName): void;
    # endregion

    # region rename_0
    function rename(string|object $idOrEntity, ?string $name, ?string $newName): void;
    # endregion

    # region move_0
    public function move(object|string $sourceIdOrEntity, ?string $sourceName, object|string $destinationIdOrEntity, ?string $destinationName): void;
    # endregion
}


class CopyMoveRename
{
    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region copy_1
                $employee1 = $session->load(Employee::class, "employees/1-A");
                $employee2 = $session->load(Employee::class, "employees/2-A");

                $session->advanced()->attachments()->copy($employee1, "photo.jpg", $employee2, "photo-copy.jpg");

                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region rename_1
                $employee = $session->load(Employee::class, "employees/1-A");

                $session->advanced()->attachments()->rename($employee, "photo.jpg", "photo-new.jpg");

                $session->saveChanges();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region move_1
                $employee1 = $session->load(Employee::class, "employees/1-A");
                $employee2 = $session->load(Employee::class, "employees/2-A");

                $session->advanced()->attachments()->move($employee1, "photo.jpg", $employee2, "photo.jpg");

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
