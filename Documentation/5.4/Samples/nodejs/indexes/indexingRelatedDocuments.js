import {
    AbstractCsharpIndexCreationTask,
    AbstractJavaScriptIndexCreationTask,
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Product { }

class Syntax {
    //region syntax_JS
    object load(relatedDocumentId, relatedCollectionName);
    //endregion
}

//region indexing_related_documents_1
class Products_ByCategoryName extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();
        
        // Call LoadDocument to load the related Category document
        // The document ID to load is specified by 'product.Category'
        // The Name field from the related Category document will be indexed
        
        this.map = `docs.Products.Select(product => new {
            CategoryName = (this.LoadDocument(product.Category, "Categories")).Name 
        })`;

        // Since NoTracking was Not specified,
        // then any change to either Products or Categories will trigger reindexing 
    }
}
//endregion

//region indexing_related_documents_1_JS
class Products_ByCategoryName_JS extends AbstractJavaScriptIndexCreationTask {
    constructor () {
        super();

        const { load } = this.mapUtils();

        this.map("Products", product => {
            return {
                // Call method 'load' to load the related Category document
                // The document ID to load is specified by 'product.Category'
                // The Name field from the related Category document will be indexed                
                categoryName: load(product.Category, "Categories").Name

                // Since NoTracking was Not specified,
                // then any change to either Products or Categories will trigger reindexing
            };
        });
    }
}
//endregion

//region indexing_related_documents_3
// The referencing document
class Author {
    constructor(id, name, bookIds) {
        this.id = id;
        this.name = name;
        
        // Referencing a list of related document IDs
        this.bookIds = bookIds;
    }
}
// The related document
class Book {
    constructor(id, name) {
        this.id = id;
        this.name = name;
    }
}
//endregion

//region indexing_related_documents_4
class Authors_ByBooks extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        // For each Book ID, call LoadDocument and index the book's name
        this.map = `docs.Authors.Select(author => new {
            BookNames = author.bookIds.Select(x => (this.LoadDocument(x, "Books")).name) 
        })`;

        // Since NoTracking was Not specified,
        // then any change to either Authors or Books will trigger reindexing
    }
}
//endregion

//region indexing_related_documents_4_JS
class Authors_ByBooks_JS extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        const { load } = this.mapUtils();

        this.map("Authors", author => {
            return {
                // For each Book ID, call 'load' and index the book's name
                BookNames: author.bookIds.map(x => load(x, "Books").name)

                // Since NoTracking was Not specified,
                // then any change to either Products or Categories will trigger reindexing
            };
        });
    }
}
//endregion

//region indexing_related_documents_6
class Products_ByCategoryName_NoTracking extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        // Call NoTracking.LoadDocument to load the related Category document w/o tracking
        this.map = `docs.Products.Select(product => new {
            CategoryName = (this.NoTracking.LoadDocument(product.Category, "Categories")).Name 
        })`;

        // Since NoTracking is used -
        // then only the changes to Products will trigger reindexing
    }
}
//endregion

//region indexing_related_documents_6_JS
class Products_ByCategoryName_NoTracking_JS extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        const { noTracking } = this.mapUtils();

        this.map("Products", product => {
            return {
                // Call 'noTracking.load' to load the related Category document w/o tracking
                categoryName: noTracking.load(product.Category, "Categories").Name
            };
        });
        
        // Since noTracking is used -
        // then only the changes to Products will trigger reindexing
    }
}
//endregion

async function Queries() {
    {
        //region indexing_related_documents_2
        const matchingProducts = await session
            .query({indexName: "Products/ByCategoryName"})
            .whereEquals("CategoryName", "Beverages")
            .all();
        //endregion
    }
    {
        //region indexing_related_documents_5
        const matchingProducts = await session
            .query({indexName: "Authors/ByBooks"})
            .whereEquals("BookNames", "The Witcher")
            .all();
        //endregion
    }
    {
        //region indexing_related_documents_7
        const matchingProducts = await session
            .query({indexName: "Products/ByCategoryName/NoTracking"})
            .whereEquals("CategoryName", "Beverages")
            .all();
        //endregion
    }
}


