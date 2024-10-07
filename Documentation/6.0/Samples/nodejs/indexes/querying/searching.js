import { DocumentStore, AbstractIndexCreationTask } from "ravendb";
import assert from "assert";

const store = new DocumentStore();
const session = store.openSession();

class Employee { }

//region index_1
class Employees_ByNotes extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // Define the index-fields 
        this.map("Employees", e => ({
            employeeNotes: e.Notes
        }));

        // Configure the index-field for FTS:
        // Set 'Search' on index-field 'employeeNotes'
        this.index("employeeNotes", "Search");
        
        // Optionally: Set your choice of analyzer for the index-field.
        // Here the text from index-field 'employeeNotes' will be tokenized by 'WhitespaceAnalyzer'.
        this.analyze("employeeNotes", "WhitespaceAnalyzer");

        // Note:
        // If no analyzer is set then the default 'RavenStandardAnalyzer' is used.
    }
}
//endregion

//region index_2
class Employees_ByEmployeeData extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // Define the index-fields 
        this.map("Employees", e => ({
            // Multiple document-fields can be indexed
            // into the single index-field 'employeeData' 
            employeeData: [e.FirstName, e.LastName, e.Title, e.Notes]
        }));

        // Configure the index-field for FTS:
        // Set 'Search' on index-field 'employeeNotes'
        this.index("employeeNotes", "Search");

        // Note:
        // Since no analyzer is set then the default 'RavenStandardAnalyzer' is used.
    }
}
//endregion

//region index_3
class Employees_ByNotes_usingDefaultAnalyzer extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // Define the index-fields 
        this.map("Employees", e => ({
            employeeNotes: e.Notes
        }));

        // Configure the index-field for FTS:
        this.index("employeeNotes", "Search");

        // Since no analyzer is explicitly set
        // then the default 'RavenStandardAnalyzer' will be used at indexing time.

        // However, when making a search query with wildcards,
        // the 'LowerCaseKeywordAnalyzer' will be used to process the search terms
        // prior to sending them to the search engine.
    }
}
//endregion

//region index_4
class Employees_ByNotes_usingCustomAnalyzer extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        this.map("Employees", e => ({
            employeeNotes: e.Notes
        }));

        // Configure the index-field for FTS:
        this.index("employeeNotes", "Search");

        // Set a custom analyzer for the index-field:
        this.analyze("employeeNotes", "RemoveWildcardsAnalyzer");
    }
}
//endregion

//region index_5
class Employees_ByFirstName_usingExactAnalyzer extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        this.map("Employees", e => ({
            firstName: e.FirstName
        }));

        // Set the Exact analyzer for the index-field:
        // (The field will not be tokenized)
        this.index("firstName", "Exact");
    }
}
//endregion

