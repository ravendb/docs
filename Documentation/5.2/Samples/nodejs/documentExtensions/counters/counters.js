import {
    DocumentStore,

} from "ravendb";


class Product {
}

const store = new DocumentStore();
{
    //region counters_region_query
    const session = store.openSession();
    const query = session.query({collection: "Products"})
        .selectFields("ProductLikes");
    const queryResults = query.all();

    for (let counterValue in queryResults) {
        console.log("ProductLikes: " + counterValue);
    }
    //endregion
}

const session = store.openSession();
{
    //region counters_region_rawqueries_counter
    //Various RQL expressions sent to the server using counter()
    //Returned Counter value is accumulated
    const rawquery1 = session.advanced
        .rawQuery("from products as p select counter(p, \"ProductLikes\")")
        .all();

    const rawquery2 = session.advanced
        .rawQuery("from products select counter(\"ProductLikes\") as ProductLikesCount")
        .all();

    const rawquery3 = session.advanced
        .rawQuery("from products where PricePerUnit > 50 select Name, counter(\"ProductLikes\")")
        .all();
    //endregion

    //
}

{
    //region counters_region_rawqueries_counterRaw
    //An RQL expression sent to the server using counterRaw()
    //Returned Counter value is distributed
    const query = session.advanced
        .rawQuery("from users as u select counterRaw(u, \"downloads\")")
        .all();
    //endregion
}


{


    //region counters_region_load_include1
    const productPage = await session.load("orders/1-A", {
        includes: includeBuilder => includeBuilder
            .includeDocuments("products/1-C")
            .includeCounters(["ProductLikes", "ProductDislikes"])
    });
    //endregion
}

{
    //region counters_region_load_include2
    const productPage = await session.load("orders/1-A", {
            documentType: Product,
            includes: includeBuilder =>
                includeBuilder.includeDocuments("products/1-C")
                    .includeCounters(["ProductLikes", "ProductDislikes"])
        }
    );
    //endregion
}

{
    //region counters_region_query_include_single_Counter
    const query = session.query({collection: "Product"})
        .include(includeBuilder =>
            includeBuilder.includeCounter("ProductLikes"));
    //endregion
}

{
    //region counters_region_query_include_multiple_Counters
    const query = session.query({collection: "Product"})
        .include(includeBuilder =>
            includeBuilder.includeCounters("ProductLikes", "ProductDownloads"));
    //endregion
}

{
    //region CountersFor-definition

    const counter = session.countersFor("documentid")
    //endregion

    //region Increment-definition
    const incerment = store.openSession();
    session.countersFor("documentid").increment("likes", 100);
    ////endregion
}

{
    //region bulk-insert-counters
    const query = session.query({collection: "User"})
        .whereBetween("Age", 0, 30)
    const result = query.all();

    const bulkInsert = store.bulkInsert();
    for (let user = 0; user < result.length; user++) {
        let userId = result[user].id;

        // Choose document
        let countersFor = bulkInsert.countersFor(userId);

        // Add or Increment a counter
        await bulkInsert.countersFor(userId).increment("downloaded", 100);
    }
    //endregion
}
