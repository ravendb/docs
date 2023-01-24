import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getIndexTerms() {
    {
        //region get_index_terms
        // Define the get terms operation
        // Pass the requested index-name, index-field, start value & page size
        const getTermsOp = new GetTermsOperation("Orders/Totals", "Employee", "employees/5-a", 10);

        // Execute the operation by passing it to maintenance.send
        const fieldTerms = await store.maintenance.send(getTermsOp);

        // fieldTerms will contain the all terms that come after term 'employees/5-a' for index-field 'Employee'
        //endregion
    }
}

{
    //region get_index_terms_syntax
    // Available overloads:
    const getTermsOp = new GetTermsOperation(indexName, field, fromValue);
    const getTermsOp = new GetTermsOperation(indexName, field, fromValue, pageSize);
    //endregion
}
