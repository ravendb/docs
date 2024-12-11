import {
    AbstractIndexCreationTask,
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    IndexDefinitionBuilder, PutAnalyzersOperation, PutServerWideAnalyzersOperation
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class SnowballAnalyzer { }

//region setting_analyzers_1
class BlogPosts_ByTagsAndContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Posts.Select(post => new {     
            tags = post.tags,
            content = post.content 
        })`;

        // Field 'tags' will be tokenized by Lucene's SimpleAnalyzer
        this.analyze("tags", "SimpleAnalyzer");

        // Field 'content' will be tokenized by the Custom analyzer SnowballAnalyzer
        this.analyze("content", "Raven.Sample.SnowballAnalyzer");
    }
}
//endregion

//region use_exact_analyzer
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Employees.Select(employee => new { " +
            "    LastName = employee.LastName, " +
            "    FirstName = employee.FirstName " +
            "})";

        // Set the Indexing Behavior:
        // ==========================

        // Set 'Exact' on index-field 'FirstName'
        this.index("FirstName", "Exact");

        // => Index-field 'FirstName' will be processed by the "Default Exact Analyzer"
    }
}
//endregion

//region use_search_analyzer
class BlogPosts_ByContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Posts.Select(post => new { " +
            "    tags = post.tags, " +
            "    content = post.content " +
            "})";

        // Set the Indexing Behavior:
        // ==========================

        // Set 'Search' on index-field 'content'
        this.index("content", "Search");

        // => Index-field 'content' will be processed by the "Default Search Analyzer"
        //    since no other analyzer is set.
    }
}
//endregion

//region use_default_analyzer
class Employees_ByFirstName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Employees.Select(employee => new { " +
            "    LastName = employee.LastName" +
            "})";

        // Index-field 'LastName' will be processed by the "Default Analyzer"
        // since:
        // * No analyzer was specified
        // * No Indexing Behavior was specified (neither Exact nor Search)
    }
}
//endregion

//region no_indexing
class BlogPosts_ByTitle extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Posts.Select(post => new { " +
            "    tags = post.tags, " +
            "    content = post.content " +
            "})";

        // Set the Indexing Behavior:
        // ==========================

        // Set 'No' on index-field 'content'
        this.index("content", "No");

        // Set 'Yes' to store the original content of field 'content' in the index
        this.store("content", "Yes");

        // => No analyzer will process field 'content',
        //    it will only be stored in the index.
    }
}
//endregion

async function analyzers() {
    const session = store.openSession();

    //region setting_analyzers_2
    const builder = new IndexDefinitionBuilder("BlogPosts/ByTagsAndContent");
    builder.map = `docs.Posts.Select(post => new {     
        tags = post.tags,     
        content = post.content 
    })`;
    builder.analyzersStrings["tags"] = "SimpleAnalyzer";
    builder.analyzersStrings["content"] = "Raven.Sample.SnowballAnalyzer";

    await store.maintenance
        .send(new PutIndexesOperation(
            builder.toIndexDefinition(store.conventions)));
    //endregion
}

let analyzerDefinition;
{
    //region put_analyzers_1
    await store.maintenance.send(new PutAnalyzersOperation(analyzerDefinition));
    //endregion
}

{
    //region put_analyzers_2
    await store.maintenance.send(new PutServerWideAnalyzersOperation(analyzerDefinition));
    //endregion
}

{
    //region put_analyzers_3
    const analyzerDefinition = {
        name: "analyzerName",
        code: "code"
    };
    //endregion
}

{
    //region my_custom_analyzer_example
    const analyzerDefinition = {
        name: "MyAnalyzer",
        code: "using System.IO;\n" +
            "using Lucene.Net.Analysis;\n" +
            "using Lucene.Net.Analysis.Standard;\n" +
            "\n" +
            "namespace MyAnalyzer\n" +
            "{\n" +
            "   public class MyAnalyzer : Lucene.Net.Analysis.Analyzer\n" +
            "    {\n" +
            "        public override TokenStream TokenStream(string fieldName, TextReader reader)\n" +
            "        {\n" +
            "            throw new CodeOmitted();\n" +
            "        }\n" +
            "    }\n" +
            "}\n"
    };

    await store.maintenance.send(new PutAnalyzersOperation(analyzerDefinition))
    //endregion
}
