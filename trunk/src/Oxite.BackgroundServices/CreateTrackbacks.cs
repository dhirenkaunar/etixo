//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web.Routing;
using System.Xml.Linq;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.BackgroundServices
{
    public class CreateTrackbacks : PostBackgroundServiceAction
    {
        private static Guid typeID = new Guid("F762DA8A-E37F-4592-818A-846BE53BC96A");

        public CreateTrackbacks(IBackgroundServiceConfiguration backgroundServiceConfiguration)
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

        public CreateTrackbacks(IBackgroundServiceConfiguration backgroundServiceConfiguration, RouteCollection routes,
                                IOxiteConfiguration config,
                                IBackgroundServiceActionRepository backgroundServiceActionRepository,
                                IAreaRepository areaRepository, IPostRepository postRepository)
            : base(
                backgroundServiceConfiguration, routes, config, backgroundServiceActionRepository, areaRepository,
                postRepository)
        {
        }

        public override Guid TypeID
        {
            get
            {
                return typeID;
            }
        }

        public override void Run()
        {
            LoadActions();

            using (TransactionScope transaction = new TransactionScope())
            {
                Dictionary<IPost, IBackgroundServiceAction> postBackgroundServicePostList =
                    BackgroundServiceActionRepository.GetRecentlyPublishedPostsAndBackgroundServiceActions(TypeID,
                                                                                                           CheckForNewInterval);

                if (postBackgroundServicePostList.Count > 0)
                {
                    foreach (
                        KeyValuePair<IPost, IBackgroundServiceAction> postBackgroundServicePost in
                            postBackgroundServicePostList)
                    {
                        if (postBackgroundServicePost.Value == null)
                        {
                            IBackgroundServiceAction action =
                                BackgroundServiceActionRepository.CreateBackgroundServiceAction();

                            action.TypeID = TypeID;
                            action.InProgress = false;
                            action.Details = new XElement(
                                new XElement(
                                    "Details",
                                    new XElement(
                                        "PostID",
                                        postBackgroundServicePost.Key.ID
                                        )
                                    )
                                );

                            BackgroundServiceActionRepository.AddBackgroundServiceAction(action);
                        }
                        else if (postBackgroundServicePost.Value.Started.HasValue &&
                                 postBackgroundServicePost.Value.Started.Value < postBackgroundServicePost.Key.Modified)
                        {
                            postBackgroundServicePost.Value.InProgress = false;
                            postBackgroundServicePost.Value.Started = null;
                            postBackgroundServicePost.Value.Completed = null;
                        }
                    }

                    BackgroundServiceActionRepository.SubmitChanges();

                    transaction.Complete();
                }
            }
        }
    }
}