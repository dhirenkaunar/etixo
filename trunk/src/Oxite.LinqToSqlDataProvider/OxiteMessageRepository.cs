//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Oxite.Data
{
    public class OxiteMessageRepository : IMessageRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteMessageRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region IMessageRepository Members

        public IEnumerable<IMessage> GetUnsentMessages(int take)
        {
            return (from m in dataContext.oxite_Messages
                    join mt in dataContext.oxite_MessageTos on m.MessageID equals mt.MessageID
                    join mta in dataContext.GetTable<oxite_MessageToAnonymous>() on mt.MessageToID equals
                        mta.MessageToID
                    where m.IsSent == false
                    select m).Take(take).Cast<IMessage>();
        }

        public IMessage CreateMessage()
        {
            return new oxite_Message();
        }

        public IMessageTo CreateMessageTo()
        {
            return new oxite_MessageTo();
        }

        public IMessageToAnonymous CreateMessageToAnonymous()
        {
            return new oxite_MessageToAnonymous();
        }

        public void AddMessage(IMessage message, IEnumerable<IMessageTo> messageTos)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            if (message.From == null || message.From.ID == Guid.Empty)
            {
                throw new ArgumentNullException("message.From");
            }
            if (messageTos == null)
            {
                throw new ArgumentNullException("messageTos");
            }
            if (messageTos.Count() == 0)
            {
                throw new ArgumentException("One or more messageTos must be provided", "messageTos");
            }

            if (message.ID == Guid.Empty)
            {
                message.ID = Guid.NewGuid();
            }
            message.IsSent = false;
            message.SentDate = null;

            dataContext.oxite_Messages.InsertOnSubmit((oxite_Message)message);

            foreach (IMessageTo messageTo in messageTos)
            {
                if (messageTo.ID == Guid.Empty)
                {
                    messageTo.ID = Guid.NewGuid();
                }
                messageTo.Message = message;

                dataContext.oxite_MessageTos.InsertOnSubmit((oxite_MessageTo)messageTo);

                if (messageTo.MessageToAnonymous != null)
                {
                    messageTo.MessageToAnonymous.MessageTo = messageTo;

                    dataContext.GetTable<oxite_MessageToAnonymous>().InsertOnSubmit(
                        (oxite_MessageToAnonymous)messageTo.MessageToAnonymous);
                }
            }
        }

        public void SubmitChanges()
        {
            dataContext.SubmitChanges(ConflictMode.FailOnFirstConflict);
        }

        #endregion
    }
}