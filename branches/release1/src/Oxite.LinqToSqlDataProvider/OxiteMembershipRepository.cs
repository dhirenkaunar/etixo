//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Oxite.Data
{
    public class OxiteMembershipRepository : IMembershipRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteMembershipRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region IMembershipRepository Members

        public IUser GetAnonymousUser()
        {
            return GetUser("Anonymous");
        }

        public IUser GetUser(Guid id)
        {
            return (from u in dataContext.oxite_Users
                    where u.UserID == id
                    select u).FirstOrDefault();
        }

        public IUser GetUser(string username)
        {
            return (from u in dataContext.oxite_Users
                    where string.Compare(u.Username, username, true) == 0
                    select u).FirstOrDefault();
        }

        public IUser GetUser(string username, string password)
        {
            string salt = (from u in dataContext.oxite_Users
                           where string.Compare(u.Username, username, true) == 0
                           select u.PasswordSalt).FirstOrDefault();

            string hashedPass = saltAndHash(password, salt);

            return (from u in dataContext.oxite_Users
                    where string.Compare(u.Username, username, true) == 0 && u.Password == hashedPass
                    select u).FirstOrDefault();
        }

        public bool GetUserIsInRole(Guid userID, Guid roleID)
        {
            return (from urr in dataContext.oxite_UserRoleRelationships
                    where urr.UserID == userID && urr.RoleID == roleID
                    select urr).Any();
        }

        public bool GetUserIsInRole(Guid userID, IArea area)
        {
            return (from arr in dataContext.oxite_AreaRoleRelationships
                    join urr in dataContext.oxite_UserRoleRelationships on arr.RoleID equals urr.RoleID
                    where arr.AreaID == area.ID && urr.UserID == userID
                    select urr).Any();
        }

        public bool GetUserIsInRoleAny(IUser user, string[] roles)
        {
            return (from urr in dataContext.oxite_UserRoleRelationships
                    join r in dataContext.oxite_Roles on urr.RoleID equals r.RoleID
                    where urr.UserID == user.ID && roles.Contains(r.RoleName)
                    select urr).Any();
        }

        public IEnumerable<IUser> GetAreaUsers(IArea area)
        {
            return (from arr in dataContext.oxite_AreaRoleRelationships
                    join urr in dataContext.oxite_UserRoleRelationships on arr.RoleID equals urr.RoleID
                    join u in dataContext.oxite_Users on urr.UserID equals u.UserID
                    where arr.AreaID == area.ID
                    select u).Cast<IUser>();
        }

        public IRole GetRole(string name)
        {
            return (from r in dataContext.oxite_Roles
                    where string.Compare(r.RoleName, name, true) == 0
                    select r).FirstOrDefault();
        }

        public void AddUserToRole(string username, string roleName)
        {
            IUser user = GetUser(username);
            if (user == null)
            {
                throw new ArgumentException(string.Format("Username '{0}' not found", username));
            }

            IRole role = GetRole(roleName);
            if (role == null)
            {
                throw new ArgumentException(string.Format("Role '{0}' not found", roleName));
            }

            AddUserToRole(user.ID, role.ID);
        }

        public void AddUserToRole(Guid userID, Guid roleID)
        {
            oxite_UserRoleRelationship userRoleRelationship = new oxite_UserRoleRelationship();

            userRoleRelationship.UserID = userID;
            userRoleRelationship.RoleID = roleID;

            dataContext.oxite_UserRoleRelationships.InsertOnSubmit(userRoleRelationship);
        }

        #endregion

        private static string saltAndHash(string rawString, string salt)
        {
            byte[] salted = Encoding.UTF8.GetBytes(string.Concat(rawString, salt));

            SHA256 hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(salted);

            return Convert.ToBase64String(hashed);
        }
    }
}