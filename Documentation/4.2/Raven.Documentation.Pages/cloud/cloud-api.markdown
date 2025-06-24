# Cloud: API
---

{NOTE: }

*RavenDB Cloud API* provides client libraries that allow you to perform API operations. 
The API is defined using the OpenAPI 3.0 standard. 

* In this page:
    * [Swagger & available clients](#swagger-&-available-clients)
    * [API methods overview](#api-methods-overview)
    * [API Keys expiration](#api-keys-expiration)
    * [Product quota](#product-quota)

{NOTE/}

---

{PANEL: Swagger & available clients}

!["Figure 1 - Swagger UI"](images\cloud-api-swagger-ui-overview.png "Figure 1 - Swagger UI")

You can use [Swagger UI](https://api.cloud.ravendb.net/swagger/index.html) to perform the API calls directly from the website
or use [Swagger JSON](https://api.cloud.ravendb.net/api/v1/swagger.json) to prepare your own API client.

Client libraries are available in the following languages:

[C#](https://www.nuget.org/packages/RavenDB.Cloud.Api.Client)  
[node.js](https://www.npmjs.com/package/ravendb-cloud)

{PANEL/}

{PANEL: API methods overview}

*The RavenDB Cloud API* contains three categories of API methods: **Account**, **Metadata**, **Products**.

| **Category** | **Description**                                                                                                                                                                       |
|--------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Account      | Returns Cloud account information - domain name.                                                                                                                                      |
| Metadata     | Return information about available instance types, regions and release channels.                                                                                                      |
| Products     | Allows to manage products - restart a node, download a client certificate, change the instance and disk type, terminate a cluster, add or remove an additional node from the cluster. |

More information about available methods, requests and responses can be found in the [Swagger & available clients](#swagger-&-available-clients) section.

{PANEL/}

{PANEL: API Keys expiration}

Max expiration period is *12 months* and cannot be longer than this period.

{WARNING: } 
RavenDB Cloud does not send reminders about upcoming API key expiration.   
{WARNING/}

You can change the expiration date or extend it for another *12 months*.  
For more details, please check the [Edit API Key](../cloud/portal/cloud-portal-api-tab#edit-api-key) section.

{PANEL/}

{PANEL: Product quota}

You cannot create another product via the *RavenDB Cloud API* if you have **three or more** products.  

{INFO: }
Contact support to extend the quota.
{INFO/}

{PANEL/}
