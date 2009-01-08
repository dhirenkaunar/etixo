//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Mvc
{
    public class HeadLink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public object HtmlAttributes { get; set; }
    }
}