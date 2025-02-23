<?php

namespace RavenDB\Samples\Indexes;

use DateInterval;
use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Type\StringArray;

/*
# region indexes_1
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
{
    // ...
}
# endregion
*/

/*
# region javaScriptindexes_1
class Employees_ByFirstAndLastName extends AbstractJavaScriptIndexCreationTask
{
    // ...
}
# endregion
*/

class Employees_ByFirstAndLastName extends AbstractIndexCreationTask
{
    # region indexes_2
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new { " .
                     "    FirstName = employee.FirstName, " .
                     "    LastName = employee.LastName " .
                     "})";
    }
    # endregion
}

# region indexes_7
class Employees_ByFullName_Result {
    private ?string $fullName = null;

    public function getFullName(): ?string
    {
        return $this->fullName;
    }

    public function setFullName(?string $fullName): void
    {
        $this->fullName = $fullName;
    }
}

class Employees_ByFullName extends AbstractIndexCreationTask
{
    public function __construct() {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new { " .
                     "    FullName = (employee.FirstName + \" \") + employee.LastName " .
                     "})";
    }
}
# endregion

# region indexes_1_0

class Employees_ByYearOfBirth_Result {
    public ?int $yearOfBirth = null;

    public function getYearOfBirth(): ?int
    {
        return $this->yearOfBirth;
    }

    public function setYearOfBirth(?int $yearOfBirth): void
    {
        $this->yearOfBirth = $yearOfBirth;
    }
}

class Employees_ByYearOfBirth extends AbstractIndexCreationTask {
    public function __construct() {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new { " .
                     "    YearOfBirth = employee.Birthday.Year " .
                     "})";
    }
}
# endregion

# region indexes_1_2
class Employees_ByBirthday_Result
{
    public ?DateTime $birthday = null;

    public function getBirthday(): ?DateTime
    {
        return $this->birthday;
    }

    public function setBirthday(?DateTime $birthday): void
    {
        $this->birthday = $birthday;
    }
}
class Employees_ByBirthday extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new { " .
                     "    Birthday = employee.Birthday " .
                     "})";
    }
}
# endregion

# region indexes_1_4
class Employees_ByCountry_Result
{
    private ?string $country = null;

    public function getCountry(): ?string
    {
        return $this->country;
    }

    public function setCountry(?string $country): void
    {
        $this->country = $country;
    }
}
class Employees_ByCountry extends AbstractIndexCreationTask {
    public function __construct() {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new { " .
                     "    Country = employee.Address.Country " .
                     "})";
    }
}
# endregion

# region indexes_1_6
class Employees_Query_Result {
    public ?StringArray $query = null;

    public function getQuery(): ?StringArray
    {
        return $this->query;
    }

    public function setQuery(?StringArray $query): void
    {
        $this->query = $query;
    }
}

class Employees_Query extends AbstractIndexCreationTask {
    public function __construct() {
        parent::__construct();

        $this->map = "docs.Employees.Select(employee => new { " .
                     "    Query = new [] { employee.FirstName, employee.LastName, employee.Title, employee.Address.City } " .
                     "})";
        $this->index("query", FieldIndexing::search());
    }
}
# endregion

# region indexes_1_6
class Companies_ByAddress_Country_Result {
    private ?string $city = null;
    private ?string $company = null;
    private ?string $phone = null;

    public function getCity(): ?string
    {
        return $this->city;
    }

    public function setCity(?string $city): void
    {
        $this->city = $city;
    }

    public function getCompany(): ?string
    {
        return $this->company;
    }

