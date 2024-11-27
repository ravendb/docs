<?php

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\CompareExchange\GetCompareExchangeValuesOperation;
use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Documents\Session\TransactionMode;
use RavenDB\Samples\Infrastructure\Entity\User;

class AtomicGuards
{
    public function testCreateAtomicGuard(): void
    {
        //var (nodes, leader) = await CreateRaftCluster(3);
            $store = new DocumentStore(
                [ "http://127.0.0.1:8080" ],
                "test"
            );
            $store->initialize();

            # region atomic-guards-enabled

            $sessionOptions = new SessionOptions();
            $sessionOptions->setTransactionMode(TransactionMode::clusterWide());

            $session = $store->openSession($sessionOptions);

            try {
                $session->store(new User(), "users/johndoe");
                // An atomic-guard is now automatically created for the new document "users/johndoe".
                $session->saveChanges();
            } finally {
                $session->close();
            }

            // Open two more cluster-wide sessions

            $sessionOptions1 = new SessionOptions();
            $sessionOptions1->setTransactionMode(TransactionMode::clusterWide());

            $session1 = $store->openSession($sessionOptions1);
            try {
                $sessionOptions2 = new SessionOptions();
                $sessionOptions2->setTransactionMode(TransactionMode::clusterWide());
                $session2 = $store->openSession($sessionOptions2);

                try {
                // The two sessions will load the same document at the same time
                var $loadedUser1 = $session1->load(User::class, "users/johndoe");
                $loadedUser1->setName("jindoe");

                $loadedUser2 = $session2->load(User::class, "users/johndoe");
                $loadedUser2->setName("jandoe");

                // Session1 will save changes first, which triggers a change in the
                // version number of the associated atomic-guard.
                $session1->saveChanges();

                // session2.saveChanges() will be rejected with ConcurrencyException
                // since session1 already changed the atomic-guard version,
                // and session2 saveChanges uses the document version that it had when it loaded the document.
                $session2->saveChanges();
                } finally {
                    $session2->close();
                }

            } finally {
                $session1->close();
            }

            # endregion

            $result = $store->operations()->sendAsync(new GetCompareExchangeValuesOperation(User::class, ""));
    }

    public function testDoNotCreateAtomicGuard(): void
    {
        //var (nodes, leader) = await CreateRaftCluster(3);
        $store = new DocumentStore(
            [ "http://127.0.0.1:8080" ],
            "test"
        );
        $store->initialize();

        # region atomic-guards-disabled

        $sessionOptions = new SessionOptions();
        $sessionOptions->setTransactionMode(TransactionMode::clusterWide());
        $sessionOptions->setDisableAtomicDocumentWritesInClusterWideTransaction(true);

        $session = $store->openSession($sessionOptions);

        try {
            $session->store(new User(), "users/johndoe");
            // No atomic-guard will be created upon saveChanges
            $session->saveChanges();
        } finally {
            $session->close();
        }
        # endregion

        // WaitForUserToContinueTheTest($store);

        $result = $store->operations()->sendAsync(new GetCompareExchangeValuesOperation(User::class, ""));

    }
}
