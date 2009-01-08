//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;

namespace Oxite.Data
{
    internal static class CommentExtensions
    {
        internal static IQueryable<oxite_Comment> Visible(this IQueryable<oxite_Comment> comments)
        {
            return
                comments.Where(
                    c => c.State == (byte)EntityState.Normal && c.PublishedDate <= DateTime.Now.ToUniversalTime());
        }
    }
}