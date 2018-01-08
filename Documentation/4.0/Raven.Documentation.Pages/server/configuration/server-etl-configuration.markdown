## Server Configuration : ETL Options

<br>

#### SQL.CommandTimeoutInSec
###### Set number of seconds after which SQL command will timeout
###### Default Value : null

If not set or set to null - use provider's default.
Can be overridden by setting *CommandTimeout* property value in SQL ETL configuration

<br><br>

#### ExtractAndTransformTimeoutInSec
###### Set number of seconds after which extraction and transformation will end and loading will start
###### Default Value : 300

<br><br>

#### MaxNumberOfExtractedDocuments
###### Max number of extracted documents in ETL batch
###### Default Value : null

If value is not set, or set to null - number of extracted documents is infinite per ETL batch 
