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

            # region atomic_guards_enabled
            // Open a cluster-wide session:
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

            // Open two concurrent cluster-wide sessions:

            $sessionOptions1 = new SessionOptions();
            $sessionOptions1->setTransactionMode(TransactionMode::clusterWide());

            $session1 = $store->openSession($sessionOptions1);
            try {
                $sessionOptions2 = new SessionOptions();
                $sessionOptions2->setTransactionMode(TransactionMode::clusterWide());
                $session2 = $store->openSession($sessionOptions2);

                try {
                // Both sessions load the same document:
                var $loadedUser1 = $session1->load(User::class, "users/johndoe");
                $loadedUser1->setName("jindoe");

                $loadedUser2 = $session2->load(User::class, "users/johndoe");
                $loadedUser2->setName("jandoe");

                // session1 saves its changes first —
                // this increments the Raft index of the associated atomic guard.
                $session1->saveChanges();

                // session2 tries to save using an outdated atomic guard version
                // and fails with a ConcurrencyException.
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

        # region atomic_guards_disabled
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

        $result = $store->operations()->sendAsync(new GetCompareExchangeValuesOperation(User::class, ""));
    }

    public function loadBeforeStoring(): void
    {
        $store = new DocumentStore(
            [ "http://127.0.0.1:8080" ],
            "test"
        );
        $store->initialize();

        # region load_before_storing
        // Open a cluster-wide session
        $sessionOptions = new SessionOptions();
        $sessionOptions->setTransactionMode(TransactionMode::clusterWide());

        $session = $store->openSession($sessionOptions);
        try {
            // Load the user document BEFORE creating or updating
            $user = $session->load(User::class, "users/johndoe");

            if ($user === null) {
                // Document doesn't exist => create a new document:
                $newUser = new User();
                $newUser->setName("John Doe");
                // ... initialize other properties

                // Store the new user document in the session
                $session->store($newUser, "users/johndoe");
            } else {
                // Document exists => apply your modifications:
                $user->setName("New name");
                // ... make any other updates
                
                // No need to call Store() again
                // RavenDB tracks changes on loaded entities
            }

            // Commit your changes
            $session->saveChanges();
        } finally {
            $session->close();
        }
        # endregion
    }
}
