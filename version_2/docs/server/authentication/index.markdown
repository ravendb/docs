#Authentication & Authorization

RavenDB comes with a built-in authentication functionality and it supports two types of authentication:    
* [Windows Authentication](index/#windows-authentication)   
* [OAuth Authentication](index/#oauth-authentication)   

Appropriate authentication type is chosen by examining incoming request headers and by default all actions except read-only are being authenticated. To change which actions are being authenticated please refer to [Raven/AnonymousAccess](../administration/configuration#authorization--authentication) configuration setting.

##Windows Authentication

##OAuth Authentication

Second supported authentication type is an [OAuth](http://oauth.net/) authentication. To leverage this type of authentication you need:   
* OAuth server (RavenDB comes with a built-in server that can be accessed under `http://RavenDB-Server-Url/OAuth/AccessToken` url - implementation of **IAuthenticateClient** interface is needed)   
* OAuth certificate (certificate will be auto-generated if none is specified)    

To change default OAuth server url, certificate path and password one must change configuration settings. How this can be achieved can be found [here](../administration/configuration#authorization--authentication).

###Example - Access Token Request

One way to authenticate using OAuth is to request Access Tokens. Below example is based on [RFC6749](http://tools.ietf.org/html/rfc6749#section-4.4).

First we need to request a token by issuing a request to our OAuth server with `grant_type` header with `client_credentials` as a value and appropriate `Accept` and `Authorization` header values (below example uses `Basic` authorization with base64 encoded username and password separated by colon).

{CODE-START:json /}
    > curl http://RavenDB-Server-Url/OAuth/AccessToken -X GET -H "grant_type:client_credentials" -H "Accept: application/json;charset=UTF-8" -H "Authorization: Basic cmF2ZW46cmF2ZW4="

	{
							"Body": {
								"UserId": "raven",
								"AuthorizedDatabases": [
									{
										"Admin": false,
										"TenantId": "*",
										"ReadOnly": false
									}
								],
								"Issued": 63492793737798.469
							},
							"Signature": "ALvhPnKFOtRsIZIH4q7s2I9WRO+JQUfQ8gtAtBh1htQi4E+94EbxPUwuiO+/4HBPuRSZ3lmYcKm1HN+q4t6jeuNofQZUcad3dr884DKTUN3PJ++r+p1ceXgFn3g6p6ncSoBQ6WjQiqlfk1eFNdBrD9cgn+R6n6AeBE8/WgSWDxY="
	}

{CODE-END /}

After a successful authentication, the token must be passed as a part of **Authorization** header (format: "Bearer token_data") to every request that need an authentication.

{CODE-START:json /}
	> curl http://RavenDB-Server-Url:8080/docs/bob -X PUT -d "{ Name: 'Bob', HomeState: 'Maryland', ObjectType: 'User' }" -H "Authorization: Bearer {Body:\"{'UserId':'raven','AuthorizedDatabases':[{'Admin':false,'TenantId':'*','ReadOnly':false}],'Issued':63492793737798.469}\",'Signature':'ALvhPnKFOtRsIZIH4q7s2I9WRO+JQUfQ8gtAtBh1htQi4E+94EbxPUwuiO+/4HBPuRSZ3lmYcKm1HN+q4t6jeuNofQZUcad3dr884DKTUN3PJ++r+p1ceXgFn3g6p6ncSoBQ6WjQiqlfk1eFNdBrD9cgn+R6n6AeBE8/WgSWDxY='}"

{CODE-END /}

###Example - API keys

Another way is to use API keys. To do it we need to create a document with `Raven/ApiKeys/key_name` as a key and `ApiKeyDefinition` as a content.

{CODE authentication_3@Server/Authentication/Index.cs /}

Now to perform any actions against specified database (`system` database must be declared explicitly), we need to provide the API key.

{CODE authentication_4@Server/Authentication/Index.cs /}

###Internal RavenDB OAuth server configuration

To configure our built-in OAuth server you need to implement and **IAuthenticateClient** interface defined below.

{CODE authentication_1@Server/Authentication/Index.cs /}

In our example we will create a very simple implementation that will authenticate and provide access to all databases to users that signed with equal `username` and `password`.

{CODE authentication_2@Server/Authentication/Index.cs /}

From now on we will have a full-fledged OAuth server that can be accessed by `http://RavenDB-Server-Url/OAuth/AccessToken` url.
