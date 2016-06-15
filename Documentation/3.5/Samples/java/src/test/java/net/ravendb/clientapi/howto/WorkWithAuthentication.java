package net.ravendb.clientapi.howto;

import net.ravendb.client.document.DocumentStore;

public class WorkWithAuthentication {
  /*
  //region api_key_def
   ApiKeyDefinition
        {
            Name = "NorthwindAdminAccess",
            Secret = "MySecret",
            Enabled = true,
            Databases = new List<ResourceAccess>
            {
                new ResourceAccess
                {
                    TenantId = "Northwind",
                    Admin = true
                }
            }
        }
  //endregion
   */

  public WorkWithAuthentication() {
    //region api_key_setup
    DocumentStore documentStore = new DocumentStore();
    documentStore.setApiKey("NorthwindAdminAccess/MySecret");
    //endregion
  }
}
