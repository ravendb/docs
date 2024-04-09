import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function deleteCompareExchange() {
    {
        //region delete_1
        // Saving a new compare-exchange item
        const putCmpXchgOp = new PutCompareExchangeValueOperation("johnDoe@gmail.com", "John Doe", 0);
        const itemResult = await documentStore.operations.send(putCmpXchgOp);
        
        // Keep the item's version
        const versionOfItem = itemResult.index; 

        // Delete the compare-exchange item:
        // =================================

        // Define the delete compare-exchange operation, pass:
        // * The item's KEY
        // * The item's INDEX (its version)
        //   The compare-exchange item will only be deleted if this number 
        //   is equal to the one stored on the server when operation is executed.
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
