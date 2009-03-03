//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc
{
    public class OxiteAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] _rolesSplit;
        private string[] _usersSplit;

        public OxiteAuthorizeAttribute()
            : base()
        {
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            MembershipRepository = dataProvider.MembershipRepository;
        }

        public OxiteAuthorizeAttribute(IOxiteConfiguration config, IMembershipRepository membershipRepository)
            : base()
        {
            Config = config;
            MembershipRepository = membershipRepository;
        }

        private string[] usersSplit
        {
            get
            {
                if (_usersSplit == null)
                {
                    _usersSplit = SplitString(Users);
                }

                return _usersSplit;
            }
        }

        private string[] rolesSplit
        {
            get
            {
                if (_rolesSplit == null)
                {
                    _rolesSplit = SplitString(Roles);
                }

                return _rolesSplit;
            }
        }

        protected IOxiteConfiguration Config { get; private set; }
        protected IMembershipRepository MembershipRepository { get; private set; }

        protected override bool AuthorizeCore(IPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            if (usersSplit.Length > 0 && !usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if (rolesSplit.Length > 0 &&
                !MembershipRepository.GetUserIsInRoleAny(MembershipRepository.GetUser(user.Identity.Name), rolesSplit))
            {
                return false;
            }

            return true;
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !string.IsNullOrEmpty(trimmed)
                        select trimmed;

            return split.ToArray();
        }
    }
}