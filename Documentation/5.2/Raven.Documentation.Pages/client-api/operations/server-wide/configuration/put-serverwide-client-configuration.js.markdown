# Put Client Configuration Operation (Server-Wide)

---

{NOTE: }

* The server-wide Client-Configuration is a set of configuration options that are set __on the server__ and apply to any client when communicating with __any__ database in the cluster.  
  See the available configuration options in the article about [put client-configuration for database](../../../../client-api/operations/maintenance/configuration/put-client-configuration#what-can-be-configured).

* To set the server-wide Client-Configuration on the server:

    * Use `PutServerWideClientConfigurationOperation` from the client code.  
      See the example below.

    * Or, set the server-wide Client-Configuration from the Studio [Client-Configuration view](../../../../studio/server/client-configuration).

* A Client-Configuration that is set on the server for the [database level](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
  will __override__ the server-wide Client-Configuration for that database.  
  A Client-Configuration that is set on the server __overrides__ the initial Client-Configuration that is set on the client when creating the Document Store.

* Once the Client-Configuration is modified on the server, the running client will [receive the updated settings](../../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date)
  the next time it makes a request to the database.

---

* In this page:
    * [Put client-configuration example](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration#put-client-configuration-example)
    * [Syntax](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration#syntax)

{NOTE /}

---

{PANEL: Put client-configuration example}

{CODE:nodejs put_config@ClientApi\Operations\Server\putClientConfig.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Operations\Server\putClientConfig.js /}

| Parameter         | Type     | Description                                                                             |
|-------------------|----------|-----------------------------------------------------------------------------------------|
| __configuration__ | `object` | Client configuration that will be set on the server<br>(server-wide, for all databases) |

{CODE:nodejs syntax_2@ClientApi\Operations\Server\putClientConfig.js /}

{PANEL/}

## Related Articles

- [Get client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [Get client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [Put client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
