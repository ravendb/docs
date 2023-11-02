# Get Client Configuration Operation <br> (for database)

---

{NOTE: }

* It is recommended to first refer to the __client-configuration description__ in the [put client-configuration](../../../../client-api/operations/maintenance/configuration/put-client-configuration) article.
  
* Use `GetClientConfigurationOperation` to get the current client-configuration set on the server for the database.

* In this page:
    * [Get client-configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration#get-client-configuration)
    * [Syntax](../../../../client-api/operations/maintenance/configuration/get-client-configuration#syntax)

{NOTE /}

---

{PANEL: Get client-configuration}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_config@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.cs /}
{CODE-TAB:csharp:Async get_config_async@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.cs /}

{CODE syntax_2@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.cs /}

{PANEL/}

## Related Articles

### Studio

- [Client Configuration](../../../../studio/server/client-configuration)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [Put client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
- [Put client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
- [Get client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
