package net.ravendb.ClientApi.Cluster;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.serverwide.ScriptResolver;
import net.ravendb.client.serverwide.operations.ModifyConflictSolverOperation;

import java.util.HashMap;
import java.util.Map;

public class DocumentConflictsInClientSide {
    public DocumentConflictsInClientSide() {
        try (IDocumentStore store = new DocumentStore()) {
            //region PUT_Sample
            try (IDocumentSession session = store.openSession()) {
                User user = new User();
                user.setName("John Doe");

                session.store(user, "users/123");
                // users/123 is a conflicted document
                session.saveChanges();
                // when this request is finished, the conflict for user/132 is resolved.
            }
            //endregion

            //region DELETE_Sample
            try (IDocumentSession session = store.openSession()) {
                session.delete("users/123"); // users/123 is a conflicted document
                session.saveChanges(); //when this request is finished, the conflict for users/132 is resolved.
            }
            //endregion

            //region Modify_conflict_resolution_sample
            try (IDocumentStore documentStore = new DocumentStore(
                new String[] { "http://<url of a database>" }, "<database name>")) {

                Map<String, ScriptResolver> resolveByCollection = new HashMap<>();
                ScriptResolver scriptResolver = new ScriptResolver();
                scriptResolver.setScript(
                    "  var final = docs[0];" +
                    "  for(var i = 1; i < docs.length; i++)" +
                    "  {" +
                    "      var currentCart = docs[i];" +
                    "      for(var j = 0; j < currentCart.Items.length; j++)" +
                    "      {" +
                    "          var item = currentCart.Items[j];" +
                    "          var match = final.Items" +
                    "                           .find( i => i.ProductId == item.ProductId);" +
                    "          if (!match)" +
                    "          {" +
                    "              // not in cart, add" +
                    "              final.Items.push(item);" +
                    "          } else { " +
                    "              match.Quantity = Math.max(" +
                    "                          item.Quantity ," +
                    "                          match.Quantity);" +
                    "          }" +
                    "      }" +
                    "  }" +
                    "  return final; // the conflict will be resolved to this variant");
                resolveByCollection.put("ShoppingCarts", scriptResolver);

                ModifyConflictSolverOperation op = new ModifyConflictSolverOperation(
                    documentStore.getDatabase(),
                    resolveByCollection,  //we specify conflict resolution scripts by document collection
                    true // if true, RavenDB will resolve conflict to the latest
                    // if there is no resolver defined for a given collection or
                    // the script returns null
                );

                store.maintenance().server().send(op);
            }
            //endregion
        }
    }

    public static class User {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
