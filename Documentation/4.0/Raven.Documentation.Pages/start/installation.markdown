# Installing RavenDB  

The following tutorial shows how to set up RavenDB.  

RavenDB 4 is cross-platform. You can use it on the follwoing platforms:

- Windows x64 / x86  
- Linux x64  
- Mac OS X  
- Raspberry Pi  
- Docker  

## Prerequisites  

For Windows, [Microsoft Visual C++ 2015 Redistributable Package](https://www.microsoft.com/en-US/download/details.aspx?id=52685) should be installed.

## Downloading the Server  

Go to [https://ravendb.net/downloads](https://ravendb.net/downloads), select the appropriate version and platform, and download the zip package.   

Below you can find the initial setup instructions for the various platforms. After getting RavenDB set up on your platform, you can 
continue to the next section, [Getting to Know RavenDB](getting-to-know).

<hr />

### Security Concerns

To let a developer start coding an application quickly, RavenDB will run with the default security mode. 

As long as the database is used inside the local machine and no outside connections are allowed, you can ignore security concerns 
and you require no authentication. Once you set RavenDB to listen to connections outside your local machine, 
your database will immediately block this now vulnerable configuration, and require the administrator to properly setup the security and 
access control to prevent unauthorized access to your data.

<strong>RavenDB will not let you listen to requests outside your local machine until you have adequately provided security for it.  </strong>  

Read more about security and how to enable authentication [here]()

<hr />

### Installing RavenDB with Docker

If you already have Docker installed, getting started is simple. Just run the following command:

    docker run -d -e UNSECURED_ACCESS_ALLOWED=PrivateNetwork 
        -e PUBLIC_SERVER_URL=http://10.0.75.2:8080 
        -e PUBLIC_TCP_SERVER_URL=http://10.0.75.2:38888 
        -p 8080:8080 
        -p 38888:38888 
        ravendb/ravendb

Docker will pull the latest stable RavenDB version and spin up a new container to host it. It is also possible to get the latest nightly build by using the ravendb/ravendb-nightly repository.

If port 8080 is already being used, RavenDB will fail to start and give you an "address in use" error (EADDRINUSE). In that case, simply change the port in the docker run command.

<strong>Our GUI, the RavenDB Management Studio, comes with every license: Community, Professional, and Enterprise. </strong>

You can access the [RavenDB Management Studio]() by going to http://10.0.75.2:8080 in your browser. This is assuming that you are using the default networking
configuration with Dokcer, and that the Dokcer instance is not exposed beyond the host machine. If you intend to host RavenDB on Docker and expose it 
externally, make sure to go through the security configuration first. 

For more options and information on running RavenDB under Docker, please visit the [Docker Section]().

<hr />

### Installing RavenDB on a Windows or Linux Host Machine

1. Extract the zip/tar file to a directory of your choice.  

2. In <strong>Windows</strong>, use the `Start.cmd` script or run `Server\Raven.Server.exe` directly.  In <strong>Linux</strong>, use the `start.sh` script or run `Server\Raven.Server` directly.  

This will run RavenDB in interactive mode inside a console application:

![Figure 1: RavenDB console.](images\console.png) 

If port 8080 is already being used, RavenDB will fail to start and give you an "address in use" error (EADDRINUSE).

You can customize the port and host from the command line by issuing the following command on <strong>Windows</strong> which will run RavenDB on port 8081, avoiding the conflicting port issue:    

    Server\Raven.Server.exe --ServerUrl=http://localhost:8081

Using <strong>Linux</strong>, the command should look like this:

    Server\Raven.Server --ServerUrl=http://localhost:8081

To change the default port, you also have the option to edit `Server\settings.json`. This file is where you would typically define server wide configurations.  

You can read all about it in the [Configuration Section](). For now we'll stick to the basics, these are the default initial settings:  

    {  
        "ServerUrl": "http://localhost:8080",  
        "DataDir": "APPDRIVE:/Raven",  
        "RunInMemory": false  
    }  

Once things are set up and the server is running, you can access our GUI, the [RavenDB Management Studio](), by going to http://localhost:8080 in your browser. The Studio comes with every license: Community, Professional, and Enterprise. 

![Figure 2: Accessing the Studio for the first time.](images\studio.png)

You are ready to continue to the next section, [Getting to Know RavenDB](getting-to-know).

<hr />

### Installing RavenDB on OS X


<hr />

### Installing RavenDB on a Raspberry Pi

<hr />
