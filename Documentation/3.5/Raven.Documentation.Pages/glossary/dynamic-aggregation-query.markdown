# Glossary: DynamicAggregationQuery

### Methods

| Signature | Description |
| ----------| ----- |
| **DynamicAggregationQuery&lt;TResult&gt; AndAggregateOn(Expression&lt;Func&lt;TResult, object&gt;&gt; path, string displayName = null)** |  Allows to specify additional data break down during aggregation |
| **DynamicAggregationQuery&lt;TResult&gt; AndAggregateOn(string path, string displayName = null)** | Allows to specify additional data break down during aggregation |
| **DynamicAggregationQuery&lt;TResult&gt; AddRanges(params Expression&lt;Func&lt;TResult, bool&gt;&gt;[] paths)** | Adds ranges |
| **DynamicAggregationQuery&lt;TResult&gt; MaxOn(Expression&lt;Func&lt;TResult, object&gt;&gt; path)** | Performs maximum aggregation |
| **DynamicAggregationQuery&lt;TResult&gt; MinOn(Expression&lt;Func&lt;TResult, object&gt;&gt; path)** | Performs minimum aggregation |
| **DynamicAggregationQuery&lt;TResult&gt; SumOn(Expression&lt;Func&lt;TResult, object&gt;&gt; path)** | Performs sum aggregation |
| **DynamicAggregationQuery&lt;TResult&gt; AverageOn(Expression&lt;Func&lt;TResult, object&gt;&gt; path)** | Performs average aggregation |
| **DynamicAggregationQuery&lt;TResult&gt; CountOn(Expression&lt;Func&lt;TResult, object&gt;&gt; path)** | Performs count aggregation |
| **[FacetResults](../glossary/facet-results) ToList()** | Executes dynamic aggregation and returns results as facets |
| **Lazy&lt;[FacetResults](../glossary/facet-results)&gt; ToListLazy()** | Schedules dynamic aggregation in lazy fashion |
| **Task&lt;[FacetResults](../glossary/facet-results)&gt; ToListAsync()** | Asynchronously executes dynamic aggregation and returns results as facets |
