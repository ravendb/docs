import { 
    DocumentStore, 
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region index_1
    // Define a Map index:
    // ===================
    class Employees_ByNotes extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            this.map("employees", employee => {
                return {
                    EmployeeNotes: employee.Notes[0]
                };
            });

            // Configure index-field 'EmployeeNotes' for highlighting:
            // =======================================================
            this.store("EmployeeNotes", "Yes");
            this.index("EmployeeNotes", "Search");
            this.termVector("EmployeeNotes", "WithPositionsAndOffsets");
        }
    }
    //endregion
    
    //region index_2
    // Define a Map-Reduce index:
    // ==========================
    class ContactDetailsPerCountry extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            // The 'map' function defines what will be indexed from each document in the collection
            this.map("companies", company => {
                return {
                    Country: company.Address.Country,
                    ContactDetails: company.Contact.Name + " " + company.Contact.Title
                };
            });

            // The 'reduce' function specifies how data is grouped and aggregated
            this.reduce(results => results.groupBy(x => x.Country).aggregate(g => {
                return {
                    // Set 'Country' as the group-by key
                    // 'ContactDetails' will be grouped per 'Country'
                    Country: g.key,

                    // Specify the aggregation
                    // here we use 'join' as the aggregation function
                    ContactDetails: g.values.map(x => x.ContactDetails).join(' ')
                }
            }));

            // Configure index-field 'Country' for highlighting:
            // =================================================
            this.store("Country", "Yes");

            // Configure index-field 'ContactDetails' for highlighting:
            // =======================================================
            this.store("ContactDetails", "Yes");
            this.index("ContactDetails", "Search");
            this.termVector("ContactDetails", "WithPositionsAndOffsets");
        }
    }
    //endregion

    async function highlights() {
        {
            //region highlight_1
            // Define a param that will get the highlighted text fragments
            let managerHighlights;
            
            const employeesResults = await session
                 // Query the map index
                .query({ indexName: "Employees/ByNotes" })
                 // Search for documents containing the term 'manager'
                .search("EmployeeNotes", "manager")
                 // Request to highlight the searched term by calling 'highlight'
                .highlight({
                    fieldName: "EmployeeNotes",
                    fragmentLength: 35,
                    fragmentCount: 2
                }, x => { managerHighlights = x; })
                .all();
            //endregion
        }

        {
            //region highlight_2
            // Define a param that will get the highlighted text fragments
            let managerHighlights;

            const employeesResults = await session
                // Query the map index
                .query({ indexName: "Employees/ByNotes" })
                // Search for documents containing the term 'manager'
                .whereEquals("EmployeeNotes", "manager")
                // Request to highlight the searched term by calling 'highlight'
                .highlight({
                    fieldName: "EmployeeNotes",
                    fragmentLength: 35,
                    fragmentCount: 2
                }, x => { managerHighlights = x; })
                .all();
            //endregion
        }

        {
            //region highlight_3
            // 'employeesResults' contains all Employee DOCUMENTS that contain the term 'manager'.
            // 'managerHighlights' contains the text FRAGMENTS that highlight the 'manager' term.

            let fragmentsHtml = "<ul>";

            employeesResults.forEach((employee) => {
                // Call 'getFragments' to get all fragments for the specified employee id
                let fragments = managerHighlights.getFragments(employee.id);
                
                fragments.forEach((fragment) => {
                    fragmentsHtml += `<li>Doc: ${employee.id}</li>`
                    fragmentsHtml += `<li>Fragment: ${fragment}</li>`;
                    fragmentsHtml += `<li></li>`;
                });
            });

            fragmentsHtml += "</ul>";
            
            // The resulting fragmentsHtml:
            // ============================

            // <ul>
            //   <li>Doc: employees/2-A</li>
            //   <li>Fragment:  to sales <b style="background:yellow">manager</b> in January</li>
            //   <li></li>
            //   <li>Doc: employees/5-A</li>
            //   <li>Fragment:  to sales <b style="background:yellow">manager</b> in March</li>
            //   <li></li>
            // </ul>
            //endregion
        }

        {
            //region highlight_4
            // Define the key by which the resulting fragments are grouped:
            // ============================================================
            const options = {
                // Set 'groupKey' to be the index's group-by key
                // The resulting fragments will be grouped per 'Country'
                groupKey: "Country"
            };

            let agentHighlights;

            // Query the map-reduce index:
            // ===========================
            const detailsPerCountry  = await session
                .query({ indexName: "ContactDetailsPerCountry" })
                 // Search for results containing the term 'agent'
                .search("ContactDetails", "agent")
                 // Request to highlight the searched term by calling 'highlight'
                 // Pass the defined 'options'
                .highlight({
                    fieldName: "ContactDetails",
                    fragmentLength: 35,
                    fragmentCount: 2,
                    ...options
                }, x => { agentHighlights = x; })
                .all();
            //endregion
        }

        {
            //region highlight_5
            // 'detailsPerCountry' contains the contacts details grouped per country.
            // 'agentHighlights' contains the text FRAGMENTS that highlight the 'agent' term.

            let fragmentsHtml = "<ul>";

            employeesResults.forEach((item) => {
                // Call 'getFragments' to get all fragments for the specified country key
                let fragments = agentHighlights.getFragments(item.Country);
                
                fragments.forEach((fragment) => {
                    fragmentsHtml += `<li>Doc: ${item.Country}</li>`
                    fragmentsHtml += `<li>Fragment: ${fragment}</li>`;
                    fragmentsHtml += `<li></li>`;
                });
            });

            fragmentsHtml += "</ul>";
            
            // The resulting fragmentsHtml:
            // ============================
            
            // <ul>
            //   <li>Country: UK</li>
            //   <li>Fragment: Devon Sales <b style="background:yellow">Agent</b> Helen Bennett</li>
            //   <li></li>
            //   <li>Country: France</li>
            //   <li>Fragment: Sales <b style="background:yellow">Agent</b> Carine Schmit</li>
            //   <li></li>
            //   <li>Country: France</li>
            //   <li>Fragment: Saveley Sales <b style="background:yellow">Agent</b> Paul Henriot</li>
            //   <li></li>
            //   <li>Country: Argentina</li>
            //   <li>Fragment: Simpson Sales <b style="background:yellow">Agent</b> Yvonne Moncad</li>
            //   <li></li>
            //   <li>Country: Argentina</li>
            //   <li>Fragment: Moncada Sales <b style="background:yellow">Agent</b> Sergio</li>
            //   <li></li>
            //   <li>Country: Brazil</li>
            //   <li>Fragment: Sales <b style="background:yellow">Agent</b> Anabela</li>
            //   <li></li>
            //   <li>Country: Belgium</li>
            //   <li>Fragment: Dewey Sales <b style="background:yellow">Agent</b> Pascale</li>
            //   <li></li>
            // </ul>
            //endregion
        }
    }
}
