# Setup Examples: AWS Linux VM

In this walkthrough, we will setup RavenDB on an AWS EC2 t2.micro virtual machine running Ubuntu 16.04.

We will go through the necessary steps that are required for RavenDB to run securely including how to configure RavenDB 
with the correct IP addresses and ports.

It's recommended to read the [Setup Wizard](../../../start/installation/setup-wizard) section where you can find a detailed 
explanation about the RavenDB setup process.

## Create the VM

Access the EC2 Dashboard and click on Launch Instance.

![1](images/aws-linux/1.png)

Select your operating system. In our example we chose "Ubuntu Server 16.04 LTS (HVM), SSD Volume Type".

![2](images/aws-linux/2.png)

Select the machine type. We chose the t2.micro with 1 core and 1 GB of memory.

![3](images/aws-linux/3.png)

We will stick with the basic default settings of the machine and configure just the minimal requirements for RavenDB. 
You would probably want to go over the entire set of options and customize your VM. 

{WARNING:Security Concerns}
The following settings are fine if you just want to experiment with RavenDB. However, when security is needed you should set 
proper firewall rules and restrict access by IP. Please visit the [AWS security documentation](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/EC2_Network_and_Security.html)
for more information about securing your VM.
{WARNING/}

{NOTE:Elastic IP address}
By default, in AWS, an instance is assigned an IP addresses through DHCP. When the DHCP lease expires, or you restart the instance, this IP is released back to the pool and you will have to re-configure the RavenDB IP address.
To solve this problem, use an [Elastic IP address](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/elastic-ip-addresses-eip.html) which doesn't change, and can even be dynamically re-assigned to other instances as you wish.
{NOTE/}

Let's open ports 443 and 38888 for use by RavenDB. You may choose other port numbers off course and restrict access by IP.
RavenDB will use port 443 for HTTPS requests and port 38888 for TCP connections. We allow all incoming traffic on these ports by using 0.0.0.0.

![4](images/aws-linux/4.png)

Review your settings and launch the VM.

![5](images/aws-linux/5.png)

You will have to download a key pair which will be used later to connect to the machine using ssh.

![6](images/aws-linux/6.png)

Wait a couple minutes for the machine to initialize and click connect.

![7](images/aws-linux/7.png)

Follow the instructions and connect to the new machine using ssh.

![8](images/aws-linux/8.png)

{CODE-BLOCK:bash}
ssh -i "RavenDBUbuntuVMKeyPair.pem" ubuntu@ec2-35-160-249-162.us-west-2.compute.amazonaws.com
{CODE-BLOCK/}

## Configure the VM

RavenDB is written in .NET Core so it requires the same set of prerequisites as .NET Core.

{NOTE: Linux}

We highly recommend **updating** your **Linux OS** prior to launching the RavenDB server. Also check if .NET Core requires any other prerequisites in the [Prerequisites for .NET Core on Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites) article written by Microsoft.

{NOTE/}

Download RavenDB's latest stable using the following command:
{CODE-BLOCK:bash}
wget -O ravendb.tar.bz2 https://hibernatingrhinos.com/downloads/RavenDB%20for%20Linux%20x64/latest
{CODE-BLOCK/}

If you wish to use another RavenDB version, download it to your local machine and transfer the tar.bz2 file to the new VM using SCP. The following example shows how to transfer the file from the local machine to the /home/ubuntu directory in the VM.

{CODE-BLOCK:bash}
scp -i "RavenDBUbuntuVMKeyPair.pem" /local/download/location/RavenDB-4.0.3-nightly-20180414-0400-linux-x64.tar.bz2 ubuntu@ec2-35-160-249-162.us-west-2.compute.amazonaws.com:/home/ubuntu
{CODE-BLOCK/}

Extract the archive.

{CODE-BLOCK:bash}
tar xvjf RavenDB-4.0.3-nightly-20180414-0400-linux-x64.tar.bz2
{CODE-BLOCK/}

Allow RavenDB to use port 443 (non-root process):

{CODE-BLOCK:bash}
sudo setcap CAP_NET_BIND_SERVICE=+eip ./RavenDB/Server/Raven.Server
{CODE-BLOCK/}

Locate the VM's private and public IP addresses in the AWS EC2 Management Console.

