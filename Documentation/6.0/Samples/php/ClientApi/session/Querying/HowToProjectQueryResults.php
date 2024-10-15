<?php

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldStorage;
use RavenDB\Documents\Queries\QueryData;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Order;

class HowToProjectQueryResults
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region projections_1
                // request name, city and country for all entities from 'Companies' collection
                $queryData = new QueryData(
                    [ "Name", "Address.city", "Address.country"],
                    [ "Name", "City", "Country"]
                );

                /** @var array<NameCityAndCountry> $results */
                $results = $session
                    ->query(Company::class)
                    ->selectFields(NameCityAndCountry::class, $queryData)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_2
                $queryData = new QueryData(
                    [ "ShipTo", "Lines[].ProductName" ],
                    [ "ShipTo", "Products" ]
                );

                /** @var array<ShipToAndProducts> $results */
                $results = $session->query(Order::class)
                    ->selectFields(ShipToAndProducts::class, $queryData)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_3
                /** @var array<FullName> $results */
                $results = $session->advanced()->rawQuery(FullName::class, "from Employees as e " .
                    "select {" .
                    "    FullName : e.FirstName + \" \" + e.LastName " .
                    "}")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_4
                /** @var array<Total> $results */
                $results = $session->advanced()->rawQuery(Total::class, "from Orders as o " .
                    "select { " .
                    "    Total : o.Lines.reduce( " .
                    "        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0) " .
                    "}")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_5
                /** @var array<OrderProjection> $results */
                $results = $session->advanced()->rawQuery(OrderProjection::class, "from Orders as o " .
                    "load o.Company as c " .
                    "select { " .
                    "    CompanyName: c.Name," .
                    "    ShippedAt: o.ShippedAt" .
                    "}")
                ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_6
                /** @var array<EmployeeProjection> $results */
                $results = $session->advanced()->rawQuery(EmployeeProjection::class, "from Employees as e " .
                    "select { " .
                    "    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), " .
                    "    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1, " .
                    "    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() " .
                    "}")
                ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_7
                /** @var array<EmployeeProjection> $results */
                $results = $session->advanced()->rawQuery(EmployeeProjection::class, "from Employees as e " .
                    "select { " .
                    "    Date : new Date(Date.parse(e.Birthday)), " .
                    "    Name : e.FirstName.substr(0,3) " .
                    "}")
                ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_8
                /** @var array<ContactDetails> $results */
                $results = $session->query(Company::class, Companies_ByContact::class)
                    ->selectFields(ContactDetails::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_10
                // query index 'Products_BySupplierName'
                // return documents from collection 'Products' that have a supplier 'Norske Meierier'
                // project them to 'Products'
                /** @var array<Product> $results */
                $results = $session->query(Products_BySupplierName_Result::class, Products_BySupplierName::class)
                    ->whereEquals("Name", "Norske Meierier")
                    ->ofType(Product::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_12
                /** @var array<Employee> $results */
                $results = $session->advanced()->rawQuery(Employee::class, "declare function output(e) { " .
                    "    var format = function(p){ return p.FirstName + \" \" + p.LastName; }; " .
                    "    return { FullName : format(e) }; " .
                    "} " .
                    "from Employees as e select output(e)")
                ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_13
                /** @var array<Employee> $results */
                $results = $session->advanced()->rawQuery(Employee::class, "from Employees as e " .
                    "select {" .
                    "     Name : e.FirstName, " .
                    "     Metadata : getMetadata(e)" .
                    "}")
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

# region projections_9_0
class Companies_ByContact extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from c in docs.Companies select new  { Name = c.Contact.Name, Phone = c.Phone } ";

        $this->storeAllFields(FieldStorage::yes()); // name and phone fields can be retrieved directly from index
    }
}
# endregion

# region projections_9_1
class ContactDetails
{
    private ?string $name = null;
    private ?string $phone = null;

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getPhone(): ?string
    {
        return $this->phone;
    }

    public function setPhone(?string $phone): void
    {
        $this->phone = $phone;
    }
}
# endregion

# region projections_11
class Products_BySupplierName extends AbstractIndexCreationTask
{
}

class Products_BySupplierName_Result
{
}
# endregion
