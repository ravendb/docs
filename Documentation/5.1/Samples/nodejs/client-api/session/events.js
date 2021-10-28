import * as assert from "assert";
import {
    AfterConversionToDocumentEventArgs,
    BeforeConversionToDocumentEventArgs,
    BeforeConversionToEntityEventArgs,
    DocumentStore,
    Item
} from "ravendb";
import {ObjectUtil} from "ravendb/dist/Utility/ObjectUtil";

const store = new DocumentStore();
const session = store.openSession();

function log() {}

class Product {
    constructor(name, unitsInStock, discontinued) {
        this.name = name;
        this.unitsInStock = unitsInStock;
        this.discontinued = discontinued;
    }
}

{
    //region on_before_store_event
    store.on("beforeStore", (args) => {
        if (args.entity instanceof Product) {
            const product = args.entity;
            if (product.unitsInStock === 0) {
                product.discontinued = true;
            }
        }
    });
    //endregion

    //region on_before_query_execute_event
    store.on("beforeQuery", args => {
        args.queryCustomization.noCaching();
    });
    //endregion

    //region on_before_query_execute_event_2
    store.on("beforeQuery", args => {
        args.queryCustomization
            .waitForNonStaleResults(30000);
    });
    //endregion

    //region on_after_save_changes_event
    store.on("afterSaveChanges", args => {
        log("Document " + args.documentId + " was saved.");
    });
    //endregion
}

async function examples() {

    let beforeStoreListener;
    //region store_session
    // subscribe to the event
    store.on("beforeStore", (args) => {
        if (args.entity instanceof Product) {
            const product = args.entity;
            if (product.unitsInStock === 0) {
                product.discontinued = true;
            }
        }
    });

    {
        const session = store.openSession();
        const product1 = new Product();
        product1.name = "RavenDB v3.5";
        product1.unitsInStock = 0;

        await session.store(product1);

        const product2 = new Product();
        product2.name = "RavenDB v4.0";
        product2.unitsInStock = 1000;
        await session.store(product2);

        await session.saveChanges();  // Here's where the event is emitted
    }
    //endregion

    let beforeDeleteListener;
    //region delete_session
    // subscribe to the event
    store.on("beforeDelete", args => {
        throw new Error("Not implemented");
    });

    // open a session and delete entity
    {
        const session = store.openSession();
        const product = await session.load("products/1-A");

        session.delete(product);

        await session.saveChanges(); // "Not implemented" error will be thrown here
    }
    //endregion

    {
    //region on_before_conversion_to_document
        store.addSessionListener("beforeConversionToDocument", (event: BeforeConversionToDocumentEventArgs) => {
            if (event.entity instanceof Item) {
                const item = event.entity;
                item.before = true;
            }
        });
    //endregion
    }
    {
    //region on_after_conversion_to_document
        store.addSessionListener("afterConversionToDocument", (event: AfterConversionToDocumentEventArgs) => {
            if (event.entity instanceof Item) {
                const item = event.entity;
                const document = ObjectUtil.clone(event.document.value);
                document.after = true;

                event.document.value = document;

                item.after = true;
            }
        });
    //endregion
    }
    {
    //region on_before_conversion_to_entity
        store.addSessionListener("beforeConversionToEntity", (event: BeforeConversionToEntityEventArgs) => {
            const document = ObjectUtil.clone(event.document) as Item;

            document.before = true;
            event.document = document;
        });
    //endregion
    }
    {
    //region on_after_conversion_to_entity
        store.addSessionListener("afterConversionToEntity", (event: AfterConversionToEntityEventArgs) => {
            if (event.entity instanceof Item) {
                const item = event.entity as Item;
                item.after = true;
            }
        });
    //endregion
    }

}
