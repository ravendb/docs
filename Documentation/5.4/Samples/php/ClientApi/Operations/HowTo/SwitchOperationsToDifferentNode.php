<?php

namespace RavenDB\Samples\ClientApi\Operations\HowTo;

use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Http\ReadBalanceBehavior;
use RavenDB\ServerWide\Operations\GetDatabaseNamesOperation;

class SwitchOperationsToDifferentNode
{
    public function samples(): void
    {
        # region for_node_1
        // Default node access can be defined on the store
        $documentStore = new DocumentStore(
            ["ServerURL_1", "ServerURL_2", "..."],
            "DefaultDB"
        );

        $conventions = new DocumentConventions();

        // For example:
        // With ReadBalanceBehavior set to: 'FastestNode':
        // Client READ requests will address the fastest node
        // Client WRITE requests will address the preferred node
        $conventions->setReadBalanceBehavior(ReadBalanceBehavior::fastestNode());
        $documentStore->setConventions($conventions);

        $documentStore->initialize();

        try {
            // Use 'ForNode' to override the default node configuration
            // The Maintenance.Server operation will be executed on the specified node
            $dbNames = $documentStore->maintenance()->server()->forNode("C")
                ->send(new GetDatabaseNamesOperation(0, 25));
        } finally {
            $documentStore->close();
        }
        # endregion
    }
}

/*
interface OperationsForDatabaseSyntax
{
    # region syntax_1
    public function forNode(string $nodeTag): ServerOperationExecutor
    # endregion
}
*/
