
# RQL Code Assistance

---

{NOTE: }

* All queries addressed to the RavenDB server use [RQL](../../indexes/querying/what-is-rql), 
  RavenDB's rich query language.  
* The **query structure** exposes the **order of the query operations** 
  handled by the RavenDB server when serving a query.  
* **RQL Code Assistance** is available when writing any query in the Studio.  
  The code auto-completion allows you to see the various paths your query 
  can take as data entities, functions, keywords, and operators are presented 
  for you to select, speeding up the writing process and making the creation 
  of intelligent queries an enjoyable task.  
* In addition, the query syntax is checked and potential errors are highlighted.  
* Code Assistance is available in the [Query](../../studio/database/queries/query-view#query-view), 
  [Patch](../../studio/database/documents/patch-view#patch-view), 
  and [Subscription Task Definition](../../studio/database/tasks/ongoing-tasks/subscription-task#subscription-task-definition) 
  views.  

* In this page:  
  * [Auto-Completion](../../studio/database/code-assistance#auto-completion)  
  * [Syntax Verification](../../studio/database/code-assistance#syntax-verification)  
{NOTE/}

---

{PANEL: Auto-Completion}

* RQL code assistance is available in the following views:  
   - [Query View](../../studio/database/queries/query-view#query-view)  
   - [Patch View](../../studio/database/documents/patch-view#patch-view)  
   - [Subscription Task View](../../studio/database/tasks/ongoing-tasks/subscription-task#subscription-task-definition)  
* To invoke auto-completion, click **CTRL+Space** while writing RQL.  
* The relevant options for the current cursor location will be listed.  
* Clicking a list item will add it to the query.  
  
---

**At the very beginning of a query**, the suggested options will be the 
most basic ones:  

* [from](../../indexes/querying/what-is-rql#from) - to select a collection to query on  
* `from index` - to select an index to query on  
* [declare function](../../indexes/querying/what-is-rql#declare) - to create a JavaScript function  

!["Click CTRL+Space To See Available Options"](images/code-assistance-1.png "Click CTRL+Space To See Available Options")

1. **List of available options**  
2. **An available option**  
3. **Option type**, which can be -  
    * A Collection  
    * A Document Field  
    * A Function  
    * A Keyword  
    * An Operator  

---

**As the query evolves**, new options that are are relevant to the current code are suggested.  

!["Evolving Query"](images/code-assistance-2.png "Evolving Query")

{INFO: For the collection query shown above, auto-completion presents: }

1. The list of **collections** that can be queried   
2. The list of **keywords** that can be used with the selected collection  
3. The list of **document fields** that can be queried  
4. The list of **operators** and **keywords** that can be used  

{INFO/}

---

### Syntax Verification

* As you write your query, its syntax is continuously verified and errors are detected.  
* When an error is detected, a warning sign is displayed on the left side of the editor.  
* Hovering over the warning sign shows the error details.  

!["Syntax Verification"](images/code-assistance-3.png "Syntax Verification")

1. **Syntax error** (this symbol cannot be used here)  
2. **Warning Sign**  
3. **Error details**  
   Hover over the warning sign to see the error details.  

{PANEL/}

## Related Articles

### Studio

- [Query View](../../studio/database/queries/query-view)  
- [Patch View](../../studio/database/documents/patch-view#patch-view)  
- [Subscription Task](../../studio/database/tasks/ongoing-tasks/subscription-task)  

### RQL

- [What is RQL](../../indexes/querying/what-is-rql)  
