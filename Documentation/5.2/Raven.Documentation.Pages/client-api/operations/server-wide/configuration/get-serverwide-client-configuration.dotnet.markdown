# Get Client Configuration Operation (Server-Wide)

---

{NOTE: }

* It is recommended to first refer to the [put server-wide client-configuration](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration) article for general knowledge.

* Use `GetServerWideClientConfigurationOperation` to get the current server-wide Client-Configuration set on the server.

* In this page:
    * [Get client-configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration#get-client-configuration)
    * [Syntax](../../../../client-api/operations/maintenance/configuration/get-client-configuration#syntax)

{NOTE /}

---

{PANEL: Get client-configuration}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_config@ClientApi\Operations\Server\GetClientConfig.cs /}
{CODE-TAB:csharp:Async get_config_async@ClientApi\Operations\Server\GetClientConfig.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Operations\Server\GetClientConfig.cs /}

| Return Value          |                                                |
|-----------------------|------------------------------------------------|
| `ClientConfiguration` | Configuration which will be used by the Client |

{PANEL/}

## Related Articles

- [Put client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
- [Put client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
- [Get client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
