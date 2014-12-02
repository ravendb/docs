# How to work with authentication?

There are two ways to authenticate database users in RavenDB. The first one is Windows Authentication which requires to provide credentials in the form of a login / password / domain triple. 
The second option you can choose is OAuth with an API Key. Before you will work with a database in authenticated manner, first you need to go to the RavenDB server and configure an access there.
Detailed information about necessary server side work to setup the authentication you will find in a server configuration article [Authentication and authorization](../../server/configuration/authentication-and-authorization).

Server recognizes the requested authentication type based on headers of a request sent by the client, so the `DocumentStore` needs to be properly configured in order to 
be able to perform any actions against a database with enabled authentication.

## Windows Authentication

The `DocumentStore` has the default authentication credentials corresponded with the current security context in which an application is running. These credentials are taken from `CredentialCache` object: 

{CODE  windows_auth_default_credentials@ClientApi\HowTo\WorkWithAuthentication.cs /}

In order to force the `DocumentStore` to use the custom user credentials you need to overwrite the default ones before calling `Initialize` method: 

{CODE windows_auth_setup@ClientApi\HowTo\WorkWithAuthentication.cs /}

## OAuth and API Key

The OAuth configuration is also very simple. You just need to set `ApiKey` property instead of `Credentials`. The ApiKey is a string in format _apiKeyName/apiKeySecret_ according to
the `ApiKeyDefinition` stored in the system database. For example for the following key definition:

{CODE api_key_def@ClientApi\HowTo\WorkWithAuthentication.cs /}

you should create the `DocumentStore` as below to have the admin access to the `Northwind` database:

{CODE api_key_setup@ClientApi\HowTo\WorkWithAuthentication.cs /}


## Connection string

The Windows credentials or Api Key can be also specified in the connection string. Look [here](../setting-up-connection-string) for more details.

##Related articles

- [Server : Configuration : Authentication & Authorization](../../server/configuration/authentication-and-authorization)
