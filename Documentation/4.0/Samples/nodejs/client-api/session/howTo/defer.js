import * as assert from "assert";
import { DocumentStore, PutCommandDataWithJson, DeleteCommandData } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    {
        //region defer_2
        const value1 = {
            "name": "My Product",
            "supplier": "suppliers/999-A",
            "@metadata": {
                "@collection": "Users"
            }
        };

        const putCommand1 =
            new PutCommandDataWithJson("products/999-A",
                null,
                value1);

        const value2 = {
            "name": "My Product",
            "supplier": "suppliers/999-A",
            "@metadata": {
                "@collection": "Suppliers"
            }
        };

        const putCommand2 =
            new PutCommandDataWithJson("suppliers/999-A",
                null,
                value2);

        const command3 = new DeleteCommandData("products/1-A", null);

        session.advanced.defer(putCommand1, putCommand2, command3);
        //endregion
    }
}

{
    let commands;
    //region defer_1
    session.advanced.defer(...commands);
    //endregion
}

