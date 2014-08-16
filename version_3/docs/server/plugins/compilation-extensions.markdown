# Plugins : Compilation Extensions

There might be a certian situations when users want to put more complex logic to calculate a value of an index entry field. To do this, in RavenDB, we have introduced an `AbstractDynamicCompilationExtension`.

{CODE plugins_6_0@Server\Plugins.cs /}

where:   
* **GetNamespacesToImport** returns a list of namespaces that RavenDB will have to import   
* **GetAssembliesToReference** returns a list of full paths to assemblies    

## Example - Check if a given word is a [palindrome](http://en.wikipedia.org/wiki/Palindrome)

{CODE plugins_6_1@Server\Plugins.cs /}

{CODE plugins_6_2@Server\Plugins.cs /}

Now we can use our `Palindrome` in our index definition.

{CODE plugins_6_3@Server\Plugins.cs /}

#### Related articles

TODO