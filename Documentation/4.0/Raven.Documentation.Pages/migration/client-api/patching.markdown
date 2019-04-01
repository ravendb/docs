# Migration: Patching

This article describes the differences between 3.x and 4.0. It's primarily focused on the JavaScript capabilities. If you are interested only in Client API changes, please visit [this article](../../client-api/operations/patching/single-document).

{PANEL:JavaScript Interpreter}

The [Jint](https://github.com/sebastienros/jint), a JavaScript interpreter used to perform JS operations (like patching), was updated from version 2 to version 3, which supports wider range of capabilities (e.g. ECMAScript 5.1 with some 6.0 features like arrow functions).

{PANEL/}

{PANEL:Removed Libraries}

### Lodash

Due to performance reasons (parsing time), [Lodash](https://lodash.com/) is no longer included as a part of every patch.

### RavenDB JavaScript extensions

All of the non-standard methods like `RemoveWhere`, `Map` (full list available [here](https://github.com/ravendb/ravendb/blob/v3.5/Raven.Database/Json/RavenDB.js)) were also removed due to additional parsing time needed to perform JS operations. Most of them can be easily substituted with ECMAScript 5.1 or 6 capabilities built-in in the Jint engine (check examples).

{PANEL/}

{PANEL:Custom Functions}

The ability to write your own custom functions and share them between patches has been dropped. Functions can be inlined inside a patching script and will work.

{PANEL/}

{PANEL:Example I}

| 3.x |
|:---:|
| {CODE-BLOCK:javascript}
var item = _.find(myArray, function(item) {
    return item.Id == args.Id;
});
{CODE-BLOCK/} 

| 4.0|
|:---:|
| {CODE-BLOCK:javascript}
var item = myArray.filter(a => a.Id == args.Id)[0];
{CODE-BLOCK/} |

{PANEL/}

{PANEL:Example II}

| 3.x |
|:---:|
| {CODE-BLOCK:javascript}
_.forEach(myArray, function(item) {
    item.Name = args.Name;
    item.City = args.City;
    item.Mascot = args.Mascot;
});
{CODE-BLOCK/} 

| 4.0|
|:---:|
| {CODE-BLOCK:javascript}
for(var i = 0; i < myArray.length; i++)
{
    var item = myArray[i];
    item.Name = args.Name;
    item.City = args.City;
    item.Mascot = args.Mascot; 
}
{CODE-BLOCK/} |

{PANEL/}

{PANEL:Example III}

| 3.x |
|:---:|
| {CODE-BLOCK:javascript}
this.MyArray.RemoveWhere(function(item) {
    return item.Id == args.Id;
});
{CODE-BLOCK/} 

| 4.0|
|:---:|
| {CODE-BLOCK:javascript}
this.MyArray = this.MyArray.filter(i => i.Id != args.Id);
{CODE-BLOCK/} |

{PANEL/}

{PANEL:Example IV}

| 3.x |
|:---:|
| {CODE-BLOCK:javascript}
var myArray = this.AllItems.Where(function(item) {
    return item.Id == args.Id;
});
{CODE-BLOCK/} 

| 4.0|
|:---:|
| {CODE-BLOCK:javascript}
var myArray = this.MyArray.filter(i => i.Id == args.Id);
{CODE-BLOCK/} |

{PANEL/}

## Related Articles

- [Client API : Patching : Single Document](../../client-api/operations/patching/single-document)
- [Client API : Patching : Set Based](../../client-api/operations/patching/set-based)
