using Raven.Client.Documents.Operations.Backups;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    internal class OlapConnectionString
    {
        public object Name { get; set; }
        public object LocalSettings { get; set; }
        public S3Settings S3Settings { get; internal set; }
    }
}