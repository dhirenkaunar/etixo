//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.IO;
using System.Web;
using MvcFakes;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpContextWrapper : HttpContextBase
    {
        private Stream inputStream;
        private FakeHttpRequestWrapper request;
        private FakeHttpResponseWrapper response;
        private FakeHttpContext wrapped;

        public FakeHttpContextWrapper(FakeHttpContext wrapped)
        {
            this.wrapped = wrapped;
        }

        public FakeHttpContextWrapper(FakeHttpContext wrapped, Stream inputStream)
            : this(wrapped)
        {
            this.inputStream = inputStream;
        }

        public override HttpRequestBase Request
        {
            get
            {
                if (request == null)
                {
                    request = new FakeHttpRequestWrapper(wrapped.Request, "/", inputStream);
                }

                return request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                if (response == null)
                {
                    response = new FakeHttpResponseWrapper(wrapped.Response);
                }

                return response;
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return wrapped.Session;
            }
        }

        public override System.Security.Principal.IPrincipal User
        {
            get
            {
                return wrapped.User;
            }
            set
            {
                wrapped.User = value;
            }
        }
    }
}