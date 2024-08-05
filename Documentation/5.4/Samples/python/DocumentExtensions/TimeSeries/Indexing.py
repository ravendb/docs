from datetime import datetime

from ravendb import PutIndexesOperation
from ravendb.documents.indexes.time_series import (
    TimeSeriesIndexDefinition,
    TimeSeriesIndexDefinitionBuilder,
    AbstractTimeSeriesIndexCreationTask,
    AbstractMultiMapTimeSeriesIndexCreationTask,
    AbstractJavaScriptTimeSeriesIndexCreationTask,
)

from examples_base import ExampleBase


# region index_1
class StockPriceTimeSeriesFromCompanyCollection(AbstractTimeSeriesIndexCreationTask):
    # The index-entry:
    # ================
    class IndexEntry:
        def __init__(
            self, trade_volume: float = None, date: datetime = None, company_id: str = None, employee_name: str = None
        ):
            # The index-fields:
            # =================
            self.trade_volume = trade_volume
            self.date = date
            self.company_id = company_id
            self.employee_name = employee_name

    def __init__(self):
        super().__init__()
        self.map = """
        from segment in timeSeries.Companies.StockPrices
        from entry in segment.Entries
        
        let employee = LoadDocument(entry.Tag, "Employees")
        
        select new
        {
            trade_volume = entry.Values[4],
            date = entry.Timestamp.Date,
            company_id = segment.DocumentId,
            employee_name = employee.FirstName + " " + employee.LastName
        }
        """


# endregion
# region index_3
class StockPriceTimeSeriesFromCompanyCollection_JS(AbstractJavaScriptTimeSeriesIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            timeSeries.map('Companies', 'StockPrices', function (segment) {

                return segment.Entries.map(entry => {
                    let employee = load(entry.Tag, 'Employees');

                    return {
                        trade_volume: entry.Values[4],
                        date: new Date(entry.Timestamp.getFullYear(),
                                       entry.Timestamp.getMonth(),
                                       entry.Timestamp.getDate()),
                        company_id: segment.DocumentId,
                        employee_name: employee.FirstName + ' ' + employee.LastName
                    };
                });
            })
            """
        }


# endregion

# region index_6
class Vechicles_ByLocation(AbstractMultiMapTimeSeriesIndexCreationTask):
    class IndexEntry:
        def __init__(
            self, latitude: float = None, longitude: float = None, date: datetime = None, document_id: str = None
        ):
            self.latitude = latitude
            self.longitude = longitude
            self.date = date
            self.document_id = document_id

    def __init__(self):
        super().__init__()
        self._add_map(
            """
            from segment in timeSeries.Planes.GPS_Coordinates
            from entry in segment.Entries
            select new
            {
                latitude = entry.Values[0],
                longitude = entry.Values[1],
                date = entry.Timestamp.Date,
                document_id = segment.DocumentId
            }
            """
        )
        self._add_map(
            """
            from segment in timeSeries.Ships.GPS_Coordinates
            from entry in segment.Entries
            select new
            {
                latitude = entry.Values[0],
                longitude = entry.Values[1],
                date = entry.Timestamp.Date,
                document_id = segment.DocumentId
            }
            """
        )


# endregion


# region index_7
class TradeVolume_PerDay_ByCountry(AbstractTimeSeriesIndexCreationTask):
    class Result:
        def __init__(self, total_trade_volume: float = None, date: datetime = None, country: str = None):
            self.total_trade_volume = total_trade_volume
            self.date = date
            self.country = country

    def __init__(self):
        super().__init__()
        # Define the Map part:
        self.map = """
        from segment in timeSeries.Companies.StockPrices
        from entry in segment.Entries
        
        let company = LoadDocument(segment.DocumentId, 'Companies')
        
        select new
        {
            date = entry.Timestamp.Date,
            country = company.Address.Country,
            total_trade_volume = entry.Values[4],
        }
        """

        # Define the Reduce part:
        self._reduce = """
        from r in results
        group r by new {r.date, r.country}
        into g
        select new 
        {
            date = g.Key.date,
            country = g.Key.country,
            total_trade_volume = g.Sum(x => x.total_trade_volume)
        }
        """


# endregion


class Indexing(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_indexing(self):
        with self.embedded_server.get_document_store("IndexingTimeSeriesData") as store:
            # region index_definition_1
            # Define the 'index definition'
            index_definition = TimeSeriesIndexDefinition(
                name="StockPriceTimeSeriesFromCompanyCollection",
                maps={
                    """
                    from segment in timeSeries.Companies.StockPrices 
                    from entry in segment.Entries 

                    let employee = LoadDocument(entry.Tag, "Employees")

                    select new 
                    { 
                        trade_volume = entry.Values[4], 
                        date = entry.Timestamp.Date,
                        company_id = segment.DocumentId,
                        employee_name = employee.FirstName + ' ' + employee.LastName 
                    }
                    """
                },
            )

            # Deploy the index to the server via 'PutIndexesOperation'
            store.maintenance.send(PutIndexesOperation(index_definition))
            # endregion

            # region index_definition_2
            # Create the index builder
            ts_index_def_builder = TimeSeriesIndexDefinitionBuilder("StockPriceTimeSeriesFromCompanyCollection")

            ts_index_def_builder.map = """
                from segment in timeSeries.Companies.StockPrices
                from entry in segment.Entries
                select new 
                {
                    trade_volume = entry.Values[4],
                    date = entry.Timestamp.Date,
                    company_id = segment.DocumentId,
                }
            """
            # Build the index definition
            index_definition_from_builder = ts_index_def_builder.to_index_definition(store.conventions)

            # Deploy the index to the server via 'PutIndexesOperation'
            store.maintenance.send(PutIndexesOperation(index_definition_from_builder))
            # endregion

            # region query_1
            with store.open_session() as session:
                # Retrieve time series data for the specified company:
                # ====================================================
                results = list(
                    session.query_index_type(
                        StockPriceTimeSeriesFromCompanyCollection, StockPriceTimeSeriesFromCompanyCollection.IndexEntry
                    ).where_equals("company_id", "Companies/91-A")
                )

                # Results will include data from all 'StockPrices' entries in document 'Companies/91-A'
            # endregion

            # region query_2
            with store.open_session() as session:
                # Find what companies had a very high trade volume:
                # =================================================
                results = list(
                    session.query_index_type(
                        StockPriceTimeSeriesFromCompanyCollection, StockPriceTimeSeriesFromCompanyCollection.IndexEntry
                    )
                    .where_greater_than_or_equal("trade_volume", 150_000_000)
                    .select_fields(OnlyCompanyName, "company_id")
                    .distinct()
                )

                # Results will contain company "Companies/65-A"
                # since it is the only company with time series entries having such high trade volume.
            # endregion


class OnlyCompanyName:
    def __init__(self, company_id: str = None):
        self.name = company_id
