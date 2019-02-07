#Authentication & Authorization

RavenDB comes with a built-in authentication functionality and it supports two types of authentication:    
* [Windows Authentication](authentication/#windows-authentication)   
* [OAuth Authentication](authentication/#oauth-authentication)   

Appropriate authentication type is chosen by examining incoming request headers and by default all actions except read-only are being authenticated. To determine which actions will be authenticated please refer to [Raven/AnonymousAccess](../administration/configuration#authorization--authentication) configuration setting.

##Windows Authentication

When action (request) needs to be authenticated and no other authentication method is detected, then the Windows Authentication is chosen. Worth noting is that all `/admin` endpoint requests are processed using this method.

By default all windows users and groups have access to all the databases, but this can be easily changed by editing `Raven/Authorization/WindowsSettings` document in `system` database. The document consists of list of users and groups that contain the list of accessible databases. For example this document could look like this:

{CODE-START:json /}
{
	"RequiredGroups": [],
	"RequiredUsers": [
	{
		"Name": "IIS AppPool\\DefaultAppPool",
		"Enabled": true,
		"Databases": [
		{
			"Admin": false,
			"TenantId": "ExampleDB",
			"ReadOnly": true
		}
		]
	}
	]
}
{CODE-END /}

Above example gives a read-only access to `ExampleDB` to `IIS AppPool\DefaultAppPool`. Similar effect can be achieved using the Studio and editing `system` database settings.

![Figure 1: `Windows Authentication` settings](images/authentication_1.PNG)

##OAuth Authentication

Second supported authentication type is an [OAuth](https://oauth.net/) authentication and to simplify the process, we have introduced the API key authentication described below.

###Example - API keys

To authenticate the user by using API keys we need to create a document with `Raven/ApiKeys/key_name` as a key and `ApiKeyDefinition` as a content on `system` database.

{CODE authentication_3@Server/Authentication/Index.cs /}

Now to perform any actions against specified database (`system` database must be declared explicitly), we need to provide the API key.

{CODE authentication_4@Server/Authentication/Index.cs /}

##Debugging authentication issues

{NOTE This feature is available in RavenDB 2.0 build 2237 or higher. /}

To grant the ability to resolve authentication issues, we have introduced `/debug/user-info` endpoint that will return information about current authenticated user and it can be accessed by executing the following code:

{CODE authentication_5@Server/Authentication/Index.cs /}

The returned results vary on the current authentication type:  
 
* **Anonymous**      

{CODE-START:json /}
{
    "Remark": "Using anonymous user"
}
{CODE-END /}

* **Windows Authentication** with full access to all databases:    

{CODE-START:json /}
{
    "Remark": "Using windows auth",
	"User": "RavenUser",
	"IsAdmin": "True"
}
{CODE-END /}

* **Windows Authentication** with restricted access:   

{CODE-START:json /}
{
    "Remark": "Using windows auth",
	"User": "RavenUser",
	"IsAdmin": "False",
	"AdminDatabases": [],
    "ReadOnlyDatabases": [ "ExampleReadOnlyDB" ],
    "ReadWriteDatabases": [ "ExampleReadWriteDB" ]
}
{CODE-END /}

* **OAuth Authentication**:    

{CODE-START:json /}
{
    "Remark": "Using OAuth",
	"User": "RavenUser",
	"IsAdmin": "False",
	"TokenBody": "<token_here>"
}
{CODE-END /}