<?php

namespace RavenDB\Samples\Indexes;

use Product;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Type\StringArray;

class IndexingRelatedDocuments
{
    public function queries(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region indexing_related_documents_2
                $matchingProducts = $session
                    ->query(Products_ByCategoryName_IndexEntry::class, Products_ByCategoryName::class)
                    ->whereEquals("CategoryName", "Beverages")
                    ->ofType(Product::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexing_related_documents_5
                // Get all authors that have books with title: "The Witcher"
                $matchingAuthors = $session
                    ->query(Authors_ByBooks_IndexEntry::class, Authors_ByBooks::class)
                    ->containsAny("BookNames", ["The Witcher"])
                    ->ofType(Author::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexing_related_documents_7
                $matchingProducts = $session
                    ->query(Products_ByCategoryName_NoTracking_IndexEntry::class, Products_ByCategoryName_NoTracking::class)
                    ->whereEquals("CategoryName", "Beverages")
                    ->ofType(Product::class)
                    ->toList();

                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

/*
interface ILoadDocument
{
    # region syntax
    T LoadDocument<T>(string relatedDocumentId);

    T LoadDocument<T>(string relatedDocumentId, string relatedCollectionName);

    T[] LoadDocument<T>(IEnumerable<string> relatedDocumentIds);

    T[] LoadDocument<T>(IEnumerable<string> relatedDocumentIds, string relatedCollectionName);
    # endregion
}
*/

# region indexing_related_documents_1
class Products_ByCategoryName_IndexEntry
{
    private ?string $categoryName = null;

    public function getCategoryName(): ?string
    {
        return $this->categoryName;
    }

    public function setCategoryName(?string $categoryName): void
    {
        $this->categoryName = $categoryName;
    }
}
class Products_ByCategoryName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from product in docs.Products " .
            'let category = this.LoadDocument(product.Category, "Categories") ' .
            "select new { CategoryName = category.Name }";

            // Since NoTracking was Not specified,
            // then any change to either Products or Categories will trigger reindexing
    }
}
# endregion

# region indexing_related_documents_1_JS
class Products_ByCategoryName_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Call method 'load' to load the related Category document
        // The document ID to load is specified by 'product.Category'
        // The Name field from the related Category document will be indexed
        $this->setMaps([
            "map('products', function(product) { " .
            "    let category = load(product.Category, 'Categories') " .
            "    return { " .
            "        CategoryName: category.Name " .
            "    }; " .
            "})"
        ]);

        // Since noTracking was Not specified,
        // then any change to either Products or Categories will trigger reindexing

    }
}
# endregion

# region indexing_related_documents_3
// The referencing document
class Author
{
    private ?string $id = null;
    private ?string $name = null;

    // Referencing a list of related document IDs
    private ?StringArray $bookIds = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getBookIds(): ?StringArray
    {
        return $this->bookIds;
    }

    public function setBookIds(?StringArray $bookIds): void
    {
        $this->bookIds = $bookIds;
    }
}

// The related document
class Book
{
    private ?string $id = null;
    private ?string $name = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }
}
# endregion

# region indexing_related_documents_4
class Authors_ByBooks_IndexEntry
{
    private ?StringArray $bookNames = null;

    public function getBookNames(): ?StringArray
    {
        return $this->bookNames;
    }

    public function setBookNames(?StringArray $bookNames): void
    {
        $this->bookNames = $bookNames;
    }
}
class Authors_ByBooks extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from author in docs.Authors " .
            "select new " .
            "{" .
            // For each Book ID, call LoadDocument and index the book's name
            '    BookNames = author.BookIds.Select(x => LoadDocument(x, "Books").Name)' .
            "}";

        // Since NoTracking was Not specified,
        // then any change to either Authors or Books will trigger reindexing
    }
}
# endregion

# region indexing_related_documents_4_JS
class Authors_ByBooks_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            // For each Book ID, call 'load' and index the book's name
            "map('Author', function(author) {
                return {
                    Books: author.BooksIds.map(x => load(x, 'Books').Name)
                }
            })"
        ]);

        // Since NoTracking was Not specified,
        // then any change to either Authors or Books will trigger reindexing
    }
}
# endregion

# region indexing_related_documents_6
class Products_ByCategoryName_NoTracking_IndexEntry
{
    private ?string $categoryName = null;

    public function getCategoryName(): ?string
    {
        return $this->categoryName;
    }

    public function setCategoryName(?string $categoryName): void
    {
        $this->categoryName = $categoryName;
    }
}

class Products_ByCategoryName_NoTracking extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from product in docs.Products " .
            # Call NoTracking.LoadDocument to load the related Category document w/o tracking
            'let category = NoTracking.LoadDocument(product.Category, "Categories") ' .
            "select new {" .
            # Index the name field from the related Category document
            " CategoryName = category.Name " .
            "}";

        // Since NoTracking is used -
        // then only the changes to Products will trigger reindexing
    }
}
# endregion

# region indexing_related_documents_6_JS
class Products_ByCategoryName_NoTracking_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            // Call 'noTracking.load' to load the related Category document w/o tracking
            "map('products', function(product) {
                let category = noTracking.load(product.Category, 'Categories')
                return {
                    CategoryName: category.Name
                };
            })"
        ]);

        // Since noTracking is used -
        // then only the changes to Products will trigger reindexing
    }
}
# endregion
