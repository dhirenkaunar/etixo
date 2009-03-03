//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using CookComputing.XmlRpc;

namespace Oxite.Handlers
{
    public class XmlRpcHandler<T> : IHttpHandler where T : XmlRpcHttpServerProtocol, new()
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        #endregion

        public void ProcessRequest(HttpContextBase context)
        {
            T service = new T();

            service.HandleHttpRequest(new XmlRpcHttpRequestWrapper(context.Request),
                                      new XmlRpcHttpResponseWrapper(context.Response));
        }
    }
}