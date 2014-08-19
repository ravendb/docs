# Transforming hierarchical data

When our data is structured hierarchically and we want to extract data from e.g. our parent records we can use `Recurse` method within our transformer projection function.

{INFO `Recurse` requires us to have a single document or collection of documents under specified property, but when we have only an Id or array of Ids available, we can mix `Recurse` with `LoadDocument` (check Example II). /}

## Example I

Let's assume that we have a following structure:

{CODE transformers_4@Transformers/Recurse.cs /}

All comment authors for each `BlogPost` can be extracted using `Recurse` in the following way:

{CODE transformers_5@Transformers/Recurse.cs /}

{CODE transformers_6@Transformers/Recurse.cs /}

## Example II

Let's assume that we have a following `Category` where we store Id of a parent record:

{CODE transformers_1@Transformers/Recurse.cs /}

Now, to extract category name and names of all parent categories and names of all parent categories for those parents and so on, we can mix our `Recurse` method with `LoadDocument`.

{CODE transformers_2@Transformers/Recurse.cs /}

{CODE transformers_3@Transformers/Recurse.cs /}

## Related articles

- [Loading documents](../transformers/loading-documents)
- [Passing parameters](../transformers/passing-parameters)
- [Nesting transformers](../transformers/nesting-transformers)
