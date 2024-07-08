from typing import List

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.counters import (
    AbstractCountersIndexCreationTask,
    AbstractJavaScriptCountersIndexCreationTask,
)

from examples_base import ExampleBase


class IndexingCounters(ExampleBase):
    def setUp(self):
        super().setUp()

    class Foo:
        """
        # region syntax
        IEnumerable<string> CounterNamesFor(object doc);
        # endregion
        """

    def test_indexing_counters(self):
        with self.embedded_server.get_document_store("IndexingCounters") as store:
            with store.open_session() as session:
                # region query_1
                companies = list(
                    session.query_index_type(Companies_ByCounterNames, Companies_ByCounterNames.Result).contains_any(
                        "counter_names", ["Likes"]
                    )
                )
                # endregion


# region index_0
class Companies_ByCounterNames(AbstractIndexCreationTask):
    class Result:
        def __init__(self, counter_names: List[str] = None):
            self.counter_names = counter_names

    def __init__(self):
        super().__init__()
        self.map = (
            "from e in docs.Employees "
            "let counterNames = CounterNamesFor(e) "
            "select new { counter_names = counterNames.ToArray() }"
        )


# endregion
# region index_1
class MyCounterIndex(AbstractCountersIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = (
            "from counter in counters "
            "select new { likes = counter.Value, name = counter.Name, user = counter.DocumentId }"
        )


# endregion
# region index_3
class MyMultiMapCounterIndex(AbstractJavaScriptCountersIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = """
        counters.map('Blogposts', 'Likes', function (counter) {
            return {
                Likes: counter.Value,
                Name: counter.Name,
                Blog Post: counter.DocumentId
            };
        })
        """


# endregion


"""
# region counter_entry
public class CounterEntry
        {
            public string DocumentId { get; set; }
            public string Name { get; set; }
            public long Value { get; set; }
        }
# endregion
"""
