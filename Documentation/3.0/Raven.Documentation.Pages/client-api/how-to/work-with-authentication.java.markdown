# How to work with authentication?

In order to authenticate database users in RavenDB you can use API Key.
Before you will work with a database in authenticated manner, first you need to go to the RavenDB server and configure an access there.
Detailed information about necessary server side work to setup the authentication you will find in a server configuration article [Authentication and authorization](../../server/configuration/authentication-and-authorization).

Server recognizes the requested authentication type based on headers of a request sent by the client, so the `DocumentStore` needs to be properly configured in order to 
be able to perform any actions against a database with enabled authentication.

## Windows Authentication

Windows authentication is not supported by RavenDB java client.

## OAuth and API Key

The OAuth configuration is also very simple. You just need to set `ApiKey` instead of `Credentials`. The ApiKey is a string in format _apiKeyName/apiKeySecret_ according to
the `ApiKeyDefinition` stored in the system database. For example for the following key definition:

{CODE:java api_key_def@ClientApi\HowTo\WorkWithAuthentication.java /}

you should create the `DocumentStore` as below to have the admin access to the `Northwind` database:

{CODE:java api_key_setup@ClientApi\HowTo\WorkWithAuthentication.java /}


## Connection string

The Api Key can be also specified in the connection string. Look [here](../setting-up-connection-string) for more details.
