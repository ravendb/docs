import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    const query = session.query();
    //region lazy_1
    query.lazily();
    //endregion

    //region lazy_4
    query.countLazily();
    //endregion

    //region lazy_6
    query.executeLazy();
    //endregion
}

{
    const query = session.query();
    //region lazy_8
    query.executeLazy();
    //endregion
}

async function examples() {
    {
        //region lazy_2
        const employeesLazy = session
            .query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .lazily();

        const employees = await employeesLazy.getValue(); // query will be executed here
        //endregion
    }

    {
        //region lazy_5
        const countLazy = session
            .query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .countLazily();

        const count = await countLazy.getValue(); // query will be executed here
        //endregion
    }

    {
        //region lazy_7
        const suggestLazy = session
            .query({ indexName: "Employees_ByFullName" })
            .suggestUsing(builder => builder.byField("FullName", "Johne"))
            .executeLazy();

        const suggestResult = await suggestLazy.getValue(); // query will be executed here
        const suggestions = suggestResult["FullName"].suggestions;
        //endregion
    }

    {
        //region lazy_9
        const facetsLazy = session
            .query({ indexName: "Camera/Costs" })
            .aggregateUsing("facets/CameraFacets")
            .executeLazy();

        const facets = await facetsLazy.getValue(); // query will be executed here
        const results = facets["manufacturer"];
        //endregion
    }
}
