<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Samples\Indexes\BlogPost;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Samples\Infrastructure\Orders\Order;
use RavenDB\Samples\Infrastructure\Orders\Product;

# region filtering_0_4
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new {" .
            "    FirstName = employee.FirstName," .
            "    LastName = employee.LastName" .
            "})";
    }
}
# endregion

# region filtering_1_4
class Products_ByUnitsInStock extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Products.Select(product => new {" .
            "        UnitsInStock = product.UnitsInStock" .
            "    })";
    }
}
# endregion

# region filtering_7_4
class Orders_ByTotalPrice_Result
{
    public ?float $totalPrice = null;

    public function getTotalPrice(): ?float
    {
        return $this->totalPrice;
    }

    public function setTotalPrice(?float $totalPrice): void
    {
        $this->totalPrice = $totalPrice;
    }
}
class Orders_ByTotalPrice extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Orders.Select(order => new {" .
            "    TotalPrice = Enumerable.Sum(order.Lines, x => ((decimal)((((decimal) x.Quantity) * x.PricePerUnit) * (1M - x.Discount))))" .
            "})";
    }
}
# endregion

# region filtering_2_4
class Order_ByOrderLinesCount extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Orders.Select(order => new {" .
            "    Lines_Count = order.Lines.Count" .
            "})";
    }
}
# endregion

# region filtering_3_4
class Order_ByOrderLines_ProductName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Orders.Select(order => new {" .
            "    Lines_ProductName = order.Lines.Select(x => x.ProductName)" .
            "})";
    }
}
# endregion

# region filtering_5_4
class BlogPosts_ByTags extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.BlogPosts.Select(post => new {" .
            "    tags = post.tags" .
            "})";
    }
}
# endregion


