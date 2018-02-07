# Operations : Server : How to Modify Conflict Solver?

Conflict solver allows to set conflict resolution script per each collection or resolve conflict using latest version. 
To modify solver configuration use **ModifyConflictSolverOperation**. 

## Syntax

{CODE solver_1@ClientApi\Operations\Server\ConflictSolver.cs /}

{CODE solver_2@ClientApi\Operations\Server\ConflictSolver.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of a database |
| **collectionByScript** | Dictionary&lt;string,ScriptResolver&gt; | Per collection conflict resolution script |
| **resolveToLatest** | bool | Indicates if conflict should be resolved using latest version |


| Return Value | |
| ------------- | ----- |
| **Key** | Name of database |
| **RaftCommandIndex** | Raft command index |
| **Solver** | Saved conflict solver configuration |

## Example I

{CODE solver_3@ClientApi\Operations\Server\ConflictSolver.cs /}


## Example II

{CODE solver_4@ClientApi\Operations\Server\ConflictSolver.cs /}
