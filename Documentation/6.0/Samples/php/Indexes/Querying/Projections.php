<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldStorage;
use RavenDB\Documents\Queries\QueryData;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Samples\Infrastructure\Orders\Order;

class Projections
{
    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region projections_1
                $results = $session
                    ->query(Employee::class, Employees_ByFirstAndLastName::class)
                    ->selectFields(Employee::class, ["FirstName", "LastName"])
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_1_stored
                $results = $session
                    ->query(Employee::class, Employees_ByFirstAndLastNameWithStoredFields::class)
                    ->selectFields(Employee::class, ["FirstName", "LastName"])
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_2
                $queryData = new QueryData(["ShipTo", "Lines[].ProductName"], ["ShipTo", "Products"]);

                $results = $session
                    ->query(Order::class, Orders_ByShipToAndLines::class)
                    ->selectFields(ShipToAndProducts::class, $queryData)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_3
                $results = $session
                    ->rawQuery(FullName::class, 'from Employees as e select { FullName: e.FirstName + " " + e.LastName }')
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_4
                $results = $session->advanced()->rawQuery(
                    Employee::class,
                    "declare function output(e) { " .
                        "    var format = function(p){ return p.FirstName + \" \" + p.LastName; }; " .
                        "    return { FullName : format(e) }; " .
                        "} " .
                        "from Employees as e select output(e)"
                    )
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_5
                $results = $session->advanced()->rawQuery(
                    OrderProjection::class,
                    "from Orders as o " .
                        "load o.company as c " .
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
                $results = $session->advanced()->rawQuery(
                    EmployeeProjection::class,
                    "from Employees as e " .
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
                $results = $session->advanced()->rawQuery(
                    EmployeeProjection::class,
                    "from Employees as e " .
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
                $results = $session->advanced()->rawQuery(
                    Employee::class,
                    "from Employees as e " .
                    "select {" .
                    "     Name : e.FirstName, " .
                    "     Metadata : getMetadata(e)" .
                    "}")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_9
                $results = $session->advanced()->rawQuery(
                    Total::class,
                    "from Orders as o " .
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
                # region selectfields_1
                $fields = [
                    "Name",
                    "Phone"
                ];

                $results = $session
                    ->advanced()
                    ->documentQuery(Company::class, Companies_ByContact::class)
                    ->selectFields(ContactDetails::class, $fields)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region selectfields_2
                    $results = $session
                    ->advanced()
                    ->documentQuery(Company::class, Companies_ByContact::class)
                    ->selectFields(ContactDetails::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projections_10
                $results = $session->query(Company::class, Companies_ByContact::class)
                    ->projectInto(ContactDetails::class)
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

# region indexes_1
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from employee in docs.Employees " .
            "select new " .
            "{" .
            "   FirstName = employee.FirstName," .
            "   LastName = employee.LastName" .
            "}";
    }
}
# endregion

# region indexes_1_stored
class Employees_ByFirstAndLastNameWithStoredFields extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from employee in docs.Employees " .
            "select new" .
            "{" .
            "   FirstName = employee.FirstName," .
            "   LastName = employee.LastName" .
            "}";

        $this->storeAllFields(FieldStorage::yes()); // FirstName and LastName fields can be retrieved directly from index
    }
}
# endregion

# region indexes_2
class Employees_ByFirstNameAndBirthday extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from employee in docs.Employees " .
            "select new " .
            "{" .
            "   FirstName = employee.FirstName," .
            "   Birthday = employee.Birthday" .
            "}";
    }
}
# endregion

# region indexes_3
class Orders_ByShipToAndLines extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from order in docs.Orders" .
            "select new { " .
            "   ShipTo = order.ShipTo, " .
            "   Lines = order.Lines " .
            "}";
    }
}
# endregion

# region indexes_4
class Orders_ByShippedAtAndCompany extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from order in docs.Orders " .
            "select new " .
            "{" .
            "   ShippedAt = order.ShippedAt," .
            "   Company = order.Company" .
            "}";
    }
}
# endregion

# region index_10
class Companies_ByContact extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "companies.Select(x => new {Name = x.Contact.Name, Phone = x.Phone})";

        $this->storeAllFields(FieldStorage::yes()); // Name and Phone fields can be retrieved directly from index
    }
}
# endregion

# region projections_10_class
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

class EmployeeProjection {}

class ShipToAndProducts {}

class Total {}

class OrderProjection {}

class FullName {}
