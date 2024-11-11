<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexPriority;
use RavenDB\Documents\Operations\Indexes\IndexPriorityParameters;
use RavenDB\Documents\Operations\Indexes\SetIndexesPriorityOperation;

class SetPriority
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region set_priority_single
            // Define the set priority operation
            // Pass index name & priority
            $setPriorityOp = new SetIndexesPriorityOperation("Orders/Totals", IndexPriority::high());

            // Execute the operation by passing it to Maintenance.Send
            // An exception will be thrown if index does not exist
            $store->maintenance()->send($setPriorityOp);
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region set_priority_multiple
            // Define the index list and the new priority:
            $parameters = new IndexPriorityParameters();
            $parameters->setIndexNames(["Orders/Totals", "Orders/ByCompany"]);
            $parameters->setPriority(IndexPriority::low());

            // Define the set priority operation, pass the parameters
            $setPriorityOp = new SetIndexesPriorityOperation($parameters);

            // Execute the operation by passing it to Maintenance.Send
            // An exception will be thrown if any of the specified indexes do not exist
            $store->maintenance()->send($setPriorityOp);
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region syntax_1
    // Available overloads:
    SetIndexesPriorityOperation(?string $indexName, ?IndexPriority $priority);
    SetIndexesPriorityOperation(?Parameters $parameters);
    # endregion
}
*/

/*
# region syntax_2
class IndexPriority
{
    public static function low(): IndexPriority;
    public static function normal(): IndexPriority;
    public static function high(): IndexPriority;

    public function isLow(): bool;
    public function isNormal(): bool;
    public function isHigh(): bool;
}
# endregion
*/

/*
# region syntax_3
public class IndexPriorityParameters
{
    private ?StringArray $indexNames = null;
    private ?IndexPriority $priority = null;

    // ... getters and setters
}
# endregion
*/
