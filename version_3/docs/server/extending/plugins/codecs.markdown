# Plugins : Codecs

The `AbstractDocumentCodec` and `AbstractIndexCodec` classes have been introduced as an entry point to custom compression methods.

{CODE plugins_3_0@Server\Extending\Plugins.cs /}

{CODE plugins_3_1@Server\Extending\Plugins.cs /}

where:   
* **Encode** is executed when given document/index is written.   
* **Decode** is executed when provided document/index is read.    
* **Initialize** and **SecondStageInit** are used in trigger initialization process.   

## Example - Compression

{CODE plugins_3_2@Server\Extending\Plugins.cs /}

## Example - Encryption

{CODE plugins_3_3@Server\Extending\Plugins.cs /}

#### Related articles

TODO