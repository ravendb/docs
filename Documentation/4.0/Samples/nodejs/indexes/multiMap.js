import { 
    DocumentStore,
    AbstractMultiMapIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

{
    //region multi_map_1
    class Dog extends Animal { }
    //endregion

    //region multi_map_2
    class Cat extends Animal { }
    //endregion

    //region multi_map_3
    class Animal {
        constructor(name) {
            this.name = name;
        } 
    }
    //endregion

    //region multi_map_4
    class Animals_ByName extends AbstractMultiMapIndexCreationTask {
        constructor() {
            super();

            this.addMap(`docs.Cats.Select(c => new {     
                name = c.name 
            })`);

            this.addMap(`docs.Dogs.Select(d => new {     
                name = d.name 
            })`);
        }
    }
    //endregion

    //region multi_map_1_0
    class Smart_Search extends AbstractMultiMapIndexCreationTask {

        constructor() {
            super();

            this.addMap(`docs.Companies.Select(c => new {     
                id = Id(c),     
                content = new string[] {         
                    c.name     
                },     
                displayName = c.name,     
                collection = this.MetadataFor(c)["@collection"] 
            })`);


            this.addMap(`docs.Products.Select(p => new {     
                id = Id(p),     
                content = new string[] {         
                    p.name     
                },     
                displayName = p.name,     
                collection = this.MetadataFor(p)["@collection"] 
            })`);

            this.addMap(`docs.Employees.Select(e => new {     
                id = Id(e),     
                content = new string[] {         
                    e.firstName,         
                    e.lastName     
                },     
                displayName = (e.firstName + " ") + e.lastName,     
                collection = this.MetadataFor(e)["@collection"] 
            })`);

            // mark 'content' field as analyzed which enables full text search operations
            this.index("content", "Search");

            // storing fields so when projection (e.g. ProjectInto)
            // requests only those fields
            // then data will come from index only, not from storage
            this.store("id", "Yes");
            this.store("displayName", "Yes");
            this.store("collection", "Yes");
        }
    }
    //endregion

    async function example() {
        const session = store.openSession();

        {
            //region multi_map_7
            const results = await session
                .query({ indexName: "Animals/ByName" })
                .whereEquals("name", "Mitzy")
                .all();
            //endregion
        }

        {
            //region multi_map_1_1
            const results = await session
                .query({ indexName: "Smart/Search" })
                .search("Content", "Lau*")
                .selectFields([ "id", "displayName", "collection" ])
                .all();

            for (const result of results) {
                console.log(result.collection + ": " + result.displayName);
                // Companies: Laughing Bacchus Wine Cellars
                // Products: Laughing Lumberjack Lager
                // Employees: Laura Callahan
            }
            //endregion
        }

    }
}
