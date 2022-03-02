class Category(object):
    def __init__(self,name):
            self.Name = name
            self.Id = None

class Product(object):
    def __init__(self,name, category_id, units_in_stock):
            self.Name = name
            self.Category = category_id
            self.UnitsInStock = units_in_stock
            self.Id = None

# region client_1
from pyravendb.store import document_store

with document_store.DocumentStore(
    urls=["http://live-test.ravendb.net"],  # URL to the Server
                                            # or list of URLs
                                            # to all Cluster Servers (Nodes)

    database="Northwind") as store:         # Default database that DocumentStore will interact with
    
    conventions = store.conventions         # DocumentStore customizations
    
    store.initialize()                      # Each DocumentStore needs to be initialized before use.
                                            # This process establishes the connection with the Server
                                            # and downloads various configurations
                                            # e.g. cluster topology or client configuration
# endregion

# region client_2
    with store.open_session() as session:           # Open a session for a default 'Database'
        category = Category("Database Category")
        
        session.store(category)                     # Assign an 'Id' and collection (Categories)
                                                    # and start tracking an entity

        product = Product(
            "RavenDB Database", 
            category.Id, 
            10)

        session.store(product)                      # Assign an 'Id' and collection (Products)
                                                    # and start tracking an entity

        session.save_changes()                      # Send to the Server
                                                    # one request processed in one transaction
# endregion

# region client_3
    with store.open_session() as session:           # Open a session for a default 'Database'

        product = session
            .include("Category")                    # Include Category
            .load(product_id, object_type=Product)  # Load the Product and start tracking

        category = session.load(                    # No remote calls,
            product.Category,                       # Session contains this entity from .include
            object_type=Category)

        product.Name = "RavenDB"                    # Apply changes
        category.Name = "Database"

        session.save_changes()                      # Synchronize with the Server
                                                    # one request processed in one transaction
# endregion

# region client_4
    with store.open_session() as session:               # Open a session for a default 'Database'

        productNames = list(                            # Materialize query
            session
                .query(object_type=Product)             # Query for Products
                .where_greater_than("UnitsInStock", 5)  # Filter
                .skip(0).take(10)                       # Page
                .select("Name")                         # Project
        )
# endregion