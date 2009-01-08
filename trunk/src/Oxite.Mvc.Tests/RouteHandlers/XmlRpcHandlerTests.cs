//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.IO;
using System.Xml.Linq;
using MvcFakes;
using Oxite.Handlers;
using Oxite.Mvc.Tests.Fakes;
using Xunit;

namespace Oxite.Mvc.Tests.RouteHandlers
{
    public class XmlRpcHandlerTests
    {
        [Fact]
        public void HandlerCallsCorrectMethod()
        {
            MemoryStream request = new MemoryStream();
            StreamWriter writer = new StreamWriter(request);

            writer.Write(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><methodCall><methodName>Fake.Test</methodName></methodCall>");
            writer.Flush();

            request.Seek(0, SeekOrigin.Begin);

            FakeHttpContextWrapper context = new FakeHttpContextWrapper(new FakeHttpContext("~/metaweblogapi", "POST"),
                                                                        request);

            XmlRpcHandler<FakeXmlRpcService> handler = new XmlRpcHandler<FakeXmlRpcService>();

            handler.ProcessRequest(context);

            context.Response.Output.Flush();
            context.Response.OutputStream.Seek(0, SeekOrigin.Begin);

            string output = new StreamReader(context.Response.OutputStream).ReadToEnd();

            Assert.NotNull(output);
            XDocument outputDoc = XDocument.Parse(output);
            Assert.Equal("Test",
                            outputDoc.Element("methodResponse").Element("params").Element("param").Element("value").
                                Element("string").Value);
        }
    }
}