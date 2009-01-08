// --------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (Ms-PL)
// http://www.codeplex.com/oxite/license
// ---------------------------------
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Search
{
    public class OxiteSearchProvider : ISearchProvider
    {
        public OxiteSearchProvider()
        {
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            PostRepository = Config.DataProvider.GetInstance().PostRepository;
        }

        public OxiteSearchProvider(IOxiteConfiguration config, IPostRepository postRepository)
        {
            Config = config;
            PostRepository = postRepository;
        }

        protected IOxiteConfiguration Config { get; private set; }
        protected IPostRepository PostRepository { get; private set; }

        public PageOfAList<ISearchResultItem> GetSearchResults(string term, int page, int count, NameValueCollection parameters)
        {
            IPageOfAList<IPost> posts = PostRepository.GetPosts(Config.Site.ID, term, page, count);
            List<ISearchResultItem> results = new List<ISearchResultItem>(posts.Count);

            foreach (IPost post in posts)
                results.Add(new OxiteSearchResultItem(post));

            return new PageOfAList<ISearchResultItem>(results, page, count, posts.TotalItemCount);
        }
    }
}
