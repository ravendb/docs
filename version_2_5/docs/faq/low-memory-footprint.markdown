#Reducing RavenDB memory footprint

By default, RavenDB will trade-off higher memory usage for better performance, you can limit the amount of memory RavenDB will use when running on memory constrained hardware.

The following configuration options instruct RavenDB to use much less memory:

    <add key="Raven/Esent/CacheSizeMax" value="256"/>
    <add key="Raven/Esent/MaxVerPages" value="32"/>

The first value is the maximum size of the cache, limited to 256 megabytes. The second is the maximum number of version pages to keep in memory, limited to 32 bytes.