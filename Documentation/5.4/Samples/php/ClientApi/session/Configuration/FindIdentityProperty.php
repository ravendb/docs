<?php

use RavenDB\Documents\DocumentStore;

interface FooInterface
{
    # region identity_1
    public function setFindIdentityProperty(?Closure $findIdentityProperty): void;
    # endregion
}

class FindIdentityProperty
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            # region identity_2
            $store->getConventions()->setFindIdentityProperty(function($property) {
                    return "Identifier" == $property->getName();
            });
            # endregion
        } finally {
            $store->close();
        }
    }
}
