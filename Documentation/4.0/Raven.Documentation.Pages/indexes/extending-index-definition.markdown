# Extending Index Definition
---

{NOTE: }

* An [index](../indexes/what-are-indexes) is defined with a mapping function that utilizes a LINQ-like syntax.  
  
* Writing a complex LINQ expression can be a non-trivial task.  
  You can extend your indexing processing capabilities by adding custom code to the index definition,  
  by calling custom functions or using external libraries logic (e.g. NodaTime).  

* The indexing process will execute the LINQ code and the invoked additional source code.  

* Adding this custom code can be done from [Studio](../../../todo-upadte-me-later) or from code using the _additional sources_ feature.  
  See [exmple](../indexes/extending-index-definition#including-additional-sources-from-client-code) below.  

* Advantages:
  * **Readability**:   Index work logic is clearer and more readable  
  * **Code usage**:    Code fragments are re-used  
  * **Performance**:   Using the additional source feature can perform better then complex LINQ expressions  
  * **Extendability**: External libraries can be included and used  

{NOTE/}

---

{PANEL: Including additional sources from client code}

* `AdditionalSources` is a property of the `AbstractIndexCreationTask` class.  
* It should be defined in your index class _constructor_ which derives from `AbstractIndexCreationTask`.  
* Example:  

{CODE additional_sources_1@Indexes\AdditionalSources.cs /}

{PANEL/}

{NOTE: Note: Deploying External DLLs}

* External DLLs that are referenced must be _manually deployed_ to the folder containing the Raven.Server executable.  

{NOTE/}
