# Cloud: API
---

{NOTE: }

*RavenDB Cloud API* provides client libraries that allow you to perform API operations. 
The API is defined using the OpenAPI 3.0 standard. 

* In this page:
    * [Swagger & available clients](../cloud/cloud-api#swagger-&-available-clients)
    * [API Keys expiration](../cloud/cloud-api#api-keys-expiration)

{NOTE/}

---

{PANEL: Swagger & available clients}

!["Figure X - Swagger UI"](images\cloud-api-swagger-ui-overview.png "Figure X - Swagger UI")

You can use [Swagger UI](https://api.cloud.ravendb.net/swagger/index.html) to perform the API calls directly from the website
or use [Swagger JSON](https://api.cloud.ravendb.net/api/v1/swagger.json) to prepare your own API client.

Client libraries are available in the following languages:

[C#](https://www.nuget.org/packages/RavenDB.Cloud.Api.Client)  
[node.js](https://www.npmjs.com/package/ravendb-cloud)

{PANEL/}

{PANEL: API Keys expiration}

Max expiration period is *12 months* and cannot be longer than this period.

{WARNING: } 
RavenDB Cloud does not send reminders about upcoming API key expiration.   
{WARNING/}

You can change the expiration date or extend it for another 12 months.  
For more details, please check the [Edit API Key](../cloud/portal/cloud-portal-api-tab#edit-api-key) section.

{PANEL/}
