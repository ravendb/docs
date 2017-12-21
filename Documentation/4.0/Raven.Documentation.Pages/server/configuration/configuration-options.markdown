# Server Configuration
RavenDB is **Safe by Default** which means its set of options are configured for best safety.  However, these options can be manually configured in order to accommodate different server behavior.

## Setting Config Options
There are few ways to configure option values before initiating the new server's instance.

### Environment Variable
Setting environment variable with the following sytax will set a configuration value.

#Usage:
 `RAVEN_<ConfigOption>` or `RAVEN.<ConfigOption>`

#Example:
```
export RAVEN_Setup.Mode=None
```

### settings.json
On the server executable directory lies `setting.json` which will be read and applied on server startup. 
Usage : `"ConfigOption": "ConfigValue"`

#Example : 
```
{
    "ServerUrl": "http://127.0.0.1:8080",
    "Setup.Mode": "None"
}
```

{NOTE setting.json config options OVERRIDES the environment variables settings! /}

### Command Line Arguments
The Raven.Server executable can configure options using arguments which can be passed to the console application (or while running as daemon)
Usage: --<ConfigOption>=<ConfigValue>

#Example:
```
./Raven.Server --Setup.Mode=None
```

{NOTE Executable arguments config options OVERRIDES environment variables settings and setting.json! /}

