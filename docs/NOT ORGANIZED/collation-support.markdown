#Collation support

Raven DB support collations for documents sorting and indexing. You can setup a specific collation for an index field, so you can sort based of culture specific rules.

The following is an example of how to create an index that allow sorting based on the Swedish sorting rules:

    new IndexDefinition
    {
        Map = "from doc in docs select new { doc.Name }",
        SortOptions = {{"Name", SortOptions.String}},
        Analyzers = {{"Name", typeof(SvCollationAnalyzer).AssemblyQualifiedName}}
    }

In general, you can sort using [Two Letters Culture Name]CollationAnalyzer, and all the cultures supported by the .NET framework are supported.