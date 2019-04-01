# Glossary: UserPermission

Holds the user permission info for the database.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **User** | string |The name of the specific database |
| **Database** | DatabaseInfo | Information about the database |
| **Method** | string |HTTP Request Method |
| **IsGranted** | bool | If we have permission to the method |
| **Reason** | string |The Reason we have or not have permission | 
