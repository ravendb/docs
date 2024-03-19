from __future__ import annotations
from typing import List

from ravendb import AbstractIndexCreationTask


# region intersection_1
class TShirt:
    def __init__(
        self, Id: str = None, release_year: int = None, manufacturer: str = None, types: List[TShirtType] = None
    ):
        self.Id = Id
        self.release_year = release_year
        self.manufacturer = manufacturer
        self.types = types


class TShirtType:
    def __init__(self, color: str = None, size: str = None):
        self.color = color
        self.size = size


# endregion


# region intersection_2
class TShirts_ByManufacturerColorSizeAndReleaseYear(AbstractIndexCreationTask):
    class Result:
        def __init__(self, manufacturer: str = None, color: str = None, size: str = None, release_year: int = None):
            self.manufacturer = manufacturer
            self.color = color
            self.size = size
            self.release_year = release_year

    def __init__(self):
        super().__init__()
        self.map = (
            "from t in docs.TShirts from type in t.types select new {"
            " manufacturer = t.manufacturer,"
            " color = type.color,"
            " size = type.size,"
            " release_year = t.release_year"
            "}"
        )


# endregion
