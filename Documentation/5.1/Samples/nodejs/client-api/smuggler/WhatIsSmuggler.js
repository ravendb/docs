import { IDocumentStore } from "../IDocumentStore";
import { StringUtil } from "../../Utility/StringUtil";
import { DatabaseSmugglerImportOptions } from "./DatabaseSmugglerImportOptions";
import { throwError } from "../../Exceptions";
import { HttpRequestParameters, HttpResponse } from "../../Primitives/Http";
import { DatabaseSmugglerExportOptions } from "./DatabaseSmugglerExportOptions";
import { HttpCache } from "../../Http/HttpCache";
import { HeadersBuilder } from "../../Utility/HttpUtil";
import { DatabaseSmugglerOptions } from "./DatabaseSmugglerOptions";
import * as fs from "fs";
import * as StreamUtil from "../../Utility/StreamUtil";
import { LengthUnawareFormData } from "../../Utility/LengthUnawareFormData";
import * as path from "path";
import { BackupUtils } from "./BackupUtils";
import { RequestExecutor } from "../../Http/RequestExecutor";
import { OperationCompletionAwaiter } from "../Operations/OperationCompletionAwaiter";
import { GetNextOperationIdCommand } from "../Commands/GetNextOperationIdCommand";
import { RavenCommand, ResponseDisposeHandling } from "../../Http/RavenCommand";
import { DocumentConventions } from "../Conventions/DocumentConventions";
import { ServerNode } from "../../Http/ServerNode";
import { DatabaseSmuggler } from "../../Http/ServerNode";

//document_store_creation
const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
store.initialize();
    //const session = store.openSession();

const store1 = new DatabaseSmuggler();
let toDatabase, toFile, fromFile, stream;


//region for_database
const northwindSmuggler = store
    .smuggler
    .forDatabase("Northwind");
//endregion

//region export_syntax
const operation = await store.smuggler.export(options, toDatabase);

const operation = await store.smuggler.export(options, toFile);
//endregion

//region export_example
const options = new DatabaseSmugglerExportOptions();
options.operateOnTypes = ["Documents"];
const operation = await store
    .smuggler
    .export(options, "C:\\ravendb-exports\\Northwind.ravendbdump");
await operation.waitForCompletion();
//endregion

//region import_syntax
const operation = await store.smuggler.import(options, fromFile);

const operation = await store.smuggler.import(options, stream);
//endregion 

//region import_example
const options = new DatabaseSmugglerImportOptions();
options.operateOnTypes = ["Documents"];
const operation = await store.smuggler.import(options, "C:\\ravendb-exports\\Northwind.ravendbdump");
await operation.waitForCompletion();
//endregion