//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Configuration;

namespace Oxite.Configuration
{
    public class SiteConfigurationElement : ConfigurationElement, ISiteConfiguration
    {
        [ConfigurationProperty("aliases")]
        public SiteAliasConfigurationElementCollection AliasesInternal
        {
            get
            {
                return (SiteAliasConfigurationElementCollection)this["aliases"];
            }
            set
            {
                this["aliases"] = value;
            }
        }

        #region ISiteConfiguration Members

        [ConfigurationProperty("id", IsRequired = true)]
        public Guid ID
        {
            get
            {
                return (Guid)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("languageDefault", IsRequired = true)]
        public string LanguageDefault
        {
            get
            {
                return (string)this["languageDefault"];
            }
            set
            {
                this["languageDefault"] = value;
            }
        }

        [ConfigurationProperty("pageTitleSeparator", IsRequired = false, DefaultValue = " - ")]
        public string PageTitleSeparator
        {
            get
            {
                return (string)this["pageTitleSeparator"];
            }
            set
            {
                this["pageTitleSeparator"] = value;
            }
        }

        [ConfigurationProperty("timeZoneOffset", IsRequired = false, DefaultValue = 0D)]
        public double TimeZoneOffset
        {
            get
            {
                return (double)this["timeZoneOffset"];
            }
            set
            {
                this["timeZoneOffset"] = value;
            }
        }

        [ConfigurationProperty("themeDefault", IsRequired = false, DefaultValue = "Default")]
        public string ThemeDefault
        {
            get
            {
                return (string)this["themeDefault"];
            }
            set
            {
                this["themeDefault"] = value;
            }
        }

        [ConfigurationProperty("favIconUrl", IsRequired = false)]
        public string FavIconUrl
        {
            get
            {
                return (string)this["favIconUrl"];
            }
            set
            {
                this["favIconUrl"] = value;
            }
        }

        [ConfigurationProperty("scriptsPath", IsRequired = false, DefaultValue = "/Skins/{0}/Scripts")]
        public string ScriptsPath
        {
            get
            {
                return (string)this["scriptsPath"];
            }
            set
            {
                this["scriptsPath"] = value;
            }
        }

        [ConfigurationProperty("cssFilePath", IsRequired = false, DefaultValue = "/Skins/{0}/Content")]
        public string CssPath
        {
            get
            {
                return (string)this["cssFilePath"];
            }
            set
            {
                this["cssFilePath"] = value;
            }
        }

        [ConfigurationProperty("commentAnonymousStateDefault", IsRequired = false, DefaultValue = "Normal")]
        public string CommentAnonymousStateDefault
        {
            get
            {
                return (string)this["commentAnonymousStateDefault"];
            }
            set
            {
                this["commentAnonymousStateDefault"] = value;
            }
        }

        [ConfigurationProperty("emailUsername", IsRequired = false, DefaultValue = "Admin")]
        public string EmailUsername
        {
            get
            {
                return (string)this["emailUsername"];
            }
            set
            {
                this["emailUsername"] = value;
            }
        }

        [ConfigurationProperty("includeOpenSearch", IsRequired = false, DefaultValue = true)]
        public bool IncludeOpenSearch
        {
            get
            {
                return (bool)this["includeOpenSearch"];
            }
            set
            {
                this["includeOpenSearch"] = value;
            }
        }

        [ConfigurationProperty("authorAutoSubscribe", IsRequired = false, DefaultValue = true)]
        public bool AuthorAutoSubscribe
        {
            get
            {
                return (bool)this["authorAutoSubscribe"];
            }
            set
            {
                this["authorAutoSubscribe"] = value;
            }
        }

        [ConfigurationProperty("postEditTimeout", IsRequired = false, DefaultValue = 24D)]
        public double PostEditTimeout
        {
            get
            {
                return (double)this["postEditTimeout"];
            }
            set
            {
                this["postEditTimeout"] = value;
            }
        }

        [ConfigurationProperty("seoRobots", IsRequired = false, DefaultValue = "index,follow")]
        public string SEORobots
        {
            get
            {
                return (string)this["seoRobots"];
            }
            set
            {
                this["seoRobots"] = value;
            }
        }

        [ConfigurationProperty("gravatarDefault", IsRequired = true)]
        public string GravatarDefault
        {
            get
            {
                return (string)this["gravatarDefault"];
            }
            set
            {
                this["gravatarDefault"] = value;
            }
        }

        [ConfigurationProperty("trackbacksEnabled", IsRequired = false, DefaultValue = true)]
        public bool TrackbacksEnabled
        {
            get
            {
                return (bool)this["trackbacksEnabled"];
            }
            set
            {
                this["trackbacksEnabled"] = value;
            }
        }

        public ISiteAliasConfigurationCollection Aliases
        {
            get
            {
                return AliasesInternal;
            }
        }

        #endregion
    }
}