<?php

use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Product;

class EndsWith
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region endsWith_1
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    // Call 'whereEndsWith' on the field
                    // Pass the postfix to search by
                    ->whereEndsWith("Name", "Lager")
                    ->toList();

                // Results will contain only Product documents having a 'Name' field
                // that ends with any case variation of 'lager'
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region endsWith_3
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    // Call 'whereEndsWith'
                    // Pass the document field and the postfix to search by
                    ->whereEndsWith("Name", "Lager")
                    ->toList();

                // Results will contain only Product documents having a 'Name' field
                // that ends with any case variation of 'lager'
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region endsWith_4
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    // Pass 'exact: true' to search for an EXACT postfix match
                    ->whereEndsWith("Name", "Lager", true)
                    ->toList();

                // Results will contain only Product documents having a 'Name' field
                // that ends with 'Lager'
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region endsWith_6
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    // Call 'whereEndsWith'
                    // Pass 'exact: true' to search for an EXACT postfix match
                    ->whereEndsWith("Name", "Lager", true)
                    ->toList();

                // Results will contain only Product documents having a 'Name' field
                // that ends with 'Lager'
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region endsWith_7
                /** @var array<Product> $products */
                $products = $session
                    ->query(Product::class)
                    // Call 'Not' to negate the next predicate
                    ->not()
                    // Call 'whereEndsWith' on the field
                    // Pass the postfix to search by
                    ->whereEndsWith("Name", "Lager")
                    ->toList();

                // Results will contain only Product documents having a 'Name' field
                // that does NOT end with 'lager' or any other case variations of it
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region endsWith_9
                /** @var array<Product> $products */
                $products = $session->advanced()
                    ->documentQuery(Product::class)
                    // Call 'Not' to negate the next predicate
                    ->not()
                    // Call 'whereEndsWith'
                    // Pass the document field and the postfix to search by
                    ->whereEndsWith("Name", "Lager")
                    ->toList();

                // Results will contain only Product documents having a 'Name' field
                // that does NOT end with 'lager' or any other case variations of it
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
