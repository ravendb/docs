# Glossary: FileChangeNotification

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **File** | string | File name |
| **Action** | [FileChangeAction](../glossary/file-change-notification#filechangeaction-enum) | Document change type enum |

<hr />

# FileChangeAction (enum)

### Members

| Name | Value |
| ---- | ----- |
| **Add** | `0` |
| **Delete** | `1` |
| **Update** | `2` |
| **Renaming** | `3` |
| **Renamed** | `4` |

{INFO: Renaming and Renamed}
Renaming - this action is raised for the original file name before a rename operation. <br />
Renamed - this action is raised for the final file name after a rename operation.
{INFO/}

