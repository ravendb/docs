# Configuration: Authentication & Authorization

RavenDB comes with built-in authentication functionality and it supports two types of authentication:    

* [Windows Authentication](../../server/configuration/authentication-and-authorization#windows-authentication)   
* [OAuth Authentication](../../server/configuration/authentication-and-authorization#oauth-authentication)   

The appropriate authentication type is chosen by examining incoming request headers. By default all actions except read-only are being authenticated. To determine which actions will be authenticated, please refer to [Raven/AnonymousAccess](../../server/configuration/configuration-options#authorization--authentication) configuration setting.

## Security system - OSS vs commercial use

The authentication feature is available only in commercial use of RavenDB. You will be able to enable it if you provide a valid commercial license to your database instance. For an open-source software the only available setting is `Raven/AnonymousAccess = Admin`, which means that no authentication is required. Then any user will have all administrative permissions.
An attempt to setup authentication for a database working under AGPL license will result in an exception thrown by a server.

In order to prevent security issues in commercial systems related to a temporary lack of a valid license (e.g. if it just expired), RavenDB stores info about a last seen valid license. This way despite the fact that the license is temporary invalid, the authentication will still work.

## Windows Authentication

When an action (request) needs to be authenticated and no other authentication method is detected, then Windows Authentication is chosen. Worth noting is that all `/admin` endpoint requests are processed using this method.
By default only admins and backup operator users have access to all databases. Other users and groups don't have any access to resources, but this can be easily changed by editing the `Raven/Authorization/WindowsSettings` document in the `system` database. The document consists of list of users and groups that contain the list of accessible databases.

### Example:

{CODE-BLOCK:json}
{
	"RequiredGroups": [],
	"RequiredUsers": [
		{
			"Name": "IIS AppPool\\DefaultAppPool",
			"Enabled": true,
			"Databases": [
				{
					"Admin": false,
					"TenantId": "Northwind",
					"ReadOnly": true
				}
			]
		}
	]
}
{CODE-BLOCK/}

The above example gives a read-only access to `Northwind` database to the user: `IIS AppPool\DefaultAppPool`. A similar effect can be achieved [using the studio](../../studio/management/windows-authentication).

### Allow to login by using an account with a blank password

By default Windows Authentication does not allow to use an account that has a blank password. However, if you really need this, you can disable this Windows security policy using:

{CODE-BLOCK:json}
	Raven.Server.exe /allow-blank-password-use
{CODE-BLOCK/}

It will disable the following policy _Limit local account use of blank passwords to console logon only_ on your Windows machine. In order to revert your changes you can use:

{CODE-BLOCK:json}
	Raven.Server.exe /deny-blank-password-use
{CODE-BLOCK/}

to get back into the default setting.

## OAuth Authentication

The second supported authentication type is an [OAuth](https://oauth.net/) authentication and to simplify the process, we have introduced the API key authentication described below.
To authenticate the user by using API keys we need to create a document with `Raven/ApiKeys/key_name` as a key and `ApiKeyDefinition` as a content on the `system` database.

### Example:

{CODE authentication_3@Server/Configuration/Authentication.cs /}

The above example gives a read-only access to `systen` database and `*` (all databases) using the API key: `sample\ThisIsMySecret`. A similar effect can be achieved [using the studio](../../studio/management/api-keys).

Now, to perform any actions against a specified database (`system` database must be declared explicitly), we need to provide the API key.

{CODE authentication_4@Server/Configuration/Authentication.cs /}

## Debugging authentication issues

To grant the ability to resolve authentication issues, we have introduced the `/debug/user-info` endpoint which returns information about the currently authenticated user. The endpoint can be accessed by executing the following code:

{CODE authentication_5@Server/Configuration/Authentication.cs /}

The returned results vary on the current authentication type:  
 
* **Anonymous**      

{CODE-BLOCK:json}
{
    "Remark": "Using anonymous user"
}
{CODE-BLOCK/}

* **Windows Authentication** with full access to all databases:    

{CODE-BLOCK:json}
{
    "Remark": "Using windows auth",
	"User": "RavenUser",
	"IsAdmin": "True"
}
{CODE-BLOCK/}

* **Windows Authentication** with restricted access:   

{CODE-BLOCK:json}
{
    "Remark": "Using windows auth",
	"User": "RavenUser",
	"IsAdmin": "False",
	"AdminDatabases": [],
    "ReadOnlyDatabases": [ "ReadOnlyNorthwind" ],
    "ReadWriteDatabases": [ "ReadWriteNorthwind" ]
}
{CODE-BLOCK/}

* **OAuth Authentication**:    

{CODE-BLOCK:json}
{
    "Remark": "Using OAuth",
	"User": "RavenUser",
	"IsAdmin": "False",
	"TokenBody": "<token_here>"
}
{CODE-BLOCK/}

## Related articles

- [Client API : How to work with authentication?](../../client-api/how-to/work-with-authentication)
- [Studio : Manage Your Server : Server Permissions](../../studio/management/server-permissions)