    public function setCompany(?string $company): void
    {
        $this->company = $company;
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

# region indexes_1_7
class Companies_ByAddress_Latitude_Result {
    private ?float $latitude = null;
    private ?float $longitude = null;
    private ?string $companyName = null;
    private ?string $companyAddress = null;
    private ?string $companyPhone = null;

    public function getLatitude(): ?float
    {
        return $this->latitude;
    }

    public function setLatitude(?float $latitude): void
    {
        $this->latitude = $latitude;
    }

    public function getLongitude(): ?float
    {
        return $this->longitude;
    }

    public function setLongitude(?float $longitude): void
    {
        $this->longitude = $longitude;
    }

    public function getCompanyName(): ?string
    {
        return $this->companyName;
    }

    public function setCompanyName(?string $companyName): void
    {
        $this->companyName = $companyName;
    }

    public function getCompanyAddress(): ?string
    {
        return $this->companyAddress;
    }

    public function setCompanyAddress(?string $companyAddress): void
    {
        $this->companyAddress = $companyAddress;
    }

    public function getCompanyPhone(): ?string
    {
        return $this->companyPhone;
    }

    public function setCompanyPhone(?string $companyPhone): void
    {
        $this->companyPhone = $companyPhone;
    }
}
# endregion

class Map
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region indexes_4
                $employees1 = $session
                    ->query(Employee::class, Employees_ByFirstAndLastName::class)
                    ->whereEquals('FirstName', "Robert")
                    ->toList();

                $employees2 = $session
                    ->query("Employees/ByFirstAndLastName")
                    ->whereEquals('FirstName', "Robert")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_8
                // notice that we're 'cheating' here
                // by marking result type in 'Query' as 'Employees_ByFullName.Result' to get strongly-typed syntax
                // and changing type using 'OfType' before sending query to server
                $employees = $session
                    ->query(Employees_ByFullName_Result::class, Employees_ByFullName::class)
                    ->whereEquals('FullName', "Robert King")
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_9
                $employees = $session
                    ->advanced()->documentQuery(Employee::class, Employees_ByFullName::class)
                    ->whereEquals("FullName", "Robert King")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_6_1
                $employees = $session
                    ->query(Employees_ByYearOfBirth_Result::class, Employees_ByYearOfBirth::class)
                    ->whereEquals("YearOfBirth", 1963)
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_6_2
                $employees = $session
                    ->advanced()
                    ->documentQuery(Employees_ByYearOfBirth_Result::class, Employees_ByYearOfBirth::class)
                    ->whereEquals("YearOfBirth", 1963)
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_5_1
                $startDate = new DateTime('1963-01-01');
                $endDate = $startDate->modify('+1 year')->sub(new DateInterval('PT0.001S'));
                $employees = $session
                    ->query(Employees_ByBirthday_Result::class, Employees_ByBirthday::class)
                    ->whereGreaterThanOrEqual("Birthday", $startDate)
                    ->andAlso()
                    ->whereLessThanOrEqual("Birthday", $endDate)
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_5_2
                $startDate = new DateTime('1963-01-01');
                $endDate = $startDate->modify('+1 year')->sub(new DateInterval('PT0.001S'));
                $employees = $session
                    ->advanced()
                    ->documentQuery(Employees_ByBirthday_Result::class, Employees_ByBirthday::class)
                    ->whereBetween("Birthday", $startDate, $endDate)
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_7_1
                $employees = $session
                    ->query(Employees_ByCountry_Result::class, Employees_ByCountry::class)
                    ->whereEquals("Country", "USA")
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_7_2
                $employees = $session
                    ->advanced()
                    ->documentQuery(Employees_ByCountry_Result::class, Employees_ByCountry::class)
                    ->whereEquals("Country", "USA")
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_1_7
                $employees = $session
                    ->query(Employees_Query_Result::class, Employees_Query::class)
                    ->search("Query", "John Doe")
                    ->ofType(Employee::class)
                    ->toList();
                    # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_1_8
                $employees = $session
                    ->advanced()
                    ->documentQuery(Employees_Query_Result::class, Employees_Query::class)
                    ->search("Query", "John Doe")
                    ->ofType(Employee::class)
                    ->toList();
                    # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_query_1_6
                $orders = $session
                    ->query(Companies_ByAddress_Country_Result::class, Companies_ByAddress_Country::class)
                    ->ofType(Company::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_query_1_7
                $orders = $session
                    ->query(Companies_ByAddress_Latitude_Result::class, Companies_ByAddress_Latitude::class)
                    ->ofType(Company::class)
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
