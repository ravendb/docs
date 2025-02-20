<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Type\StringArray;
use RavenDB\Type\TypedArray;

class IndexingNestedData
{
    public function createSampleData(): void
    {
        $store = new DocumentStore();
        try {
            # region sample_data
            // Creating sample data for the examples in this article:
            // ======================================================

            $onlineShops = [];

            // Shop1
            $onlineShops[] = new OnlineShop(
                shopName: "Shop1",
                email: "sales@shop1.com",
                tShirts: TShirtArray::fromArray([
                    new TShirt(color: "Red", size: "S", logo: "Bytes and Beyond", price: 25, sold: 2),
                    new TShirt(color: "Red", size: "M", logo: "Bytes and Beyond", price: 25, sold: 4),
                    new TShirt(color: "Blue", size: "M", logo: "Query Everything", price: 28, sold: 5),
                    new TShirt(color: "Green", size: "L", logo: "Data Driver", price: 30, sold:3)
                ])
            );

            // Shop2
            $onlineShops[] = new OnlineShop(
                    shopName: "Shop2",
                    email: "sales@shop2.com",
                    tShirts: TShirtArray::fromArray([
                        new TShirt(color: "Blue", size: "S", logo: "Coffee, Code, Repeat", price: 22, sold: 12 ),
                        new TShirt(color: "Blue", size: "M", logo: "Coffee, Code, Repeat", price: 22, sold: 7 ),
                        new TShirt(color: "Green", size: "M", logo: "Big Data Dreamer", price: 25, sold: 9 ),
                        new TShirt(color: "Black", size: "L", logo: "Data Mining Expert", price: 20, sold: 11)
                    ])
            );

            // Shop3
            $onlineShops[] = new OnlineShop(
                shopName: "Shop3",
                email: "sales@shop3.com",
                tShirts: TShirtArray::fromArray([
                    new TShirt(color: "Red", size: "S", logo: "Bytes of Wisdom", price: 18, sold: 2 ),
                    new TShirt(color: "Blue", size: "M", logo: "Data Geek", price: 20, sold: 6 ),
                    new TShirt(color: "Black", size: "L", logo: "Data Revolution", price: 15, sold: 8 ),
                    new TShirt(color: "Black", size: "XL", logo: "Data Revolution", price: 15, sold: 10 )
                ])
            );

            $session = $store->openSession();
            try {
                /** @var OnlineShop $shop */
                foreach ($onlineShops as $shop) {
                    $session->store($shop);
                }

                $session->SaveChanges();
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }

  public function QueryNestedData(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region simple_index_query_1
                // Query for all shop documents that have a red TShirt
                $shopsThatHaveRedShirts = $session
                    ->query(Shops_ByTShirt_Simple_IndexEntry::class, Shops_ByTShirt_Simple::class)
                     // Filter query results by a nested value
                    ->containsAny("colors", [ "red" ])
                    ->ofType(OnlineShop::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region simple_index_query_3
                // Query for all shop documents that have a red TShirt
                $shopsThatHaveRedShirts = $session->advanced()
                    ->documentQuery(Shops_ByTShirt_Simple_IndexEntry::class, Shops_ByTShirt_Simple::class)
                     // Filter query results by a nested value
                    ->containsAny("colors", [ "Red" ])
                    ->ofType(OnlineShop::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            # region results_1
            // Results will include the following shop documents:
            // ==================================================
            // * Shop1
            // * Shop3
            # endregion

            $session = $store->openSession();
            try {
                # region results_2
                // You want to query for shops containing "Large Green TShirts",
                // aiming to get only "Shop1" as a result since it has such a combination,
                // so you attempt this query:
                $greenAndLarge = $session
                    ->query(Shops_ByTShirt_Simple_IndexEntry::class, Shops_ByTShirt_Simple::class)
                    ->whereEquals("color", "green")
                    ->andAlso()
                    ->whereEquals("size", "L")
                    ->ofType(OnlineShop::class)
                    ->toList();

                // But, the results of this query will include BOTH "Shop1" & "Shop2"
                // since the index-entries do not keep the original sub-objects structure.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fanout_index_query_1
                // Query the fanout index:
                // =======================
                $shopsThatHaveMediumRedShirts = $session
                    ->query(Shops_ByTShirt_Fanout_IndexEntry::class, Shops_ByTShirt_Fanout::class)
                     // Query for documents that have a "Medium Red TShirt"
                     ->whereEquals("color", "red")
                    ->andAlso()
                    ->whereEquals("size", "M")
                    ->ofType(OnlineShop::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fanout_index_query_3
                // Query the fanout index:
                // =======================
                $shopsThatHaveMediumRedShirts = $session->advanced()
                    ->documentQuery(Shops_ByTShirt_Fanout_IndexEntry::class, Shops_ByTShirt_Fanout::class)
                     // Query for documents that have a "Medium Red TShirt"
                    ->whereEquals("color", "red")
                    ->andAlso()
                    ->whereEquals("size", "M")
                    ->ofType(OnlineShop::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            # region results_3
            // Query results:
            // ==============

            // Only the 'Shop1' document will be returned,
            // since it is the only document that has the requested combination within the TShirt list.
            # endregion

            $session = $store->openSession();
            try {
                # region fanout_index_query_4
                // Query the fanout index:
                // =======================
                /** @var Sales_ByTShirtColor_Fanout_IndexEntry $queryResult */
                $queryResult = $session
                    ->query(Sales_ByTShirtColor_Fanout_IndexEntry::class, Sales_ByTShirtColor_Fanout::class)
                     // Query for index-entries that contain "black"
                     ->whereEquals("color", "black")
                    ->firstOrDefault();

                // Get total sales for black TShirts
                $blackShirtsSales = $queryResult?->getTotalSales() ?? 0;
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region fanout_index_query_6
                // Query the fanout index:
                // =======================
                /** @var Sales_ByTShirtColor_Fanout_IndexEntry $queryResult */
                $queryResult = $session->advanced()
                    ->documentQuery(Sales_ByTShirtColor_Fanout_IndexEntry::class, Sales_ByTShirtColor_Fanout::class)
                    // Query for index-entries that contain "black"
                    ->whereEquals("color", "black")
                    ->firstOrDefault();

                // Get total sales for black TShirts
                $blackShirtsSales = $queryResult?->getTotalSales() ?? 0;
                # endregion
            } finally {
                $session->close();
            }

            # region results_4
            // Query results:
            // ==============

            // With the sample data used in this article,
            // The total sales revenue from black TShirts sold (in all shops) is 490.0
            # endregion
        } finally {
            $store->close();
        }
    }
}

# region online_shop_class
class OnlineShop
{
    private ?string $shopName = null;
    private ?string $email = null;
    public ?TShirtArray $tShirts = null; // Nested data

    public function __construct(
        ?string $shopName = null,
        ?string $email = null,
        ?TShirtArray $tShirts = null
    ) {
        $this->shopName = $shopName;
        $this->email = $email;
        $this->tShirts = $tShirts;
    }

    public function getShopName(): ?string
    {
        return $this->shopName;
    }

    public function setShopName(?string $shopName): void
    {
        $this->shopName = $shopName;
    }

    public function getEmail(): ?string
    {
        return $this->email;
    }

    public function setEmail(?string $email): void
    {
        $this->email = $email;
    }

    public function getTShirts(): ?TShirtArray
    {
        return $this->tShirts;
    }

    public function setTShirts(?TShirtArray $tShirts): void
    {
        $this->tShirts = $tShirts;
    }
}

class TShirt
{
    private ?string $color = null;
    private ?string $size = null;
    private ?string $logo = null;
    private ?float $price = null;
    private ?int $sold = null;

    public function __construct(
        ?string $color = null,
        ?string $size = null,
        ?string $logo = null,
        ?float $price = null,
        ?int $sold = null
    ) {
        $this->color = $color;
        $this->size = $size;
        $this->logo = $logo;
        $this->price = $price;
        $this->sold = $sold;
    }

    public function getColor(): ?string
    {
        return $this->color;
    }

    public function setColor(?string $color): void
    {
        $this->color = $color;
    }

    public function getSize(): ?string
    {
        return $this->size;
    }

    public function setSize(?string $size): void
    {
        $this->size = $size;
    }

    public function getLogo(): ?string
    {
        return $this->logo;
    }

    public function setLogo(?string $logo): void
    {
        $this->logo = $logo;
    }

    public function getPrice(): ?float
    {
        return $this->price;
    }

    public function setPrice(?float $price): void
    {
        $this->price = $price;
    }

    public function getSold(): ?int
    {
        return $this->sold;
    }

    public function setSold(?int $sold): void
    {
        $this->sold = $sold;
    }
}

class TShirtArray extends TypedArray
{
    public function __construct()
    {
        parent::__construct(TShirt::class);
    }
}
# endregion

# region simple_index
class Shops_ByTShirt_Simple_IndexEntry
{
    // The index-fields:
    private ?StringArray $colors = null;
    private ?StringArray $sizes = null;
    private ?StringArray $logos = null;

    public function getColors(): ?StringArray
    {
        return $this->colors;
    }

    public function setColors(?StringArray $colors): void
    {
        $this->colors = $colors;
    }

    public function getSizes(): ?StringArray
    {
        return $this->sizes;
    }

    public function setSizes(?StringArray $sizes): void
    {
        $this->sizes = $sizes;
    }

    public function getLogos(): ?StringArray
    {
        return $this->logos;
    }

    public function setLogos(?StringArray $logos): void
    {
        $this->logos = $logos;
    }
}

class Shops_ByTShirt_Simple extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from shop in docs.OnlineShops " .
            "select new { " .
                // Each index-field will hold a collection of nested values from the document
            "    colors = shop.t_shirts.Select(x => x.color)," .
            "    sizes = shop.t_shirts.Select(x => x.size)," .
            "    logos = shop.t_shirts.Select(x => x.logo)" .
            "}";
    }
}
# endregion

# region fanout_index_1
// A fanout map-index:
// ===================
class Shops_ByTShirt_Fanout_IndexEntry
{
    // The index-fields:
    private ?string $color = null;
    private ?string $size = null;
    private ?string $logo = null;

    public function getColor(): ?string
    {
        return $this->color;
    }

    public function setColor(?string $color): void
    {
        $this->color = $color;
    }

    public function getSize(): ?string
    {
        return $this->size;
    }

    public function setSize(?string $size): void
    {
        $this->size = $size;
    }

    public function getLogo(): ?string
    {
        return $this->logo;
    }

    public function setLogo(?string $logo): void
    {
        $this->logo = $logo;
    }
}

class Shops_ByTShirt_Fanout extends AbstractIndexCreationTask
{
        public function __construct()
        {
            parent::__construct();

            $this->map =
                "from shop in docs.OnlineShops " .
                "from shirt in shop.t_shirts " .
                // Creating MULTIPLE index-entries per document,
                // an index-entry for each sub-object in the TShirts list
                "select new {" .
                "    color = shirt.color," .
                "    size = shirt.size," .
                "    logo = shirt.logo" .
                "}";
    }
}
# endregion

# region fanout_index_2
// A fanout map-reduce index:
// ==========================
class Sales_ByTShirtColor_Fanout_IndexEntry {
    // The index-fields:
    private ?string $color = null;
    private ?int $itemsSold = null;
    private ?float $totalSales = null;

    public function getColor(): ?string
    {
        return $this->color;
    }

    public function setColor(?string $color): void
    {
        $this->color = $color;
    }

    public function getItemsSold(): ?int
    {
        return $this->itemsSold;
    }

    public function setItemsSold(?int $itemsSold): void
    {
        $this->itemsSold = $itemsSold;
    }

    public function getTotalSales(): ?float
    {
        return $this->totalSales;
    }

    public function setTotalSales(?float $totalSales): void
    {
        $this->totalSales = $totalSales;
    }
}
class Sales_ByTShirtColor_Fanout extends  AbstractIndexCreationTask
{

        public function __construct()
        {
            parent::__construct();

            # Creating MULTIPLE index-entries per document,
            # an index-entry for each sub-object in the TShirts list
            $this->map =
                "from shop in docs.OnlineShops " .
                "from shirt in shop.t_shirts " .
                // Creating MULTIPLE index-entries per document,
                // an index-entry for each sub-object in the TShirts list
                "select new {" .
                "    color = shirt.color, " .
                "    items_sold = shirt.sold, " .
                "    total_sales = shirt.price * shirt.sold" .
                "}";

            $this->reduce =
                "from result in results " .
                "group result by result.color " .
                "into g select new {" .
                "    color = g.Key," .
                    // Calculate sales per color
                "    items_sold = g.Sum(x => x.items_sold)," .
                "    total_sales = g.Sum(x => x.total_sales)" .
                "}";

    }
}
# endregion

# region fanout_index_js
class Shops_ByTShirt_JS extends AbstractJavaScriptIndexCreationTask
{
        public function __construct()
        {
            parent::__construct();

            $this->setMaps([
                "map('OnlineShops', function (shop){ 
                       var res = [];
                       shop.t_shirts.forEach(shirt => {
                           res.push({
                               color: shirt.color,
                               size: shirt.size,
                               logo: shirt.logo
                           })
                        });
                        return res;
                    })
                "
            ]);
    }
}
# endregion
