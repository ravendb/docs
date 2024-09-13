import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function deleteCompareExchange() {
    {
        //region delete_1
        // Get an existing compare-exchange item
        const getCmpXchgOp = new GetCompareExchangeValueOperation("johnDoe@gmail.com");
        const itemResult = await documentStore.operations.send(getCmpXchgOp);
        
        // Keep the item's version
        const versionOfItem = itemResult.index; 

        // Delete the compare-exchange item:
        // =================================

        // Define the delete compare-exchange operation, pass:
        // * The item's KEY
        // * The item's INDEX (its version)
        //   The compare-exchange item will only be deleted if this number 
        //   is equal to the one stored on the server when the delete operation is executed.
        const deleteCmpXchgOp = new DeleteCompareExchangeValueOperation("johnDoe@gmail.com", versionOfItem);

        // Execute the operation by passing it to operations.send
        const deleteResult = await documentStore.operations.send(deleteCmpXchgOp);

        // Verify delete results:
        assert.ok(deleteResult.successful);
        //endregion
    }
}

//region syntax

//region syntax_1 
const deleteCmpXchgOp = new DeleteCompareExchangeValueOperation(key, index, clazz?);
//endregion

//region syntax_2 
// Return value of store.operations.send(deleteCmpXchgOp)
// ======================================================
class CompareExchangeResult {
    successful;
    value;
    index;
}
//endregion

//endregion
