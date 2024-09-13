import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function putCompareExchange() {
    {
        //region put_1
        // Create a new CmpXchg item:
        // ==========================
        
        // Define the put compare-exchange operation. Pass:
        // * KEY: a new unique identifier (e.g. a user's email)
        // * VALUE: an associated value (e.g. the user's name)
        // * INDEX: pass '0' to indicate that this is a new key
        const putCmpXchgOp = new PutCompareExchangeValueOperation("johnDoe@gmail.com", "John Doe", 0);
        
        // Execute the operation by passing it to operations.send
        const result = await documentStore.operations.send(putCmpXchgOp);
        
        // Check results
        const successful = result.successful; // Has operation succeeded
        const indexForItem = result.index;    // The version number assigned to the new item
        
        // If successful is true then a new compare-exchange item has been created
        // with the unique email key and the associated value.
        //endregion
    }
    {
        //region put_2
        // Create a new CmpXchg item with metadata:
        // ========================================

        // Define the put compare-exchange operation.
        // Pass a 4'th parameter with the metadata object.
        const putCmpXchgOp = new PutCompareExchangeValueOperation("+48-123-456-789", "John Doe", 0,
            { 
                "Provider": "T-Mobile",
                "Network": "5G",
                "Work phone": false
            });

        // Execute the operation by passing it to operations.send
        const result = await documentStore.operations.send(putCmpXchgOp);

        // Check results
        const successful = result.successful; // Has operation succeeded
        const indexForItem = result.index;    // The version number assigned to the new item

        // If successful is true then a new compare-exchange item has been created
        // with the unique phone number key, value, and metadata.
        //endregion
    }
    {
        //region put_3
        // Modify an existing CmpXchg item:
        // ================================

        // Get the existing compare-exchange item
        const item = await documentStore.operations.send(
            new GetCompareExchangeValueOperation("+48-123-456-789")
        );

        // Make some changes
        const newValue = "Jane Doe";
        const metadata = item.metadata;
        metadata["Work phone"] = true;

        // Update the compare-exchange item:
        // The put operation will succeed only if the 'index' of the compare-exchange item
        // has not changed between the read and write operations.
        const result = await documentStore.operations.send(
            new PutCompareExchangeValueOperation("+48-123-456-789", newValue, item.index, metadata)
        );

        // Check results
        const successful = result.successful; // Has operation succeeded
        const newIndex = result.index;        // The new version number assigned to the item 

        // Version has increased
        assert(newIndex > item.index);
        //endregion
    }
}

//region syntax

//region syntax_1 
// Available overloads:
// ====================
const putCmpXchgOp = new PutCompareExchangeValueOperation(key, value, index);
const putCmpXchgOp = new PutCompareExchangeValueOperation(key, value, index, metadata);
//endregion

//region syntax_2 
// Return value of store.operations.send(putCmpXchgOp)
// ===================================================
class CompareExchangeResult {
    successful;
    value;
    index;
}
//endregion

//endregion
