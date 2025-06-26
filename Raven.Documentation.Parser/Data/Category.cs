namespace Raven.Documentation.Parser.Data
{
	using System.ComponentModel;

	using Raven.Documentation.Parser.Attributes;

	public enum Category
	{
		[Prefix("client-api")]
		[Description("Client API")]
		ClientApi,

		[Prefix("server")]
		Server,
        
        [Prefix("database")]
        Database,

		[Prefix("studio")]
		Studio,

		[Prefix("transformers")]
		Transformers,

		[Prefix("indexes")]
		Indexes,

		[Prefix("glossary")]
		Glossary,

		[Prefix("start")]
		[Description("Getting Started")]
		Start,

		[Prefix("file-system")]
		[Description("File System")]
		FileSystem,

		[Prefix("users-issues")]
		[Description("Users Issues")]
		UsersIssues,

		// legacy categories

		[Prefix("intro")]
		Intro,

		[Prefix("theory")]
		Theory,

		[Prefix("http-api")]
		[Description("HTTP API")]
		HttpApi,

		[Prefix("appendixes")]
		Appendixes,

		[Prefix("faq")]
		[Description("FAQ")]
		Faq,

		[Prefix("samples")]
		Samples,

		[Prefix("index")]
		Index,

        [Prefix("articles")]
        Articles,

        [Prefix("migration")]
        Migration,

        [Prefix("integrations")]
        Integrations,

        [Prefix("cloud")]
        Cloud,

        [Prefix("sharding")]
        Sharding,

        [Prefix("document-extensions")]
        [Description("Document Extensions")]
        DocumentExtensions,
        
        [Prefix("data-archival")]
        [Description("Data Archival")]
        DataArchival,
        
        [Prefix("ai-integration")]
        [Description("AI Integration")]
        AiIntegration
    }
}
