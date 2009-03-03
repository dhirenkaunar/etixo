//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Transactions;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.BackgroundServices
{
    public class SendMessages : BackgroundService
    {
        private int messageSendCount;

        public SendMessages(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            Routes = RouteTable.Routes;

            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            AreaRepository = dataProvider.AreaRepository;
            MessageRepository = dataProvider.MessageRepository;
        }

        public SendMessages(RouteCollection routes, IBackgroundServiceConfiguration backgroundServiceConfiguration,
                            IOxiteConfiguration config, IAreaRepository areaRepository,
                            IMessageRepository messageRepository)
            : base(routes, backgroundServiceConfiguration, config, areaRepository)
        {
            MessageRepository = messageRepository;
        }

        protected IMessageRepository MessageRepository { get; set; }

        protected override void LoadFromConfig(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            string messageSendCountString = backgroundServiceConfiguration.ConfigurationProperties["MessageSendCount"];

            messageSendCount = 5;

            if (!string.IsNullOrEmpty(messageSendCountString))
            {
                int messageSendCountValue;

                if (int.TryParse(messageSendCountString, out messageSendCountValue))
                {
                    messageSendCount = messageSendCountValue;
                }
            }
        }

        public override void Run()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                IEnumerable<IMessage> unsentMessages = MessageRepository.GetUnsentMessages(messageSendCount);
                SmtpClient mailClient = new SmtpClient();

                foreach (IMessage message in unsentMessages)
                {
                    MailMessage mailMessage = new MailMessage();

                    mailMessage.From = new MailAddress(message.From.Email, message.From.DisplayName);
                    mailMessage.Subject = message.Subject;
                    mailMessage.Body = message.Body;

                    foreach (IMessageTo messageTo in message.MessageTos)
                    {
                        mailMessage.To.Add(new MailAddress(messageTo.MessageToAnonymous.Email,
                                                           messageTo.MessageToAnonymous.Name));
                    }

                    mailClient.Send(mailMessage);

                    message.IsSent = true;
                    message.SentDate = DateTime.Now.ToUniversalTime();
                }

                transaction.Complete();
            }
        }
    }
}