package net.ravendb.Indexes;

import com.google.common.collect.Maps;
import com.google.common.collect.Sets;
import net.ravendb.client.documents.indexes.AbstractJavaScriptIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.IndexFieldOptions;
import org.apache.commons.collections.map.SingletonMap;

import java.util.*;

public class JavaScript {

    /*
    //region javaScriptindexes_1
    public static class Employees_ByFirstAndLastName extends AbstractJavaScriptIndexCreationTask {
        // ...
    }
    //endregion
    */

    /*
    //region javaScriptindexes_2
    public Employees_ByFirstAndLastName() {
        setMaps(Sets.newHashSet("map('Employees', function (employee){\n" +
            "                        return {\n" +
            "                            FirstName : employee.FirstName,\n" +
            "                            LastName : employee.LastName\n" +
            "                        };\n" +
            "                    })"));
    }
    //endregion
    */

    //region javaScriptindexes_6
    public static class Employees_ByFirstAndLastName extends AbstractJavaScriptIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            setMaps(Sets.newHashSet("map('Employees', function (employee){\n" +
                "                return {\n" +
                "                    FirstName : employee.FirstName,\n" +
                "                    LastName : employee.LastName\n" +
                "                };\n" +
                "        })"));
        }
    }
    //endregion

    //region javaScriptindexes_7
    public static class Employees_ByFullName extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String fullName;

            public String getFullName() {
                return fullName;
            }

            public void setFullName(String fullName) {
                this.fullName = fullName;
            }
        }

        public Employees_ByFullName() {
            setMaps(Sets.newHashSet("map('Employees', function (employee){\n" +
                "            return {\n" +
                "                FullName  : employee.FirstName + ' ' + employee.LastName\n" +
                "            };\n" +
                "        })"));
        }
    }
    //endregion


    //region javaScriptindexes_1_0
    public static class Employees_ByYearOfBirth extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private int yearOfBirth;

            public int getYearOfBirth() {
                return yearOfBirth;
            }

            public void setYearOfBirth(int yearOfBirth) {
                this.yearOfBirth = yearOfBirth;
            }
        }

        public Employees_ByYearOfBirth() {
            setMaps(Sets.newHashSet("map('Employees', function (employee){\n" +
                "            return {\n" +
                "                Birthday : employee.Birthday.Year\n" +
                "            }\n" +
                "        })"));
        }
    }

    //region javaScriptindexes_1_2
    public static class Employees_ByBirthday extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private Date birthday;

            public Date getBirthday() {
                return birthday;
            }

            public void setBirthday(Date birthday) {
                this.birthday = birthday;
            }
        }

        public Employees_ByBirthday() {
            setMaps(Sets.newHashSet("map('Employees', function (employee){\n" +
                "            return {\n" +
                "                Birthday : employee.Birthday\n" +
                "                                }\n" +
                "        })"));
        }
    }
    //endregion

    //region javaScriptindexes_1_4
    public static class Employees_ByCountry extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String country;

            public String getCountry() {
                return country;
            }

            public void setCountry(String country) {
                this.country = country;
            }
        }

        public Employees_ByCountry() {
            setMaps(Sets.newHashSet("map('Employees', function (employee){\n" +
                "            return {\n" +
                "                Country : employee.Address.Country\n" +
                "            }\n" +
                "        })"));
        }
    }
    //endregion

    //region javaScriptindexes_1_6
    public static class Employees_Query extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String[] query;

            public String[] getQuery() {
                return query;
            }

            public void setQuery(String[] query) {
                this.query = query;
            }
        }

        public Employees_Query() {
            setMaps(Sets.newHashSet("map('Employees', function (employee) {\n" +
                "            return {\n" +
                "                Query : [employee.FirstName,\n" +
                "                employee.LastName,\n" +
                "                employee.Title,\n" +
                "                employee.Address.City]\n" +
                "            }\n" +
                "        })"));

            IndexFieldOptions fieldOptions = new IndexFieldOptions();
            fieldOptions.setIndexing(FieldIndexing.SEARCH);
            getFields().put("Query", fieldOptions);
        }
    }
    //endregion

    //region multi_map_5
    public static class Animals_ByName extends AbstractJavaScriptIndexCreationTask {
        public Animals_ByName() {
            setMaps(Sets.newHashSet(
                "map('cats', function (c){ return {name: c.name}})",
                "map('dogs', function (d){ return {name: d.name}})"
            ));
        }
    }
    //endregion

    //region map_reduce_0_0
    public static class Products_ByCategory extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String category;
            private int count;
        }

        public Products_ByCategory() {
            setMaps(Sets.newHashSet("map('products', function(p){\n" +
                "            return {\n" +
                "                Category: load(p.Category, 'Categories').Name,\n" +
                "                Count: 1\n" +
                "            }\n" +
                "        })"));

            setReduce("groupBy(x => x.Category)\n" +
                "    .aggregate(g => {\n" +
                "        return {\n" +
                "            Category: g.key,\n" +
                "            Count: g.values.reduce((count, val) => val.Count + count, 0)\n" +
                "        };\n" +
                "    })");
        }
    }
    //endregion

    //region map_reduce_1_0
    public static class Product_Average_ByCategory extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String category;
            private double priceSum;
            private double priceAverage;
            private int productCount;

            public String getCategory() {
                return category;
            }

            public void setCategory(String category) {
                this.category = category;
            }

            public double getPriceSum() {
                return priceSum;
            }

            public void setPriceSum(double priceSum) {
                this.priceSum = priceSum;
            }

            public double getPriceAverage() {
                return priceAverage;
            }

            public void setPriceAverage(double priceAverage) {
                this.priceAverage = priceAverage;
            }

            public int getProductCount() {
                return productCount;
            }

            public void setProductCount(int productCount) {
                this.productCount = productCount;
            }
        }

        public Product_Average_ByCategory() {
            setMaps(Sets.newHashSet("map('products', function(product){\n" +
                "    return {\n" +
                "        Category: load(product.Category, 'Categories').Name,\n" +
                "        PriceSum: product.PricePerUnit,\n" +
                "        PriceAverage: 0,\n" +
                "        ProductCount: 1\n" +
                "    }\n" +
                "})"));

            setReduce("groupBy(x => x.Category)\n" +
                "        .aggregate(g => {\n" +
                "          var pricesum = g.values.reduce((sum,x) => x.PriceSum + sum,0);\n" +
                "          var productcount = g.values.reduce((sum,x) => x.ProductCount + sum,0);\n" +
                "          return {\n" +
                "            Category: g.key,\n" +
                "            PriceSum: pricesum,\n" +
                "            ProductCount: productcount,\n" +
                "            PriceAverage: pricesum / productcount\n" +
                "          }\n" +
                "        })");
        }
    }
    //endregion

    //region map_reduce_2_0
    public static class Product_Sales extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String product;
            private int count;
            private double total;

            public String getProduct() {
                return product;
            }

            public void setProduct(String product) {
                this.product = product;
            }

            public int getCount() {
                return count;
            }

            public void setCount(int count) {
                this.count = count;
            }

            public double getTotal() {
                return total;
            }

            public void setTotal(double total) {
                this.total = total;
            }
        }

        public Product_Sales() {
            setMaps(Sets.newHashSet("map('orders', function(order){\n" +
                "            var res = [];\n" +
                "            order.Lines.forEach(l => {\n" +
                "              res.push({\n" +
                "                Product: l.Product,\n" +
                "                Count: 1,\n" +
                "                Total:  (l.Quantity * l.PricePerUnit) * (1- l.Discount)\n" +
                "              })\n" +
                "            });\n" +
                "            return res;\n" +
                "        })"));

            setReduce("groupBy(x => x.Product)\n" +
                "    .aggregate(g => {\n" +
                "        return {\n" +
                "            Product : g.key,\n" +
                "            Count: g.values.reduce((sum, x) => x.Count + sum, 0),\n" +
                "            Total: g.values.reduce((sum, x) => x.Total + sum, 0)\n" +
                "        }\n" +
                "    })");
        }
    }
    //endregion

    //region map_reduce_3_0
    public static class Product_Sales_ByMonth extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String product;
            private Date month;
            private int count;
            private double total;

            public String getProduct() {
                return product;
            }

            public void setProduct(String product) {
                this.product = product;
            }

            public Date getMonth() {
                return month;
            }

            public void setMonth(Date month) {
                this.month = month;
            }

            public int getCount() {
                return count;
            }

            public void setCount(int count) {
                this.count = count;
            }

            public double getTotal() {
                return total;
            }

            public void setTotal(double total) {
                this.total = total;
            }
        }

        public Product_Sales_ByMonth() {
            setMaps(Sets.newHashSet("map('orders', function(order){\n" +
                "            var res = [];\n" +
                "            order.Lines.forEach(l => {\n" +
                "            res.push({\n" +
                "                Product: l.Product,\n" +
                "                Month: new Date( (new Date(order.OrderedAt)).getFullYear(),(new Date(order.OrderedAt)).getMonth(),1),\n" +
                "                Count: 1,\n" +
                "                Total: (l.Quantity * l.PricePerUnit) * (1- l.Discount)\n" +
                "            })\n" +
                "        });\n" +
                "        return res;\n" +
                "    })"));

            setReduce("groupBy(x => ({Product: x.Product, Month: x.Month}))\n" +
                "    .aggregate(g => {\n" +
                "        return {\n" +
                "            Product: g.key.Product,\n" +
                "            Month: g.key.Month,\n" +
                "            Count: g.values.reduce((sum, x) => x.Count + sum, 0),\n" +
                "            Total: g.values.reduce((sum, x) => x.Total + sum, 0)\n" +
                "        }\n" +
                "    })");

            setOutputReduceToCollection("MonthlyProductSales");
        }
    }
    //endregion

    //region fanout_index_def_1
    public static class Orders_ByProduct extends AbstractJavaScriptIndexCreationTask {
        public Orders_ByProduct() {
            setMaps(Sets.newHashSet("map('Orders', function (order){\n" +
                "    var res = [];\n" +
                "    order.Lines.forEach(l => {\n" +
                "        res.push({\n" +
                "            Product: l.Product,\n" +
                "            ProductName: l.ProductName\n" +
                "        })\n" +
                "    });\n" +
                "    return res;\n" +
                "})"));
        }
    }
    //endregion

    //region static_sorting2
    private static class Products_ByName extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String analyzedName;

            public String getAnalyzedName() {
                return analyzedName;
            }

            public void setAnalyzedName(String analyzedName) {
                this.analyzedName = analyzedName;
            }
        }

        public Products_ByName() {
            setMaps(Sets.newHashSet("map('products', function (u){\n" +
                "    return {\n" +
                "        Name: u.Name,\n" +
                "        _: {$value: u.Name, $name:'AnalyzedName'}\n" +
                "        };\n" +
                "    })"));

            IndexFieldOptions indexFieldOptions = new IndexFieldOptions();
            indexFieldOptions.setIndexing(FieldIndexing.SEARCH);
            indexFieldOptions.setAnalyzer("StandardAnalyzer");

            HashMap<String, IndexFieldOptions> fields = new HashMap<>();
            fields.put("AnalyzedName", indexFieldOptions);

            setFields(fields);
        }
    }
    //endregion

    //region indexing_related_documents_2
    public static class Products_ByCategoryName extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String categoryName;

            public String getCategoryName() {
                return categoryName;
            }

            public void setCategoryName(String categoryName) {
                this.categoryName = categoryName;
            }
        }

        public Products_ByCategoryName() {
            setMaps(Sets.newHashSet("map('products', function(product ){\n" +
                "            return {\n" +
                "                CategoryName : load(product .Category, 'Categories').Name,\n" +
                "            }\n" +
                "        })"));
        }
    }
    //endregion

    //region indexing_related_documents_5
    public static class Authors_ByNameAndBookNames extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String name;
            private List<String> books;

            public String getName() {
                return name;
            }

            public void setName(String name) {
                this.name = name;
            }

            public List<String> getBooks() {
                return books;
            }

            public void setBooks(List<String> books) {
                this.books = books;
            }
        }

        public Authors_ByNameAndBookNames() {
            setMaps(Sets.newHashSet("map('author', function(a){\n" +
                "            return {\n" +
                "                name: a.name,\n" +
                "                books: a.booksIds.forEach(x => load(x, 'Book').name)\n" +
                "            }\n" +
                "        })"));
        }
    }
    //endregion

    //region indexes_2
    public static class BlogPosts_ByCommentAuthor extends AbstractJavaScriptIndexCreationTask {
        public static class Result {
            private String[] authors;

            public String[] getAuthors() {
                return authors;
            }

            public void setAuthors(String[] authors) {
                this.authors = authors;
            }
        }

        public BlogPosts_ByCommentAuthor() {
            setMaps(Sets.newHashSet("map('BlogPosts', function(b){\n" +
                "            var names = [];\n" +
                "            b.comments.forEach(x => getNames(x, names));\n" +
                "                return {\n" +
                "                   authors : names\n" +
                "                };" +
                "            })"));

            java.util.Map<String, String> additionalSources = new HashMap<>();
            additionalSources.put("The Script", "function getNames(x, names){\n" +
                "        names.push(x.author);\n" +
                "        x.comments.forEach(x => getNames(x, names));\n" +
                "    }");

            setAdditionalSources(additionalSources);
        }
    }
    //endregion

    //region spatial_search_1
    public static class Events_ByNameAndCoordinates extends AbstractJavaScriptIndexCreationTask {
        public Events_ByNameAndCoordinates() {
            setMaps(Sets.newHashSet("map('events', function (e){\n" +
                "    return {\n" +
                "        name: e.name  ,\n" +
                "        coordinates: createSpatialField(e.latitude, e.longitude)\n" +
                "    };\n" +
                "})"));
        }
    }
    //endregion
}
