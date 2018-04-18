package net.ravendb.ClientApi;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;

import java.io.FileInputStream;
import java.io.IOException;
import java.security.GeneralSecurityException;
import java.security.KeyStore;

public class Certificate {

    public Certificate() throws Exception {

        //region client_cert
        // load certificate
        KeyStore clientStore = KeyStore.getInstance("PKCS12");
        clientStore.load(new FileInputStream("c:\\ravendb\\client-cert.pfx"), "passwordToPfx".toCharArray());

        try (DocumentStore store = new DocumentStore())  {
            store.setCertificate(clientStore);
            store.setDatabase("Northwind");
            store.setUrls(new String[]{ "https://my_secured_raven" });

            store.initialize();

            // do your work here
        }
        //endregion

    }
}
