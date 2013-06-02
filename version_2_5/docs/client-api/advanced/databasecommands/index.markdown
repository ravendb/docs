
### DatabaseCommands

The `DatabaseCommands` is the actual engine behind the Client API. The Client API introduces features like Unit-of-Work and transactions. Therefore, while the `DatabaseCommands` are exposed in the Client API to allow for advanced usages, all operations that are performed directly against them are NOT transactional, and are NOT safe by default.

#### Document operations

{CODE index_1@ClientApi\Advanced\DatabaseCommands\Index.cs /}

#### Attachments

{CODE index_2@ClientApi\Advanced\DatabaseCommands\Index.cs /}
    
#### Indexes

{CODE index_3@ClientApi\Advanced\DatabaseCommands\Index.cs /}
    
#### Transaction API

{CODE index_4@ClientApi\Advanced\DatabaseCommands\Index.cs /}
    
#### Misc

{CODE index_5@ClientApi\Advanced\DatabaseCommands\Index.cs /}
