import * as assert from "assert";
import { DocumentStore, PutCommandDataWithJson, DeleteCommandData } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

async function example() {

    const conventions = store.conventions;
    //region find_type_name
    conventions.findJsTypeName = 
        type => // determine the type name based on type
    //endregion
    "";

    //region find_clr_type
    conventions.findJsType((id, doc) => {
            const metadata = doc["@metadata"];
            if (metadata) {
                const jsType = metadata["Raven-Node-Type"];
                return this.getJsTypeByDocumentType(jsType);
            }

            return null;
    });
    //endregion

    //region find_type_collection_name
    conventions.findCollectionName =
        type => // function that provides the collection name based on the entity type 
    //endregion
        "";

    //region find_collection_name_for_object_literal
    conventions.findCollectionNameForObjectLiteral = 
        entity => entity["category"]; 
        // function that provides the collection name based on the entity object
    //endregion
        "";

    //region transform_collection_name_to_prefix
    conventions.transformClassCollectionNameToDocumentIdPrefix =
        collectionName => // transform the collection name to the prefix of an identifier, e.g. [prefix]/12
    //endregion
        "";

    //region find_identity_property
    // TODO
    //conventions.getIdentityProperty(fieldInfo -> "Id".equals(fieldInfo.getName()));
    //endregion

    //region find_identity_property_name_from_collection_name
    conventions.findIdentityPropertyNameFromCollectionName =
        collectionName => "id";
    //endregion

    //region identity_part_separator
    conventions.identityPartsSeparator = "/";
    //endregion
}
