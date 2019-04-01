# Commands: Patches: How to use JavaScript to patch your documents?

**Patch** command is used to perform partial document updates without having to load, modify, and save a full document. This is usually useful for updating denormalized data in entities.

## Syntax

{CODE:java patch_1@ClientApi\Commands\Patches\Javascript.java /}

**Parameters**

{CODE:java scriptedpatchrequest@Common.java /}

## Methods, objects and variables

Before we will move to the examples, let's look at the methods, objects, and variables available:

| ------ |:------:| ------ |
| `__document_id` | variable | Id for current document |
| `this` | object | Current document (with metadata) |
| `LoadDocument(key)` | method | Allows document loading, increases maximum number of allowed steps in script. See `Raven/AdditionalStepsForScriptBasedOnDocumentSize` [here](../../../server/configuration/configuration-options#javascript-parser). |
| `PutDocument(key, data, metadata)` | method | Allows document putting, returns generated key |
| `IncreaseNumberOfAllowedStepsBy(number)` | method | Will increase the maximum allowed number of steps in script by given value. Only available if `Raven/AllowScriptsToAdjustNumberOfSteps` is set to `true`. |
| `_` | object | [Lo-Dash 2.4.1](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md) |
| `trim()` | string.prototype | trims the string e.g. `this.FirstName.trim()` |
| `output(...)` | method | Allows debug your patch, prints passed messages in output tab |
| `indexOf(...)` | Array.prototype | wrapper for [_.indexOf](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md#_indexofarray-value-fromindex0) |
| `filter(...)` | Array.prototype | wrapper for [_.filter](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md#_filtercollection-callbackidentity-thisarg) |
| `Map(...)` | Array.prototype | wrapper for [_.map](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md#_mapcollection-callbackidentity-thisarg) |
| `Where(...)` | Array.prototype | wrapper for [_.filter](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md#_filtercollection-callbackidentity-thisarg) |
| `RemoveWhere(...)` | Array.prototype | wrapper for [_.remove](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md#_removearray-callbackidentity-thisarg) returning Array for easier chaining |
| `Remove(...)` | Array.prototype | wrapper for [_.pull](https://github.com/lodash/lodash/blob/2.4.1/doc/README.md#_pullarray-value) returning Array for easier chaining |

## Custom functions

Beside built-in functions, custom ones can be introduced. Please visit [this](../../../studio/overview/settings/custom-functions) page if you want to know how to add custom functions.

## Example I

{CODE:java patch_2@ClientApi\Commands\Patches\Javascript.java /}

## Example II

{CODE:java patch_3@ClientApi\Commands\Patches\Javascript.java /}

## Example III

{CODE:java patch_4@ClientApi\Commands\Patches\Javascript.java /}

## Example IV

{CODE:java patch_5@ClientApi\Commands\Patches\Javascript.java /}

## Example V

{CODE:java patch_6@ClientApi\Commands\Patches\Javascript.java /}

## Example VI

{CODE:java patch_7@ClientApi\Commands\Patches\Javascript.java /}

## Example VII

{CODE:java patch_8@ClientApi\Commands\Patches\Javascript.java /}

## Example VIII

{CODE:java patch_9@ClientApi\Commands\Patches\Javascript.java /}

## Example IX

{CODE:java patch_1_0@ClientApi\Commands\Patches\Javascript.java /}

## Example X

{CODE:java patch_1_1@ClientApi\Commands\Patches\Javascript.java /}

## Example XI

{CODE:java patch_1_2@ClientApi\Commands\Patches\Javascript.java /}

## Related articles

- [How to work with **patch requests**?](../../../client-api/commands/patches/how-to-work-with-patch-requests) 
- [Studio : How to add **custom functions**?](../../../studio/overview/settings/custom-functions)
