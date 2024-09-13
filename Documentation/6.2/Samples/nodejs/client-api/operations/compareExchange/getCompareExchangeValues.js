import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function getCompareExchangeValues() {
    {
        //region get_values_1
        // Save some new compare-exchange items
        await documentStore.operations.send(
            new PutCompareExchangeValueOperation("employees/1-A", "someValue1", 0));
        await documentStore.operations.send(
            new PutCompareExchangeValueOperation("employees/2-A", "someValue2", 0));
        await documentStore.operations.send(
            new PutCompareExchangeValueOperation("employees/3-A", "someValue3", 0));

        // Get multiple compare-exchange items by specifying keys:
        // =======================================================

        // Define the get compare-exchange operation,
        // Specify the keys of the items to get 
        const getCmpXchgOp = new GetCompareExchangeValuesOperation({
            keys: ["employees/1-A", "employees/3-A"]
        });

        // Execute the operation by passing it to operations.send
        const items = await documentStore.operations.send(getCmpXchgOp);

        // Access the returned items:
        assert.equal(Object.keys(items).length, 2);
        assert.equal(items["employees/1-A"].value, "someValue1");
        assert.equal(items["employees/3-A"].value, "someValue3");
        //endregion
    }
    {
        //region get_values_2
        // Get multiple compare-exchange items with common key prefix:
        // ===========================================================

        // Define the get compare-exchange operation, specify:
        // * startWith: The common key prefix
        // * pageSize:  Max items to get (this is optional)
        // * start:     The start position (this is optional)
        const getCmpXchgOp = new GetCompareExchangeValuesOperation({
            startWith: "employees",
            pageSize: 10,
            start: 0
        });

        // Execute the operation by passing it to operations.send
        const items = await documentStore.operations.send(getCmpXchgOp);

        // Results will include only cmpXchg items with keys that start with "employees" 
        //endregion
    }
}

//region syntax

//region syntax_1 
const getCmpXchgOp = new GetCompareExchangeValuesOperation(parameters);
//endregion

//region syntax_2 
// the parameters object:
{
    // Keys of the items to retrieve 
    keys?; // string[]

    // The common key prefix of the items to retrieve
    startWith?; // string
    
    // The number of items that should be skipped
    start?; // number
    
    // The maximum number of values that will be retrieved
    pageSize?; // number
    
    // When the item's value is a class, you can specify its type in this parameter
    clazz?; // object
}
//endregion

//region syntax_3 
class CompareExchangeValue {
    key;
    value;
    metadata;
    index;
}
//endregion

//endregion
