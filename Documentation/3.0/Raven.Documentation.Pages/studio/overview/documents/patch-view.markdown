# Documents : Patch View

Single documents, entire collections or query results can be patched using this view. More detailed information of JavaScript patching capabilities can be found [here](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents), this article just described Studio side of the patching.

## Action Bar

Action bar contains following buttons:

- Patch type selector (single document, collection or index),
- Test - you can test your patch here, without modifying actual data,
- Save - patch script can be saved for later using this action,
- Load - saved patch scripts can be loaded using this action,
- Patch - execute patch on actual data

![Figure 1. Studio. Patch View. Action Bar.](images/patch-view-action-bar.png)  

## Patch Scripts

After specifying patch type, we need to do one of the following:

- select a document to patch by typing its key,
- another option is to select a collection that we want to patch,
- third option is to select an index and type a query that we want to use for patching

After doing that, we need to type our script (example scripts [here](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents)) with parameters (if needed) and click on `Patch` button to apply it.

![Figure 2. Studio. Patch View. Script.](images/patch-view-script.png)  

## Testing Patch

If you want to test your patch before applying it to real data, you can test it by pressing `Test` button. This will fill up `After Patch` section with patched data and list what documents were putted (`PutDocument`) or loaded (`LoadDocument`) using your script.

![Figure 3. Studio. Patch View. Test.](images/patch-view-test.png)  

## Saving Patches

Each patch can be saved using the `Save` action from `Action Bar` for further use (you need to type its name). To load it just press `Load` button from `Action Bar` and select desired patch.

