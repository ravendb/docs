from pyravendb.store.document_store import DocumentStore


class Employee(object):
    def __init__(self, first_name, last_name):
        self.first_name = first_name
        self.last_name = last_name

store = DocumentStore()
store.initialize()


class LoadingSession(object):
    @staticmethod
    def open_session():

        # region loading_entities_1
        def load(self, key_or_keys, object_type=None, includes=None, nested_object_types=None):
        # endregion

        # region loading_entities_2
        with store.open_session() as session:
            # dynamic_employee will be a DynamicStructure
            employee = session.load("employees/1")
            # endregion

        # region loading_entities_3
        with store.open_session() as session:
            # employee will be Employee object
            employee = session.load("employees/1", object_type=Employee )
            # endregion

        # region loading_entities_3.1
        class Student(object):
            def __init__(self, first_name, last_name):
                self.first_name = first_name
                self.last_name = last_name

        class Teacher(object):
            def __init__(self, first, last):
                self.first_name = first
                self.last_name = last

        with store.open_session() as session:
            # student will be Student object
            student = session.load("employees/1", object_type=Student)

            teacher = session.load("employees/1", object_type=Teacher)
            # Will raise TypeError because the way we convert the document to entity
            # endregion

        # region loading_entities_4
        with store.open_session() as session:
            keys_to_load = ["employees/1", "employees/2", "employees/3"]
            employee = session.load(keys_to_load, object_type=Employee )
            # endregion


# region loading_entities_5
class Order(object):
    def __init__(self, company):
        self.company = company


class Company(object):
    def __init__(self, name, address):
        self.name = name
        self.address = address

with store.open_session() as session:
    session.store(Company("Hibernating Rhinos", "Israel" ))
    session.store(Order(company="companies/1"), key="orders/1")
    session.save_changes()

with store.open_session() as session:
    order = session.load("orders/1", object_type=Order, includes="company")
# endregion


# region loading_entities_6
class Dog(object):
    def __init__(self, brand, color, age):
        self.brand = brand
        self.color = color
        self.age = age


class Car(object):
    def __init__(self, brand, color):
        self.brand = brand
        self.color = color


class Person(object):
    def __init__(self, name, dog, car):
        self.name = name
        self.dog = dog  # instance of Dog
        self.car = car  # instance of Car

with store.open_session() as session:
    person = session.load("people/1", object_type=Person,
                          nested_object_types={"dog": Dog, "car": Car})

# endregion
