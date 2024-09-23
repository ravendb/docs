<?php

namespace RavenDB\Samples\ClientApi\Session;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\Session\DocumentSessionInterface;
use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Documents\Session\TransactionMode;
use RavenDB\Http\RequestExecutor;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;

class FooInterface {
    # region open_session_1
    // Open session for a 'default' database configured in 'DocumentStore'
    public function openSession(): DocumentSessionInterface;

    // Open session for a specified database
    public function openSession(string $database): DocumentSessionInterface;

    public function openSession(SessionOptions $sessionOptions): DocumentSessionInterface;
    # endregion
}

class FooInterface2 {
    # region session_options
    private ?string $database = null;
    private bool $noTracking = false;
    private bool $noCaching = false;
    private ?RequestExecutor $requestExecutor = null;

    // Initialized to TransactionMode::singleNode() in constructor
    private TransactionMode $transactionMode;

    // getters and setters
    # endregion
}

class OpeningSession extends TestCase
{
    public function testSamples(): void
    {
        $databaseName = "DB1";

        $store = DocumentStoreHolder::getStore();
        try {
            # region open_session_2
            $store->openSession(new SessionOptions());
            # endregion

            {
            # region open_session_3
            $sessionOptions = new SessionOptions();
            $sessionOptions->setDatabase($databaseName);
            $store->openSession($sessionOptions);
            # endregion
            }

            # region open_session_4
            $session = $store->openSession();
            try {
                // code here
            } finally {
                $session->close();
            }
            # endregion

            {
                # region open_session_no_tracking
                $sessionOptions = new SessionOptions();
                $sessionOptions->setNoTracking(true);
                $session = $store->openSession();
                try {
                    $employee1 = $session->load(Employee::class, "employees/1-A");
                    $employee2 = $session->load(Employee::class, "employees/1-A");

                    // because NoTracking is set to 'true'
                    // each load will create a new Employee instance
                    $this->assertNotSame($employee1, $employee2);
                } finally {
                    $session->close();
                }
                # endregion
            }

            {
                # region open_session_no_caching
                $sessionOptions = new SessionOptions();
                $sessionOptions->setNoCaching(true);
                $session = $store->openSession();
                try {
                    // code here
                } finally {
                    $session->close();
                }
                # endregion
            }
        } finally {
            $store->close();
        }
    }
}
