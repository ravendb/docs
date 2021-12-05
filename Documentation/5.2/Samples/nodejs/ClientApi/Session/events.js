import {
    AfterConversionToDocumentEventArgs, AfterConversionToEntityEventArgs,
    BeforeConversionToDocumentEventArgs, BeforeConversionToEntityEventArgs,
    DocumentStore, Item,
    SessionAfterSaveChangesEventArgs,
    SessionBeforeDeleteEventArgs, SessionBeforeQueryEventArgs,
    SessionBeforeStoreEventArgs
} from "ravendb";
import {ObjectUtil} from "ravendb/dist/Utility/ObjectUtil";

const store = new DocumentStore();
const session = store.openSession();

class Product {
    constructor(name, unitsInStock, discontinued) {
        this.name = name;
        this.unitsInStock = unitsInStock;
        this.discontinued = discontinued;
    }
}


//beforeStore Block
{
    let session;
    let documentId, entity;

    //region beforeStoreEventArgs
    const beforeStoreEventArgs = new SessionBeforeStoreEventArgs(session, documentId, entity);
    //endregion

    //region on_before_store_event
    function onBeforeStore(args) {
        if (args.getEntity() instanceof Product) {
            const product = args.getEntity();
            if (product.unitsInStock === 0) {
                product.discontinued = true;
            }
        }
    }
    //endregion

    //region OnBeforeStore
    store.addSessionListener("beforeStore", event => onBeforeStore(event));
    //endregion

    //region store_session
    store.addSessionListener("beforeStore", event => onBeforeStore(event));

    session = store.openSession();
    await session.store(new Product(
            {
                name: "RavenDB v3.5",
                UnitsInStock: 0
            })
    )

    await session.store(new Product(
        {
            name: "RavenDB v4.0",
            UnitsInStock: 1000
        })
    )

    await session.saveChanges(); // Here the method is invoked
    //endregion

}//beforeStore Block End

//beforedelete Block
{

    let session;
    let documentId, entity;

    //region beforeDeleteEventArgs
    const beforeDeleteEventArgs = new SessionBeforeDeleteEventArgs(session, documentId, entity);
    //endregion

    //region on_Before_Delete_Event
    function onBeforeDelete(args) {
        throw new Error("Not implemented");
    }
    //endregion

    //region before_delete_event
    store.addSessionListener("beforedelete", event => onBeforeDelete(event));
    //endregion

    //region delete_session
    store.addSessionListener("beforedelete", event => onBeforeDelete(event));

    session = store.openSession();
    let product = await session.load("products/1-A", Product);
    let product2 = await session.load("products/2-A", Product);

    // onBeforeDelete is triggered whether you
    // call delete() on an entity or on its ID
    await session.delete(product);
    await session.saveChanges(); // NotSupportedException will be thrown

    await session.delete("products/2-A");
    await session.saveChanges(); // NotSupportedException will be thrown
    //endregion

}//beforedelete Block End

//onaftersavechanges Block
{
    let session;
    let documentId, entity;


    //region afterSaveChangesEventArgs
    const afterSaveChangesEventArgs = new SessionAfterSaveChangesEventArgs(session, documentId, entity);
    //endregion


    //region on_after_save_changes_event
    function onAfterSaveChanges(args) {
        console.log("Document " + args.documentId + " was saved.");
    }

    //endregion

    //region after_save_event
    store.addSessionListener("afterSave", event => onAfterSaveChanges(event));
    //endregion

}//onaftersavechanges Block End

//OnBeforeQuery Block
{
    let session;
    let documentId, entity;


    //region beforeQueryEventArgs
    const beforeQueryEventArgs = new SessionBeforeQueryEventArgs(session, documentId, entity);
    //endregion


    //region on_before_query_execute_event
    function onBeforeQuery(args) {
        args.queryCustomization.noCaching();
    }
    //endregion

    //region before_query_event
    store.addSessionListener("beforeQuery", event => onBeforeQuery(event));
    //endregion

    //region on_before_query_execute_event_2
    function onBeforeQuery(args) {
        args.queryCustomization.waitForNonStaleResults(30);
    }
    //endregion
}

// on_before_conversion_to_document Block
{
    let session;
    let documentId, entity;

    //region beforeConversionToDocument
    const beforeConversionToDocumentEventArgs = new BeforeConversionToDocumentEventArgs(session, documentId, entity);
    //endregion


    //region on_before_conversion_to_document
    function onBeforeConversionToDocument(args) {
        if (args.getEntity() instanceof Product) {
            const product = args.getEntity();
            product.before=true;
        }
    }
    //endregion

    //region beforeConversionToDocument_event
    store.addSessionListener("beforeConversionToDocument", event => onBeforeConversionToDocument(event));
    //endregion


}// on_before_conversion_to_document Block End


//Todo
//OnAfterConversionToDocument Block
{
    let session;
    let documentId, entity, document;

    //region afterConversionToDocument
    const afterConversionToDocument = new AfterConversionToDocumentEventArgs(session, documentId, entity, document);
    //endregion


    //region on_after_conversion_to_document
    function onAfterConversionToDocument(args) {
        if (args.getEntity() instanceof Product) {
            const product = args.getEntity();
            if(product.document.after == null){
                product.document.after = true;
            }
        }
    }
    //endregion

    //region afterConversionToDocument_event
    store.addSessionListener("afterConversionToDocument", event => onAfterConversionToDocument(event));
    //endregion

}// OnAfterConversionToDocument Block End

{
    //region on_before_conversion_to_entity
    store.addSessionListener("beforeConversionToEntity", (event: BeforeConversionToEntityEventArgs) => {
        const document = ObjectUtil.clone(event.document);

        document.before = true;
        event.document = document;
    });
    //endregion
}
{
    //region on_after_conversion_to_entity
    store.addSessionListener("afterConversionToEntity", (event: AfterConversionToEntityEventArgs) => {
        if (event.entity instanceof Item) {
            const item = event.entity;
            item.after = true;
        }
    });
    //endregion
}