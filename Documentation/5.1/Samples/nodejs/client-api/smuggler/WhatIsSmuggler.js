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

//region for_database
const northwindSmuggler = store
    .smuggler
    .forDatabase("Northwind");
//endregion

//region export_syntax
 public async export (options: DatabaseSmugglerExportOptions, toDatabase: DatabaseSmuggler): Promise<OperationCompletionAwaiter>;
 public async export (options: DatabaseSmugglerExportOptions, toFile: string): Promise<OperationCompletionAwaiter>;
//endregion

//region export_example
const options = new DatabaseSmugglerExportOptions(operateOnTypes: DatabaseItemType.Indexes);
const operation = await store
    .smugglers
    .export(options, "C:\ravendb-exports\Northwind.ravendbdump");
await operation.waitForCompletion();
//endregion

//region import_syntax
public async import(options: DatabaseSmugglerImportOptions, fromFile: string): Promise < OperationCompletionAwaiter >;
public async import(options: DatabaseSmugglerImportOptions, stream: NodeJS.ReadableStream): Promise < OperationCompletionAwaiter >;
//endregion 

//region import_example
const options = new DatabaseSmugglerImportOptions(operateOnTypes: DatabaseItemType.Documents)
const operation = await dstStore.smuggler.import(options, "C:\ravendb-exports\Northwind.ravendbdump");
await operation.waitForCompletion();
//endregion