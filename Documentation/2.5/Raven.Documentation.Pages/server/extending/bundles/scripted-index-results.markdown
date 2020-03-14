# Bundle: Scripted Index Results

Scripted Index Results bundle allows you to attach scripts to indexes. Those scripts can operate on the results of the indexing. It opens a lot of opportunities like a modification
of documents by index calculated values or recursive map/reduce indexes.

In order to enable this bundle you need to add `ScriptedIndexResults` to `Raven/ActiveBundles` setting of a database document when a database is created (or via the studio):

{CODE activate_bundle@Server\Extending\Bundles\ScriptedIndexResults.cs /}

The activation of the bundle adds a database index update trigger which is run when an index entry is created or deleted. In order to take advantage of this feature for a selected index
you need to put a special setup document under key `Raven/ScriptedIndexResults/[IndexName]` that will contain the appropriate scripts to apply:

{CODE setup_doc@Server\Extending\Bundles\ScriptedIndexResults.cs /}

## Example

Let us assume that we have the following index:

{CODE index_def@Server\Extending\Bundles\ScriptedIndexResults.cs /}

Now we want to embed the reduced values inside the company document, so let's create the setup document:

{CODE sample_setup_doc@Server\Extending\Bundles\ScriptedIndexResults.cs /}

Since this document is stored in the database every time when _Orders/ByCompany_ index creates a new index entry then _IndexScript_ will be applied to reduce result. Under
`this` keyword in the _IndexScript_ script you have an access to the _Company_ , _Count_ and _Total_ values.  As you can see the script uses the built-in `LoadDocument` and `PutDocument` functions
in order to modify a company document. Note that we need to ensure that if the index entry is deleted we will revert the changes by using the _DeleteScript_ script. Notice that we no longer have access under _this_ to our calculated values, the only available variable is 'key' which is a document key.

Now if you take a look at the documents from the companies collection after orders indexation then you will see the added values. For example:

{CODE-START:json /}
	{ 
		"Id" : "companies/1", 
		...
		"Orders" : {
			"Count" : 7,
			"Total" : 1234
		}
	}
{CODE-END /}