![9](images/aws-linux/9.png)

You have a few choices on how to run the RavenDB server. 
We will use the [Setup Wizard](../../../start/installation/setup-wizard), but you can also configure things [manually](../../../start/installation/manual).

Let's edit the [settings.json](../../../server/configuration/configuration-options#json) file so that we can perform the setup remotely using the browser.
Notice that when we run the server for the first time, `settings.json` is created from `settings.default.json`. So if `settings.json` doesn't exist, edit `settings.default.json` instead.

{CODE-BLOCK:bash}
vi RavenDB/Server/settings.default.json
{CODE-BLOCK/}

Edit the `ServerUrl` field to contain the **Private IP** and the `PublicServerUrl` to contain the **Public DNS**. Also make sure to set the `Security.UnsecuredAccessAllowed` field to **PublicNetwork** which will allow you to connect remotely.

![10](images/aws-linux/10.png)

{NOTE:Write Permissions}

RavenDB requires write permissions to the following locations:

- The folder where RavenDB server is running
- The data folder
- The logs folder

{NOTE/}

Now we will setup and start the RavenDB service. 

Open a terminal and create the file /etc/systemd/system/ravendb.service, using super user permissions containing:

    [Unit]
    Description=RavenDB v4.0
    After=network.target

    [Service]
    LimitCORE=infinity
    LimitNOFILE=65536
    LimitRSS=infinity
    LimitAS=infinity
    User=<desired user>
    Restart=on-failure
    Type=simple
    ExecStart=/path/to/RavenDB/run.sh

    [Install]
    WantedBy=multi-user.target

![11](images/aws-linux/11.png)

Then run the following commands:

{CODE-BLOCK:bash}
sudo systemctl daemon-reload
sudo systemctl enable ravendb.service
sudo systemctl start ravendb.service
{CODE-BLOCK/}

## Run the RavenDB Setup Wizard

RavenDB is running and you can access it from your (local) browser using the VM's Public DNS (e.g. http://ec2-35-160-249-162.us-west-2.compute.amazonaws.com:443).

![12](images/aws-linux/12.png)

Accept the agreement and choose the setup type you want to do. In the example we choose to setup securely with a Let's Encrypt certificate.
You will need to claim your domain, read more [here](../../../start/installation/setup-wizard#secure-setup-with-a-let).

When you reach the point where you have to enter the IP addresses, you can go to the EC2 management console and check the machine's IP addresses.

Choose the **private IP address** here.

![13](images/aws-linux/13.png)

Check the External IP box and enter the **public IP address**. Then start the installation.

If you encounter errors during the process, please visit the [FAQ section](../../../server/security/common-errors-and-faq).

![14](images/aws-linux/14.png)

When the setup is finished, you will receive a configuration ZIP file which contains an admin client certificate which will allow you to connect using the browser. Keep the file safe.

Restart the server. 

![15](images/aws-linux/15.png)

## Access the Studio

If everything went well you should be redirected to the studio and Chrome should let you choose the client certificate to use (the one which was just created).

Some environments don't allow to set the client certificate automatically in the setup wizard so if you are not redirected to the Studio and you get an authentication error, please **close all instances of the browser** and install the admin client certificate manually. 

Now you can access the Studio. Open the browser and enter your new domain (e.g. https://a.aws.development.run).

Chrome will let you select the certificate. 

![16](images/aws-linux/16.png)
![17](images/aws-linux/17.png)

Access the certificate view to see both the loaded server certificate and the admin client certificate. Make sure to read the [security section](../../../server/security/overview) for better understanding of certificates and security issues.

![18](images/aws-linux/18.png)

Congratulations! You have a secure RavenDB server running on a simple EC2 machine. 

Don't forget to delete the `Security.UnsecuredAccessAllowed` property from [settings.json](../../../server/configuration/configuration-options#json). It's not necessary anymore because access to the server now requires using a registered client certificate. 

Connecting a few servers in a cluster is easy. Follow [these instructions](../../../start/installation/setup-wizard) to construct a cluster during setup.

## Related Articles

### Getting Started

- [Getting Started](../../../start/getting-started)

### Installation

- [Setup Wizard](../../../start/installation/setup-wizard)
- [Manual Setup](../../../start/installation/manual)
