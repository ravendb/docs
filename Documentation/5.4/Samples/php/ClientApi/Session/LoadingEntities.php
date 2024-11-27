<?php

namespace RavenDB\Samples\ClientApi\Session;

use RavenDB\Documents\Session\ConditionalLoadResult;
use RavenDB\Documents\Session\Loaders\LoaderWithIncludeInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Entity\User;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Samples\Infrastructure\Orders\Product;
use RavenDB\Samples\Infrastructure\Orders\Supplier;
use RavenDB\Type\ObjectArray;
use RavenDB\Type\StringArray;

interface FooInterface {
    # region loading_entities_1_0
    public function load(?string $className, ?string $id): ?object;
    # endregion
}

interface FooInterface2 {
    # region loading_entities_2_0
    function include(?string $path): LoaderWithIncludeInterface;

    public function load(string $className, array $ids): ObjectArray;
    public function load(string $className, StringArray $ids): ObjectArray;

    public function load(string $className, string $id): ?object;
    # endregion
}

interface FooInterface3 {
    # region loading_entities_3_0
    public function load(string $className, array $ids): ObjectArray;
    public function load(string $className, StringArray $ids): ObjectArray;
    # endregion

    # region loading_entities_4_0
    public function loadStartingWith(
        string $className,
        ?string $idPrefix,
        ?string $matches = null,
        int $start = 0,
        int $pageSize = 25,
        ?string $exclude = null,
        ?string $startAfter = null
    ): ObjectArray;

    # endregion

    # region loading_entities_6_0
    function isLoaded(string $id): bool;
    # endregion

    # region loading_entities_7_0
    function conditionalLoad(?string $className, ?string $id, ?string $changeVector): ConditionalLoadResult;
    # endregion
}

class LoadingEntities
{
    public function testSamples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region loading_entities_1_1

                /** @var Employee $employee */
                $employee = $session->load(Employee::class, "employees/1");
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region loading_entities_2_1
                // loading 'products/1'
                // including document found in 'supplier' property

                /** @var Product $product */
                $product = $session
                    ->include("Supplier")
                    ->load(Product::class, "products/1");

                $supplier = $session->load(Supplier::class, $product->getSupplier()); // this will not make server call
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region loading_entities_2_2
                // loading 'products/1'
                // including document found in 'supplier' property
                /** @var Product $product */
                $product = $session
                    ->include("Supplier")
                    ->load(Product::class, "products/1");

                $supplier = $session->load(Supplier::class, $product->getSupplier()); // this will not make server call
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region loading_entities_3_1
                $employees = $session->load(Employee::class, "employees/1", "employees/2", "employees/3");
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region loading_entities_4_1
                // return up to 128 entities with Id that starts with 'employees'
                $result = $session
                    ->advanced()
                    ->loadStartingWith(Employee::class, "employees/", null, 0, 128);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region loading_entities_4_2
                // return up to 128 entities with Id that starts with 'employees/'
                // and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
                $result = $session
                    ->advanced()
                    ->loadStartingWith(Employee::class, "employees/", "1*|2*", 0, 128);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region loading_entities_6_1
                $isLoaded = $session->advanced()->isLoaded("employees/1"); //false
                $employee = $session->load(Employee::class, "employees/1");
                $isLoaded = $session->advanced()->isLoaded("employees/1"); // true
                # endregion
            } finally {
                $session->close();
            }

            # region loading_entities_7_1
            $session = $store->openSession();
            try {
                $changeVector = "";
                $user = new User("Bob");

                $session->store($user, "users/1");
                $session->saveChanges();

                $changeVector = $session->advanced()->getChangeVectorFor($user);
            } finally {
                $session->close();
            }

            $user = new User("Bob");
            $changeVector = "a";

            $session = $store->openSession();
            try {
                // New session which does not track our User entity

                // The given change vector matches
                // the server-side change vector
                // Does not load the document
                $result1 = $session->advanced()
                    ->conditionalLoad(User::class, "users/1", $changeVector);

                // Modify the document
                $user->setName("Bob Smith");
                $session->store($user);
                $session->saveChanges();

                // Change vectors do not match
                // Loads the document
                $result2 = $session->advanced()
                    ->conditionalLoad(User::class, "users/1", $changeVector);
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }
}
