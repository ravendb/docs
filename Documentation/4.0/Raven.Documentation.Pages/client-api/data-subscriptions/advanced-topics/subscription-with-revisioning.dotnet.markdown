# Revisions support

---

{NOTE: }

Data subscription supports subscribing not only on documents, but also on all their revisions.  
Revision support should be defined in the subscription, it also requires revisions to be configured on the collection in question.  
While regular subscriptions process single documents, subscription on documents revisions processes pairs of subsequent document revisions.  
Such functionality allows keeping track of each change that was performed on a document, and even to compare two subsequent versions of a document.  
Both document revisions are accessible in the filtering and the projection process.

In this page:  
[Revisions processing](#revisions-processing-order)  
[Simple declaration and usage](#simple-declaration-and-usage)   
[Revisions processing and projection](#revisions-processing-and-projection)  

{NOTE/}

---

{PANEL:Revisions processing order}
Documents revisions will be processed in pairs, meaning if a document was changed 6 times in a row, subscription will process 6 times 6 pairs of versions of that document.
{WARNING For the subscription revisions to work properly, it's crucial to make sure that the revisions configuration stores documents revisions enough time, without discarding unprocessed revisions/}
{PANEL/}

{PANEL:Simple declaration and usage}
Here we declare a simple revisions subscription, that will send pairs of subsequent document revisions to the client

Creation:
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_simple_revisions_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_simple_revisions_subscription_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

Consumption:
{CODE use_simple_revision_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{PANEL/}

{PANEL:Revisions processing and projection}
Here we declare a revisions subscription, that will filter and project data from revisions pairs:

Creation:
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_projected_revisions_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_projected_revisions_subscription_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

Consumption:
{CODE use_simple_revision_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{PANEL/}

## Related articles

- [What are data subscriptions?](../what-are-data-subscriptions)
- [How to **consume** a data subscription?](../subscription-consumption/how-to-consume-data-subscription)
- [How to **create** a data subscription?](../subscription-creation/how-to-create-data-subscription)
