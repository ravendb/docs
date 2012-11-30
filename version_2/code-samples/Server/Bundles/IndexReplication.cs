namespace RavenCodeSamples.Server.Bundles
{
	using System.Linq;

	public class IndexReplication : CodeSampleBase
	{
		private class Question
		{
			public string Title { get; set; }
			public Vote[] Votes { get; set; }
		}

		private class Vote
		{
			public bool Up { get; set; }
			public string Comment { get; set; }
		}

		private class Docs
		{
			public Question[] Questions { get; set; }
		}

		public IndexReplication()
		{
			#region indexreplication1

			var q = new Question
						{
							Title = "How to replicate to relational database?",
							Votes =
								new[]
							        {
								        new Vote { Up = true, Comment = "Good!" }, 
										new Vote { Up = false, Comment = "Nah!" }, 
										new Vote { Up = true, Comment = "Nice..." },
								        new Vote { Up = false, Comment = "No!" }
							        }
						};
			#endregion

			var docs = new Docs();

			var z =

			#region indexreplication2
				// Questions/TitleAndVoteCount
				from question in docs.Questions
				select new
						   {
							   Title = question.Title,
							   UpVotes = question.Votes.Count(x => x.Up),
							   DownVotes = question.Votes.Count(x => !x.Up)
						   };

			#endregion

			#region indexreplication3

			new Raven.Bundles.IndexReplication.Data.IndexReplicationDestination
				{
					Id = "Raven/IndexReplication/Questions/TitleAndVoteCount",
					ColumnsMapping =
						{
							{ "Title", "Title" }, 
							{ "UpVotes", "UpVotes" }, 
							{ "DownVotes", "DownVotes" },
						},
					ConnectionStringName = "Reports",
					PrimaryKeyColumnName = "Id",
					TableName = "QuestionSummaries"
				};

			#endregion
		}
	}
}