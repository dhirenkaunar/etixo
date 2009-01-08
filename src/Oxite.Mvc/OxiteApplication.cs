//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.BackgroundServices;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Routing;

namespace Oxite.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class OxiteApplication : HttpApplication
    {
        private IEnumerable<BackgroundServiceExecutor> backgroundServiceExecutors;
        private IRoutingRegistration routes;

        public OxiteApplication()
            : base()
        {
            Routes = RouteTable.Routes;
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AreaRepository = Config.DataProvider.GetInstance().AreaRepository;

            this.BeginRequest += new EventHandler(OxiteApplication_BeginRequest);
        }

        public OxiteApplication(RouteCollection routes, IOxiteConfiguration config, IAreaRepository areaRepository)
            : base()
        {
            Routes = routes;
            Config = config;
            AreaRepository = areaRepository;

            this.BeginRequest += new EventHandler(OxiteApplication_BeginRequest);
        }

        protected RouteCollection Routes { get; private set; }
        protected IOxiteConfiguration Config { get; private set; }
        protected IAreaRepository AreaRepository { get; private set; }

        protected void Application_Start()
        {
            OnStart();
        }

        protected void Application_End()
        {
            OnEnd();
        }

        public void RegisterRoutes()
        {
            if (routes == null)
            {
                if (Config.Routes != null && !string.IsNullOrEmpty(Config.Routes.Type))
                {
                    string[] typeParts = Config.Routes.Type.Split(',');
                    string typeName = Assembly.CreateQualifiedName(typeParts[1].Trim(), typeParts[0].Trim());
                    Type type = Type.GetType(typeName);

                    routes =
                        (IRoutingRegistration)
                        Activator.CreateInstance(type,
                                                 new object[] {Routes, Config, AreaRepository, typeof (MvcRouteHandler)});
                }
                else
                {
                    routes = new OxiteRoutes(Routes, Config, AreaRepository, typeof (MvcRouteHandler));
                }
            }

            routes.RegisterRoutes();
        }

        protected virtual void OnStart()
        {
            RegisterRoutes();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new OxiteViewEngine(Config.Site.ThemeDefault));

            backgroundServiceExecutors = BackgroundServicesExecutor.Start(Config);
        }

        protected virtual void OnEnd()
        {
            BackgroundServicesExecutor.End(backgroundServiceExecutors);
        }

        private void OxiteApplication_BeginRequest(object sender, EventArgs e)
        {
            string host = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host,
                                        Request.Url.Port != 80 ? ":" + Request.Url.Port : "");

            if (string.Compare(Config.Site.Host, host, true) != 0)
            {
                ISiteAliasConfiguration foundAlias = null;

                foreach (ISiteAliasConfiguration alias in Config.Site.Aliases)
                {
                    if (string.Compare(alias.Host, host, true) == 0)
                    {
                        foundAlias = alias;
                        break;
                    }
                }

                if (foundAlias == null)
                {
                    throw new Exception(string.Format("No site or alias was found for '{0}'", host));
                }
                
                if (foundAlias.Redirect)
                {
                    UriBuilder builder = new UriBuilder(Request.Url);
                    UriBuilder builder2 = new UriBuilder(Config.Site.Host);

                    builder.Scheme = builder2.Scheme;
                    builder.Host = builder2.Host;
                    builder.Port = builder2.Port;

                    Response.RedirectLocation = builder.Uri.ToString();
                    Response.StatusCode = 301;
                    Response.End();
                }
            }
        }
    }
}