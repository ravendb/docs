# Glossary: UserInfo

Holds the user info for the database.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Remark** | string | Inforamtion about the type of user |
| **User** | string | The specific Name of the user |
| **IsAdminGlobal** | bool | Server administrator privileges(global) |
| **IsAdminCurrentDb** | bool | If using current admin db |
| **Databases** | List<DatabaseInfo> | Return a list of all the database on the server |
| **Principal** | IPrincipal | |
| **AdminDatabases** | HashSet<string> | Return a HashSet of all the admin database on the server |
| **ReadOnlyDatabases** | HashSet<string> |Return a HashSet of all the ReadOnly database on the server |
| **ReadWriteDatabases** | HashSet<string> | Return a HashSet of all the ReadWrite database on the server |
| **AccessTokenBody** | AccessTokenBody | Return the access token body |
