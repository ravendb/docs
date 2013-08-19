namespace RavenCodeSamples.Server.Extending.Bundles
{
	using System.Linq;

	public class Question
	{
		public string Title { get; set; }
		public Vote[] Votes { get; set; }
	}

	public class Vote
	{
		public bool Up { get; set; }
		public string Comment { get; set; }
	}

	public class Docs
	{
		public Question[] Questions { get; set; }
	}

	public class IndexReplication : CodeSampleBase
	{
		public IndexReplication()
		{
			#region indexreplication1
			var q = new Question
			{
				Title = "How to replicate to relational database?",
				Votes = new[]
				{
					new Vote {Up = true, Comment = "Good!"},
					new Vote {Up = false, Comment = "Nah!"},
					new Vote {Up = true, Comment = "Nice..."}
				}
			};

			#endregion

			var docs = new Docs();

			var x =

			#region indexreplication2
			// Questions/TitleAndVoteCount
			from question in docs.Questions
			select new
			{
				Title = question.Title,
				VoteCount = question.Votes.Length
			};

			#endregion

			#region indexreplication3
			new Raven.Bundles.IndexReplication.Data.IndexReplicationDestination
			{
				Id = "Raven/IndexReplication/Questions/TitleAndVoteCount",
				ColumnsMapping =
					{
						{"Title", "Title"},
						{"UpVotes", "UpVotes"},
						{"DownVotes", "DownVotes"},
					},
				ConnectionStringName = "Reports",
				PrimaryKeyColumnName = "Id",
				TableName = "QuestionSummaries"
			};

			#endregion
		}
	}
}