import { DocumentStore, AbstractJavaScriptIndexCreationTask } from "ravendb";
import assert from "assert";

const store = new DocumentStore();
const session = store.openSession();

{    
    //region online_shop_class
    class OnlineShop {
        constructor(
            shopName = '',
            email = '',
            tShirts = {} // Will contain the nested data
        ) {
            Object.assign(this, { shopName, email, tShirts });
        }
    }

    class TShirt {
        constructor(
            color = '',
            size = '',
            logo = '',
            price = 0,
            sold = 0
        ) {
            Object.assign(this, { color, size, logo, price, sold });
        }
    }
    //endregion

//region simple_index
    class Shops_ByTShirt_Simple extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            // Creating a SINGLE index-entry per document:
            this.map("OnlineShops", shop => {
                return {
                    // Each index-field will hold a collection of nested values from the document
                    colors: shop.tShirts.map(x => x.color),
                    sizes: shop.tShirts.map(x => x.size),
                    logos: shop.tShirts.map(x => x.logo)
                };
            });
        }
    }
//endregion

    //region fanout_index_1
    // A fanout map-index:
    // ===================
    class Shops_ByTShirt_Fanout extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            // Creating MULTIPLE index-entries per document,
            // an index-entry for each sub-object in the tShirts list
            this.map("OnlineShops", shop => {
                return shop.tShirts.map(shirt => {
                    return {
                        color: shirt.color,
                        size: shirt.size,
                        logo: shirt.logo
                    };
                });
            });
        }
    }
    //endregion

    //region fanout_index_2
    // A fanout map-reduce index:
    // ==========================
    class Sales_ByTShirtColor_Fanout extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            this.map("OnlineShops", shop => {
                return shop.tShirts.map(shirt => {
                    return {
                        // Define the index-fields:
                        color: shirt.color,
                        itemsSold: shirt.sold,
                        totalSales: shirt.price * shirt.sold
                    };
                });
            });

            this.reduce(results => results
                .groupBy(shirt => shirt.color)
                .aggregate(g => {
                    return {
                        // Calculate sales per color
                        color: g.key,
                        itemsSold: g.values.reduce((p, c) => p + c.itemsSold, 0),
                        totalSales: g.values.reduce((p, c) => p + c.totalSales, 0),
                    }
                }));
        }
    }
    //endregion

    async function indexingNestedData() {

        //region sample_data
        // Creating sample data for the examples in this article:
        // ======================================================
        
        const bulkInsert = store.bulkInsert();

        const onlineShops = [
            new OnlineShop("Shop1", "sales@shop1.com", [
                new TShirt("Red", "S", "Bytes and Beyond", 25, 2),
                new TShirt("Red", "M", "Bytes and Beyond", 25, 4),
                new TShirt("Blue", "M", "Query Everything", 28, 5),
                new TShirt("Green", "L", "Data Driver", 30, 3)
            ]),
            new OnlineShop("Shop2", "sales@shop2.com", [
                new TShirt("Blue", "S", "Coffee, Code, Repeat", 22, 12),
                new TShirt("Blue", "M", "Coffee, Code, Repeat", 22, 7),
                new TShirt("Green", "M", "Big Data Dreamer", 25, 9),
                new TShirt("Black", "L", "Data Mining Expert", 20, 11)
            ]),
            new OnlineShop("Shop3", "sales@shop3.com", [
                new TShirt("Red", "S", "Bytes of Wisdom", 18, 2),
                new TShirt("Blue", "M", "Data Geek", 20, 6),
                new TShirt("Black", "L", "Data Revolution", 15, 8),
                new TShirt("Black", "XL", "Data Revolution", 15, 10)
            ])
        ];

        for (const shop of onlineShops ) {
            await bulkInsert.store(shop);
        }

        await bulkInsert.finish();
        //endregion

        //region simple_index_query_1
        // Query for all shop documents that have a red TShirt
        const results = await session
            .query({ indexName: "Shops/ByTShirt/Simple" })
             // Filter query results by a nested value
            .containsAny("colors", ["red"])
            .all();
        //endregion

        //region results_1
        // Results will include the following shop documents:
        // ==================================================
        // * Shop1
        // * Shop3
        //endregion

        //region results_2
        // You want to query for shops containing "Large Green TShirts",
        // aiming to get only "Shop1" as a result since it has such a combination,
        // so you attempt this query:
        const greenAndLarge = await session
            .query({ indexName: "Shops/ByTShirt/Simple" })
            .containsAny("colors", ["green"])
            .andAlso()
            .containsAny("sizes", ["L"])
            .all();

        // But, the results of this query will include BOTH "Shop1" & "Shop2"
        // since the index-entries do not keep the original sub-objects structure.
        //endregion

        //region fanout_index_query_1
        // Query the fanout index:
        // =======================
        const shopsThatHaveMediumRedShirts = await session
            .query({ indexName: "Shops/ByTShirt/Fanout" })
             // Query for documents that have a "Medium Red TShirt"
            .whereEquals("color", "red")
            .andAlso()
            .whereEquals("size", "M")
            .all();
        //endregion

        //region results_3
        // Query results:
        // ==============

        // Only the 'Shop1' document will be returned,
        // since it is the only document that has the requested combination within the tShirt list.
        //endregion

        //region fanout_index_query_2
        // Query the fanout index:
        // =======================
        const queryResult = await session
            .query({ indexName: "Sales/ByTShirtColor/Fanout" })
             // Query for index-entries that contain "black"
            .whereEquals("color", "black")
            .firstOrNull();

        // Get total sales for black TShirts
        const blackShirtsSales = queryResult?.totalSales ?? 0;
        //endregion

        //region results_4
        // Query results:
        // ==============

        // With the sample data used in this article,
        // The total sales revenue from black TShirts sold (in all shops) is 490
        //endregion
    }    
}
