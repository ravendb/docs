#How to index across entities?

RavenDB allows you to easily index a single entity by specifying:

    from user in docs.Users
    select new { user.Name}

But what happen if you want to index Users or Employees? You can do that easily enough using:

    from doc in docs
    let entityName = doc["@metadata"]["Raven-Entity-Name"]
    where entityName == "Users" || entityName == "Employees"
    select new { doc.Name }

This method allow you to index all users and all employees in the same index, and queries on this index will be able to find either type of documents.