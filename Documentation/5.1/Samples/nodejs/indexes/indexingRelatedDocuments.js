import { AbstractIndexCreationTask, DocumentStore, IndexDefinition, PutIndexesOperation } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Product { }

//region indexing_related_documents_4
class Book {
    constructor(id, name) {
        this.id = id;
        this.name = name;
    }
}

class Author {
    constructor(id, name, bookIds) {
        this.id = id;
        this.name = name;
        this.bookIds = bookIds;
    }
}
//endregion

//region indexing_related_documents_2
class Products_ByCategoryName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Products.Select(product => new {     
            CategoryName = (this.LoadDocument(product.Category, "Categories")).Name 
        })`;
    }
}
//endregion

//region indexing_related_documents_5
class Authors_ByNameAndBooks extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Authors.Select(author => new {     
            name = author.name,     
            books = author.bookIds.Select(x => (this.LoadDocument(x, "Books")).name) 
        })`;

    }
}
//endregion

async function example() {
    {
        //region indexing_related_documents_3
        const indexDefinition = new IndexDefinition();
        indexDefinition.name = "Products/ByCategoryName";
        indexDefinition.maps = new Set([
            `from product in products    
             select new {        
                 CategoryName = LoadDocument(product.Category, ""Categories"").name    
            }`]);

        await store.maintenance.send(new PutIndexesOperation(indexDefinition));
        //endregion


        {
            //region indexing_related_documents_7
            const results = session
                .query({ indexName: "Products/ByCategoryName" })
                .whereEquals("CategoryName", "Beverages")
                .ofType(Product)
                .all();
            //endregion
        }
    }

    {
        //region indexing_related_documents_6
        const indexDefinition = new IndexDefinition();
        indexDefinition.name = "Authors/ByNameAndBooks";
        indexDefinition.maps = new Set([
            `from author in docs.Authors      
             select new 
             {          
                 name = author.name,          
                 books = author.bookIds.Select(x => LoadDocument(x, ""Books"").id)      
             }`
        ]);

        await store.maintenance.send(new PutIndexesOperation(indexDefinition));
        //endregion

        {
            //region indexing_related_documents_8
            const results = await session
                .query({ indexName: "Authors/ByNameAndBooks" })
                .whereEquals("name", "Andrzej Sapkowski")
                .whereEquals("books", "The Witcher")
                .ofType(Author)
                .all();
            //endregion
        }
    }

}


