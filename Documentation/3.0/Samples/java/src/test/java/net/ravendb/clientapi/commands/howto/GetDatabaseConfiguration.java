package net.ravendb.clientapi.commands.howto;

import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class GetDatabaseConfiguration {
	@SuppressWarnings("unused")
	private interface IFoo {
		//region database_configuration_1
		RavenJObject getDatabaseConfiguration();
		//endregion
	}

	@SuppressWarnings("unused")
	public GetDatabaseConfiguration() throws Exception {
		try (IDocumentStore store = new DocumentStore()) {
			//region database_configuration_2
			RavenJObject configuration = store
				.getDatabaseCommands()
					.getAdmin()
					.getDatabaseConfiguration();
			//endregion
		}
	}

}
