# Commands: How to disable caching?

To disable caching in Commands use `DisableAllCaching` method.

## Syntax

{CODE disable_caching_1@ClientApi\Commands\HowTo\DisableCaching.cs /}

| Return Value | |
| ------------- | ----- |
| IDisposable | Method that will re-enable caching when disposed. |

## Example

{CODE disable_caching_2@ClientApi\Commands\HowTo\DisableCaching.cs /}

## Related articles

- [Conventions related to caching](../../configuration/conventions/caching)  
