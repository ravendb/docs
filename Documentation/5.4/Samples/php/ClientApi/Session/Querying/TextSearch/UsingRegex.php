<?php

use RavenDB\Documents\Session\FilterDocumentQueryBaseInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Product;

interface FooInterface
{
    # region syntax
    function whereRegex(?string $fieldName, ?string $pattern): FilterDocumentQueryBaseInterface;
    # endregion
}

class UsingRegex
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region regex_1
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    ->whereRegex("Name", "^[NA]")
                    ->toList();
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
    public ?string $firstName = null;

    public function getFirstName(): ?string
    {
        return $this->firstName;
    }

    public function setFirstName(?string $firstName): void
    {
        $this->firstName = $firstName;
    }
}
