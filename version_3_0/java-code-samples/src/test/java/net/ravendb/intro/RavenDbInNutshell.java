package net.ravendb.intro;

import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNull;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;

public class RavenDbInNutshell {
	public static void main(String[] args) throws Exception {

		//#region nutshell1
		try (IDocumentStore store = new DocumentStore("http://localhost:8080", "crud")) {
			store.initialize();
			try (IDocumentSession session = store.openSession()) {
				Person marcin = new Person("Marcin", 26);
				assertNull(marcin.getId());
				session.store(marcin);
				assertNotNull(marcin.getId());

				Person john = new Person("John", 43);
				session.store(john);

				session.saveChanges();
			}
		}
		//#endregion
	}
}
