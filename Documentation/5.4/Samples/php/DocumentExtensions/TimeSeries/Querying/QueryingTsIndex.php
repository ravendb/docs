<?php

namespace RavenDB\Samples\DocumentExtensions\TimeSeries\Querying;

use DateTime;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\TimeSeries\AbstractTimeSeriesIndexCreationTask;

class QueryingTsIndex
{
    public function getDocumentStore(): DocumentStore
    {
        $store = new DocumentStore("http://localhost:8080", "TestDB");
        $store->initialize();

        return $store;
    }

    public function samples(): void
    {
        $store = $this->getDocumentStore();
        try {
            # region query_index_1
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session
                     // Query the index
                    ->query(TsIndex_IndexEntry::class, TsIndex::class)
                     // Query for all entries w/o any filtering
                    ->toList();

                // Access results:
                /** @var TsIndex_IndexEntry $entryResult */
                $entryResult = $results[0];
                $employeeName = $entryResult->getEmployeeName();
                $BPM = $entryResult->getBpm();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_3
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                     // Query the index
                    ->documentQuery(TsIndex_IndexEntry::class, TsIndex::class)
                     // Query for all entries w/o any filtering
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_4
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                     // Query the index for all entries w/o any filtering
                    ->rawQuery(TsIndex_IndexEntry::class, "
                          from index 'TsIndex'
                     ")
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_5
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session
                    ->query(TsIndex_IndexEntry::class, TsIndex::class)
                     // Retrieve only time series entries with high BPM values for a specific employee
                    ->whereEquals("EmployeeName", "Robert King")
                    ->andAlso()
                    ->whereGreaterThan("BPM", 85)
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_7
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                    ->documentQuery(TsIndex_IndexEntry::class, TsIndex::class)
                     // Retrieve only time series entries with high BPM values for a specific employee
                    ->whereEquals("EmployeeName", "Robert King")
                    ->andAlso()
                    ->whereGreaterThan("BPM", 85)
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_8
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                     // Retrieve only time series entries with high BPM values for a specific employee
                    ->rawQuery(TsIndex_IndexEntry::class, "
                          from index 'TsIndex'
                          where EmployeeName == 'Robert King' and BPM > 85.0
                     ")
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_9
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session
                    ->query(TsIndex_IndexEntry::class, TsIndex::class)
                     // Retrieve time series entries where employees had a low BPM value
                    ->whereLessThan("BPM", 58)
                     // Order by the 'Date' index-field (descending order)
                    ->orderByDescending("Date")
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion
            
            # region query_index_11
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                    ->documentQuery(TsIndex_IndexEntry::class, TsIndex::class)
                     // Retrieve time series entries where employees had a low BPM value
                    ->whereLessThan("BPM", 58)
                     // Order by the 'Date' index-field (descending order)
                    ->orderByDescending("Date")
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_12
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                     // Retrieve entries with low BPM value and order by 'Date' descending
                    ->rawQuery(TsIndex_IndexEntry::class, "
                          from index 'TsIndex'
                          where BPM < 58.0
                          order by Date desc
                     ")
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_13
            $session = $store->openSession();
            try {
                /** @var array<EmployeeDetails> $results */
                $results = $session
                    ->query(TsIndex_IndexEntry::class, TsIndex::class)
                    ->whereGreaterThan("BPM", 100)
                     // Return only the EmployeeID index-field in the results
                    ->selectFields(EmployeeDetails::class, "EmployeeID")
                     // Optionally: call 'Distinct' to remove duplicates from results
                    ->distinct()
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_15
            $fieldsToProject = [
                "EmployeeID"
            ];

            $session = $store->openSession();
            try {
                /** @var array<EmployeeDetails> $results */
                $results = $session->advanced()
                    ->documentQuery(TsIndex_IndexEntry::class, TsIndex::class)
                    ->whereGreaterThan("BPM", 100)
                     // Return only the EmployeeID index-field in the results
                    ->selectFields(EmployeeDetails::class, $fieldsToProject)
                     // Optionally: call 'Distinct' to remove duplicates from results
                    ->distinct()
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion

            # region query_index_16
            $session = $store->openSession();
            try {
                /** @var array<TsIndex_IndexEntry> $results */
                $results = $session->advanced()
                     // Return only the EmployeeID index-field in the results
                    ->rawQuery(TsIndex_IndexEntry::class, "
                          from index 'TsIndex'
                          where BPM > 100.0
                          select distinct EmployeeID
                     ")
                    ->toList();
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }
}

# region sample_ts_index
// The index-entry:
// ================
class TsIndex_IndexEntry
{
    // The index-fields:
    // =================
    public ?float $BPM = null;
    public ?DateTime $date = null;
    public ?string $tag = null;
    public ?string $employeeID = null;
    public ?string $employeeName = null;

    public function getBPM(): ?float
    {
        return $this->BPM;
    }

    public function setBPM(?float $BPM): void
    {
        $this->BPM = $BPM;
    }

    public function getDate(): ?DateTime
    {
        return $this->date;
    }

    public function setDate(?DateTime $date): void
    {
        $this->date = $date;
    }

    public function getTag(): ?string
    {
        return $this->tag;
    }

    public function setTag(?string $tag): void
    {
        $this->tag = $tag;
    }

    public function getEmployeeID(): ?string
    {
        return $this->employeeID;
    }

    public function setEmployeeID(?string $employeeID): void
    {
        $this->employeeID = $employeeID;
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

class TsIndex extends AbstractTimeSeriesIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =  "
            from ts in timeSeries.Employees.HeartRates
            from entry in ts.Entries
            let employee = LoadDocument(ts.DocumentId, \"Employees\")
            select new 
            {
                BPM = entry.Values[0],
                Date = entry.Timestamp.Date,
                Tag = entry.Tag,
                EmployeeId = ts.DocumentId,
                EmployeeName = employee.FirstName + ' ' + employee.LastName
            }
        ";
    }
}
# endregion

# region employee_details_class
// This class is used when projecting index-fields via DocumentQuery
class EmployeeDetails
{
    private ?string $employeeName = null;
    private ?string $employeeID = null;

    public function getEmployeeName(): ?string
    {
        return $this->employeeName;
    }

    public function setEmployeeName(?string $employeeName): void
    {
        $this->employeeName = $employeeName;
    }

    public function getEmployeeID(): ?string
    {
        return $this->employeeID;
    }

    public function setEmployeeID(?string $employeeID): void
    {
        $this->employeeID = $employeeID;
    }
}
# endregion
