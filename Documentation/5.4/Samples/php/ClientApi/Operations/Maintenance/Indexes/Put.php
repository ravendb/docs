<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexConfiguration;
use RavenDB\Documents\Indexes\IndexDefinition;
use RavenDB\Documents\Indexes\IndexDefinitionBuilder;
use RavenDB\Documents\Indexes\IndexDeploymentMode;
use RavenDB\Documents\Indexes\IndexPriority;
use RavenDB\Documents\Operations\Indexes\PutIndexesOperation;

class Put
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region put_1
            // Create an index definition
            $indexDefinition = new IndexDefinition();

            // Name is mandatory, can use any string
            $indexDefinition->setName("OrdersByTotal");

            // Define the index Map functions, string format
            // A single string for a map-index, multiple strings for a multi-map-index
            $indexDefinition->setMaps([
                "// Define the collection that will be indexed:" .
                "from order in docs.Orders" .
                "   // Define the index-entry:" .
                "   select new" .
                "   {" .
                "     // Define the index-fields within each index-entry:" .
                "     Employee = order.Employee," .
                "     Company = order.Company," .
                "     Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))" .
                "   }"
            ]);

            // $indexDefinition->setReduce(...);

            // Can provide other index definitions available on the IndexDefinition class
            // Override the default values, e.g.:
            $indexDefinition->setDeploymentMode(IndexDeploymentMode::rolling());
            $indexDefinition->setPriority(IndexPriority::high());

            $configuration = new IndexConfiguration();
            $configuration->offsetSet("Indexing.IndexMissingFieldsAsNull", "true");
            $indexDefinition->setConfiguration($configuration);

            // See all available properties in syntax below

            // Define the put indexes operation, pass the index definition
            // Note: multiple index definitions can be passed, see syntax below
            $putIndexesOp = new PutIndexesOperation($indexDefinition);

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($putIndexesOp);
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region put_1_JS
            // Create an index definition
            $indexDefinition = new IndexDefinition();

            // Name is mandatory, can use any string
            $indexDefinition->setName("OrdersByTotal");

            // Define the index Map functions, string format
            // A single string for a map-index, multiple strings for a multi-map-index
            $indexDefinition->setMaps([
                "map('Orders', function(order) {" .
                "    return {" .
                "        Employee: order.Employee," .
                "                  Company: order.Company," .
                "                  Total: order.Lines.reduce(function(sum, l) {" .
                "            return sum + (l.Quantity * l.PricePerUnit) * (1 - l.Discount);" .
                "        }, 0)" .
                "    };" .
                "});"
            ]);

            // $indexDefinition->setReduce(...);

            // Can provide other index definitions available on the IndexDefinition class
            // Override the default values, e.g.:

            $indexDefinition->setDeploymentMode(IndexDeploymentMode::rolling());
            $indexDefinition->setPriority(IndexPriority::high());

            $configuration = new IndexConfiguration();
            $configuration->offsetSet("Indexing.IndexMissingFieldsAsNull", "true");
            $indexDefinition->setConfiguration($configuration);
            // See all available properties in syntax below

            // Define the put indexes operation, pass the index definition
            // Note: multiple index definitions can be passed, see syntax below
            $putIndexesOp = new PutIndexesOperation($indexDefinition);

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($putIndexesOp);
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region put_2
            // Create an index definition builder
            $builder = new IndexDefinitionBuilder();
            $builder->setMap(
                "// Define the collection that will be indexed:" .
                "      from order in docs.Orders" .
                "        // Define the index-entry:" .
                "        select new" .
                "        {" .
                "            // Define the index-fields within each index-entry:" .
                "            Employee = order.Employee," .
                "            Company = order.Company," .
                "            Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))" .
                "        }        "
            );

            // Can provide other properties available on the IndexDefinitionBuilder class, e.g.:
            $builder->setDeploymentMode(IndexDeploymentMode::rolling());
            $builder->setPriority(IndexPriority::high());
            // $builder->setReduce(...);

            // Generate index definition from builder
            // Pass the conventions, needed for building the Maps property
            $indexDefinition = $builder->toIndexDefinition($store->getConventions());

            // Optionally, set the index name, can use any string
            // If not provided then default name from builder is used, e.g.: "IndexDefinitionBuildersOfOrders"
            $indexDefinition->setName("OrdersByTotal");

            // Define the put indexes operation, pass the index definition
            // Note: multiple index definitions can be passed, see syntax below
            $putIndexesOp = new PutIndexesOperation($indexDefinition);

            // Execute the operation by passing it to maintenance.send
            $store->maintenance()->send($putIndexesOp);
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region syntax
    PutIndexesOperation(IndexDefinition|IndexDefinitionArray|array ...$indexToAdd)
    # endregion
}
*/
