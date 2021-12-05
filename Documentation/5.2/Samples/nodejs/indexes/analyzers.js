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

//region analyzers_1
class BlogPosts_ByTagsAndContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Posts.Select(post => new {     
            tags = post.tags,     
            content = post.content 
        })`;

        this.analyze("tags", "SimpleAnalyzer");
        this.analyze("content", "Raven.Sample.SnowballAnalyzer");
    }
}
//endregion

//region analyzers_3
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Employees.Select(employee => new { " +
            "    LastName = employee.LastName, " +
            "    FirstName = employee.FirstName " +
            "})";

        this.index("FirstName", "Exact");
    }
}
//endregion

//region analyzers_4
class BlogPosts_ByContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Posts.Select(post => new { " +
            "    tags = post.tags, " +
            "    content = post.content " +
            "})";

        this.index("content", "Search");
    }
}
//endregion

//region analyzers_5
class BlogPosts_ByTitle extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Posts.Select(post => new { " +
            "    tags = post.tags, " +
            "    content = post.content " +
            "})";

        this.index("content", "No");
        this.store("content", "Yes");
    }
}
//endregion

async function analyzers() {
    const session = store.openSession();

    //region analyzers_2
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

{
    //region analyzer_definition
    const analyzerDefinition = {
        name: "analyzerName",
        code: "code"
    };
    //endregion
}

let analyzerDefinition;
{
    //region put_analyzers_operation
    await store.maintenance.send(new PutAnalyzersOperation(analyzerDefinition));
    //endregion
}
{
    //region put_serverWide_analyzers_operation
    await store.maintenance.send(new PutServerWideAnalyzersOperation(analyzerDefinition));
    //endregion
}
{
    //region analyzers_7
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



