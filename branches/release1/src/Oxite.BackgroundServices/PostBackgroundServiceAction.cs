//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Xml.Linq;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.BackgroundServices
{
    public abstract class PostBackgroundServiceAction : BackgroundServiceAction
    {
        public PostBackgroundServiceAction()
        {
        }

        public PostBackgroundServiceAction(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            Routes = RouteTable.Routes;

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            AreaRepository = dataProvider.AreaRepository;
            BackgroundServiceActionRepository = dataProvider.BackgroundServiceActionRepository;
            PostRepository = dataProvider.PostRepository;
            Actions = new List<IBackgroundServiceAction>(10);

            RegisterRoutes();
        }

        public PostBackgroundServiceAction(IBackgroundServiceConfiguration backgroundServiceConfiguration,
                                           RouteCollection routes, IOxiteConfiguration config,
                                           IBackgroundServiceActionRepository backgroundServiceActionRepository,
                                           IAreaRepository areaRepository, IPostRepository postRepository)
            : base(routes, backgroundServiceConfiguration, config, areaRepository, backgroundServiceActionRepository)
        {
            PostRepository = postRepository;

            RegisterRoutes();
        }

        protected IPostRepository PostRepository { get; set; }

        public virtual int EntryActionLoadCount
        {
            get
            {
                return 1;
            }
        }

        protected virtual IPost GetPost(IBackgroundServiceAction action)
        {
            IPost post = null;

            if (action.Details != null)
            {
                XElement postIDElement = action.Details.Element("Details").Element("PostID");

                if (postIDElement != null)
                {
                    Guid postID = Guid.Empty;

                    try
                    {
                        postID = new Guid(postIDElement.Value);
                    }
                    catch
                    {
                    }

                    if (postID != Guid.Empty)
                    {
                        post = PostRepository.GetPost(postID);
                    }
                }
            }

            return post;
        }

        protected string GetEntryPath(IPost post)
        {
            return post.GetAreaPostUrl(CreateRequestContext(), Routes);
        }
    }
}