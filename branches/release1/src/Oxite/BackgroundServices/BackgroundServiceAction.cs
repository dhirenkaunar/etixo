//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.BackgroundServices
{
    public abstract class BackgroundServiceAction : BackgroundService
    {
        public BackgroundServiceAction()
        {
        }

        public BackgroundServiceAction(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            Routes = RouteTable.Routes;

            LoadFromConfig(backgroundServiceConfiguration);

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            AreaRepository = dataProvider.AreaRepository;
            BackgroundServiceActionRepository = dataProvider.BackgroundServiceActionRepository;
            Actions = new List<IBackgroundServiceAction>(10);
        }

        public BackgroundServiceAction(RouteCollection routes,
                                       IBackgroundServiceConfiguration backgroundServiceConfiguration,
                                       IOxiteConfiguration config, IAreaRepository areaRepository,
                                       IBackgroundServiceActionRepository backgroundServiceActionRepository)
            : base(routes, backgroundServiceConfiguration, config, areaRepository)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            BackgroundServiceActionRepository = backgroundServiceActionRepository;
            Actions = new List<IBackgroundServiceAction>(10);
        }

        protected IBackgroundServiceActionRepository BackgroundServiceActionRepository { get; set; }
        protected List<IBackgroundServiceAction> Actions { get; set; }

        public TimeSpan CheckForNewInterval { get; set; }
        public abstract Guid TypeID { get; }

        public virtual int ActionLoadCount
        {
            get
            {
                return 1;
            }
        }

        public override void Run()
        {
            CleanupActions();

            LoadActions();
        }

        protected override void LoadFromConfig(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            base.LoadFromConfig(backgroundServiceConfiguration);

            int checkForNewInterval = 3; // Minutes

            string checkForNewIntervalString =
                backgroundServiceConfiguration.ConfigurationProperties["CheckForNewInterval"];

            if (!string.IsNullOrEmpty(checkForNewIntervalString))
            {
                int checkForNewIntervalValue;

                if (int.TryParse(checkForNewIntervalString, out checkForNewIntervalValue))
                {
                    checkForNewInterval = checkForNewIntervalValue;
                }
            }

            CheckForNewInterval = new TimeSpan(checkForNewInterval, 0, 0, 0);
        }

        protected virtual void CleanupActions()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                IEnumerable<IBackgroundServiceAction> actions =
                    BackgroundServiceActionRepository.GetActionsInProgress(TypeID);

                foreach (IBackgroundServiceAction action in actions)
                {
                    if (action.Started.Value.Add(Timeout) < DateTime.Now.ToUniversalTime())
                    {
                        action.InProgress = false;
                        action.Started = null;
                    }
                }

                BackgroundServiceActionRepository.SubmitChanges();

                transaction.Complete();
            }
        }

        protected virtual void LoadActions()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                IEnumerable<IBackgroundServiceAction> actions =
                    BackgroundServiceActionRepository.GetActionsNotInProgress(TypeID, ActionLoadCount);

                foreach (IBackgroundServiceAction action in actions)
                {
                    action.Started = DateTime.Now.ToUniversalTime();
                    action.InProgress = true;
                }

                if (Actions.Count == 0)
                {
                    Actions = actions.ToList();
                }
                else
                {
                    Actions.AddRange(actions);
                }

                BackgroundServiceActionRepository.SubmitChanges();

                transaction.Complete();
            }
        }

        protected virtual void ExecuteActions(Action<IBackgroundServiceAction> executeAction)
        {
            lock (Actions)
            {
                if (Actions.Count > 0)
                {
                    foreach (IBackgroundServiceAction action in Actions)
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            try
                            {
                                executeAction(action);

                                action.Completed = DateTime.Now.ToUniversalTime();
                            }
                            catch
                            {
                            }

                            action.InProgress = false;

                            BackgroundServiceActionRepository.SubmitChanges();

                            Actions.Remove(action);

                            transaction.Complete();
                        }
                    }
                }
            }
        }
    }
}