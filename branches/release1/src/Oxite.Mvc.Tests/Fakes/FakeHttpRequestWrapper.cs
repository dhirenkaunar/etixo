//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpRequestWrapper : HttpRequestBase
    {
        private string appPath;
        private NameValueCollection headers;
        private Stream inputStream;
        private HttpRequestBase wrapped;

        public FakeHttpRequestWrapper(HttpRequestBase wrapped, string appPath, Stream inputStream)
        {
            this.wrapped = wrapped;
            this.appPath = appPath;
            this.inputStream = inputStream;
        }

        public override string ApplicationPath
        {
            get
            {
                return appPath;
            }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return wrapped.AppRelativeCurrentExecutionFilePath;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return wrapped.Cookies;
            }
        }

        public override NameValueCollection Form
        {
            get
            {
                return wrapped.Form;
            }
        }

        public override string HttpMethod
        {
            get
            {
                return wrapped.HttpMethod;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return inputStream;
            }
        }

        public override bool IsAuthenticated
        {
            get
            {
                return wrapped.IsAuthenticated;
            }
        }

        public override bool IsLocal
        {
            get
            {
                return wrapped.IsLocal;
            }
        }

        public override string PathInfo
        {
            get
            {
                return wrapped.PathInfo;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return wrapped.QueryString;
            }
        }

        public override Uri Url
        {
            get
            {
                return wrapped.Url;
            }
        }

        public override NameValueCollection Headers
        {
            get
            {
                if (headers == null)
                {
                    headers = new NameValueCollection();
                }

                return headers;
            }
        }
    }
}