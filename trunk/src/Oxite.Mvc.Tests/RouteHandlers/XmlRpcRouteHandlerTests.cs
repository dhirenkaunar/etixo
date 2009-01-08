//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Web;
using System.Web.Routing;
using MvcFakes;
using Oxite.Handlers;
using Oxite.Mvc.Tests.Fakes;
using Xunit;

namespace Oxite.Mvc.Tests.RouteHandlers
{
    public class XmlRpcRouteHandlerTests
    {
        [Fact]
        public void HandlerReturnsXmlRpcHander()
        {
            XmlRpcRouteHandler<FakeXmlRpcService> handler = new XmlRpcRouteHandler<FakeXmlRpcService>();

            IHttpHandler httpHandler =
                handler.GetHttpHandler(new RequestContext(new FakeHttpContext("~/metaweblogapi"), new RouteData()));

            Assert.IsType<XmlRpcHandler<FakeXmlRpcService>>(httpHandler);
        }
    }
}