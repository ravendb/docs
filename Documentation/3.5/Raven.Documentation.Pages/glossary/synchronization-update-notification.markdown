# Glossary: SynchronizationUpdateNotification

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **File** | string | The file name |
| **DestinationFileSystemUrl** | string | URL of a destination file system |
| **SourceFileSystemUrl** | string | URL of a source file system |
| **SourceServerId** | Guid | The identifier of a source file system |
| **Type** | [SynchronizationType](../glossary/synchronization-update-notification#synchronizationtype-enum) | The synchronization type enum |
| **Action** | [SynchronizationAction](../glossary/synchronization-update-notification#synchronizationaction-enum) | The synchronization action type enum |
| **Direction** | [SynchronizationDirection](../glossary/synchronization-update-notification#synchronizationdirection-enum) | The synchronization direction type enum |


<hr />

# SynchronizationType (enum)

### Members

| Name | Value |
| ---- | ----- |
| **ContentUpdate** | `1` |
| **MetadataUpdate** | `2` |
| **Rename** | `3` |
| **Renamed** | `4` |

# SynchronizationAction (enum)

### Members

| Name | Value |
| ---- | ----- |
| **Enqueue** | `0` |
| **Start** | `1` |
| **Finish** | `2` |

# SynchronizationDirection (enum)

### Members

| Name | Value |
| ---- | ----- |
| **Outgoing** | `0` |
| **Incoming** | `1` |

