<?php

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

interface FooInterface {
    # region intersect_1
    public function intersect(): DocumentQueryInterface;
    # endregion
}

class HowToUseIntersect
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {

            $session = $store->openSession();
            try {
                # region intersect_2
                // return all T-shirts that are manufactured by 'Raven'
                // and contain both 'Small Blue' and 'Large Gray' types
                /** @var array<TShirt> $tShirts */
                $tShirts = $session
                    ->query(TShirt::class, TShirts_ByManufacturerColorSizeAndReleaseYear::class)
                    ->whereEquals("manufacturer", "Raven")
                    ->intersect()
                    ->whereEquals("color", "Blue")
                    ->andAlso()
                    ->whereEquals("size", "Small")
                    ->intersect()
                    ->whereEquals("color", "Gray")
                    ->andAlso()
                    ->whereEquals("size", "Large")
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

class TShirt {

}

class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask
{
}
