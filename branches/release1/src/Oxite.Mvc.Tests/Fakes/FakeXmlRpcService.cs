//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using CookComputing.XmlRpc;

namespace Oxite.Mvc.Tests.Fakes
{
    public interface IFakeXmlRpcService
    {
        [XmlRpcMethod("Fake.Test")]
        string Test();
    }

    public class FakeXmlRpcService : XmlRpcHttpServerProtocol, IFakeXmlRpcService
    {
        #region IFakeXmlRpcService Members

        public string Test()
        {
            return "Test";
        }

        #endregion
    }
}