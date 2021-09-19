package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.northwind.Company;

import java.util.Arrays;
import java.util.List;

public class IndexingCounters {
    private interface IFoo {
        //region syntax
        List<String> counterNamesFor(Object doc);
        //endregion
    }

    //region index
    public static class Companies_ByCounterNames extends AbstractIndexCreationTask {
        public Companies_ByCounterNames() {
            map = "from e in docs.Employees\n" +
                "let counterNames = counterNamesFor(e)\n" +
                "select new\n" +
                "{\n" +
                "   counterNames = counterNames.ToArray()\n" +
                "}";
        }
    }
    //endregion

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region query1
                // return all companies that have 'Likes' counter
                List<myClass.Company> companies = session
                    .query(myClass.Company.class, myClass.Companies_ByCounterNames.class)
                    .containsAny("counterNames",  Arrays.asList("companies_likes"))
                    .selectFields(myClass.Company.class, "likes")
                    .toList();
                //endregion
            }
        }
    }

    //region
    protected void addMap(String map)
    //endregion
    {
        if (map == null) {
            throw new IllegalArgumentException("Map cannot be null");
        }
        maps.add(map);
    }

    //region index_1
    public static class MyCounterIndex extends AbstractCountersIndexCreationTask {
        public MyCounterIndex() {
            map = "counters.Companies.HeartRate.Select(counter => new {\n" +
                    "    heartBeat = counter.Value,\n" +
                    "    name = counter.Name,\n" +
                    "    user = counter.DocumentId\n" +
                    "})";
        }
    }
    //endregion

    //region index_3
    public static class MyMultiMapCounterIndex extends AbstractJavaScriptCountersIndexCreationTask {
        public MyMultiMapCounterIndex() {
            setMaps(Collections.singleton("counters.map('Blogposts', 'Likes', function (counter) {\n" +
                "return {\n" +
                "    ikes: counter.Value,\n" +
                "    name: counter.Name,\n" +
                "    bolg post: counter.DocumentId\n" +
                "};\n" +
                "})"));
        }
    }
    //endregion


}
