import * as assert from "assert";
import { 
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

//region indexes_1
class Employees_ByReversedFirstName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Employees.Select(employee => new { " +
            "    FirstName = employee.FirstName.Reverse() " +
            "})";
    }
}
//endregion

//region indexes_3
class Item_Parse extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Items.Select(item => new {" +
            "    item = item, " +
            "    parts = item.version.Split('.', System.StringSplitOptions.None) " +
            "}).Select(this0 => new { " +
            "    majorWithDefault = this0.parts[0].ParseInt(), " + // will return default(int) in case of parsing failure
            "    majorWithCustomDefault = this0.parts[0].ParseInt(-1) " + // will return -1 in case of parsing failure
            "})";

        this.storeAllFields("Yes");
    }
}
//endregion

//region indexes_4
class Item {
    constructor(version) {
        this.version = version;
    }
}
//endregion

class Employee { }

async function indexingLinqExtensions() {
    
    const session = store.openSession();

    {
        //region indexes_2
        const results = await session
            .query({ indexName: "Employees/ByReversedFirstName" })
            .whereEquals("firstName", "treboR")
            .all();
        //endregion
    }

    {
        //region indexes_5
        const item1 = new Item("3.0.1");
        const item2 = new Item("Unknown");

        await session.store(item1);
        await session.store(item2);

        await session.saveChanges();

        const results = await session
            .query({ indexName: "Item/Parse" })
            .all();

        assert.strictEqual(2, results.length);
        assert.ok(results.some(x => x.majorWithDefault === 3));
        assert.ok(results.some(x => x.majorWithCustomDefault === 3));
        assert.ok(results.some(x => x.majorWithDefault === 0));
        assert.ok(results.some(x => x.majorWithCustomDefault === -1));
        //endregion
    }
}
