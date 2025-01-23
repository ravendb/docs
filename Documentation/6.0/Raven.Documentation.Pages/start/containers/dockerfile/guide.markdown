# Working with RavenDB's Dockerfile

RavenDB’s image and Dockerfile provide a universal and flexible base.
If you need to, you can either extend the existing `ravendb/ravendb` image with your modifications or build a new image entirely by tweaking its Dockerfile.
Additionally, for complex scenarios, you can replace the default entry script (`run-raven.sh`) in both cases to fully customize the startup process.


To learn about Dockerfile, visit Containers > Dockerfile > Overview.


If you want to modify the image, or use it as a base, go to Containers > Dockerfile > Extending.


To go deep with running RavenDB on prod in containers, visit Containers > Requirements, or read some of our articles - deployment guides.


If you have encountered a unique significant problem, contact our [support](https://ravendb.net/support) for help.
