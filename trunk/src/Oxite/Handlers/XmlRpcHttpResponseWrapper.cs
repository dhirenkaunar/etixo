//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using CookComputing.XmlRpc;

namespace Oxite.Handlers
{
    public class XmlRpcHttpResponseWrapper : IHttpResponse
    {
        private HttpResponseBase response;

        public XmlRpcHttpResponseWrapper(HttpResponseBase response)
        {
            this.response = response;
        }

        #region IHttpResponse Members

        public string ContentType
        {
            get
            {
                return response.ContentType;
            }
            set
            {
                response.ContentType = value;
            }
        }

        public System.IO.TextWriter Output
        {
            get
            {
                return response.Output;
            }
        }

        public System.IO.Stream OutputStream
        {
            get
            {
                return response.OutputStream;
            }
        }

        public int StatusCode
        {
            get
            {
                return response.StatusCode;
            }
            set
            {
                response.StatusCode = value;
            }
        }

        public string StatusDescription
        {
            get
            {
                return response.StatusDescription;
            }
            set
            {
                response.StatusDescription = value;
            }
        }

        #endregion
    }
}