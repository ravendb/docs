<?php

namespace RavenDB\Samples\Server;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\DocumentStoreInterface;
use RavenDB\Documents\Operations\CompareExchange\CompareExchangeResult;
use RavenDB\Documents\Operations\CompareExchange\DeleteCompareExchangeValueOperation;
use RavenDB\Documents\Operations\CompareExchange\PutCompareExchangeValueOperation;
use RavenDB\Documents\Session\CmpXchg;
use RavenDB\Documents\Session\SessionOptions;
use RavenDB\Documents\Session\TransactionMode;
use RavenDB\Exceptions\ConcurrencyException;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Contact;
use RavenDB\Type\Duration;
use RuntimeException;

class CompareExchange
{
    private ?DocumentStore $store = null;

    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            # region email
            $email = "user@example.com";

            $user = new User();
            $user->setEmail($email);

            $session = $store->openSession();
            try {
                $session->store($user);

                // At this point, the user document has an Id assigned

                // Try to reserve a new user email
                // Note: This operation takes place outside of the session transaction,
                //       It is a cluster-wide reservation

                /** @var CompareExchangeResult $cmpXchgResult */
                $cmpXchgResult = $store->operations()->send(new PutCompareExchangeValueOperation("emails/" . $email, $user->getId(), 0));

                if (!$cmpXchgResult->isSuccessful()) {
                    throw new RuntimeException("Email is already in use");
                }

                // At this point we managed to reserve/save the user email -
                // The document can be saved in SaveChanges
                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion


            $session = $store->openSession();
            try {
                # region query_cmpxchg
                $query = $session->advanced()->rawQuery(User::class,
                    "from Users as s where id() == cmpxchg(\"emails/ayende@ayende.com\")")
                    ->toList();
                # endregion

                # region document_query_cmpxchg
                $q = $session->advanced()
                    ->documentQuery(User::class)
                    ->whereEquals("id", CmpXchg::value("emails/ayende@ayende.com"));
                # endregion
            } finally {
                $session->close();
            }

        } finally {
            $store->close();
        }
    }
}

class User
{
    private ?string $email = null;
    private ?string $id = null;
    private ?int $age = null;

    public function getEmail(): ?string
    {
        return $this->email;
    }

    public function setEmail(?string $email): void
    {
        $this->email = $email;
    }

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getAge(): ?int
    {
        return $this->age;
    }

    public function setAge(?int $age): void
    {
        $this->age = $age;
    }
}

# region shared_resource
class SharedResource
{
    private ?DateTime $reservedUntil = null;

    public function getReservedUntil(): ?DateTime
    {
        return $this->reservedUntil;
    }

    public function setReservedUntil(?DateTime $reservedUntil): void
    {
        $this->reservedUntil = $reservedUntil;
    }
}

class CompareExchangeSharedResource
{
    private ?DocumentStore $store = null;

    public function printWork(): void
    {
        // Try to get hold of the printer resource
        $reservationIndex = $this->lockResource($this->store, "Printer/First-Floor", Duration::ofMinutes(20));

        try {
            // Do some work for the duration that was set.
            // Don't exceed the duration, otherwise resource is available for someone else.
        } finally {
            $this->releaseResource($this->store, "Printer/First-Floor", $reservationIndex);
        }
    }

    /**  throws InterruptedException */
    public function lockResource(DocumentStoreInterface $store, ?string $resourceName, Duration $duration): int
    {
        while (true) {
            $now = new DateTime();

            $resource = new SharedResource();
            $resource->setReservedUntil($now->add($duration->toDateInterval()));

            /** @var CompareExchangeResult<SharedResource> $saveResult */
            $saveResult = $store->operations()->send(
                new PutCompareExchangeValueOperation($resourceName, $resource, 0));

            if ($saveResult->isSuccessful()) {
                // resourceName wasn't present - we managed to reserve
                return $saveResult->getIndex();
            }

            // At this point, Put operation failed - someone else owns the lock or lock time expired
            if ($saveResult->getValue()->getReservedUntil() < $now) {
                // Time expired - Update the existing key with the new value
                /** @var CompareExchangeResult<SharedResource> takeLockWithTimeoutResult */
                $takeLockWithTimeoutResult = $store->operations()->send(
                    new PutCompareExchangeValueOperation($resourceName, $resource, $saveResult->getIndex()));

                if ($takeLockWithTimeoutResult->isSuccessful()) {
                    return $takeLockWithTimeoutResult->getIndex();
                }
            }

            // Wait a little bit and retry
            usleep(20000);
        }
    }

    public function releaseResource(DocumentStoreInterface $store, ?string $resourceName, int $index): void
    {
        $deleteResult = $store->operations()->send(
            new DeleteCompareExchangeValueOperation(SharedResource::class, $resourceName, $index)
        );

        // We have 2 options here:
        // $deleteResult->isSuccessful is true - we managed to release resource
        // $deleteResult->isSuccessful is false - someone else took the lock due to timeout
    }
}

# endregion


# region create_uniqueness_control_documents
// When you create documents that must contain a unique value such as a phone or email, etc.,
// you can create reference documents that will have that unique value in their IDs.
// To know if a value already exists, all you need to do is check whether a reference document with such ID exists.

public

class PhoneReference
{
    public ?string $id = null;
    public ?string $companyId = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getCompanyId(): ?string
    {
        return $this->companyId;
    }

    public function setCompanyId(?string $companyId): void
    {
        $this->companyId = $companyId;
    }
}

// The reference document class
class UniquePhoneReference
{
    public function sample(): void
    {
        // A company document class that must be created with a unique 'Phone' field
        $newCompany = new Company();
        $newCompany->setName("companyName");
        $newCompany->setPhone("phoneNumber");

        $newContact = new Contact();
        $newContact->setName("contactName");
        $newContact->setTitle("contactTitle");

        $newCompany->setContact($newContact);

        $this->createCompanyWithUniquePhone($newCompany);
    }

    public function createCompanyWithUniquePhone(Company $newCompany): void
    {
        // Open a cluster-wide session in your document store
        $sessionOptions = new SessionOptions();
        $sessionOptions->setTransactionMode(TransactionMode::clusterWide());
        $session = DocumentStoreHolder::getStore()->openSession($sessionOptions);

        try {
            // Check whether the new company phone already exists
            // by checking if there is already a reference document that has the new phone in its ID.
            $phoneRefDocument = $session->load(PhoneReference::class, "phones/" . $newCompany->getPhone());
            if ($phoneRefDocument != null) {
                $msg = "Phone '" . $newCompany->getPhone() . "' already exists in ID: " . $phoneRefDocument->getCompanyId();
                throw new ConcurrencyException($msg);
            }

            // If the new phone number doesn't already exist, store the new entity
            $session->store($newCompany);
            // Store a new reference document with the new phone value in its ID for future checks.
            $newPhoneReference = new PhoneReference();
            $newPhoneReference->setCompanyId($newCompany->getId());
            $session->store($newPhoneReference, "phones/" . $newCompany->getPhone());

            // May fail if called concurrently with the same phone number
            $session->saveChanges();
        } finally {
            $session->close();
        }
    }
}
# endregion
