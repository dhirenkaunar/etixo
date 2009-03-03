//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.IO;
using System.Web;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpResponseWrapper : HttpResponseBase
    {
        private StreamWriter output;
        private MemoryStream outputStream;
        private HttpResponseBase wrapped;

        public FakeHttpResponseWrapper(HttpResponseBase wrapped)
        {
            this.wrapped = wrapped;
            this.outputStream = new MemoryStream();
            this.output = new StreamWriter(outputStream);
        }

        public override string ContentType { get; set; }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return wrapped.Cookies;
            }
        }

        public override System.IO.TextWriter Output
        {
            get
            {
                return output;
            }
        }

        public override System.IO.Stream OutputStream
        {
            get
            {
                return outputStream;
            }
        }

        public override string Status
        {
            get
            {
                return wrapped.Status;
            }
            set
            {
                wrapped.Status = value;
            }
        }

        public override string StatusDescription
        {
            get
            {
                return wrapped.StatusDescription;
            }
            set
            {
                wrapped.StatusDescription = value;
            }
        }

        public override int StatusCode { get; set; }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        public override void Clear()
        {
            wrapped.Clear();
        }

        public override string ToString()
        {
            return wrapped.ToString();
        }

        public override void Write(string s)
        {
            wrapped.Write(s);
        }
    }
}