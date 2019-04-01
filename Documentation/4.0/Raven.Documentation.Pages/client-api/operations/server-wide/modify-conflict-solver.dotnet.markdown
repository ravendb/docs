# Operations: Server: How to Modify a Conflict Solver

The conflict solver allows you to set a conflict resolution script for each collection or resolve conflicts using the latest version. 

To modify the solver configuration, use **ModifyConflictSolverOperation**. 

## Syntax

{CODE solver_1@ClientApi\Operations\Server\ConflictSolver.cs /}

{CODE solver_2@ClientApi\Operations\Server\ConflictSolver.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of a database |
| **collectionByScript** | Dictionary&lt;string,ScriptResolver&gt; | Per collection conflict resolution script |
| **resolveToLatest** | bool | Indicates if a conflict should be resolved using the latest version |


| Return Value | |
| ------------- | ----- |
| **Key** | Name of database |
| **RaftCommandIndex** | RAFT command index |
| **Solver** | Saved conflict solver configuration |

## Example I

{CODE solver_3@ClientApi\Operations\Server\ConflictSolver.cs /}


## Example II

{CODE solver_4@ClientApi\Operations\Server\ConflictSolver.cs /}
