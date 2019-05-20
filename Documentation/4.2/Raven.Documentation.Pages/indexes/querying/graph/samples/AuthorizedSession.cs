using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;

namespace AuthorizationBundle
{
    public sealed class AuthorizedSession : IDisposable
    {
        private IDocumentSession _session;

        private AuthorizedSession(IDocumentStore store, IDocumentSession session, User user)
        {
            _store = store;
            _session = session;
            _user = user;
        }

        public static AuthorizedSession OpenSession(IDocumentStore store , string userId, string password)
        {
            IDocumentSession session = null;
            try
            {
                session = store.OpenSession();                
                var user = session.Load<User>(userId);
                if (user == null)
                    ThrowNoPermissions(userId);
                //TODO:change to graph query
                if (CheckValidAuthorizedUser(userId, password, user.PasswordHash) == false)
                    ThrowNoPermissions(userId);                
                return new AuthorizedSession(store, session, user);
            }
            catch
            {
                session?.Dispose();
                throw;
            }
        }

        private static readonly string Root = "RootUser"; 
        public void CreateUser(string user, string password)
        {
            try
            {
                using (var session = _store.OpenSession(new SessionOptions
                {
                    TransactionMode = TransactionMode.ClusterWide
                }))
                {
                    var value = session.Advanced.ClusterTransaction.GetCompareExchangeValue<string>("users/" + Root);
                    if (value.Value != _user.Id)
                    {
                        throw new UnauthorizedAccessException($"User {_user.Id} is not allowed to create new users");
                    }

                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue("users/" + user, 0);
                    session.Store(new User
                    {
                        Id = user,
                        PasswordHash = ComputeHash(user, password),
                        Groups = new HashSet<string>()
                    });
                    session.SaveChanges();
                }
            }
            catch (ConcurrencyException _)
            {
                using (OpenSession(_store, user, password))
                {
                    //Concurrent creation of the same user
                }
            }
        }

        //TODO: add method to add user to group
        public void AddUserToGroup(string userId, string groupName)
        {
            using (var session = _store.OpenSession(new SessionOptions
            {
                TransactionMode = TransactionMode.ClusterWide
            }))
            {
                var groupValue = session.Advanced.ClusterTransaction.GetCompareExchangeValue<Group.GroupVersion>("groups/" + groupName);
                if (groupValue.Value.Creator != _user.Id)
                {
                    throw new UnauthorizedAccessException($"Only the group creator {_user.Id} may add new members to {groupName} group");
                }
                var userVersion = session.Advanced.ClusterTransaction.GetCompareExchangeValue<int>("users/" + userId);
                session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(new CompareExchangeValue<int>(userVersion.Key,userVersion.Index,userVersion.Value+1));
                var user = session.Load<User>(userId);
                var group = session.Load<Group>(groupName);                
                session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(
                    new CompareExchangeValue<Group.GroupVersion>(groupValue.Key, groupValue.Index, 
                        new Group.GroupVersion
                        {
                            Creator = groupValue.Value.Creator,
                            Version = groupValue .Value.Version
                        }));
                group.Members.Add(userId);
                session.Store(group);
                user.Groups.Add(groupName);
                session.Store(user);
                session.SaveChanges();
            }
        }

        //TODO: add methods for giving and revoking permissions
        public void GiveUserPermissionToAccessDocument(string userId, string documentId, string collection)
        {
            if (CheckIfUserHasPermissionTo(documentId, collection) == false)
            {
                throw new UnauthorizedAccessException(
                    $"User {_user.Id} doesn't have permissions to access document {documentId} in collection {collection}");
            }

            using (var session = _store.OpenSession(new SessionOptions
            {
                TransactionMode = TransactionMode.ClusterWide
            }))
            {
                var userVersion = session.Advanced.ClusterTransaction.GetCompareExchangeValue<int>("users/" + userId);
                if(userVersion == null)
                    throw new InvalidOperationException($"No user named {userId} in the system");
                var user = session.Load<User>(userId);
                session.Advanced.ClusterTransaction.UpdateCompareExchangeValue(new CompareExchangeValue<int>(userVersion.Key, userVersion.Index, userVersion.Value+1));
                if (user.Permissions == null)
                {
                    user.Permissions = new Permission{Ids = new HashSet<string>{documentId}};
                }
                else if (user.Permissions.Ids == null)
                {
                    user.Permissions.Ids = new HashSet<string> { documentId };
                }
                else
                {
                    user.Permissions.Ids.Add(documentId);
                }
                session.Store(user, userId);
                session.SaveChanges();
            }
        }

