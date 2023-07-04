import { DocumentStore, QueryData } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function hightlightQueryResults() {

    {
        //region highlight_1
        // Make a full-text search dynamic query:
        // ======================================
        
        // Define a param that will get the highlighted text fragments
        let salesHighlights;

        const employeesResults = await session
             // Make a dynamic query on 'Employees' collection
            .query({ collection: "Employees" })
             // Search for documents containing the term 'sales' in their 'Notes' field
            .search("Notes", "sales")
             // Request to highlight the searched term by calling 'highlight'
            .highlight({ 
                fieldName: 'Notes', // The document-field name in which we search 
                fragmentLength: 35, // Max length of each text fragment
                fragmentCount: 4 }, // Max number of fragments to return per document
                x => { salesHighlights = x; }) // Output param 'salesHighlights' will be filled
                                               // with the highlighted text fragments when query returns. 
             // Execute the query
            .all();
        //endregion
        
        //region fragments_1
        // Process results:
        // ================

        // 'employeesResults' contains all Employee DOCUMENTS that have 'sales' in their 'Notes' field.
        // 'salesHighlights' contains the text FRAGMENTS that highlight the 'sales' term.
        
        let fragmentsHtml = "<ul>";

        employeesResults.forEach((employee) => {
            // Call 'getFragments' to get all fragments for the specified employee id
            let fragments = salesHighlights.getFragments(employee.id);
            
            fragments.forEach((fragment) => {
                fragmentsHtml += `<li>Doc: ${employee.id} Fragment: ${fragment}</li>`;
            });
        });

        fragmentsHtml += "</ul>";

        // The resulting fragmentsHtml:
        // ============================

        // <ul>
        //   <li>Doc: employees/2-A Fragment: company as a <b style="background:yellow">sales</b></li>
        //   <li>Doc: employees/2-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
        //   <li>Doc: employees/2-A Fragment: president of <b style="background:yellow">sales</b> in March 1993</li>
        //   <li>Doc: employees/2-A Fragment: member of the <b style="background:yellow">Sales</b> Management</li>
        //   <li>Doc: employees/3-A Fragment: hired as a <b style="background:yellow">sales</b> associate in</li>
        //   <li>Doc: employees/3-A Fragment: promoted to <b style="background:yellow">sales</b> representativ</li>
        //   <li>Doc: employees/5-A Fragment: company as a <b style="background:yellow">sales</b> representativ</li>
        //   <li>Doc: employees/5-A Fragment: promoted to <b style="background:yellow">sales</b> manager in</li>
        //   <li>Doc: employees/5-A Fragment: <b style="background:yellow">Sales</b> Management." </li>
        //   <li>Doc: employees/6-A Fragment: for the <b style="background:yellow">Sales</b> Professional.</li>
        // </ul>
        //endregion
    }

    {
        //region highlight_2
        // Define customized tags to use for highlighting the searched terms
        // =================================================================
        const tagsToUse = {
            preTags: ["+++", "<<<"],
            postTags: ["+++", ">>>"]
        };
        
        // Make a full-text search dynamic query:
        // ======================================
        let salesHighlights;
        let managerHighlights;

        const employeesResults = await session
            .query({ collection: "Employees" })
             // Search for:
             //   * documents containing the term 'sales' in their 'Notes' field
             //   * OR for documents containing the term 'manager' in their 'Title' field
            .search("Notes", "sales")
            .search("Title", "manager")
             // Call 'highlight' for each field searched
             // Pass 'tagsToUse' to OVERRIDE the default tags used 
            .highlight({ fieldName: 'Notes', fragmentLength: 35, fragmentCount: 1, ...tagsToUse },
                    x => { salesHighlights = x; })
            .highlight({ fieldName: 'Title', fragmentLength: 35, fragmentCount: 1, ...tagsToUse },
                    x => { managerHighlights = x; })
            .all();
        //endregion

        //region fragments_2
        // The resulting salesHighlights fragments:
        // ========================================
        
        // "for the +++Sales+++ Professional."
        // "hired as a +++sales+++ associate in"
        // "company as a +++sales+++"
        // "company as a +++sales+++ representativ"
        
        // The resulting managerHighlights fragments:
        // ==========================================
        
        // "Sales <<<Manager>>>"        
        //endregion

        //region highlight_3
        // Make a full-text search dynamic query & project results:
        // ========================================================

        // Define a param that will get the highlighted text fragments
        let termsHighlights;

        // Define the class for the projected results
        class Result {
            constructor () {
                this.Name = null;
                this.Title = null;
            }
        }

        // Make a dynamic query on 'Employees' collection
        const employeesResults = await session
            .query({ collection: "Employees" })
             // Search for documents containing 'sales' or 'german' in their 'Notes' field
            .search("Notes", "manager german")
             // Request to highlight the searched terms from the 'Notes' field 
            .highlight({ fieldName: "Notes", fragmentLength: 35, fragmentCount: 2 },
                    x => { termsHighlights = x; })
             // Define the projection
            .selectFields(QueryData.customFunction("o", "{ Name: o.FirstName + ' ' + o.LastName, Title: o.Title }" ),
                Result)
            .all();
        //endregion

        //region fragments_3
        // The resulting fragments from termsHighlights:
        // =============================================
        
        // "to sales <b style=\"background:yellow\">manager</b> in March"
        // "and reads <b style=\"background:lawngreen\">German</b>.  He joined"
        // "to sales <b style=\"background:yellow\">manager</b> in January"
        // "in French and <b style=\"background:lawngreen\">German</b>."
        
        // NOTE: each search term is wrapped with a different color
        // 'manager' is wrapped with yellow
        // 'german' is wrapped with lawngreen
        //endregion
        
    }
}

//region syntax
{    
    {
        //region syntax_1
        highlight(parameters, hightlightingsCallback);
        //endregion
    }
    {
        //region syntax_2
        class Highlightings {
            // Name of the field that contains the searched terms to highlight.
            get fieldName();
            // The resulting keys (document IDs, or the map-reduce keys)
            get resultIndents();
            // Returns the list of the highlighted text fragments for the passed document ID, or the map-reduce key
            getFragments(key);
        }
        //endregion
    }
}
//endregion
