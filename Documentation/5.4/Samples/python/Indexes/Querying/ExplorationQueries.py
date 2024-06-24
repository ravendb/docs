from examples_base import ExampleBase, Employee


class ExplorationQueries(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_exploration_queries(self):
        with self.embedded_server.get_document_store("ExplorationQueries") as store:
            # filter in a collection query
            with store.open_session() as session:
                # region exploration-queries_1.3
                result = session.advanced.raw_query(
                    "from Employees as e " "filter e.Address.Country = 'USA' " "filter_limit 500", Employee
                ).single()
                # endregion

                # filter in an index query
                # region exploration-queries_2.3
                emp = (
                    session.advanced.raw_query(
                        "from Employees as e "
                        "where e.Title = $title "
                        "filter e.Address.Country = $country "
                        "filter_limit $limit",
                        Employee,
                    )
                    .add_parameter("title", "Sales Representative")
                    .add_parameter("country", "USA")
                    .add_parameter("limit", 500)
                    .single()
                )
                # endregion

                # filter and projection
                # region exploration-queries_3.3
                emp3 = session.advanced.raw_query(
                    "from Employees as e "
                    "filter startsWith(e.FirstName, 'A') "
                    "select { FullName: e.FirstName + ' ' + e.LastName }",
                    Employee,
                )
                # endregion