        private bool CheckIfUserHasPermissionTo(string documentId, string collection)
        {
            var res = _session.Advanced.GraphQuery<dynamic>("match (dp) or (u)-[Groups]->(ag) or (u)-[Groups]->(Groups as g)-recursive (0, shortest) {[Parent]->(Groups)}-[Parent]->(ag)")
                .With("u", _session.Query<User>().Where(u => u.Id == _user.Id))
                .With("dp",
                    _session.Query<User>().Where(u =>
                        u.Id == _user.Id && (u.Permissions.Ids.Contains(documentId) ||
                                             u.Permissions.Collections.Contains(collection))))
                .With("ag",
                    _session.Query<Group>().Where(g =>
                        g.Permission.Ids.Contains(documentId) || g.Permission.Collections.Contains(collection)));
            if (res.FirstOrDefault() == null)
            {
                //check if root user
                using (var session = _store.OpenSession(new SessionOptions
                {
                    TransactionMode = TransactionMode.ClusterWide
                }))
                {
                    var value = session.Advanced.ClusterTransaction.GetCompareExchangeValue<string>("users/" + Root);
                    return value.Value == _user.Id;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowNoPermissions(string userId)
        {
            throw new UnauthorizedAccessException($"User {userId} is not a registered user");
        }

        private static bool CheckValidAuthorizedUser(string user, string password, string userHash)
        {
            return userHash == ComputeHash(user, password);
        }

        private static string ComputeHash(string user, string password)
        {
            var sha = SHA256.Create();
            var userBuffer = Encoding.UTF8.GetBytes(user);
            var passwordBuffer = Encoding.UTF8.GetBytes(password);
            var buffer = new byte[userBuffer.Length + passwordBuffer.Length];
            Buffer.BlockCopy(userBuffer, 0, buffer, 0, user.Length);
            Buffer.BlockCopy(passwordBuffer, 0, buffer, user.Length, passwordBuffer.Length);
            var hash = Convert.ToBase64String(sha.ComputeHash(buffer));
            return hash;
        }

        private User _user;
        private IDocumentStore _store;

        public void Dispose()
        {
            _session?.Dispose();
        }

        public static void CreateRootUser(IDocumentStore store, string root, string password)
        {
            using (var session = store.OpenSession(new SessionOptions
            {
                TransactionMode = TransactionMode.ClusterWide
            }))
            {
                try
                {
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue("users/" + Root, root);
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue("users/" + root, 0);
                    session.Store(new User
                    {
                        Id = root,
                        PasswordHash = ComputeHash(root, password)
                    });
                    session.SaveChanges();
                }
                catch (ConcurrencyException _)
                {
                    var rootName = session.Advanced.ClusterTransaction.GetCompareExchangeValue<string>("users/" + Root);
                    if (rootName.Value != root)
                    {
                        throw new UnauthorizedAccessException($"Can't generate root user {root} since there is another root user named {rootName.Value}");
                    }
                }
            }
        }

        public void CreateGroup(string groupName,string parent, string description, Permission permission)
        {
            using (var session = _store.OpenSession(new SessionOptions
            {
                TransactionMode = TransactionMode.ClusterWide
            }))
            {
                try
                {
                    session.Advanced.ClusterTransaction.CreateCompareExchangeValue("groups/" + groupName, 
                        new Group.GroupVersion
                        {
                            Creator = _user.Id,
                            Version = 0
                        });
                    session.Store(new Group
                    {
                        Parent = parent,
                        Description = description,
                        Members = new HashSet<string>(),
                        Permission = permission
                    }, groupName);
                    session.SaveChanges();
                }
                catch (ConcurrencyException _)
                {
                    //Group was already created 
                    var creator = session.Advanced.ClusterTransaction.GetCompareExchangeValue<Group.GroupVersion>("groups/" + groupName);
                    if (creator.Value.Creator != _user.Id)
                    {
                        throw new UnauthorizedAccessException($"Can't generate group {groupName} since such a group was already created by {creator.Value.Creator}");
                    }
                }
            }
        }
    }
}
