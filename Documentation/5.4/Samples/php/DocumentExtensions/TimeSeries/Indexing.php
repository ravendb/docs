<?php

namespace RavenDB\Samples\DocumentExtensions\TimeSeries;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\TimeSeries\AbstractJavaScriptTimeSeriesIndexCreationTask;
use RavenDB\Documents\Indexes\TimeSeries\AbstractMultiMapTimeSeriesIndexCreationTask;
use RavenDB\Documents\Indexes\TimeSeries\AbstractTimeSeriesIndexCreationTask;
use RavenDB\Documents\Indexes\TimeSeries\TimeSeriesIndexDefinition;
use RavenDB\Documents\Indexes\TimeSeries\TimeSeriesIndexDefinitionBuilder;
use RavenDB\Documents\Operations\Indexes\PutIndexesOperation;

class Indexing
{
    public function samples(): void
    {
        $documentStore = new DocumentStore("http://localhost:8080", "products");
        $documentStore->initialize();

        try {
            # region index_definition_1
            // Define the 'index definition'
            $indexDefinition = new TimeSeriesIndexDefinition();
            $indexDefinition->setName("StockPriceTimeSeriesFromCompanyCollection ");
            $indexDefinition->setMaps(["
                from segment in timeSeries.Companies.StockPrices
                from entry in segment.Entries

                let employee = LoadDocument(entry.Tag, \"Employees\")

                select new
                {
                    TradeVolume = entry.Values[4],
                    Date = entry.Timestamp.Date,
                    CompanyId = segment.DocumentId,
                    EmployeeName = employee.FirstName + ' ' + employee.LastName
                }"
            ]);

            // Deploy the index to the server via 'PutIndexesOperation'
            $documentStore->maintenance()->send(new PutIndexesOperation($indexDefinition));
            # endregion

            # region index_definition_2
            // Create the index builder
            $TSIndexDefBuilder = new TimeSeriesIndexDefinitionBuilder("StockPriceTimeSeriesFromCompanyCollection ");

            // "StockPrices"
            $TSIndexDefBuilder->setMap("
                from segment in timeSeries.Companies.StockPrices
                from entry in segment.Entries
                select new 
                {
                    TradeVolume = entry.Values[4],
                    Date = entry.Timestamp.Date,
                    CompanyId = segment.DocumentId,
                }
            ");

            // Build the index definition
            $indexDefinitionFromBuilder = $TSIndexDefBuilder->toIndexDefinition($documentStore->getConventions());

            // Deploy the index to the server via 'PutIndexesOperation'
            $documentStore->maintenance()->send(new PutIndexesOperation($indexDefinitionFromBuilder));
            # endregion

            # region query_1
            $session = $documentStore->openSession();
            try {
                // Retrieve time series data for the specified company:
                // ====================================================
                /** @var array<StockPriceTimeSeriesFromCompanyCollection_IndexEntry> $results */
                $results = $session
                   ->query(StockPriceTimeSeriesFromCompanyCollection_IndexEntry::class,
                       StockPriceTimeSeriesFromCompanyCollection::class)
                   ->whereEquals("CompanyId", "Companies/91-A")
                   ->toList();
            } finally {
                $session->close();
            }

            // Results will include data from all 'StockPrices' entries in document 'Companies/91-A'.
            # endregion

            # region query_2
            $session = $documentStore->openSession();
            try {
                // Find what companies had a very high trade volume:
                // ==================================================
                /** @var array<string> $results */
                $results = $session
                    ->query(StockPriceTimeSeriesFromCompanyCollection_IndexEntry::class,
                        StockPriceTimeSeriesFromCompanyCollection::class)
                    ->whereGreaterThan("TradeVolume",  150000000)
                    ->selectFields(OnlyCompanyName::class, "CompanyId")
                    ->distinct()
                    ->toList();
            } finally {
                $session->close();
            }

            // Results will contain company "Companies/65-A"
            // since it is the only company with time series entries having such high trade volume.
            # endregion
        } finally {
            $documentStore->close();
        }
    }
}

# region index_1
class StockPriceTimeSeriesFromCompanyCollection_IndexEntry
{
    // The index-fields:
    // =================
    public ?float $tradeVolume = null;
    public ?DateTime $date = null;
    public ?string $companyID = null;
    public ?string $employeeName = null;

    public function getTradeVolume(): ?float
    {
        return $this->tradeVolume;
    }

    public function setTradeVolume(?float $tradeVolume): void
    {
        $this->tradeVolume = $tradeVolume;
    }

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }

    public function getCompanyID(): ?string
    {
        return $this->companyID;
    }

    public function setCompanyID(?string $companyID): void
    {
        $this->companyID = $companyID;
    }

    public function getEmployeeName(): ?string
    {
        return $this->employeeName;
    }

    public function setEmployeeName(?string $employeeName): void
    {
        $this->employeeName = $employeeName;
    }
}
class StockPriceTimeSeriesFromCompanyCollection extends AbstractTimeSeriesIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from segment in timeSeries.Companies.StockPrices" .
            "from entry in segment.Entries" .

