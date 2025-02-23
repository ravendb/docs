<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Samples\Infrastructure\Orders\Order;

class Boosting
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region query_1
                $orders = $session
                     // Query the index
                    ->query(Orders_ByCountries_BoostByField_IndexEntry::class, Orders_ByCountries_BoostByField::class)
                    ->whereEquals("ShipToCountry", "Poland")
                    ->orElse()
                    ->whereEquals("CompanyCountry", "Portugal")
                    ->ofType(Order::class)
                    ->toList();

                // Because index-field 'ShipToCountry' was boosted (inside the index definition),
                // then documents containing 'Poland' in their 'ShipTo.Country' field will get a higher score than
                // documents containing a company that is located in 'Portugal'.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_3
                $orders = $session->advanced()
                     // Query the index
                    ->documentQuery(Orders_ByCountries_BoostByField_IndexEntry::class, Orders_ByCountries_BoostByField::class)
                    ->whereEquals("ShipToCountry", "Poland")
                    ->orElse()
                    ->whereEquals("CompanyCountry", "Portugal")
                    ->ofType(Order::class)
                    ->toList();

                // Because index-field 'ShipToCountry' was boosted (inside the index definition),
                // then documents containing 'Poland' in their 'ShipTo.Country' field will get a higher score than
                // documents containing a company that is located in 'Portugal'.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                    # region query_4
                    $orders = $session
                         // Query the index
                        ->query(Orders_ByCountries_BoostByIndexEntry_IndexEntry::class, Orders_ByCountries_BoostByIndexEntry::class)
                        ->whereEquals("ShipToCountry", "Poland")
                        ->orElse()
                        ->whereEquals("CompanyCountry", "Portugal")
                        ->ofType(Order::class)
                        ->toList();

                    // The resulting score per matching document is affected by the value of the document-field 'Freight'.
                    // Documents with a higher 'Freight' value will rank higher.
                    # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query_6
                $orders = $session->advanced()
                     // Query the index
                    ->documentQuery(Orders_ByCountries_BoostByIndexEntry_IndexEntry::class, Orders_ByCountries_BoostByIndexEntry::class)
                    ->whereEquals("ShipToCountry", "Poland")
                    ->orElse()
                    ->whereEquals("CompanyCountry", "Portugal")
                    ->ofType(Order::class)
                    ->toList();

                // The resulting score per matching document is affected by the value of the document-field 'Freight'.
                // Documents with a higher 'Freight' value will rank higher.
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

//# region index_1
class Orders_ByCountries_BoostByField_IndexEntry
{
    // Index-field 'ShipToCountry' will be boosted in the map definition below
    public ?string $shipToCountry = null;
    public ?string $companyCountry = null;

    public function getShipToCountry(): ?string
    {
        return $this->shipToCountry;
    }

    public function setShipToCountry(?string $shipToCountry): void
    {
        $this->shipToCountry = $shipToCountry;
    }

    public function getCompanyCountry(): ?string
    {
        return $this->companyCountry;
    }

    public function setCompanyCountry(?string $companyCountry): void
    {
        $this->companyCountry = $companyCountry;
    }
}

class Orders_ByCountries_BoostByField extends AbstractIndexCreationTask
{
        public function __construct()
        {
            parent::__construct();

            // Boost index-field 'ShipToCountry':
            // * Use method 'Boost', pass a numeric value to boost by
            // * Documents that match the query criteria for this field will rank higher
            $this->map =
                "docs.Orders.Select(order => new { " .
                "   ShipToCountry = order.ShipTo.Country.Boost(10), " .
                "   CompanyCountry = this.LoadDocument(order.Company, \"Companies\").Address.Country " .
                "})";
        }
}
# endregion

# region index_1_js
class Orders_ByCountries_BoostByField_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps(["map('Orders', function (order) {\n" .
            "   let company = load(order.Company, 'Companies')\n" .
            "   return {\n" .
            "       ShipToCountry: boost(order.ShipTo.Country, 10),\n" .
            "       CompanyCountry: company.Address.Country\n" .
            "   }\n" .
            "})"]);
    }
}
# endregion

# region index_2
class Orders_ByCountries_BoostByIndexEntry_IndexEntry
{
    public ?string $shipToCountry = null;
    public ?string $companyCountry = null;

    public function getShipToCountry(): ?string
    {
        return $this->shipToCountry;
    }

    public function setShipToCountry(?string $shipToCountry): void
    {
        $this->shipToCountry = $shipToCountry;
    }

    public function getCompanyCountry(): ?string
    {
        return $this->companyCountry;
    }

    public function setCompanyCountry(?string $companyCountry): void
    {
        $this->companyCountry = $companyCountry;
    }
}

class Orders_ByCountries_BoostByIndexEntry extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Boost the whole index-entry:
        // * Use method 'Boost'
        // * Pass a document-field that will set the boost level dynamically per document indexed.
        // * The boost level will vary from one document to another based on the value of this field.

        $this->map =
            "docs.Orders.Select(order => new { " .
            "   ShipToCountry = order.ShipTo.Country, " .
            "   CompanyCountry = this.LoadDocument(order.Company, \"Companies\").Address.Country " .
            "}.Boost((float) order.Freight))";
    }
}
# endregion

# region index_2_js
class Orders_ByCountries_BoostByIndexEntry_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();


        $this->setMaps(["map('Orders', function (order) {\n" .
            "   let company = load(order.Company, 'Companies')\n" .
            "   return boost({\n" .
            "       ShipToCountry: order.ShipTo.Country,\n" .
            "       CompanyCountry: company.Address.Country\n" .
            "   }, order.Freight)\n" .
            "})"]);
    }
}
# endregion
