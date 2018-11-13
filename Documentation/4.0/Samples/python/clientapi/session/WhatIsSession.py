from pyravendb.store.document_store import DocumentStore
from unittest import TestCase


class Company(object):
    def __init__(self, name="", external_id="", phone="", fax="", address=None, contact=None):
        self.Id = None
        self.name = name
        self.external_id = external_id
        self.phone = phone
        self.fax = fax
        if not Address:
            address = Address()
        self.address = address
        if not Contact:
            contact = Contact()
        self.contact = contact


class Address(object):
    def __init__(self, line1="", line2="", city="", region="", postal_code="", country=""):
        self.Line1 = line1
        self.Line2 = line2
        self.City = city
        self.Region = region
        self.PostalCode = postal_code
        self.Country = country


class Contact(object):
    def __init__(self, name="", title=""):
        self.Name = name
        self.Title = title


class WhatIsSession(object):
    @staticmethod
    def what_is_session():
        with DocumentStore() as store:
            store.initialize()
            # region session_usage_1

            with store.open_session() as session:
                entity = Company(name= "Company")
                session.store(entity)
                session.save_changes()
                company_id = entity.Id

            with store.open_session() as session:
                entity = session.load(company_id, object_type= Company)
                print(entity.name)

            # endregion

            # region session_usage_2

            with store.open_session() as session:
                entity = session.load(company_id, object_type=Company)
                entity.name = "Another Company"
                session.save_changes()

            # endregion

            with store.open_session() as session:
                # region session_usage_3
                TestCase.assertTrue(session.load(company_id, object_type=Company)
                                    is session.load(company_id, object_type=Company))
                # endregion
