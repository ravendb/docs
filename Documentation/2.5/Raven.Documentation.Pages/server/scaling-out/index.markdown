# Scaling out

RavenDB comes bundled with complete replication and sharding support. Both features are orthogonal, meaning they can be used in conjunction with one another.

Sharding is a client-side feature, meaning the entire decision making is made on the client side. Replication however is split between both ends - the servers replicate between themselves, and the needs to be aware of cases when one of the nodes comes down, and when the nodes report on a replication conflict.

We will discuss each of those features in their own chapters. Much of this is being demoed live [in a video on our YouTube channel](https://www.youtube.com/watch?v=yPnfT36P7Cs).

{FILES-LIST/}