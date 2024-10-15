<?php

use RavenDB\Documents\Queries\Explanation\ExplanationOptions;
use RavenDB\Documents\Queries\Explanation\Explanations;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Product;

interface FooInterface {
    # region syntax
    public function includeExplanations(?ExplanationOptions $options, Explanations &$explanations): DocumentQueryInterface;
    # endregion
}

class IncludeExplanations
{
    public function example(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region explain
                $explanations = new Explanations();

                /** @var array<Product> $syrups */
                $syrups = $session->advanced()->documentQuery(Product::class)
                    ->includeExplanations(null, $explanations)
                    ->search("Name", "Syrup")
                    ->toList();

                $scoreDetails = $explanations->getExplanations($syrups[0]->getId());
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
