namespace Raven.Documentation.Samples.FileSystem.ClientApi.Listeners
{
	using Abstractions.FileSystem;

	public class Conflict
	{
		#region interface
		public interface IFilesConflictListener
		{
			/// <summary>
			/// Invoked when a conflict has been detected over a file.
			/// </summary>
			/// <param name="local">The file in conflict in its local version</param>
			/// <param name="remote">The file in conflict in its remote version</param>
			/// <param name="sourceServerUri">The source file system URL that tried to synchronize a file to a destination where the conflict appeared</param>
			/// <returns>A resolution strategy for this conflict</returns>
			ConflictResolutionStrategy ConflictDetected(FileHeader local, FileHeader remote, string sourceServerUri);

			/// <summary>
			/// Invoked when a file conflict has been resolved.
			/// </summary>
			/// <param name="instance">The file with the resolved conflict</param>
			void ConflictResolved(FileHeader instance);

		}
		#endregion

		#region example
		public class TakeNewestConflictsListener : IFilesConflictListener
		{
			public ConflictResolutionStrategy ConflictDetected(FileHeader local, FileHeader remote, string sourceServerUri)
			{
				if (local.LastModified.CompareTo(remote.LastModified) >= 0)
					return ConflictResolutionStrategy.CurrentVersion;
				else
					return ConflictResolutionStrategy.RemoteVersion;
			}

			public void ConflictResolved(FileHeader header)
			{
			}
		}
		#endregion
	}
}