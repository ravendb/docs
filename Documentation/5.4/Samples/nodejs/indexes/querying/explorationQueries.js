import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function explorationQueries() {

    const session = documentStore.openSession();

    //region exploration-queries_1_1
    const filteredCompanies = await session
         // Make a full-collection query
        .query({ collection: "companies" })
         // Apply a filter, scan only first 50 records from query results
        .filter(x => x.equals("Address.Country", "USA"), 50)
        .all();
    
    // Results: 
    // ========
    
    // * While a full-collection query on the 'companies' colletion yields 91 results
    //   only the first 50 records are scanned by the filter operation -
    //   resulting in 5 matching documents.
    //
    // * No auto-index is created.
    //endregion

    //region exploration-queries_1_2
    const filteredCompanies = await session
        .advanced
        .rawQuery("from Companies filter Address.Country == 'USA' filter_limit 50")
        .all();
    //endregion

    //region exploration-queries_2_1
    const filteredCompanies = await session
         // Make a dynamic query on a collection
        .query({ collection: "companies" })
         // Apply some condition - this will trigger auto-index creation
        .whereEquals("Contact.Title", "Sales Representative")
         // Apply a filter 
        .filter(x => x.equals("Address.Country", "Germany"))
        .all();

    // Results: 
    // ========
    
    // * The dynamic query results (before applying the filter) contain 17 results.
    //   Applying the filter results in 4 matching documents. 
    //
    // * Since a query predicate was applied (using 'whereEquals') 
    //   an auto-index that is indexing field 'Contact.Title' is created.
    //
    // * Field 'Address.Country' is Not included in the auto-index
    //   since it is part of the filter operation. 
    //endregion

    //region exploration-queries_2_2
    const filteredCompanies = await session
        .advanced
        .rawQuery(`from Companies 
                   where Contact.Title == 'Sales Representative'
                   filter Address.Country == 'Germany'`)
        .all();
    //endregion

    //region exploration-queries_3_1
    const filteredCompanies = await session
         // Make a collection query
        .query({ collection: "companies" })
         // Apply a filter 
        .filter(x => x.equals("Address.Country", "Germany"))
         // Any fields can be projected in the results
        .selectFields([ "Name", "Address.City", "Address.Country"])
        .all();

     // Results: 
     // ========
    
     // * Results include all companies with country = 'Germany'
     //   Each resluting object contains only the selected fields.
     //
     // * No auto-index is created.
    //endregion

    //region exploration-queries_3_2
    const filteredCompanies = await session
        .advanced
        .rawQuery(`from Companies 
                   filter Address.Country == 'Germany'
                   select Name, Address.City, Address.Count`)
        .all();
    //endregion
}

{
    //region syntax
    filter(builder);
    filter(builder, limit);
    //endregion
}
