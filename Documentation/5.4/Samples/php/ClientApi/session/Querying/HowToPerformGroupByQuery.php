<?php

use RavenDB\Documents\Queries\GroupBy;
use RavenDB\Documents\Session\GroupByField;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Type\StringArray;

class HowToPerformGroupByQuery
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region group_by_1
                /** @var array<CountryAndQuantity> $orders */
                $orders = $session->query(Order::class)
                    ->groupBy("ShipTo.Country")
                    ->selectKey("ShipTo.Country", "Country")
                    ->selectSum(new GroupByField("Lines[].Quantity", "OrderedQuantity"))
                    ->ofType(CountryAndQuantity::class)
                    ->toList();

                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_2
                $results = $session->query(Order::class)
                    ->groupBy("Employee", "Company")
                    ->selectKey("Employee", "EmployeeIdentifier")
                    ->selectKey("Company")
                    ->selectCount()
                    ->ofType(CountByCompanyAndEmployee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_3
                /** @var array<CountOfEmployeeAndCompanyPairs> $orders */
                $orders = $session->query(Order::class)
                    ->groupBy("Employee", "Company")
                    ->selectKey("key()", "EmployeeCompanyPair")
                    ->selectCount("Count")
                    ->ofType(CountOfEmployeeAndCompanyPairs::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_4
                /** @var array<ProductsInfo> $products */
                $products = $session->query(Order::class)
                    ->groupBy(GroupBy::array("Lines[].Product"))
                    ->selectKey("key()", "Products")
                    ->selectCount()
                    ->ofType(ProductsInfo::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_5
                /** @var array<ProductInfo> $results */
                $results = $session->advanced()->documentQuery(Order::class)
                    ->groupBy("Lines[].Product", "ShipTo.Country")
                    ->selectKey("Lines[].Product", "Product")
                    ->selectKey("ShipTo.Country", "Country")
                    ->selectCount()
                    ->ofType(ProductInfo::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_6
                /** @var array<ProductInfo> $results */
                $results = $session->query(Order::class)
                    ->groupBy(GroupBy::array("Lines[].Product"), GroupBy::array("Lines[].Quantity"))
                    ->selectKey("Lines[].Product", "Product")
                    ->selectKey("Lines[].Quantity", "Quantity")
                    ->selectCount()
                    ->ofType(ProductInfo::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_7
                /** @var array<ProductsInfo> $results */
                $results = $session->query(Order::class)
                    ->groupBy(GroupBy::array("Lines[].Product"))
                    ->selectKey("key()", "Products")
                    ->selectCount()
                    ->ofType(ProductsInfo::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_8
                /** @var array<ProductsInfo> $results */
                $results = $session->query(Order::class)
                    ->groupBy(GroupBy::array("Lines[].Product"), GroupBy::field("ShipTo.Country"))
                    ->selectKey("Lines[].Product", "Products")
                    ->selectKey("ShipTo.Country", "Country")
                    ->selectCount()
                    ->ofType(ProductsInfo::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region group_by_9
                /** @var array<ProductsInfo> $results */
                $results = $session->query(Order::class)
                    ->groupBy(GroupBy::array("Lines[].Product"), GroupBy::array("Lines[].Quantity"))
                    ->selectKey("Lines[].Product", "Products")
                    ->selectKey("Lines[].Quantity", "Quantities")
                    ->selectCount()
                    ->ofType(ProductsInfo::class)
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

class CountByCompanyAndEmployee
{
    private ?int $count = null;
    private ?string $company = null;
    private ?string $employeeIdentifier = null;

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getCompany(): ?string
    {
        return $this->company;
    }

    public function setCompany(?string $company): void
    {
        $this->company = $company;
    }

    public function getEmployeeIdentifier(): ?string
    {
        return $this->employeeIdentifier;
    }

    public function setEmployeeIdentifier(?string $employeeIdentifier): void
    {
        $this->employeeIdentifier = $employeeIdentifier;
    }
}

class Order
{

}

class CountryAndQuantity
{
    private ?string $country = null;
    private ?int $orderedQuantity = null;

    public function getCountry(): ?string
    {
        return $this->country;
    }

    public function setCountry(?string $country): void
    {
        $this->country = $country;
    }

    public function getOrderedQuantity(): ?int
    {
        return $this->orderedQuantity;
    }

    public function setOrderedQuantity(?int $orderedQuantity): void
    {
        $this->orderedQuantity = $orderedQuantity;
    }
}

class EmployeeAndCompany
{
    private ?string $employee = null;
    private ?string $company = null;

    public function getEmployee(): ?string
    {
        return $this->employee;
    }

    public function setEmployee(?string $employee): void
    {
        $this->employee = $employee;
    }

    public function getCompany(): ?string
    {
        return $this->company;
    }

    public function setCompany(?string $company): void
    {
        $this->company = $company;
    }
}

class CountOfEmployeeAndCompanyPairs
{
    private ?EmployeeAndCompany $employeeCompanyPair = null;
    private ?int $count = null;

    public function getEmployeeCompanyPair(): ?EmployeeAndCompany
    {
        return $this->employeeCompanyPair;
    }

    public function setEmployeeCompanyPair(?EmployeeAndCompany $employeeCompanyPair): void
    {
        $this->employeeCompanyPair = $employeeCompanyPair;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }
}

class ProductInfo
{
    private ?int $count = null;
    private ?string $product = null;
    private ?int $quantity = null;

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getProduct(): ?string
    {
        return $this->product;
    }

    public function setProduct(?string $product): void
    {
        $this->product = $product;
    }

    public function getQuantity(): ?int
    {
        return $this->quantity;
    }

    public function setQuantity(?int $quantity): void
    {
        $this->quantity = $quantity;
    }
}

class ProductsInfo
{
    private ?int $count = null;
    private ?StringArray $products = null;
    private ?int $quantity = null;

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getProducts(): ?StringArray
    {
        return $this->products;
    }

    public function setProducts(?StringArray $products): void
    {
        $this->products = $products;
    }

    public function getQuantity(): ?int
    {
        return $this->quantity;
    }

    public function setQuantity(?int $quantity): void
    {
        $this->quantity = $quantity;
    }

}
