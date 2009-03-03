//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IMessageRepository
    {
        IEnumerable<IMessage> GetUnsentMessages(int take);
        IMessage CreateMessage();
        IMessageTo CreateMessageTo();
        IMessageToAnonymous CreateMessageToAnonymous();
        void AddMessage(IMessage message, IEnumerable<IMessageTo> toAddresses);
        void SubmitChanges();
    }
}