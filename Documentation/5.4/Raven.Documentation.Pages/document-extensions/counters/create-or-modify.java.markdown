# Create or Modify Counters
---

{NOTE: }

* Use the `countersFor.increment` method to **create** a new Counter or **modify** an existing Counter's value.  

*  If the Counter exists, `increment` will add the specified number to the Counter's current value.  
   If the Counter doesn't exist, `increment` will create it and set its initial value.  

* For all other `countersFor` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:
  - [`increment` uage](../../document-extensions/counters/create-or-modify#increment-usage)
  - [Example](../../document-extensions/counters/create-or-modify#example)
  - [Syntax](../../document-extensions/counters/create-or-modify#syntax)

{NOTE/}

---

{PANEL: `increment` usage}

**Flow**:  

* Open a session.  
* Create an instance of `countersFor`.  
     * Either pass `countersFor` an explicit document ID, -or-  
     * Pass it an entity tracked by the session, e.g. a document object returned from `session.query` or from `session.load`.  
* Call `countersFor.increment`. 
* Call `session.saveChanges` for the changes to take effect.  

**Note**:  

* Modifying a Counter using `increment` only takes effect when `session.saveChanges()` is executed.  
* To **decrease** a Counter's value, pass the method a negative number to the `increment` method.  

{PANEL/}

{PANEL: Example}

{CODE:java counters_region_Increment@DocumentExtensions\Counters\Counters.java /}

{PANEL/}

{PANEL: Syntax}

{CODE:java Increment-definition@DocumentExtensions\Counters\Counters.java /}

| Parameter        | Type   | Description                                                                                                      |
|------------------|--------|------------------------------------------------------------------------------------------------------------------|
| `counterName`    | String | Counter's name                                                                                                   |
| `incrementValue` | Long   | Increase Counter by this value.<br>Default value is 1.<br>For a new Counter, this number will be its initial value. |

{PANEL/}

## Related articles

**Client-API - Session Articles**:  
[Counters Overview](../../document-extensions/counters/overview)  
[Deleting a Counter](../../document-extensions/counters/delete)  
[Retrieving Counter Values](../../document-extensions/counters/retrieve-counter-values)  
[Counters and other features](../../document-extensions/counters/counters-and-other-features)  
[Counters In Clusters](../../document-extensions/counters/counters-in-clusters)  
