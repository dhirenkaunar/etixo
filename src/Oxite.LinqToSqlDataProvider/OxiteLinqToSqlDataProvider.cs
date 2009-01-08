//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Data
{
    public class OxiteLinqToSqlDataProvider : IOxiteDataProvider
    {
        private IAreaRepository areaRepository;
        private IBackgroundServiceActionRepository backgroundServiceActionRepository;
        private string connectionString;

        private OxiteDataContext dataContext;
        private ILanguageRepository languageRepository;
        private IMembershipRepository membershipRepository;
        private IMessageRepository messageRepository;
        private IPostRepository postRepository;
        private IResourceRepository resourceRepository;
        private ITagRepository tagRepository;
        private ITrackbackRepository trackbackRepository;

        public OxiteLinqToSqlDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public OxiteDataContext DataContext
        {
            get
            {
                if (dataContext == null)
                {
                    dataContext = new OxiteDataContext(connectionString);
                }

                return dataContext;
            }
        }

        #region IOxiteDataProvider Members

        public virtual IAreaRepository AreaRepository
        {
            get
            {
                if (areaRepository == null)
                {
                    areaRepository = new OxiteAreaRepository((OxiteDataContext)DataContext);
                }

                return areaRepository;
            }
        }

        public virtual IBackgroundServiceActionRepository BackgroundServiceActionRepository
        {
            get
            {
                if (backgroundServiceActionRepository == null)
                {
                    backgroundServiceActionRepository =
                        new OxiteBackgroundServiceActionRepository((OxiteDataContext)DataContext);
                }

                return backgroundServiceActionRepository;
            }
        }

        public virtual ILanguageRepository LanguageRepository
        {
            get
            {
                if (languageRepository == null)
                {
                    languageRepository = new OxiteLanguageRepository((OxiteDataContext)DataContext);
                }

                return languageRepository;
            }
        }

        public virtual IMembershipRepository MembershipRepository
        {
            get
            {
                if (membershipRepository == null)
                {
                    membershipRepository = new OxiteMembershipRepository((OxiteDataContext)DataContext);
                }

                return membershipRepository;
            }
        }

        public virtual IMessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                {
                    messageRepository = new OxiteMessageRepository((OxiteDataContext)DataContext);
                }

                return messageRepository;
            }
        }

        public virtual IPostRepository PostRepository
        {
            get
            {
                if (postRepository == null)
                {
                    postRepository = new OxitePostRepository((OxiteDataContext)DataContext);
                }

                return postRepository;
            }
        }

        public virtual IResourceRepository ResourceRepository
        {
            get
            {
                if (resourceRepository == null)
                {
                    resourceRepository = new OxiteResourceRepository((OxiteDataContext)DataContext);
                }

                return resourceRepository;
            }
        }

        public virtual ITagRepository TagRepository
        {
            get
            {
                if (tagRepository == null)
                {
                    tagRepository = new OxiteTagRepository((OxiteDataContext)DataContext);
                }

                return tagRepository;
            }
        }

        public virtual ITrackbackRepository TrackbackRepository
        {
            get
            {
                if (trackbackRepository == null)
                {
                    trackbackRepository = new OxiteTrackbackRepository((OxiteDataContext)DataContext);
                }

                return trackbackRepository;
            }
        }

        #endregion
    }
}