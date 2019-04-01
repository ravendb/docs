# Session: Opening a session

To open synchronous session use `open_session` method from `documentstore`.

## Syntax

{CODE:python open_session_1@ClientApi\Session\OpeningSession.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | str | The name of the database (default value is None)|
| **api_key** | str | The api_key for the database (default value is None)|
| **force_read_from_master** | bool | If set to true force the database to always read from primary database in replication (default value is False)|

| Return Value | |
| ------------- | ----- |
| documentsession | Implements Unit of Work for accessing the RavenDB server |


## Remarks

`with` statement can be used when open a session 

## Related articles

- [What is a session and how does it work?](./what-is-a-session-and-how-does-it-work)  
