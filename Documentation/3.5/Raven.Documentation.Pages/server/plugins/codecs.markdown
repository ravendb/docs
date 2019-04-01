# Plugins: Codecs

The `AbstractDocumentCodec` and `AbstractIndexCodec` classes have been introduced as an entry point to custom compression methods.

{CODE plugins_3_0@Server\Plugins.cs /}

{CODE plugins_3_1@Server\Plugins.cs /}

where:   
* **Encode** is executed when the given document/index is written.   
* **Decode** is executed when the provided document/index is read.    
* **Initialize** and **SecondStageInit** are used to trigger initialization process.   

## Example - Compression

{CODE plugins_3_2@Server\Plugins.cs /}

## Example - Encryption

{CODE plugins_3_3@Server\Plugins.cs /}
