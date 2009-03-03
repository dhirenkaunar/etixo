//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public class OxiteCancelDataItemEventArgs<T> : EventArgs
    {
        private T item;

        public OxiteCancelDataItemEventArgs(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get
            {
                return item;
            }
        }

        public bool Cancel { get; set; }
    }
}