class Filtering
{
    public function Samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_0_1
                /** @var array<Employee> $results */
                $results = $session
                        ->query(Employee::class, Employees_ByFirstAndLastName::class)                // query 'Employees/ByFirstAndLastName' index
                        ->whereEquals("FirstName", "Robert")
                        ->andAlso()
                        ->whereEquals("LastName", "King")    // filtering predicates
                        ->toList();                                                      // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_0_2
                /** @var array<Employee> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Employee::class, Employees_ByFirstAndLastName::class)    // query 'Employees/ByFirstAndLastName' index
                    ->whereEquals("FirstName", "Robert")                    // filtering predicates
                    ->andAlso()                                                  // by default OR is between each condition
                    ->whereEquals("LastName", "King")                       // filtering predicates
                    ->toList();                                                  // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_1_1
                /** @var array<Product> $results */
                $results = $session
                    ->query(Product::class, Products_ByUnitsInStock::class)  // query 'Products/ByUnitsInStock' index
                    ->whereGreaterThan("UnitsInStock", 50)            // filtering predicates
                    ->toList();                                  // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_1_2
                /** @var array<Product> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Product::class, Products_ByUnitsInStock::class)  // query 'Products/ByUnitsInStock' index
                    ->whereGreaterThan("UnitsInStock", 50)          // filtering predicates
                    ->toList();                                          // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_7_1
                /** @var array<Order> $results */
                $results = $session
                    ->query(Orders_ByTotalPrice_Result::class, Orders_ByTotalPrice::class)
                    ->whereGreaterThan("TotalPrice", 50)
                    ->ofType(Order::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_7_2
                /** @var array<Order> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Orders_ByTotalPrice_Result::class, Orders_ByTotalPrice::class)
                    ->whereGreaterThan("TotalPrice", 50)
                    ->ofType(Order::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_2_1
                /** @var array<Order> $results */
                $results = $session
                    ->query(Order::class, Order_ByOrderLinesCount::class)  // query 'Order/ByOrderLinesCount' index
                    ->whereGreaterThan("Lines_Count", 50)                               // filtering predicates
                    ->toList();                                                                       // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_2_2
                /** @var array<Order> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Order::class, Order_ByOrderLinesCount::class) // query 'Order/ByOrderLinesCount' index
                    ->whereGreaterThan("Lines_Count", 50)                                   // filtering predicates
                    ->toList();                                                                           // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_3_1
                /** @var array<Order> $results */
                $results = $session
                    ->query(Order::class, Order_ByOrderLines_ProductName::class)                                 // query 'Order/ByOrderLines/ProductName' index
                    ->whereEquals("Lines_ProductName", "Teatime Chocolate Biscuits")    // filtering predicates
                    ->toList();                                                                          // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_3_2
                /** @var array<Order> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Order::class, Order_ByOrderLines_ProductName::class) // query 'Order/ByOrderLines/ProductName' index
                    ->whereEquals("Lines_ProductName", "Teatime Chocolate Biscuits")               // filtering predicates
                    ->toList();                                                                                  // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_4_1
                /** @var array<Employee> $results */
                $results = $session
                    ->query(Employee::class, Employees_ByFirstAndLastName::class)    // query 'Employees/ByFirstAndLastName' index
                    ->whereIn("FirstName", ["Robert", "Nancy"])    // filtering predicates (remember to add `Raven.Client.Linq` namespace to usings)
                    ->toList();                                             // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_4_2
                /** @var array<Employee> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Employee::class, Employees_ByFirstAndLastName::class)    // query 'Employees/ByFirstAndLastName' index
                    ->whereIn("FirstName", [ "Robert", "Nancy" ])       // filtering predicates
                    ->toList();                                                  // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_5_1
                /** @var array<BlogPost> $results */
                $results = $session
                    ->query(BlogPost::class, BlogPosts_ByTags::class) // query 'BlogPosts/ByTags' index
                    ->containsAny("Tags", [ "Development", "Research" ])                // filtering predicates
                    ->toList();                                                                  // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_5_2
                /** @var array<BlogPost> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(BlogPost::class, BlogPosts_ByTags::class) // query 'BlogPosts/ByTags' index
                    ->containsAny("Tags", [ "Development", "Research" ])                     // filtering predicates
                    ->toList();                                                                       // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_6_1
                /** @var array<BlogPost> $results */
                $results = $session
                    ->query(BlogPost::class, BlogPosts_ByTags::class) // query 'BlogPosts/ByTags' index
                    ->containsAll("Tags", [ "Development", "Research" ])                // filtering predicates
                    ->toList();                                                                  // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_6_2
                /** @var array<BlogPost> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(BlogPost::class, BlogPosts_ByTags::class)    // query 'BlogPosts/ByTags' index
                    ->containsAll("Tags", [ "Development", "Research" ])                        // filtering predicates
                    ->toList();                                                                          // materialize query by sending it to server for processing
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_8_1
                // return all products which name starts with 'ch'
                /** @var array<Product> $results */
                $results = $session
                    ->query(Product::class)
                    ->whereStartsWith("Name", "ch")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_8_2
                // return all products which name starts with 'ch'
                /** @var array<Product> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Product::class)
                    ->whereStartsWith("Name", "ch")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_9_1
                // return all products which name ends with 'ra'
                /** @var array<Product> $results */
                $results = $session
                    ->query(Product::class)
                    ->whereEndsWith("Name", "ra")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_9_2
                // return all products which name ends with 'ra'
                /** @var array<Product> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(Product::class)
                    ->whereEndsWith("Name", "ra")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region filtering_10_1
                // return all orders that were shipped to 'Albuquerque'
                /** @var array<Order> $results */
                $results = $session
                    ->query(Order::class)
                    ->whereEquals("ShipTo_City", "Albuquerque")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_10_2
                // return all orders that were shipped to 'Albuquerque'
                $results = $session
                    ->advanced()
                    ->documentQuery(Order::class)
                    ->whereEquals("ShipTo_City", "Albuquerque")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_11_1
                /** @var Order $order */
                $order = $session
                    ->query(Order::class)
                    ->whereEquals("Id", "orders/1-A")
                    ->firstOrDefault();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_11_2
                /** @var Order $order */
                $order = $session
                    ->advanced()
                    ->documentQuery(Order::class)
                    ->whereEquals("Id", "orders/1-A")
                    ->firstOrDefault();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_12_1
                /** @var array<Order> $orders */
                $orders = $session
                    ->query(Order::class)
                    ->whereStartsWith("Id", "orders/1")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region filtering_12_2
                /** @var array<Order> $orders */
                $orders = $session
                    ->advanced()
                    ->documentQuery(Order::class)
                    ->whereStartsWith("Id", "orders/1")
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
