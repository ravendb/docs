<?php

namespace RavenDB\Samples\ClientApi\Session\HowTo;

use RavenDB\Documents\DocumentStore;
use RavenDB\Http\ServerNode;
use RavenDB\Http\ServerNodeRole;
use RavenDB\Type\Url;
use RavenDB\Type\ValueObjectInterface;

interface FooInterface
{
    # region current_session_node_1
    public function getCurrentSessionNode(): ServerNode;
    # endregion
}

class GetCurrentSessionNode
{
    public function example(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region current_session_node_3
                $serverNode = $session->advanced()->getCurrentSessionNode();
                echo $serverNode->getUrl();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region current_session_node_2
class ServerNode
{
    private ?Url $url = null;
    private ?string $database = null;
    private string $clusterTag;
    private ?ServerNodeRole $serverRole = null;

    public function __construct()
    {
        $this->serverRole = ServerNodeRole::none();
    }

    // ... getters and setters
}

class ServerNodeRole
{
    public static function none(): ServerNodeRole;
    public static function promotable(): ServerNodeRole;
    public static function member(): ServerNodeRole;
    public static function rehab(): ServerNodeRole;
}
# endregion
