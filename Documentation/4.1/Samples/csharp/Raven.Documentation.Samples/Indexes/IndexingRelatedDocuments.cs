using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Indexes.Foo;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    namespace Foo
    {
        #region indexing_related_documents_4
        public class Book
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }

        public class Author
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public IList<string> BookIds { get; set; }
        }

        #endregion

        #region indexing_related_documents_2
        public class Products_ByCategoryName : AbstractIndexCreationTask<Product>
        {
            public class Result
            {
                public string CategoryName { get; set; }
            }

            public Products_ByCategoryName()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      CategoryName = LoadDocument<Category>(product.Category).Name
                                  };
            }
        }
        #endregion

        #region indexing_related_documents_5
        public class Authors_ByNameAndBooks : AbstractIndexCreationTask<Author>
        {
            public class Result
            {
                public string Name { get; set; }

                public IList<string> Books { get; set; }
            }

            public Authors_ByNameAndBooks()
            {
                Map = authors => from author in authors
                                 select new
                                 {
                                     Name = author.Name,
                                     Books = author.BookIds.Select(x => LoadDocument<Book>(x).Name)
                                 };
            }
        }
        #endregion
    }

    public class IndexingRelatedDocuments
    {
        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                #region indexing_related_documents_3
                store.Maintenance.Send(new PutIndexesOperation
                (
                    new IndexDefinition
                    {
                        Name = "Products/ByCategoryName",
                        Maps =
                        {
                            @"from product in products
                            select new
                            {
                                CategoryName = LoadDocument(product.Category, ""Categories"").Name
                            }"
                        }
                    }
                ));
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexing_related_documents_7
                    IList<Product> results = session
                        .Query<Products_ByCategoryName.Result, Products_ByCategoryName>()
                        .Where(x => x.CategoryName == "Beverages")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }

                #region indexing_related_documents_6
                store.Maintenance.Send(new PutIndexesOperation
                (
                    new IndexDefinition
                    {
                        Name = "Authors/ByNameAndBooks",
                        Maps =
                        {
                            @"from author in docs.Authors
                            select new
                            {
                                Name = author.Name,
                                Books = author.BookIds.Select(x => LoadDocument(x, ""Books"").Id)
                            }"
                        }
                    }
                ));
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexing_related_documents_8
                    IList<Author> results = session
                        .Query<Authors_ByNameAndBooks.Result, Authors_ByNameAndBooks>()
                        .Where(x => x.Name == "Andrzej Sapkowski" || x.Books.Contains("The Witcher"))
                        .OfType<Author>()
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
