<?php

namespace RavenDB\Samples\ClientApi\Session;

use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Documents\Session\TransactionMode;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Type\Duration;

interface FooInterface
{
    # region saving_changes_1
    public function saveChanges(): void;
    # endregion
}

class SavingChanges
{
    public function savingChanges(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region saving_changes_2
                $employee = new Employee();
                $employee->setFirstName("John");
                $employee->setLastName("Doe");

                $session->store($employee);
                $session->saveChanges();;
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region saving_changes_3
                $session->advanced()->waitForIndexesAfterSaveChanges(function ($builder) use ($session) {
                    $builder->withTimeout(Duration::ofSeconds(30))
                        ->throwOnTimeout(true)
                        ->waitForIndexes("index/1", "index/2");

                    $employee = new Employee();
                    $employee->setFirstName("John");
                    $employee->setLastName("Doe");

                    $session->store($employee);

                    $session->saveChanges();
                });
                # endregion
            } finally {
                $session->close();
            }

        } finally {
            $store->close();
        }

        # region cluster_store_with_compare_exchange
        $session = $store->openSession();
        try {
            $sessionOptions = new SessionOptions();
            // default is: TransactionMode::singleNode();
            $sessionOptions->setTransactionMode(TransactionMode::clusterWide());
            $session = $store->openSession($sessionOptions);
            try {
                $user = new Employee();
                $user->setFirstName("John");
                $user->setLastName("Doe");

                $session->store($user);

                // this transaction is now conditional on this being
                // successfully created (so, no other users with this name)
                // it also creates an association to the new user's id
                $session->advanced()->clusterTransaction()
                    ->createCompareExchangeValue("usernames/John", $user->getId());

                $session->saveChanges();
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
        # endregion
    }
}
