//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Xml.Linq;

namespace Oxite.Data
{
    public interface IBackgroundServiceAction
    {
        Guid ID { get; set; }
        Guid TypeID { get; set; }
        DateTime? Started { get; set; }
        bool InProgress { get; set; }
        DateTime? Completed { get; set; }
        XElement Details { get; set; }
    }
}