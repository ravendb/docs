# Session: Storing entities in session

To store entities inside session object use the `store` method.
{CODE:python store_entities_1@ClientApi\Session\StoringEntities.py /}

## Syntax

Extracts Id from entity using Conventions or generates new one if it is not available.
{CODE:python store_entities_2@ClientApi\Session\StoringEntities.py /}
Extracts Id from entity using Conventions or generates new one if it is not available and forces concurrency check with given etag.
{CODE:python store_entities_3@ClientApi\Session\StoringEntities.py /}
Stores entity in session with given id.
{CODE:python store_entities_4@ClientApi\Session\StoringEntities.py /}
Stores entity in session with given id and forces concurrency check with given etag
{CODE:python store_entities_5@ClientApi\Session\StoringEntities.py /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be stored |
| **key** | str | Entity will be stored under this key, (`None` to generate automatically) |
| **etag** | str | Current entity etag, used for concurrency checks (`None` to skip check) |
| **force_concurrency_check** | bool | Force concurrency check (`False` as a default value) |

## Example

{CODE:python store_entities_6@ClientApi\Session\StoringEntities.py /}

## Related articles

- [Saving changes](./saving-changes)  
