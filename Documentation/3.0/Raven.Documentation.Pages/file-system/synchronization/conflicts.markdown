#Conflicts

Each file in the file system has a version number and history (both stored in file metadata). Every time you modify a file its version is increased and then the old one is moved to the history.
The conflict detection mechanism works based on these metadata values. A conflict can appear if two files having the same name are synchronized but they have different and independent versions.
Such scenario is possible if two files with have been uploaded under the same name to two different file systems and one of them attempts to synchronize its file to the second one.
From RavenFS perspective those two files are two completely unrelated, so they cannot override each other.