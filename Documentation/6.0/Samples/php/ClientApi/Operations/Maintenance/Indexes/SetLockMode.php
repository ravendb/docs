<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexLockMode;
use RavenDB\Documents\Operations\Indexes\IndexLockParameters;
use RavenDB\Documents\Operations\Indexes\SetIndexesLockOperation;

class SetLockMode
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region set_lock_single
            // Define the set lock mode operation
            // Pass index name & lock mode
            $setLockModeOp = new SetIndexesLockOperation("Orders/Totals", IndexLockMode::lockedIgnore());

            // Execute the operation by passing it to Maintenance.Send
            // An exception will be thrown if index does not exist
            $store->maintenance()->send($setLockModeOp);

            // Lock mode is now set to 'LockedIgnore'
            // Any modifications done now to the index will Not be applied, and will Not throw
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region set_lock_multiple
            // Define the index list and the new lock mode:
            $parameters = new IndexLockParameters();
            $parameters->setIndexNames([ "Orders/Totals", "Orders/ByCompany" ]);
            $parameters->setMode(IndexLockMode::lockedError());

            // Define the set lock mode operation, pass the parameters
            $setLockModeOp = new SetIndexesLockOperation($parameters);

            // Execute the operation by passing it to Maintenance.Send
            // An exception will be thrown if any of the specified indexes do not exist
            $store->maintenance()->send($setLockModeOp);

            // Lock mode is now set to 'LockedError' on both indexes
            // Any modifications done now to either index will throw
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
    SetIndexesLockOperation(?string $indexName, ?IndexLockMode $mode);
    SetIndexesLockOperation(?Parameters $parameters);
    # endregion
}
*/

/*
# region syntax_2
class IndexLockMode
{
    public static function unlock(): IndexLockMode;
    public static function lockedIgnore(): IndexLockMode
    public static function lockedError(): IndexLockMode;

    public function isUnlock(): bool;
    public function isLockedIgnore(): bool;
    public function isLockedError(): bool;
}
# endregion
*/

/*
# region syntax_3
public class IndexLockParameters
{
    private ?StringArray $indexNames = null;
    private ?IndexLockMode $mode = null;

    // ... getters and setters
}
# endregion
*/
