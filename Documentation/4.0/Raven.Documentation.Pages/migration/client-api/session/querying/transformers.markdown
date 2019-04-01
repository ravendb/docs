# Migration: How to Migrate Transformers from 3.x

Transformers were removed and substituted by a server-side projection support. Methods like `TransformWith` are no longer available and simple `Select` should be used instead. 

We encourage you to read our article that covers the projection functionality which can be found [here](../../../../client-api/session/querying/how-to-project-query-results).

## Example

| 3.x |
|:---:|
| {CODE transformers_1@Migration\ClientApi\Session\Querying\Transformers.cs /} |
| {CODE transformers_2@Migration\ClientApi\Session\Querying\Transformers.cs /} |

| 4.0 |
|:---:|
| {CODE transformers_3@Migration\ClientApi\Session\Querying\Transformers.cs /} |
