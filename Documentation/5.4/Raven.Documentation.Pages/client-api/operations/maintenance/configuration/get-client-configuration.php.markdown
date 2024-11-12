# Get Client Configuration Operation <br> (for database)

---

{NOTE: }

* It is recommended to first refer to the **client-configuration description** in the [put client-configuration](../../../../client-api/operations/maintenance/configuration/put-client-configuration) article.
  
* Use `GetClientConfigurationOperation` to get the current client-configuration set on the server for the database.

* In this page:
    * [Get client-configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration#get-client-configuration)
    * [Syntax](../../../../client-api/operations/maintenance/configuration/get-client-configuration#syntax)

{NOTE /}

---

{PANEL: Get client-configuration}

{CODE:php get_config@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.php /}

{CODE:php syntax_2@ClientApi\Operations\Maintenance\Configuration\GetClientConfig.php /}

{PANEL/}

## Related Articles

### Studio

- [Client Configuration](../../../../studio/server/client-configuration)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [Put client-configuration (for database)](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
- [Put client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
- [Get client-configuration (server-wide)](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
