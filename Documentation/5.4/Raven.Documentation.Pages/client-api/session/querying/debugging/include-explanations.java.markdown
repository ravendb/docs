# Include Query Explanations

`includeExplanations` allows to investigate details related to score assigned for each query result.

{INFO: }

* Including explanations is available only when using **Lucene** as the underlying search engine.  
* You can configure which search engine will be used. Learn how in [Selecting the search engine](../../indexes/search-engine/corax#selecting-the-search-engine).

{INFO/}

---

## Syntax


{CODE:java explain_1@ClientApi\Session\Querying\Debugging\IncludeExplanations.java /}

## Example

{CODE-TABS}
{CODE-TAB:java:Query explain_2@ClientApi\Session\Querying\Debugging\IncludeExplanations.java /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products" 
where search(Name, "Syrup")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

<hr />
Sample explanation:

```
4.650658 = (MATCH) fieldWeight(search(Name):syrup in 2), product of:
  1 = tf(termFreq(search(Name):syrup)=1)
  4.650658 = idf(docFreq=1, maxDocs=77)
  1 = fieldNorm(field=search(Name), doc=2)
```
