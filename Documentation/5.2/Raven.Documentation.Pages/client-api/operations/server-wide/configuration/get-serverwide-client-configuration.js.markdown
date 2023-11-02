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

{CODE:nodejs get_config@ClientApi\Operations\Server\getClientConfig.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Operations\Server\getClientConfig.js /}

{CODE:nodejs syntax_2@ClientApi\Operations\Server\getClientConfig.js /}

{PANEL/}

## Related Articles

- [Put client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
- [Put client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
- [Get client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
