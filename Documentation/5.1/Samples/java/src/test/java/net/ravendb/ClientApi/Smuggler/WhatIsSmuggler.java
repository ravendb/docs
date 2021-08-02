import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.Operation;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.smuggler.DatabaseSmuggler;
import net.ravendb.client.documents.smuggler.*;

import java.io.IOException;

public class SmugglerSample {

    public static void main (String[] args){
        try (IDocumentStore store = new DocumentStore( new String[]{ "http://localhost:8080" }, "Northwind")) {
            store.initialize();
            DatabaseSmugglerExportOptions exportOptions;
            DatabaseSmugglerImportOptions importOptions;
            String toFile;
            DatabaseSmuggler toDatabase;
            String fromFile;

            try (IDocumentSession session = store.openSession()) {
                /*
                Employee employee1 = session.load(Employee.class, "employees/1-A");
                Employee employee2 = session.load(Employee.class, "employees/1-A");
                // because NoTracking is set to 'true'
                // each load will create a new Employee instance
                Assert.assertNotSame(employee1, employee2);
                 */
                //region for_database
                DatabaseSmuggler northwindSmuggler = store.smuggler().forDatabase("Northwind");
                //endregion

                DatabaseSmugglerExportOptions  smugglerExportOptions =null;
                exportOptions=null;
                importOptions=null;

                //region export_syntax
                //export
                public Operation exportAsync(DatabaseSmugglerExportOptions options, String toFile);

                public Operation exportAsync(DatabaseSmugglerExportOptions options, DatabaseSmuggler toDatabase);
                //endregion
                //region export_example
                // export only Indexes and Documents to a given file
                Operation exportOperation = store.smuggler().exportAsync(exportOptions,"C:\\ravendb-exports\\Northwind.ravendbdump");
                //endregion

                //region import_syntax
                public Operation importAsync(DatabaseSmugglerImportOptions options, String fromFile);
                public Operation importAsync(DatabaseSmugglerImportOptions options, InputStream stream);
                //endregion

                //region import_example
                Operation importOperation =  store.smuggler().importAsync(importOptions,"C:\\ravendb-exports\\Northwind.ravendbdump");
                //endregion


            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}
