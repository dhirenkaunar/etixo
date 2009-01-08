//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IMessage
    {
        Guid ID { get; set; }
        IUser From { get; set; }
        IEnumerable<IMessageTo> MessageTos { get; }
        string Subject { get; set; }
        string Body { get; set; }
        bool IsSent { get; set; }
        DateTime? SentDate { get; set; }
    }
}