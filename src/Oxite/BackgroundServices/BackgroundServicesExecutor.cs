//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oxite.Configuration;

namespace Oxite.BackgroundServices
{
    public static class BackgroundServicesExecutor
    {
        public static IEnumerable<BackgroundServiceExecutor> Start(IOxiteConfiguration config)
        {
            IEnumerable<IBackgroundService> backgroundServices = load(config);
            List<BackgroundServiceExecutor> backgroundServiceExecutors = null;

            if (backgroundServices.Count() > 0)
            {
                backgroundServiceExecutors = new List<BackgroundServiceExecutor>(backgroundServices.Count());

                foreach (IBackgroundService backgroundService in backgroundServices)
                {
                    if (backgroundService.Enabled)
                    {
                        BackgroundServiceExecutor backgroundServiceExecutor =
                            new BackgroundServiceExecutor(backgroundService);

                        backgroundServiceExecutor.Start();

                        backgroundServiceExecutors.Add(backgroundServiceExecutor);
                    }
                }
            }

            return backgroundServiceExecutors;
        }

        public static void End(IEnumerable<BackgroundServiceExecutor> backgroundServiceExecutors)
        {
            if (backgroundServiceExecutors != null && backgroundServiceExecutors.Count() > 0)
            {
                foreach (BackgroundServiceExecutor backgroundServiceExecutor in backgroundServiceExecutors)
                {
                    backgroundServiceExecutor.Stop();
                }
            }
        }

        private static IEnumerable<IBackgroundService> load(IOxiteConfiguration config)
        {
            List<IBackgroundService> backgroundServices = new List<IBackgroundService>(5);

            if (config != null && config.BackgroundServices != null && config.BackgroundServices.Enabled)
            {
                foreach (BackgroundServiceConfigurationElement element in config.BackgroundServices)
                {
                    if (!string.IsNullOrEmpty(element.Type))
                    {
                        string[] typeParts = element.Type.Split(',');
                        string typeName = Assembly.CreateQualifiedName(typeParts[1].Trim(), typeParts[0].Trim());
                        Type type = Type.GetType(typeName);
                        IBackgroundService backgroundService =
                            (IBackgroundService)Activator.CreateInstance(type, new object[] {element});

                        backgroundServices.Add(backgroundService);
                    }
                }
            }

            return backgroundServices;
        }
    }
}