# Configuration: Backup

{PANEL:Server.LocalRootPath}

Local backups can only be created under this root path.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Server.AllowedDestinations}

Semicolon separated list of allowed backup destinations. If not specified, all destinations are allowed. 

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

Possible values: 

- `None`
- `Local`
- `Azure`
- `AmazonGlacier`
- `AmazonS3`
- `FTP`

{PANEL/}

{PANEL:Server.AllowedAwsRegions}

Semicolon separated list of allowed AWS regions. If not specified, all regions are allowed.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}
