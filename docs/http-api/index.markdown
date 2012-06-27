# HTTP API - Overview

RavenDB provides an HTTP API for accessing and manipulating data on the server. This API sits next to the C# Client API, providing most of the same functionality, but with a platform agnostic, web friendly interface. Using the HTTP API, it's possible to write a fully functioning RavenDB application using just Javascript and HTML.

As part of being web friendly, the HTTP API follows commonly understood RESTful principles. For example, database documents are addressable resources via unique URLs and those resources can be acted upon using the HTTP verbs GET, PUT, POST and DELETE.

However, while being RESTful is a goal of the HTTP API, it is secondary to the goal of exposing easy to use and powerful functionality such as batching and multi-document transactions.

The remaining sections of this documentation explain how to use this HTTP API to build powerful web applications. Throughout these sections example request and responses are shown. These requests were created using the powerful command line tool, curl. More details about curl can be found at: [http://curl.haxx.se/docs/manual.html](http://curl.haxx.se/docs/manual.html)

{FILES-LIST/}