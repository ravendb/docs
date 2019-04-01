# Commands: How to disable caching?

To disable caching in Commands use `disableAllCaching` method.

## Syntax

{CODE:java disable_caching_1@ClientApi\Commands\HowTo\DisableCaching.java /}

| Return Value | |
| ------------- | ----- |
| AutoCloseable | Method that will re-enable caching when disposed. |

## Example

{CODE:java disable_caching_2@ClientApi\Commands\HowTo\DisableCaching.java /}

## Related articles

- [Conventions related to caching](../../configuration/conventions/caching)  
