//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Configuration;
using System.Text;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite
{
    public class PageTitleHelper
    {
        public PageTitleHelper() : this((IOxiteConfiguration)ConfigurationManager.GetSection("oxite"))
        {
        }

        public PageTitleHelper(IOxiteConfiguration config)
            : this(config != null ? config.Site.Name : "", config != null ? config.Site.PageTitleSeparator : "")
        {
        }

        public PageTitleHelper(string siteName, string pageTitleSeparator)
        {
            SiteName = siteName;
            PageTitleSeparator = pageTitleSeparator;
        }

        protected string SiteName { get; private set; }
        protected string PageTitleSeparator { get; private set; }
        public IPost Post { get; set; }
        public IArea Area { get; set; }
        public string[] AdditionalPageTitleSegments { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(25);

            sb.Append(SiteName);

            if (Area != null)
            {
                sb.Insert(0, PageTitleSeparator);
                sb.Insert(0, !string.IsNullOrEmpty(Area.DisplayName) ? Area.DisplayName : Area.Name);
            }

            if (Post != null)
            {
                sb.Insert(0, PageTitleSeparator);
                sb.Insert(0, Post.Title);
            }

            if (AdditionalPageTitleSegments != null)
            {
                for (int i = 0; i < AdditionalPageTitleSegments.Length; i++)
                {
                    sb.Insert(0, PageTitleSeparator);
                    sb.Insert(0, AdditionalPageTitleSegments[i]);
                }
            }

            return sb.ToString();
        }
    }
}