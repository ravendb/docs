import {
    AbstractCountersIndexCreationTask, AbstractRawJavaScriptCountersIndexCreationTask,
    DocumentStore
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region index_1
export class MyCounterIndex  extends AbstractCountersIndexCreationTask {
    constructor() {
        super();
        this.map = `from counter in docs.counters select new {
                Likes = counter.Value,
                Name = counter.Name,
                User = counter.DocumentId
                }`;
    }
}
//endregion

let map,reduce;
//region javaScriptIndexCreationTask
class CsharpCountersIndexCreationTask  extends AbstractCountersIndexCreationTask {
    public constructor() {
        super();

        this.map = map;

        this.reduce = reduce;
    }
}
//endregion

//region index_3
class MyCounterIndex extends AbstractRawJavaScriptCountersIndexCreationTask {
    public constructor() {
        super();

        this.maps.add(
            "counters.map('Companies', 'HeartRate', function (counter) {\n" +
            "return {\n" +
            "    heartBeat: counter.Value,\n" +
            "    name: counter.Name,\n" +
            "    user: counter.DocumentId\n" +
            "};\n" +
            "})"
        );
    }
}
//endregion

//region index_0
class Companies_ByCounterNames extends AbstractCountersIndexCreationTask{
    constructor() {
        super();
        this.map = "docs.Companies.Select(e => new {\n"+
            "e = e,\n"+
        "    counterNames = this.CounterNamesFor(e)\n"+
        "}).Select(this0 => new {\n"+
        "    CounterNames = Enumerable.ToArray(this0.counterNames)\n"+
        "})"
    }
}
//endregion


//region query1
const companies = session
    .query({index: "Companies_ByCounterNames"})
    .containsAny("counterNames", ["Likes"])
    .all();
//endregion