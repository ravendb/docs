
# RQL Code Assistance

---

{NOTE: }

* [RQL](../../indexes/querying/what-is-rql) is a much richer and more versatile language than the small set 
  of querying instructions that many users have learned to get by with.  
  Studio's **RQL Code Assistance** allows you to invoke an **auto-completion** 
  tool while writing your code to see the various paths your query can take, 
  and continuously checks your **syntax** and highlights potential errors 
  to help you avoid them.  

* RQL code assistance's auto-completion tool presents data entities, functions, 
  keywords, and operators that can be used by the query part your cursor is at.  
  The options it suggests can be selected and added to the query, 
  speeding up the writing process and making the creation of intelligent 
  queries an enjoyable task.  

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

* The RQL auto-completion tool can be used in these views:  
   - The [Query View](../../studio/database/queries/query-view#query-view)  
   - The [Patch View](../../studio/database/documents/patch-view#patch-view)  
   - The [Subscription Task View](../../studio/database/tasks/ongoing-tasks/subscription-task#subscription-task-definition)  
* To invoke auto-completion, click **CTRL+Space** while writing RQL.  
* Options relevant for the query section that your cursor is placed at will be listed.  
* Clicking a list item will add it to the query.  
  
---

**At the very beginning of a query**, the suggested options will be the 
most basic ones: [from](../../indexes/querying/what-is-rql#from) (to select 
a collection), `from index` (to select an index to query on), and 
[declare function](../../indexes/querying/what-is-rql#declare) (to 
create a JavaScript function).  

!["Click CTRL+Space To See Available Options"](images/code-assistance-1.png "Click CTRL+Space To See Available Options")

1. **The list of available options**  
2. **An available option**  
3. The **Option type**, which can be -  
    * A Collection  
    * A Document Field  
    * A Function  
    * A Keyword  
    * An Operator  

---

**As the query evolves**, the auto-completion tool will suggest new options.  

!["Evolving Query"](images/code-assistance-2.png "Evolving Query")

{INFO: For the queries shown above, auto-completion presents: }

1. The list of **collections** that can be queried   
2. The list of **keywords** that can be used with the selected collection  
3. The list of **document fields** that can be queried  
4. The list of **operators** and **keywords** that can be used  

{INFO/}

---

### Syntax Verification

* As you write your query, its syntax is continuously verified and errors are detected.  
* When an error is detected, an error indicator is displayed on the left side of the edit box.  
* Hovering over the error indicator shows the error details.  

!["Syntax Verification"](images/code-assistance-3.png "Syntax Verification")

1. a **Syntax error**  
2. the **Error indicator**  
3. **Error details**  
   Hovering over the error indicator opens a pop-up that specifies the error details.  

{PANEL/}

## Related Articles

### Studio

- [Query View](../../studio/database/queries/query-view)  
- [Patch View](../../studio/database/documents/patch-view#patch-view)  
- [Subscription Task](../../studio/database/tasks/ongoing-tasks/subscription-task)  

### RQL

- [What is RQL](../../indexes/querying/what-is-rql)  
