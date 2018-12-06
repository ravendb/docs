import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region throw_if_query_page_is_not_set
store.conventions.throwIfQueryPageSizeIsNotSet = true;
//endregion
