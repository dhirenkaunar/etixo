//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using CookComputing.XmlRpc;

namespace Oxite.Handlers
{
    public class XmlRpcHttpRequestWrapper : IHttpRequest
    {
        private HttpRequestBase request;

        public XmlRpcHttpRequestWrapper(HttpRequestBase request)
        {
            this.request = request;
        }

        #region IHttpRequest Members

        public string HttpMethod
        {
            get
            {
                return request.HttpMethod;
            }
        }

        public System.IO.Stream InputStream
        {
            get
            {
                return request.InputStream;
            }
        }

        #endregion
    }
}