import { DocumentStore, DocumentConventions } from "ravendb";

const store = new DocumentStore();

{

    {
        //region identity_1
        store.conventions.identityProperty;
        //endregion
    }

    {
        //region identity_2
        store.conventions.identityProperty = "Identifier"; 
        //endregion
    }
}
