//region group_by_using
import { DocumentStore, GroupBy, GroupByField } from "ravendb";
//endregion

const store = new DocumentStore();
const session = store.openSession();

async function examples() {
    {
        //region group_by_1
        const orders = await session.query({ collection: "Orders" })
            .groupBy("ShipTo.Country")
            .selectKey("ShipTo.Country", "Country")
            .selectSum(new GroupByField("Lines[].Quantity", "OrderedQuantity"))
            .ofType(CountryAndQuantity.class)
            .all();
        //endregion
    }

    {
        //region group_by_2
        const results = await session.query({ collection: "Orders" })
            .groupBy("Employee", "Company")
            .selectKey("Employee", "EmployeeIdentifier")
            .selectKey("Company")
            .selectCount()
            .ofType(CountByCompanyAndEmployee)
            .all();
        //endregion
    }

    {
        //region group_by_3
        const orders = await session.query({ collection: "Orders" })
            .groupBy("Employee", "Company")
            .selectKey("key()", "EmployeeCompanyPair")
            .selectCount("Count")
            .ofType(CountOfEmployeeAndCompanyPairs)
            .all();
        //endregion
    }

    {
        //region group_by_4
        const products = await session.query({ collection: "Orders" })
            .groupBy(GroupBy.array("Lines[].Product"))
            .selectKey("key()", "Products")
            .selectCount()
            .ofType(ProductsInfo)
            .all();
        //endregion
    }

    {
        //region group_by_5
        const results = await session.advanced.documentQuery({ collection: "Orders" })
            .groupBy("Lines[].Product", "ShipTo.Country")
            .selectKey("Lines[].Product", "Product")
            .selectKey("ShipTo.Country", "Country")
            .selectCount()
            .ofType(ProductInfo)
            .all();
        //endregion
    }

    {
        //region group_by_6
        const results = await session.query({ collection: "Orders" })
            .groupBy(GroupBy.array("Lines[].Product"), GroupBy.array("Lines[].Quantity"))
            .selectKey("Lines[].Product", "Product")
            .selectKey("Lines[].Quantity", "Quantity")
            .selectCount()
            .ofType(ProductInfo)
            .all();
        //endregion
    }

    {
        //region group_by_7
        const results = await session.query({ collection: "Orders" })
            .groupBy(GroupBy.array("Lines[].Product"))
            .selectKey("key()", "Products")
            .selectCount()
            .ofType(ProductsInfo)
            .all();
        //endregion
    }

    {
        //region group_by_8
        const results = await session.query({ collection: "Orders" })
            .groupBy(GroupBy.array("Lines[].Product"), GroupBy.field("ShipTo.Country"))
            .selectKey("Lines[].Product", "Products")
            .selectKey("ShipTo.Country", "Country")
            .selectCount()
            .ofType(ProductsInfo)
            .toList();
        //endregion
    }

    {
        //region group_by_9
        const results = await session.query({ collection: "Orders" })
            .groupBy(GroupBy.array("Lines[].Product"), GroupBy.array("Lines[].Quantity"))
            .selectKey("Lines[].Product", "Products")
            .selectKey("Lines[].Quantity", "Quantities")
            .selectCount()
            .ofType(ProductsInfo)
            .all();
        //endregion
    }

    class CountByCompanyAndEmployee { }

    class Order { }

    class CountryAndQuantity { }

    class EmployeeAndCompany { }

    class CountOfEmployeeAndCompanyPairs { }

    class ProductInfo { }

    class ProductsInfo { }
}
