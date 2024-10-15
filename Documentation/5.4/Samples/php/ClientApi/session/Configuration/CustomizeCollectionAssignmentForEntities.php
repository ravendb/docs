<?php

use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Samples\Infrastructure\Orders\Category;

class CustomizeCollectionAssignmentForEntities
{
    public function example(): void
    {
        $store = new DocumentStore();

        # region custom_collection_name
        $store->getConventions()->setFindCollectionName(function($className) {
            if (is_subclass_of($className, Category::class)) {
                return "ProductGroups";
            }

            return DocumentConventions::defaultGetCollectionName($className);
        });
        # endregion
    }
}
