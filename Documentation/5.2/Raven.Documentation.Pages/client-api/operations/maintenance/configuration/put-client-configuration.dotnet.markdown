# Put Client Configuration Operation <br> (for database)

{NOTE: }

* __What is__ the Client-Configuration:  
  The Client-Configuration is a set of configuration options that apply to the client when communicating with the database.
  See [what can be configured](../../../../client-api/operations/maintenance/configuration/put-client-configuration#what-can-be-configured) below.  

* __Initializing__ the Client-Configuration:  
  This configuration can be initialized from the client code when creating the Document Store via the [Conventions](../../../../client-api/configuration/conventions).
  
* __Overriding__ the initial Client-Configuration for the database:  

    * From the client code:  
      Use `PutClientConfigurationOperation` to set the client-configuration options on the server.  
      See the example below.
    
    * From the Studio:  
      Set the Client-Configuration from the [Client-Configuration view](../../../../studio/database/settings/client-configuration-per-database).

* __Updating__ the running client:  
  Once the Client-Configuration is modified on the server, the running client will [receive the updated settings](../../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date)
  the next time it makes a request to the database. 

* The client-configuration set for the database overrides the [server-wide client-configuration](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration).

---

* In this page:
  * [What can be configured](../../../../client-api/operations/maintenance/configuration/put-client-configuration#what-can-be-configured)
  * [Put client-configuration example](../../../../client-api/operations/maintenance/configuration/put-client-configuration#put-client-configuration-example)
  * [Syntax](../../../../client-api/operations/maintenance/configuration/put-client-configuration#syntax)

{NOTE /}

---

{PANEL: What can be configured}

The following client-configuration options are available:  

* __Identity parts separator__:  
  Set the separator used with automatically generated document identity IDs (default separator is `/`).  
  Learn more about identity IDs creation [here](../../../../server/kb/document-identifier-generation#identity).

* __Maximum number of requests per session__:  
  Set this number to restrict the number of requests (Reads & Writes) per session in the client API.

* __Read balance behavior__:  
  Set the Read balance method the client will use when accessing a node with Read requests.  
  Learn more in [Balancing client requests - overview](../../../../client-api/configuration/load-balance/overview) and [Read balance behavior](../../../../client-api/configuration/load-balance/read-balance-behavior).
  
* __Load balance behavior__:  
  Set the Load balance method for Read & Write requests.  
  Learn more in [Load balance behavior](../../../../client-api/configuration/load-balance/load-balance-behavior).

{PANEL/}

{PANEL: Put client-configuration example}

{CODE put_config_1@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.cs /}

{CODE-TABS}
{CODE-TAB:csharp:Sync put_config_2@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.cs /}
{CODE-TAB:csharp:Async put_config_3@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.cs /}

| Parameter         | Type                  | Description                                      |
|-------------------|-----------------------|--------------------------------------------------|
| __configuration__ | `ClientConfiguration` | Client configuration to be used for the database |

{CODE syntax_2@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.cs /}

{PANEL/}

## Related Articles

### Studio

- [Client Configuration View](../../../../studio/database/settings/client-configuration-per-database)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [Get Client Configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [Get Configuration (Server-Wide)](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [Put Client Configuration (Server-Wide)](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)


### Load balancing

- [Load balancing client requests](../../../../client-api/configuration/load-balance/overview)
