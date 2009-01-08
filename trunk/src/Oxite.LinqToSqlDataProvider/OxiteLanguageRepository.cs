//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Data
{
    public class OxiteLanguageRepository : ILanguageRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteLanguageRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region ILanguageRepository Members

        public ILanguage GetLanguage(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            return (from l in dataContext.oxite_Languages
                    where l.LanguageID == id
                    select l).FirstOrDefault();
        }

        public ILanguage GetLanguage(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return (from l in dataContext.oxite_Languages
                    where string.Compare(l.LanguageName, name, true) == 0
                    select l).FirstOrDefault();
        }

        public IEnumerable<ILanguage> GetLanguages()
        {
            return (from l in dataContext.oxite_Languages
                    orderby l.LanguageDisplayName
                    select l).Cast<ILanguage>();
        }

        public IEnumerable<ILanguage> GetLanguages(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (user.ID == Guid.Empty)
            {
                throw new ArgumentNullException("user.ID");
            }

            if (user.IsAnonymous)
            {
                return GetLanguages();
            }

            return (from ul in dataContext.oxite_UserLanguages
                    join l in dataContext.oxite_Languages on ul.LanguageID equals l.LanguageID
                    where ul.UserID == user.ID
                    orderby l.LanguageDisplayName
                    select l).Cast<ILanguage>();
        }

        #endregion
    }
}