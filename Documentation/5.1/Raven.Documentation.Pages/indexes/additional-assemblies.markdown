# Indexes: Additional Assemblies

---

{NOTE: }

* Index capabilities can now be expanded by importing whole libraries 
with useful classes and methods that can then be used in the index syntax. 
**Additional Assemblies** makes it very easy to import assemblies from:  
  * **NuGet**  
  * **Runtime**  
  * **Local file**  

* Indexes can be enhanced with capabilities like [machine learning image recognition](https://ayende.com/blog/192001-B/using-machine-learning-with-ravendb) or 
[full text searching in Office files](https://ayende.com/blog/192385-A/ravendb-5-1-features-searching-in-office-documents).  

* This is similar to the [Additional Sources](../indexes/extending-indexes) feature, 
through which you can add methods and classes to an index in the form of a file or 
pure text. These two features can be used together: an index's Additional Sources 
code has access to all of the index's Additional Assemblies.  

* In this page:  
  * [Syntax](../indexes/additional-assemblies#syntax)
  * [Examples](../indexes/additional-assemblies#examples)

{NOTE/}

---

{PANEL: Syntax}

Additional assemblies are defined using the `AdditionalAssembly` object.

{CODE-BLOCK: csharp}
public class AdditionalAssembly
{
    public static AdditionalAssembly FromRuntime(string assemblyName, 
                                                 HashSet<string> usings = null);

    public static AdditionalAssembly FromPath(string assemblyPath, 
                                              HashSet<string> usings = null);

    public static AdditionalAssembly FromNuGet(string packageName, 
                                               string packageVersion, 
                                               string packageSourceUrl = null, 
                                               HashSet<string> usings = null);
}
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **assemblyName** | `string` | The name of an assembly to import from runtime |
| **assemblyPath** | `string` | Local path to an assembly |
| **packageName** | `string` | Name of a NuGet package to import |
| **packageVersion** | `string` | The version number of the NuGet package - optional |
| **packageSourceUrl** | `string` | The URL of the package's original source - optional |
| **usings** | `HashSet<string>` | A set of namespaces to attach to the compiled index with `using` - optional |

---

The `AdditionalAssembly`s are collected in `AdditionalAssemblies`, a property of 
`IndexDefinition`:  

{CODE-BLOCK: csharp}
public HashSet<AdditionalAssembly> AdditionalAssemblies;
{CODE-BLOCK/}

{PANEL/}

{PANEL: Examples}

#### Basic example

This index is able to use the method `GetFileName()` from the class `Path` 
because the namespace `System.IO` has been imported as an additional assembly. 
It takes a `string` file path and retrieves just the file name and extension.  

{CODE:csharp simple_index@Indexes/AdditionalAssemblies.cs /}

#### Complex example

This index uses a machine learning algorithm imported from NuGet that can 
recognize the contents of images and classify them with an appropriate tag. 
These tags are then stored in the index just like any other term.  

{CODE:csharp complex_index@Indexes/AdditionalAssemblies.cs /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../indexes/what-are-indexes)  
- [Indexing Basics](../indexes/indexing-basics)  
- [Extending Indexes (Additional Sources)](../indexes/extending-indexes)  
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)  
