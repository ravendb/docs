# Glossary: ConflictItem

RavenFS client API class that represents a synchronization conflict.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **FileName** | string | The file name |
| **RemoteServerUrl** | string | The remote (source) file system URL |
| **CurrentHistory** | IList&lt;HistoryItem&gt;  | The history of conflicted file on the destination side |
| **RemoteHistory** | IList&lt;HistoryItem&gt; |  The history of file on the source side that was attempted to be synchronized |
