import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Employee { }

//region index_1
class Employees_ByNotes extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        // Define the index-fields 
        this.map("Employees", e => ({
            employeeNotes : e.Notes
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
            employeeData : [e.FirstName, e.LastName, e.Title, e.Notes]
        }));

        // Configure the index-field for FTS:
        // Set 'Search' on index-field 'employeeNotes'
        this.index("employeeNotes", "Search");

        // Note:
        // Since no analyzer is set then the default 'RavenStandardAnalyzer' is used.
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
             // Call 'Search':
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
}

