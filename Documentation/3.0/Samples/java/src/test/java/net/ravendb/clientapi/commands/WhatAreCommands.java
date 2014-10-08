package net.ravendb.clientapi.commands;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.connection.IDatabaseCommands;
import net.ravendb.client.document.DocumentStore;


public class WhatAreCommands {
  @SuppressWarnings("unused")
  public WhatAreCommands() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region what_are_commands_1
      IDatabaseCommands commands = store.getDatabaseCommands();
      //endregion
    }
  }
}
