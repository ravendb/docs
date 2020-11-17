# Indexes: Additional Assemblies

---

{NOTE: }

* Enhance index capabilities by importing assemblies from:  
  * **NuGet**  
  * **Runtime**  
  * **Local file**  

* See Oren Eini's blog for examples of how indexes can be given capabilities like 
[ML image recognition](https://ayende.com/blog/192001-B/using-machine-learning-with-ravendb) or 
[full text searching in Office files](https://ayende.com/blog/192385-A/ravendb-5-1-features-searching-in-office-documents).  

* This is quite similar to the [Additional Sources](../indexes/extending-indexes) feature.  

* In this page:  
  * [Syntax](../indexes/additional-assemblies#syntax)
  * [Examples](../indexes/additional-assemblies#examples)

{NOTE/}

---

{PANEL: Syntax}

Additional assemblies are defined using the `AdditionalAssembly` object.

{CODE-BLOCK: csharp}
public class AdditionalAssembly {

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

#### Importing from NuGet

{CODE:csharp index_3@Indexes/AdditionalAssemblies.cs /}

#### Importing from Runtime

{CODE:csharp index_1@Indexes/AdditionalAssemblies.cs /}

#### Importing from Local Path

{CODE:csharp index_2@Indexes/AdditionalAssemblies.cs /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../indexes/what-are-indexes)  
- [Indexing Basics](../indexes/indexing-basics)  
- [Extending Indexes (Additional Sources)](../indexes/extending-indexes)  
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)  
