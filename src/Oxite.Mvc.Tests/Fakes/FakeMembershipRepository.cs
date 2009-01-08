//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeMembershipRepository : IMembershipRepository
    {
        public FakeMembershipRepository()
        {
            Users = new List<FakeUser>();
            Roles = new List<FakeRole>();
            UserRoles = new List<KeyValuePair<Guid, Guid>>();
            AreaRoles = new List<KeyValuePair<Guid, Guid>>();
        }

        public FakeMembershipRepository(params FakeUser[] user)
            : this()
        {
            Users.AddRange(user);
        }

        public List<FakeUser> Users { get; set; }
        public List<FakeRole> Roles { get; set; }
        public List<KeyValuePair<Guid, Guid>> UserRoles { get; set; }
        public List<KeyValuePair<Guid, Guid>> AreaRoles { get; set; }

        #region IMembershipRepository Members

        public IUser GetAnonymousUser()
        {
            throw new NotImplementedException();
        }

        public IUser GetUser(Guid id)
        {
            return Users.Where(u => u.ID == id).FirstOrDefault();
        }

        public IUser GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public IUser GetUser(string username, string password)
        {
            return Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
        }

        public IRole GetRole(string name)
        {
            return Roles.Where(r => string.Compare(r.Name, name, true) == 0).FirstOrDefault();
        }

        public bool GetUserIsInRole(Guid userID, Guid roleID)
        {
            return UserRoles.Where(ur => ur.Key == userID && ur.Value == roleID).Any();
        }

        public bool GetUserIsInRole(Guid userID, IArea area)
        {
            if (AreaRoles.Count > 0)
            {
                return (from ar in AreaRoles
                        join ur in UserRoles on ar.Value equals ur.Value
                        where ur.Key == userID && ar.Key == area.ID
                        select ur).Any();
            }
            else
            {
                return true;
            }
        }

        public bool GetUserIsInRoleAny(IUser user, string[] roles)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> GetAreaUsers(IArea area)
        {
            return (from ar in AreaRoles
                    join ur in UserRoles on ar.Value equals ur.Value
                    where ar.Key == area.ID
                    select GetUser(ur.Key)).Cast<IUser>();
        }

        public void AddUserToRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(Guid userID, Guid roleID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}