﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpSessionState(SessionStateItemCollection sessionItems)
        {
            _sessionItems = sessionItems;
        }

        public override void Add(string name, object value)
        {
            _sessionItems[name] = value;
        }

        public override int Count
        {
            get
            {
                return _sessionItems.Count;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return _sessionItems.GetEnumerator();
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return _sessionItems.Keys;
            }
        }

        public override object this[string name]
        {
            get
            {
                return _sessionItems[name];
            }
            set
            {
                _sessionItems[name] = value;
            }
        }

        public override object this[int index]
        {
            get
            {
                return _sessionItems[index];
            }
            set
            {
                _sessionItems[index] = value;
            }
        }

        public override void Remove(string name)
        {
            _sessionItems.Remove(name);
        }
    }
}
