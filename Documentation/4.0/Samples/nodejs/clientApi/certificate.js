//region client_cert
import { DocumentStore } from "ravendb";
import * as fs from "fs";

// load certificate and prepare authentication options
const authOptions = {
  certificate: fs.readFileSync("C:\\ravendb\\client-cert.pfx"),
  type: "pfx", // or "pem"
  password: "my passphrase" 
};

const store = new DocumentStore([ "https://my_secured_raven" ], "Northwind", authOptions);
store.initialize();

// proceed with your work here

//endregion
