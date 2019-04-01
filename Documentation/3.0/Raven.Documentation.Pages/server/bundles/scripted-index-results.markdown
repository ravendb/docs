# Bundle: Scripted Index Results

Scripted Index Results bundle allows you to attach scripts to indexes. Those scripts can operate on the results of the indexing. This creates new opportunities, such as modification
of documents by index calculated values or recursive map/reduce indexes.

In order to enable this bundle, you need to add  the `ScriptedIndexResults` to the `Raven/ActiveBundles` while setting off a database document when a database is created (or via the Studio):

{CODE activate_bundle@Server\Bundles\ScriptedIndexResults.cs /}

The activation of the bundle adds a database index update trigger which is run when an index entry is created or deleted. In order to take advantage of this feature for a selected index,
you need to put a special set up document under the key `Raven/ScriptedIndexResults/[IndexName]` that will contain the appropriate scripts to apply:

{CODE setup_doc@Server\Bundles\ScriptedIndexResults.cs /}

## Example I - basics

Let us assume that we have the following index:

{CODE index_def@Server\Bundles\ScriptedIndexResults.cs /}

Now we want to embed the reduced values inside the company document, so let's create the setup document:

{CODE sample_setup_doc@Server\Bundles\ScriptedIndexResults.cs /}

Since this document is stored in the database every time the _Orders/ByCompany_ index creates a new index entry, the _IndexScript_ will be applied to reduce results. 
Under `this` keyword in the _IndexScript_ and _DeleteScript_ script you have an access to the Lucene document stored in index ( _Company_ , _Count_ , and _Total_ values).  As you can see, the script uses the built-in `LoadDocument` and `PutDocument` functions in order to modify a company document. Note that we need to ensure that if the index entry is deleted, we will revert the changes by using the _DeleteScript_ script. Notice that we no longer have access to our calculated values under _this_, and the only available variable is a 'key', which is a document key.

Now, if you take a look at the documents from the companies collection after orders indexation, you will see the added values. For example:

{CODE-BLOCK:json}
{ 
	"Id" : "companies/1", 
	...
	"Orders" : {
		"Count" : 7,
		"Total" : 1234
	}
}
{CODE-BLOCK/}

## Example II - AbstractScriptedIndexCreationTask

For easier configuration we have created `AbstractScriptedIndexCreationTask` where you can specify both, index definition and scripted index setup document. Each time the task is executed, it will update (if needed) stored index definition, stored setup document and reset index if any of those changed.

{CODE index_def_2@Server\Bundles\ScriptedIndexResults.cs /}
