<?php

use RavenDB\Documents\Commands\Batches\CommandDataInterface;
use RavenDB\Documents\Commands\Batches\DeleteCommandData;
use RavenDB\Documents\Commands\Batches\ForceRevisionCommandData;
use RavenDB\Documents\Commands\Batches\PatchCommandData;
use RavenDB\Documents\Commands\Batches\PutCommandDataWithJson;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\PatchRequest;

interface FooInterface
{
    # region syntax
    /**
     * Usage:
     *   - defer(CommandDataInterface $command): void
     *   - defer(CommandDataInterface ...$commands): void
     *   - defer(array<CommandDataInterface> $commands): void
     *
     *   - defer(CommandDataInterface $command, array $commands): void
     *
     * Example:
     *   - defer($cmd);
     *   - defer($cmd1, $cmd2, $cmd3, $cmd4 ...)
     *   - defer([$cmd1, $cmd2, $cmd4, $cmd4, ...])
     *   - defer($cmd1, [$cmd2, $cmd3])
     *
     */
    public function defer(...$commands): void;
    # endregion
}

class Defer
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region defer_1

                // Defer is available in the session's `advanced` methods
                $session->advanced()->defer(
                    // Define commands to be executed:

                    // i.e. Put a new document
                    new PutCommandDataWithJson(
                        "products/999-A",
                        null,
                        null,
                        [
                            "Name" => "My Product",
                            "Supplier" => "suppliers/1-A",
                            "@metadata" => [
                                "@collection" => "Products"
                            ]
                        ]
                    ),

                    // Patch document
                    new PatchCommandData(
                        "products/999-A",
                        null,
                        PatchRequest::forScript("this.Supplier = 'suppliers/2-A';"),
                        null
                    ),

                    // Force a revision to be created
                    new ForceRevisionCommandData("products/999-A"),

                    // Delete a document
                    new DeleteCommandData("products/1-A", null)
                );

                // All deferred commands will be sent to the server upon calling SaveChanges
                $session->saveChanges();

                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
