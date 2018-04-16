# Setup Examples : Docker on AWS Linux VM

In this walkthrough we will setup RavenDB using Docker on an AWS EC2 t2.micro virtual machine running Ubuntu 16.04.

We will go through the necessary steps that are required for RavenDB to run securely including how to configure RavenDB 
with the correct IP addresses and ports.

It's recommended to read the [Setup Wizard](../../../start/installation/setup-wizard) section where you can find a detailed 
explanation about the RavenDB setup process.

## Creating the VM

Access the EC2 Dashboard,  and click on Launch Instance.

![1](images/aws-linux/1.png)

Select your operating system. In our example we choose "Ubuntu Server 16.04 LTS (HVM), SSD Volume Type".

![2](images/aws-linux/2.png)

Select the machine type. We choose the t2.micro with 1 core and 1 GB of memory.

![3](images/aws-linux/3.png)

We stick with the basic default settings of the machine and configure just the minimal requirements for RavenDB. 
You would probably want to go over the entire set of options and customize your VM. 

{WARNING:Security Concerns}
The following settings are fine if you just want to experiment with RavenDB, however when security is needed you should set 
proper firewall rules and restrict access by IP. Please visit the [AWS security documentation](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/EC2_Network_and_Security.html)
for more information about securing your VM.
{WARNING/}

Let's open ports 8080 and 38888 for use by RavenDB. You may choose other port numbers off course and restrict access by IP.
RavenDB will use port 8080 for HTTPS requests and port 38888 for TCP connections. We allow all incoming traffic on these ports by using 0.0.0.0.

![4](images/aws-docker/ports.png)

Review your settings and launch the VM.

![5](images/aws-docker/review.png)

You will have to download a key pair which will be used later to connect to the machine using ssh.

Wait a couple minutes for the machine to initialize and click connect.

![7](images/aws-docker/connect.png)

Follow the instructions and connect to the new machine using ssh.

{CODE-BLOCK:bash}
ssh -i "RavenDBUbuntuVMKeyPair.pem" ubuntu@ec2-54-200-27-219.us-west-2.compute.amazonaws.com
{CODE-BLOCK/}

Let's update the OS and install Docker, in Ubuntu for example it looks something like this:

{CODE-BLOCK:bash}
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
sudo apt-get update
sudo apt-get install -y docker-ce
{CODE-BLOCK/}

Run the following command to create a new RavenDB container. For more information visit the [Docker documentation section](../running-in-docker-container).

{CODE-BLOCK:bash}
sudo docker run -d --name raven4 -p 8080:8080 -p 38888:38888 ravendb/ravendb
{CODE-BLOCK/}

Use this command to view the status of your containers
{CODE-BLOCK:bash}
sudo docker ps
{CODE-BLOCK/}

View the full details of the container and notice the IPAddress field (172.17.0.2 in our example). This is the **private IP address** that RavenDB binds to inside the container. You will use this address later in the wizard.
{CODE-BLOCK:bash}
sudo docker inspect raven4
{CODE-BLOCK/}

You can connect to the container using
{CODE-BLOCK:bash}
sudo docker exec -it raven4 bash
{CODE-BLOCK/}

And then you can connect to the RavenDB CLI using:
{CODE-BLOCK:bash}
./rvn admin-channel
{CODE-BLOCK/}

You have a few choices on how to run the RavenDB server. 
We will use the [Setup Wizard](../../../start/installation/setup-wizard), but you can also configure things [manually](../../../start/installation/manual).

RavenDB is running and you can access it from your (local) browser using the VM's Public DNS (e.g. http://ec2-54-200-27-219.us-west-2.compute.amazonaws.com:8080).

![12](images/aws-docker/browser.png)

Accept the agreement and choose the setup type you want to do. In the example we choose to setup securely with a Let's Encrypt certificate.
You will need to claim your domain, read more [here](../../../start/installation/setup-wizard#secure-setup-with-a-let).

When you reach the point where you have to enter the IP addresses, you can go to the EC2 management console and check the machine's public IP address.

![9](images/aws-docker/addresses.png)

Back in the Wizard, enter the private IP address RavenDB binds to (172.17.0.2 in our example) in the "IP Address / Hostname" field.

Check the External IP box and enter the **public IP address** of the VM (54.200.27.219 in our example). Make sure to enter the ports you exposed in the docker run command in the "External Port" fields (8080 and 38888 in our example). 

Start the installation.

![14](images/aws-docker/ips.png)

If you encounter errors during the process, please visit the [FAQ section](../../../server/security/common-errors-and-faq).

When the setup is finished, you will receive a configuration ZIP file which contains an admin client certificate which will allow you to connect using the browser. Keep the file safe.

Restart the server. 

![15](images/aws-docker/restart.png)

If everything went well, you should be redirected to the studio and Chrome should let you choose the client certificate to use (the one which was just created).

Some environments don't allow to set the client certificate automatically in the setup wizard so if you are not redirected to the Studio, and you get an authentication error, please **close all instances of the browser** and install the admin client certificate manually. 

Now you can access the Studio, open the browser and enter your new domain (e.g. https://a.docker.development.run).

Chrome will let you select the certificate. 

![16](images/aws-docker/cert.png)
![17](images/aws-docker/studio.png)

Access the certificate view to see both the loaded server certificate and the admin client certificate. Make sure to read the [security section](../../../server/security/overview) for better understanding of certificates and security issues.

![18](images/aws-docker/certview.png)

Congratulations, you have a secure RavenDB server running on Docker on a simple EC2 machine. Have fun with it!

Connecting a few servers in a cluster is easy. Follow [these instructions](../../../start/installation/setup-wizard) to construct a cluster during setup.
