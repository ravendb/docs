using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.Indexes
{
    public class Sidebyside
    {
        public Sidebyside()
        {
            using (var store = new DocumentStore
            {
                Urls = new []{ "http://localhost:8080/" },
                Database = "Northwind"
            })
            {
                store.Initialize();

            }
        }
    }
}
