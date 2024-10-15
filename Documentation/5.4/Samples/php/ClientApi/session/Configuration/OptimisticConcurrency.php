<?php

use RavenDB\Documents\DocumentStore;
use RavenDB\Samples\Infrastructure\Orders\Product;

class OptimisticConcurrency
{
    public function examples(): void
    {
        $store = new DocumentStore();
        try {
            # region optimistic_concurrency_1
            $session = $store->openSession();
            try {
                $session->advanced()->setUseOptimisticConcurrency(true);

                $product = new Product();
                $product->setName("Some Name");

                $session->store($product, "products/999");
                $session->saveChanges();

                $otherSession = $store->openSession();
                try {
                    $otherProduct = $otherSession->load(Product::class, "products/999");
                    $otherProduct->setName("Other Name");

                    $otherSession->saveChanges();
                } finally {
                    $otherSession->close();
                }

                $product->setName("Better Name");
                $session->saveChanges(); //  will throw ConcurrencyException
            } finally {
                $session->close();
            }
            # endregion

            # region optimistic_concurrency_2
            $store->getConventions()->setUseOptimisticConcurrency(true);

            $session = $store->openSession();
            try {
                $isSessionUsingOptimisticConcurrency = $session->advanced()->isUseOptimisticConcurrency(); // will return true
            } finally {
                $session->close();
            }
            # endregion

            # region optimistic_concurrency_3
            $session = $store->openSession();
            try {
                $product = new Product();
                $product->setName("Some Name");

                $session->store(product, "products/999");
                $session->saveChanges();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                $session->advanced()->setUseOptimisticConcurrency(true);

                $product = new Product();
                $product->setName("Some Other Name");

                $session->store(product, null, "products/999");
                $session->saveChanges(); // will NOT throw Concurrency exception
            } finally {
                $session->close();
            }
            # endregion

            # region optimistic_concurrency_4
            $session = $store->openSession();
            try {
                $product = new Product();
                $product->setName("Some Name");
                $session->store($product, "products/999");
                $session->saveChanges();
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                $session->advanced()->setUseOptimisticConcurrency(false); // default value

                $product = new Product();
                $product->setName("Some Other Name");

                $session->store(product, "", "products/999");
                $session->saveChanges(); // will throw Concurrency exception
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }
}
