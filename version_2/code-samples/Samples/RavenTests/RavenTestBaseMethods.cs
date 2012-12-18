//-----------------------------------------------------------------------
// <copyright file="RavenTest.cs" company="Hibernating Rhinos LTD">
//     Copyright (c) Hibernating Rhinos LTD. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using Raven.Abstractions.MEF;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database;
using Raven.Database.Config;
using Raven.Database.Extensions;
using Raven.Database.Impl;
using Raven.Database.Plugins;
using Raven.Database.Server;
using Raven.Database.Storage;
using Raven.Json.Linq;
using Raven.Server;
using Xunit;

namespace Raven.Tests.Helpers
{
	public class RavenTestBase : IDisposable
	{
		protected readonly string DataDir = string.Format(@".\TestDatabase-{0}\", DateTime.Now.ToString("yyyy-MM-dd,HH-mm-ss"));

		private string path;
		protected readonly List<IDocumentStore> stores = new List<IDocumentStore>();

		public RavenTestBase()
		{
			CommonInitializationUtil.Initialize();

			ClearDatabaseDirectory();
			Directory.CreateDirectory(DataDir);
		}

		#region RavenTestBaseMethods
		public EmbeddableDocumentStore NewDocumentStore(bool runInMemory = true, string requestedStorage = null, ComposablePartCatalog catalog = null,
														bool deleteDirectory = true,	bool deleteDirectoryOnDispose = true);
		//Creates an embedded database in port 8079 and adds it to a list of stores

		public IDocumentStore NewRemoteDocumentStore(bool fiddler = false);
		//Creates a new database in port 8079

		public static void WaitForIndexing(IDocumentStore store);
		//Waits for all indexes to be non stale

		public static void WaitForUserToContinueTheTest(EmbeddableDocumentStore documentStore, bool debug = true);
		//Only active when debugging a test, will load the studio for the tested store

		protected void WaitForUserToContinueTheTest();
		//Only active when debugging a test, will load the studio for a server in port 8079
		#endregion

		#region RavenTestBaseViruals
		protected virtual void ModifyStore(DocumentStore documentStore);

		protected virtual void ModifyStore(EmbeddableDocumentStore documentStore);

		protected virtual void ModifyConfiguration(RavenConfiguration configuration);
		#endregion
	}
}