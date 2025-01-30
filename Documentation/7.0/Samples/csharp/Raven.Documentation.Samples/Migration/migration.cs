using System;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents.DataArchival;

namespace Documentation.Samples.Migration
{
    public class migration
    {
        public void SwitchCompressionAlgorithm()
        {
            using (var store = new DocumentStore())
            {
                #region SwitchCompressionAlgorithm
                var DocumentConventions = new DocumentConventions
                {
                    // Switch HTTP compression algorithm
                    HttpCompressionAlgorithm = HttpCompressionAlgorithm.Gzip
                };
                #endregion
            }
        }

        public void switchBulkInsertState()
        {
            using (var store = new DocumentStore())
            {
                #region switchBulkInsertState
                using (var bulk = store.BulkInsert(new BulkInsertOptions
                {
                    // Disable bulk-insert compression
                    CompressionLevel = CompressionLevel.NoCompression
                }));
                #endregion
            }
        }

        public interface ISubscriptionCreationOverloadsNew
        {
            #region create_1
            // The create overload using a predicate:
            // ======================================
            string Create<T>(Expression<Func<T, bool>> predicate = null,
                PredicateSubscriptionCreationOptions options = null,
                string database = null);

            Task<string> CreateAsync<T>(Expression<Func<T, bool>> predicate = null,
                PredicateSubscriptionCreationOptions options = null,
                string database = null,
                CancellationToken token = default);
            
            // The options class:
            // ==================
            public sealed class PredicateSubscriptionCreationOptions
            {
                public string Name { get; set; }
                public string ChangeVector { get; set; }
                public string MentorNode { get; set; }
                public bool Disabled { get; set; }
                public bool PinToMentorNode { get; set; }
                public ArchivedDataProcessingBehavior? ArchivedDataProcessingBehavior { get; set; }
            }
            #endregion
        }

        public interface ISubscriptionCreationOverloadsOld
        {
            #region create_2
            // The create overload using a predicate:
            // ======================================
            string Create<T>(Expression<Func<T, bool>> predicate = null,
                SubscriptionCreationOptions options = null,
                string database = null);

            Task<string> CreateAsync<T>(Expression<Func<T, bool>> predicate = null,
                SubscriptionCreationOptions options = null,
                string database = null,
                CancellationToken token = default);
            
            // The options class:
            // ==================
            public class SubscriptionCreationOptions
            {
                public string Name { get; set; }
                public string Query { get; set; }
                public string ChangeVector { get; set; }
                public string MentorNode { get; set; }
                public virtual bool Disabled { get; set; }
                public virtual bool PinToMentorNode { get; set; }
                public ArchivedDataProcessingBehavior? ArchivedDataProcessingBehavior { get; set; }
            }
            #endregion
        }
    }
}