            // Can load the document referenced in the TAG:
            "let employee = LoadDocument(entry.Tag, \"Employees\")" .

            // Define the content of the index-fields:
            // =======================================
            "select new" .
            "{" .
            // Retrieve content from the time series ENTRY:
            "    TradeVolume = entry.Values[4]," .
            "    Date = entry.Timestamp.Date," .
            // Retrieve content from the SEGMENT:
            "    CompanyId = segment.DocumentId," .
            // Retrieve content from the loaded DOCUMENT:
            "    EmployeeName = employee.FirstName + \" \" + employee.LastName" .
            "}" ;
        // Call 'AddMap', specify the time series name to be indexed
    }
}
# endregion

# region index_2
class StockPriceTimeSeriesFromCompanyCollection_NonTyped extends AbstractTimeSeriesIndexCreationTask
{
    public function createIndexDefinition(): TimeSeriesIndexDefinition
    {
        $definition = new TimeSeriesIndexDefinition();
        $definition->setName("StockPriceTimeSeriesFromCompanyCollection_NonTyped");
        $definition->setMaps(["
                from segment in timeSeries.Companies.StockPrices
                from entry in segment.Entries

                let employee = LoadDocument(entry.Tag, \"Employees\")

                select new
                {
                    TradeVolume = entry.Values[4],
                    Date = entry.Timestamp.Date,
                    CompanyID = segment.DocumentId,
                    EmployeeName = employee.FirstName + ' ' + employee.LastName
                }"]);

        return $definition;
    }
}
# endregion

# region index_3
class StockPriceTimeSeriesFromCompanyCollection_JS extends AbstractJavaScriptTimeSeriesIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps(["
            timeSeries.map('Companies', 'StockPrices', function (segment) {

                return segment.Entries.map(entry => {
                    let employee = load(entry.Tag, 'Employees');

                    return {
                        TradeVolume: entry.Values[4],
                        Date: new Date(entry.Timestamp.getFullYear(),
                                       entry.Timestamp.getMonth(),
                                       entry.Timestamp.getDate()),
                        CompanyID: segment.DocumentId,
                        EmployeeName: employee.FirstName + ' ' + employee.LastName
                    };
                });
            })"
        ]);
    }
}
# endregion

# region index_4
class AllTimeSeriesFromCompanyCollectionIndex_Entry
{
    private ?float $value = null;
    private ?DateTime $date = null;

    public function getValue(): ?float
    {
        return $this->value;
    }

    public function setValue(?float $value): void
    {
        $this->value = $value;
    }

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }
}


# region index_6
class Vehicles_ByLocation_IndexEntry
{
    private ?float $latitude = null;
    private ?float $longitude = null;
    private ?DateTime $date = null;
    private ?string $documentId = null;

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

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }

    public function getDocumentId(): ?string
    {
        return $this->documentId;
    }

    public function setDocumentId(?string $documentId): void
    {
        $this->documentId = $documentId;
    }
}
class Vehicles_ByLocation extends AbstractMultiMapTimeSeriesIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Call 'AddMap' for each collection you wish to index
        // ===================================================
        // "GPS_Coordinates"
        $this->addMap("
            from segment in timeSeries.Planes.GPS_Coordinates
            from entry in segment.Entries
            select new
            {
                Latitude = entry.Values[0],
                Longitude = entry.Values[1],
                Date = entry.Timestamp.Date,
                DocumentId = segment.DocumentId
            }
        ");

        $this->addMap("
            from segment in timeSeries.Ships.GPS_Coordinates
            from entry in segment.Entries
            select new
            {
                Latitude = entry.Values[0],
                Longitude = entry.Values[1],
                Date = entry.Timestamp.Date,
                DocumentId = segment.DocumentId
            }
        ");
    }
}
# endregion

# region index_7
class TradeVolume_PerDay_ByCountry_Result
{
    private ?float $totalTradeVolume = null;
    private ?DateTime $date = null;
    private ?string $country = null;

    public function getTotalTradeVolume(): ?float
    {
        return $this->totalTradeVolume;
    }

    public function setTotalTradeVolume(?float $totalTradeVolume): void
    {
        $this->totalTradeVolume = $totalTradeVolume;
    }

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }

    public function getCountry(): ?string
    {
        return $this->country;
    }

    public function setCountry(?string $country): void
    {
        $this->country = $country;
    }
}
class TradeVolume_PerDay_ByCountry extends AbstractTimeSeriesIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Define the Map part:
        // "StockPrices"
        $this->map = "
            from segment in timeSeries.Companies.StockPrices
            from entry in segment.Entries
            
            let company = LoadDocument(segment.DocumentId, 'Companies')
            
            select new
            {
                Date = entry.Timestamp.Date,
                Country = company.Address.Country,
                TotalTradeVolume = entry.Values[4],
            }
        ";

        // Define the Reduce part:
        $this->reduce = "
            from r in results
            group r by new {r.date, r.country}
            into g
            select new 
            {
                Date = g.Key.date,
                Country = g.Key.country,
                TotalTradeVolume = g.Sum(x => x.total_trade_volume)
            }
        ";
    }
}
# endregion
