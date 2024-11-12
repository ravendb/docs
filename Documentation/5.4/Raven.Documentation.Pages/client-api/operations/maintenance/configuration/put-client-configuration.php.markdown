# Put Client Configuration Operation <br> (for database)

---

{NOTE: }

* The **client configuration** is a set of configuration options applied during 
  client-server communication.  
* The initial client configuration can be set by the client when creating the Document Store.  
* A database administrator can modify the current client configuration on the server using the 
  `PutClientConfigurationOperation` operation or via Studio, to gain dynamic control over 
  client-server communication.  
  The client will be updated with the modified configuration the next time it sends a request to the database.  

---

* In this page:
  
  * [Client configuration overview and modification](../../../../client-api/operations/maintenance/configuration/put-client-configuration#client-configuration-overview-and-modification)
  * [What can be configured](../../../../client-api/operations/maintenance/configuration/put-client-configuration#what-can-be-configured)
  * [Put client configuration (for database)](../../../../client-api/operations/maintenance/configuration/put-client-configuration#put-client-configuration-(for-database))
  * [Syntax](../../../../client-api/operations/maintenance/configuration/put-client-configuration#syntax)

{NOTE /}

---

{PANEL: Client configuration overview and modification}

* **What is the client configuration**:  
  The client configuration is a set of configuration options that apply to the client when communicating with the database.
  See [what can be configured](../../../../client-api/operations/maintenance/configuration/put-client-configuration#what-can-be-configured) below.  

* **Initializing the client configuration** (on the client):  
  This configuration can be initially customized from the client code when creating the Document Store via the [Conventions](../../../../client-api/configuration/conventions).
  
* **Overriding the initial client configuration for the database** (on the server):  

    * From the client code:  
      Use `PutClientConfigurationOperation` to set the client configuration options on the server.  
      See the example below.
    
    * From Studio:  
      Set the client configuration from the [Client Configuration](../../../../studio/database/settings/client-configuration-per-database) view.

* **Updating the running client**:  

  * Once the client configuration is modified on the server, the running client will [receive the updated settings](../../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date)
    the next time it makes a request to the database.  

  * Setting the client configuration on the server enables administrators to dynamically control 
    the client behavior after it has started running.  
    e.g. manage load balancing of client requests on the fly in response to changing system demands.

* The client configuration set for the database level **overrides** the 
  [server-wide client configuration](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration).

{PANEL/}

{PANEL: What can be configured}

The following client configuration options are available:  

* **Identity parts separator**:  
  Set the separator used with automatically generated document identity IDs (default separator is `/`).  
  Learn more about identity IDs creation [here](../../../../server/kb/document-identifier-generation#strategy--3).

* **Maximum number of requests per session**:  
  Set this number to restrict the number of requests (Reads & Writes) per session in the client API.

* **Read balance behavior**:  
  Set the Read balance method the client will use when accessing a node with Read requests.  
  Learn more in [Balancing client requests - overview](../../../../client-api/configuration/load-balance/overview) and [Read balance behavior](../../../../client-api/configuration/load-balance/read-balance-behavior).
  
* **Load balance behavior**:  
  Set the Load balance method for Read & Write requests.  
  Learn more in [Load balance behavior](../../../../client-api/configuration/load-balance/load-balance-behavior).

{PANEL/}

{PANEL: Put client configuration (for database)}

{CODE:php put_config_1@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.php /}

{CODE:php put_config_2@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.php /}

| Parameter         | Type                  | Description                                                            |
|-------------------|-----------------------|------------------------------------------------------------------------|
| **$configuration** | `?ClientConfiguration` | Client configuration that will be set on the server (for the database) |

{CODE:php syntax_2@ClientApi\Operations\Maintenance\Configuration\PutClientConfig.php /}

{PANEL/}

## Related Articles

### Studio

- [Client Configuration View](../../../../studio/database/settings/client-configuration-per-database)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [Get client configuration (for database)](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [Get client configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [Put client configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)


### Load balancing

- [Load balancing client requests](../../../../client-api/configuration/load-balance/overview)
