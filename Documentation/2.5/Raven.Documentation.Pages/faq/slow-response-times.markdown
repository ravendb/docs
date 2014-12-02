#Resolving slow response times

In general, standard requests for RavenDB should take less than 10 milliseconds to complete. If it takes more, it is usually a cause for concern.

When you run RavenDB in debug mode, you can see the time a request took on the server, and this is the first place to look when you see slow response times on RavenDB. If you see a very fast response time on the server console, but very slow in your code, this is usually an indication of a network problem.

We had several reports about NOD32's HTTP Scanner slowing things down significantly, and other anti virus / proxies may do the same.