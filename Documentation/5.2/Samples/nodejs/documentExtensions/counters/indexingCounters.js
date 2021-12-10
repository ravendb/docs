import {
    AbstractCountersIndexCreationTask,
    AbstractCsharpCountersIndexCreationTask,
    AbstractCsharpMultiMapIndexCreationTask,
    DocumentStore
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region index_1
export class MyCounterIndex  extends AbstractCsharpMultiMapIndexCreationTask {
    public constructor() {
        super();
        this.addMap(`from counter in docs.counters select new {
                Likes = counter.Value,
                Name = counter.Name,
                User = counter.DocumentId
                }`);
    }
}
//endregion

let map,reduce;
//region javaScriptIndexCreationTask
class CsharpCountersIndexCreationTask  extends AbstractCsharpCountersIndexCreationTask {
    public constructor() {
        super();

        this.map = map;

        this.reduce = reduce;
    }
}
//endregion

//region index_3
class CounterIndex extends AbstractCsharpCountersIndexCreationTask {
    public constructor() {
        super();

        this.map = "counters.Companies.HeartRate.Select(counter => new {\n" +
            "    heartBeat = counter.Value,\n" +
            "    name = counter.Name,\n" +
            "    user = counter.DocumentId\n" +
            "})";
    }
}
//endregion

//region index_0
class Companies_ByCounterNames extends AbstractCountersIndexCreationTask{
    constructor() {
        super();
        this.map = "from e in docs.Employees\n" +
            "let counterNames = counterNamesFor(e)\n" +
            "select new{\n" +
            "   counterNames = counterNames.ToArray()\n" +
            "}";
    }
}
//endregion


//region query1
const companies = session
    .query({index: "Companies_ByCounterNames"})
    .containsAny("counterNames", ["Likes"]).all()
//endregion