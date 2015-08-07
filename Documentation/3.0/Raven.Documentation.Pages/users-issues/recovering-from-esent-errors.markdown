#Recovering from Esent errors 

##Symptoms: 
- RavenDB server side error messages like:
    - `Secondary index is corrupt. The database must be defragmented`
    - `System.InvalidOperationException: Could not open transactional storage`

##Cause:
An error in Esent's file.

##Resolution:
Run Esent Utility for recovering from errors. There are two approaches: recovery or repair:

- Recovery - a loss-less operation that tries to recover the Esent file (`esentutl /r RVN /l logs /s system`)
- Repair - a more aggressive approach that may cause (`esentutl /p Data`)

 In both cases, at the end, the defragmentation should be performed (`esentutl /d Data`)

<hr/>

##Further read:
[Esentutl](https://technet.microsoft.com/en-us/library/hh875546.aspx)
