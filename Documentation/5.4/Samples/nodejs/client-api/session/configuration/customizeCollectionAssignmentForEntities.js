import { DocumentStore, DocumentConventions } from "ravendb";

const store = new DocumentStore();

async function example() {
    //region custom_collection_name
    store.conventions.findCollectionName = clazz => {
        if (clazz === Category) {
            return "ProductGroups";
        }

        return DocumentConventions.defaultGetCollectionName(clazz);
    };
    //endregion
}

class Category { }
