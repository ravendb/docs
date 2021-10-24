import {
    DocumentStore,
    AbstractCountersIndexCreationTask,

} from "ravendb";

let employee
const store = new DocumentStore();
const session = store.openSession();


{
    //region index_1
    class MyCounterIndex extends AbstractCountersIndexCreationTask {
         constructor() {
            super();

            this.map = "counters.Companies.HeartRate.Select(counter => new {\n" +
                "    heartBeat = counter.Value,\n" +
                "    name = counter.Name,\n" +
                "    user = counter.DocumentId\n" +
                "})";
        }
    }
    //endregion
}

{
    //region index_2
    //Didn't understand how to do the addMapsForAll
    //endregion
}

{
    //TODO - Didn't understand how to do it
    //region javaScriptIndexCreationTask
    class MyMultiMapCounterIndex extends AbstractCountersIndexCreationTask{
         constructor() {
            super();

        }
    }
    //endregion

    //TODO - Didn't understand how to do it
    //region index_3
    //Didn't understand how to do the sample to MyMultiMapCounterIndex
    //endregion
}

{
    //region syntax
    let counterNames = counterNamesFor(employee);
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
        .containsAny("counterNames", Lists.newArrayList("Likes"))
    //endregion
}

