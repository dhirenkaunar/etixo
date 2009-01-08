//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Configuration;
using System.Web;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Routing;

namespace Oxite.BackgroundServices
{
    public abstract class BackgroundService : IBackgroundService
    {
        private static OxiteRoutes oxiteRoutes;
        private string sitePath;

        public BackgroundService()
        {
        }

        public BackgroundService(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            Routes = RouteTable.Routes;
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AreaRepository = Config.DataProvider.GetInstance().AreaRepository;
        }

        public BackgroundService(RouteCollection routes, IBackgroundServiceConfiguration backgroundServiceConfiguration,
                                 IOxiteConfiguration config, IAreaRepository areaRepository)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            Routes = routes;
            Config = config;
            AreaRepository = areaRepository;
        }

        protected RouteCollection Routes { get; set; }
        protected IOxiteConfiguration Config { get; set; }
        protected IAreaRepository AreaRepository { get; set; }

        protected virtual string SitePath
        {
            get
            {
                if (sitePath == null)
                {
                    sitePath = Config.Site.Host;
                }

                return sitePath;
            }
        }

        #region IBackgroundService Members

        public string Name { get; set; }
        public bool Enabled { get; set; }
        public TimeSpan Interval { get; set; }
        public TimeSpan Timeout { get; set; }

        public abstract void Run();

        #endregion

        protected virtual void LoadFromConfig(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            Name = backgroundServiceConfiguration.Name;
            Enabled = backgroundServiceConfiguration.Enabled;
            Interval = new TimeSpan(0, 0, backgroundServiceConfiguration.Interval);
            Timeout = new TimeSpan(0, 0, backgroundServiceConfiguration.Timeout);
        }

        protected virtual void RegisterRoutes()
        {
            if (oxiteRoutes == null)
            {
                oxiteRoutes = new OxiteRoutes(Routes, Config, AreaRepository, null);

                oxiteRoutes.RegisterRoutes();
            }
        }

        protected virtual RequestContext CreateRequestContext()
        {
            Uri uri = new Uri(SitePath);

            return new RequestContext(new HttpContextInternal(uri.AbsolutePath), new RouteData());
        }

        protected virtual string GetAbsolutePath(string path)
        {
            Uri uri = new Uri(SitePath);
            UriBuilder builder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);

            builder.Path = path;

            return builder.Uri.ToString().Replace("%23", "#");
        }

        //HACK: (erikpo) This class is to get around Route requiring an instance of HttpContextBase

        #region Nested type: HttpContextInternal

        private sealed class HttpContextInternal : HttpContextBase
        {
            private string applicationPath;
            private HttpRequestInternal request;

            public HttpContextInternal(string applicationPath)
            {
                this.applicationPath = applicationPath;
            }

            public override HttpRequestBase Request
            {
                get
                {
                    if (request == null)
                    {
                        request = new HttpRequestInternal(applicationPath);
                    }

                    return request;
                }
            }

            #region Nested type: HttpRequestInternal

            private sealed class HttpRequestInternal : HttpRequestBase
            {
                private string applicationPath;

                public HttpRequestInternal(string applicationPath)
                {
                    this.applicationPath = applicationPath;
                }

                public override string ApplicationPath
                {
                    get
                    {
                        return applicationPath;
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}