async function searching() {
    const session = store.openSession();
    
    {
        //region search_1
        const employees = await session
             // Query the index
            .query({ indexName: "Employees/ByNotes" })
             // Call 'search':
             // pass the index field name that was configured for FTS and the term to search for.
            .search("employeeNotes", "French")
            .all();

        // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
        //
        // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
        //   which preserves casing.
        //endregion
    }

    {
        //region search_2
        const employees = await session
             // Query the static-index
            .query({ indexName: "Employees/ByEmployeeData" })
            .openSubclause()
             // A logical OR is applied between the following two Search calls:
            .search("employeeData", "Manager")
             // A logical AND is applied between the following two terms: 
            .search("employeeData", "French Spanish", "AND")
            .closeSubclause()
            .all();
        
        // * Results will contain all Employee documents that have:
        //   ('Manager' in any of the 4 document-fields that were indexed)
        //   OR 
        //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
        //
        // * Search is case-insensitive since the default analyzer is used
        //endregion
    }

    {
        //region search_3
        let explanations;
        
        const employees = await session
            .query({ indexName: "Employees/ByNotes/usingDefaultAnalyzer" })
             // If you request to include explanations,
             // you can see the exact term that was sent to the search engine.
            .includeExplanations(e => explanations = e)
             // Provide a term with a wildcard to the search method:
            .search("employeeNotes", "*rench")
            .all();

        // Results will contain all Employee documents that have terms that end with 'rench'
        // (e.g. French). 
        
        // Checking the explanations, you can see that the search term 'rench'
        // was sent to the search engine WITH the leading wildcard, i.e. '*rench'
        // since the 'LowerCaseKeywordAnalyzer' is used in this case. 
        const explanation = explanations.explanations[employees[0].id][0];
        const expectedVal = "employeeNotes:*rench";
        
        assert.ok(explanation.includes(expectedVal), 
            `'${explanation}' does not contain '${expectedVal}.'`);
        //endregion
    }

    {
        //region search_4
        let explanations;

        const employees = await session
            .query({ indexName: "Employees/ByNotes/usingCustomAnalyzer" })
            .includeExplanations(e => explanations = e)
             // Provide a term with wildcards to the Search method:
            .search("employeeNotes", "*French*")
            .all();

        // Even though a wildcard was provided,
        // the results will contain only Employee documents that contain the exact term 'French'.
        
        // The search term was sent to the search engine WITHOUT the wildcard,
        // as the custom analyzer's logic strips them out.

        // This can be verified by checking the explanations:
        const explanation = explanations.explanations[employees[0].id][0];
        
        const expectedVal = "employeeNotes:french";
        assert.ok(explanation.includes(expectedVal),
            `'${explanation}' does not contain '${expectedVal}.'`);

        const notExpectedVal = "employeeNotes:*french";
        assert.ok(!explanation.includes(notExpectedVal),
            `'${explanation}' does not contain '${notExpectedVal}.'`);
        //endregion
    }

    {
        //region search_5
        let explanations;

        const employees = await session
            .query({ indexName: "Employees/ByFirstName/usingExactAnalyzer" })
            .includeExplanations(e => explanations = e)
             // Provide a term with a wildcard to the Search method:
            .search("firstName", "Mich*")
            .all();

        // Results will contain all Employee documents with FirstName that starts with 'Mich'
        // (e.g. Michael).
        
        // The search term, 'Mich*', is sent to the search engine
        // exactly as was provided to the Search method, WITH the wildcard.
        
        const explanation = explanations.explanations[employees[0].id][0];
        const expectedVal = "firstName:Mich*";
        
        assert.ok(explanation.includes(expectedVal),
            `'${explanation}' does not contain '${expectedVal}.'`);
        //endregion
    }

    {
        //region analyzer_1
        // The custom analyzer:
        // ====================
        
        const removeWildcardsanalyzer = `
            using System.IO;
            using Lucene.Net.Analysis; 
            using Lucene.Net.Analysis.Standard;
            namespace CustomAnalyzers
            {
                public class RemoveWildcardsAnalyzer : StandardAnalyzer
                {
                    public RemoveWildcardsAnalyzer() : base(Lucene.Net.Util.Version.LUCENE_30)
                    {
                    }
    
                    public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
                    {
                        // Read input stream and remove wildcards (*)
                        string text = reader.ReadToEnd();
                        string processedText = RemoveWildcards(text);
                        StringReader newReader = new StringReader(processedText);
                        
                        return base.TokenStream(fieldName, newReader);
                    }
    
                    private string RemoveWildcards(string input)
                    {
                        // Replace wildcard characters with an empty string
                        return input.Replace("*", "");
                    }
                }
            }`;

        // Deploying the custom analyzer:
        // ==============================
        
        const analyzerDefinition = {
            name: "RemoveWildcardsAnalyzer",
            code: RemoveWildcardsAnalyzer
        };
        
        await documentStore.maintenance.send(new PutAnalyzersOperation(analyzerDefinition));
        //endregion
    }
}

