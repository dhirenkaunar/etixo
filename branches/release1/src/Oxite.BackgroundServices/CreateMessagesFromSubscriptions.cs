//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.BackgroundServices
{
    public class CreateMessagesFromSubscriptions : BackgroundService
    {
        public CreateMessagesFromSubscriptions(IBackgroundServiceConfiguration backgroundServiceConfiguration)
            : base(backgroundServiceConfiguration)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            Routes = RouteTable.Routes;
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            AreaRepository = dataProvider.AreaRepository;
            MembershipRepository = dataProvider.MembershipRepository;
            MessageRepository = dataProvider.MessageRepository;
            PostRepository = dataProvider.PostRepository;
            ResourceRepository = dataProvider.ResourceRepository;

            RegisterRoutes();
        }

        public CreateMessagesFromSubscriptions(RouteCollection routes,
                                               IBackgroundServiceConfiguration backgroundServiceConfiguration,
                                               IOxiteConfiguration config, IAreaRepository areaRepository,
                                               IMembershipRepository membershipRepository,
                                               IMessageRepository messageRepository, IPostRepository postRepository,
                                               IResourceRepository resourceRepository)
            : base(routes, backgroundServiceConfiguration, config, areaRepository)
        {
            MembershipRepository = membershipRepository;
            MessageRepository = messageRepository;
            PostRepository = postRepository;
            ResourceRepository = resourceRepository;

            RegisterRoutes();
        }

        protected IMembershipRepository MembershipRepository { get; set; }
        protected IMessageRepository MessageRepository { get; set; }
        protected IPostRepository PostRepository { get; set; }
        protected IResourceRepository ResourceRepository { get; set; }

        public override void Run()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                Dictionary<IComment, Guid> items =
                    PostRepository.GetCommentsWithPostSubscriptionsWithoutMessages(TimeSpan.FromDays(3));

                if (items.Count > 0)
                {
                    foreach (KeyValuePair<IComment, Guid> item in items)
                    {
                        if (item.Value == Guid.Empty)
                        {
                            IMessage message = generateMessage(item.Key);

                            MessageRepository.AddMessage(message, generateMessageToList(item.Key, message));
                        }
                    }

                    MessageRepository.SubmitChanges();

                    transaction.Complete();
                }
            }
        }

        private IMessage generateMessage(IComment comment)
        {
            IMessage message = MessageRepository.CreateMessage();
            IUser fromUser = MembershipRepository.GetUser(Config.Site.EmailUsername);

            if (fromUser == null)
            {
                throw new Exception(string.Format("User '{0}' could not be found and is required to generate messages",
                                                  Config.Site.EmailUsername));
            }

            message.From = fromUser;
            message.Subject = "RE: " + comment.Title; //TODO: (erikpo) Localize "RE: "
            message.Body = generateBody(comment);

            return message;
        }

        private IEnumerable<IMessageTo> generateMessageToList(IComment comment, IMessage message)
        {
            IEnumerable<ISubscription> subscriptions = PostRepository.GetSubscriptions(comment);
            List<IMessageTo> messageToList = new List<IMessageTo>(subscriptions.Count());

            foreach (ISubscription subscription in subscriptions)
            {
                IMessageTo messageTo = MessageRepository.CreateMessageTo();

                messageTo.User = subscription.User;

                messageToList.Add(messageTo);

                if (subscription.User.IsAnonymous && subscription.SubscriptionAnonymous != null)
                {
                    IMessageToAnonymous messageToAnonymous = MessageRepository.CreateMessageToAnonymous();

                    messageToAnonymous.Name = subscription.SubscriptionAnonymous.Name;
                    messageToAnonymous.Email = subscription.SubscriptionAnonymous.Email;

                    messageTo.MessageToAnonymous = messageToAnonymous;
                }
            }

            return messageToList;
        }

        private string generateBody(IComment comment)
        {
            const string stringResourceNameFormat = "Oxite{0}.Messages.NewComment";
            string stringResourceName = string.Format(stringResourceNameFormat, string.Format(".{0:N}", Config.Site.ID));
            string language = comment.Language.Name;
            IStringResource stringResource = ResourceRepository.GetString(stringResourceName, language);

            if (stringResource == null)
            {
                stringResource = ResourceRepository.GetString(string.Format(stringResourceNameFormat, ""), language);
            }

            if (stringResource == null)
            {
                throw new Exception(string.Format("String resource '{0}' could not be found", stringResourceName));
            }

            string body = stringResource.Value;
            double timeZoneOffset = Config.Site.TimeZoneOffset;
                //TODO: (erikpo) Change this to come from the user this message is going to if applicable

            body = body.Replace("{Site.Name}", Config.Site.Name);
            body = body.Replace("{User.Name}", comment.CreatorName);
            body = body.Replace("{Post.Title}", comment.Post.Title);
            body = body.Replace("{Post.Published}",
                                convertToLocalTime(comment.Published.Value, timeZoneOffset).ToString(
                                    "MMMM, d yyyy h:mm tt"));
                //TODO: (erikpo) Change the published date to be relative (e.g. 5 minutes ago)
            body = body.Replace("{Comment.Body}", comment.Body);
            body = body.Replace("{Comment.Permalink}", GetAbsolutePath(comment.GetUrl(CreateRequestContext(), Routes)));

            return body;
        }

        private static DateTime convertToLocalTime(DateTime dateTime, double timeZoneOffset)
        {
            return timeZoneOffset != 0 ? dateTime.AddHours(timeZoneOffset) : dateTime;
        }
    }
}