# RavenDB 4.0 Passes Intense Security Check<br/><small>by <a href="https://www.linkedin.com/in/ravendb/">Oren Eini</a></small>

![RavenDB 4.0 Passes Intense Security Check](images/ravendb40-passes-intense-security-check.png)

{SOCIAL-MEDIA-LIKE/}

<br/>

Security has always been the top priority, even higher than performance. As part of the release cycle for RavenDB 4.0, we wanted an external audit to go over our security infrastructure and ensure that we didn’t miss any hole to which someone can enter. After over 100,000 instances of MongoDB, ElasticSearch, Apache CouchDB, and other non-relational databases were hacked into, we knew that we had to perform even more stringent security tests than ever before to make RavenDB 4.0 safer than ever.

{SOCIAL-MEDIA-FOLLOW/}

We hired one of the best security companies, staffed with the fiercest hackers in the industry, to do their worst to break down RavenDB’s defenses. We tasked them with finding out all of our vulnerabilities and holes so we know exactly what needs reinforcement.

The security team found 15 areas of weakness. We immediately got to work and resolved all the issues.

You can read the full report and the overall security architecture of RavenDB 4.0 here:
<a href="https://ravendb.net/Content/pdf/ravendb-final-security-report-jan-12-2017.pdf">https://ravendb.net/Content/pdf/ravendb-final-security-report-jan-12-2017.pdf</a>

## What You Get with RavenDB 4.0 Security

RavenDB deploys cryptography essentially on two different fronts: symmetric cryptography of all data on disk, and asymmetric cryptography via X.509 certificates as a means of authentication between clients and servers.

All symmetric encryption uses Daniel J. Bernstein’s XChaCha20Poly1305 algorithm, as implemented in libsodium, with a randomized 192-bit nonce. There is no possibility of nonce-reuse, which means that it is considerably more resilient than adhoc designs that might make a best-effort attempt to avoid nonce-reuse, without ensuring it. Symmetric encryption covers the database main data store, index definitions, journal, temporary file streams, and secret handling. Such secret handling uses the Windows APIs for protected data, but only for a randomly generated encryption key, which is then used as part of the XChaCha20Poly1305 AEAD, to add a form of authentication. All long-term symmetric secrets are derived from a master key using the Blake2b hash function with a usage-specific context identifier.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

At setup time, client and server certificates are generated. Clients trust the server’s self-signed certificate, and the server trusts each client based on a fingerprint of each client’s certificate. All data is exchanged over TLS, and TLS version failures for certificate failures are handled gracefully, with a webpage being shown indicating the failure status, rather than aborting the TLS handshake. Server certificates are optionally signed by Let’s Encrypt using a vendor-specific domain name. Certificates are generated using BouncyCastle and are 4096-bit RSA. 
Keys, nonces, and certificate private keys are randomly generated using the operating system’s CSPRNG, either through libsodium or through BouncyCastle.

## Easy to Use Database Security

Erecting proper security layers is always a challenge. But we take the approach that the onus is on us to make the fortress impenetrable, and for you to be able to construct it with ease. We know that if database security requires pages and pages of documents with sophisticated instructions, it is easy for something to get overlooked and leave you and your data exposed.

An impenetrable fortress is useless if you forget to close all the locks properly, right? That will not be happening with RavenDB 4.0.

Most security breaches occur in businesses where an employee made a minor mistake that was overlooked by the security team. The bigger the process for setting up security, the greater the vulnerability that someone in your organization will miss something and an unruly hacker will find it. Making installation and maintenance simple is its own security layer.

Watch me set up a secure multi-node cluster, ready for deployment, <em>all in just 10 minutes</em>!

<div class="youtube-frame">
    <div class="embed-responsive embed-responsive-16by9">
        <iframe class="embed-responsive-item" width="516" height="315" src="https://www.youtube.com/embed/K-2iZ_lJVag" frameborder="0" allowfullscreen></iframe>
    </div>
</div>

<div class="bottom-line">
    <p>
        Try it out! <a href="https://ravendb.net/downloads#server/dev"><strong>Grab RavenDB 4.0 for free</strong></a>, and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running for your next project. RavenDB can be used on cloud solutions like Amazon Web Services (AWS) and Microsoft Azure.
    </p>
    <p>
        <a href="http://ravendb.net/"><strong>RavenDB 4.0</strong></a> is an ACID document database that specializes in online transaction processing (OLTP). It's fully transactional (ACID) across entire database, and features an SQL-like querying language. You can have the best of relational databases while enjoying the high performance of a non-relational solution. RavenDB gives you a distributed data cluster, flexibility, and rapid scalability with low overhead. RavenDB is an easy to use all-in-one database, striving to minimize your need for third party applications, tools, or support. You can set up RavenDB in a matter of minutes. 
    </p>
    <p>
        RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 writes per second on a single node. You can build high-performance, low-latency applications quickly and efficiently.
    </p>
</div>
