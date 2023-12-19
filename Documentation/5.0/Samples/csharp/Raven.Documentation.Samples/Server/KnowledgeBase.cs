using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide.Operations;
using Xunit;
using Xunit.Abstractions;
using Raven.Client.Documents.Operations.Configuration;
using static Raven.Documentation.Samples.ClientApi.Session.Configuration.PerSessionTopology;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.KnowledgeBase
{
    public class KnowledgeBase
    {
        public KnowledgeBase(ITestOutputHelper output)
        {
        }

        [Fact]
        public void IdentityPartsSeparatorIdentityId()
        {
            using (var store = new DocumentStore())
            {
                #region IdentityPartsSeparator_Identity
                // Change the ID separator from the default `/` to `-`
                store.Maintenance.Send(
                    new PutClientConfigurationOperation(
                        new ClientConfiguration { IdentityPartsSeparator = '-' }));

                using (var session = store.OpenSession())
                {
                    // The `|` causes the cluster to generate an identity
                    // The ID is unique over the whole cluster
                    // The first generated ID will be `Prefix-1`
                    session.Store(new User
                    {
                        Name = "John",
                        Id = "Prefix|"
                    });

                    session.SaveChanges();
                }
                #endregion
            }
        }

        [Fact]
        public void IdentityPartsSeparatorServerSideId()
        {
            using (var store = new DocumentStore())
            {
                #region IdentityPartsSeparator_ServerSideId
                // Change the ID separator from the default `/` to `-`
                store.Maintenance.Send(
                    new PutClientConfigurationOperation(
                        new ClientConfiguration { IdentityPartsSeparator = '-' }));

                using (var session = store.OpenSession())
                {
                    // Since an ID wasn't explicitly provided, the server generates one.
                    // The first generated ID on node A will be `users-1-A`
                    session.Store(new User
                    {
                        Name = "John",
                    });

                    session.SaveChanges();
                }
                #endregion
            }
        }

    }
}
