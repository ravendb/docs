package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.indexes.IndexFieldOptions;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;

import java.util.Collections;
import java.util.HashMap;

public class Storing {

    //region storing_1
    public static class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            map =  "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    LastName = employee.LastName" +
                "})";

            store("FirstName", FieldStorage.YES);
            store("LastName", FieldStorage.YES);
        }
    }
    //endregion

    public Storing() {
        try (IDocumentStore store = new DocumentStore()) {
            //region storing_2
            IndexDefinition indexDefinition = new IndexDefinition();
            indexDefinition.setName("Employees_ByFirstAndLastName");
            indexDefinition.setMaps(Collections.singleton("docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    LastName = employee.LastName" +
                "})"));

            java.util.Map<String, IndexFieldOptions> fields = new HashMap<>();
            indexDefinition.setFields(fields);

            IndexFieldOptions firstNameOptions = new IndexFieldOptions();
            firstNameOptions.setStorage(FieldStorage.YES);
            fields.put("FirstName", firstNameOptions);

            IndexFieldOptions lastNameOptions = new IndexFieldOptions();
            lastNameOptions.setStorage(FieldStorage.YES);
            fields.put("LastName", lastNameOptions);

            store
                .maintenance()
                .send(new PutIndexesOperation(indexDefinition));
            //endregion
        }
    }
}
