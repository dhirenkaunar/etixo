//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using System.Web.Routing;
using CookComputing.XmlRpc;

namespace Oxite.Handlers
{
    public class XmlRpcRouteHandler<T> : IRouteHandler where T : XmlRpcHttpServerProtocol, new()
    {
        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new XmlRpcHandler<T>();
        }

        #endregion
    }
}