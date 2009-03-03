//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Configuration
{
    public interface ISiteConfiguration
    {
        ISiteAliasConfigurationCollection Aliases { get; }
        Guid ID { get; set; }
        string Name { get; set; }
        string Host { get; set; }
        string LanguageDefault { get; set; }
        double TimeZoneOffset { get; set; }
        string PageTitleSeparator { get; set; }
        string ThemeDefault { get; set; }
        string FavIconUrl { get; set; }
        string ScriptsPath { get; set; }
        string CssPath { get; set; }
        string CommentAnonymousStateDefault { get; set; }
        string EmailUsername { get; set; }
        bool IncludeOpenSearch { get; set; }
        bool AuthorAutoSubscribe { get; set; }
        double PostEditTimeout { get; set; }
        string SEORobots { get; set; }
        string GravatarDefault { get; set; }
        bool TrackbacksEnabled { get; set; }
    }
}