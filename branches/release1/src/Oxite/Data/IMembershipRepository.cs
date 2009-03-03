//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IMembershipRepository
    {
        IUser GetAnonymousUser();
        IUser GetUser(Guid id);
        IUser GetUser(string username);
        IUser GetUser(string username, string password);
        IRole GetRole(string name);
        bool GetUserIsInRole(Guid userID, Guid role);
        bool GetUserIsInRole(Guid userID, IArea area);
        bool GetUserIsInRoleAny(IUser user, string[] roles);
        IEnumerable<IUser> GetAreaUsers(IArea area);
        void AddUserToRole(string username, string roleName);
        void AddUserToRole(Guid userID, Guid roleID);
    }
}