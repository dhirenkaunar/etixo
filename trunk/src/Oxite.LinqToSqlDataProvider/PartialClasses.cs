//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Oxite.Data
{

    #region BackgroundServiceAction

    partial class oxite_BackgroundServiceAction : IBackgroundServiceAction
    {
        #region IBackgroundServiceAction Members

        public Guid ID
        {
            get
            {
                return BackgroundServiceActionID;
            }
            set
            {
                BackgroundServiceActionID = value;
            }
        }

        #endregion
    }

    #endregion

    #region Area

    partial class oxite_Area : IArea
    {
        #region IArea Members

        public Guid ID
        {
            get
            {
                return AreaID;
            }
            set
            {
                AreaID = value;
            }
        }

        public string Name
        {
            get
            {
                return AreaName;
            }
            set
            {
                AreaName = value;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        public DateTime? Modified
        {
            get
            {
                return ModifiedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : ModifiedDate;
            }
            set
            {
                if (value == null)
                {
                    ModifiedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    ModifiedDate = value.Value;
                }
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
            Modified = SqlDateTime.MaxValue.Value;
        }
    }

    #endregion

    #region Comment

    partial class oxite_Comment : IComment, IFeedItem
    {
        #region IComment Members

        public IPost Post
        {
            get
            {
                return oxite_Post;
            }
        }

        public Guid ID
        {
            get
            {
                return CommentID;
            }
            set
            {
                CommentID = value;
            }
        }

        public string Title
        {
            get
            {
                return Post.Title;
            }
        }

        public IUser CreatorUser
        {
            get
            {
                return oxite_User;
            }
        }

        public ILanguage Language
        {
            get
            {
                return oxite_Language;
            }
        }

        public string CreatorName
        {
            get
            {
                string creatorName = "";

                if (CreatorUser.IsAnonymous)
                {
                    creatorName = oxite_CommentAnonymous.Name;
                }
                else
                {
                    creatorName = CreatorUser.DisplayName;
                }

                return creatorName;
            }
        }

        public string CreatorEmail
        {
            get
            {
                return CreatorUser.IsAnonymous
                           ? oxite_CommentAnonymous.Email
                           : CreatorUser.Email;
            }
        }

        public string CreatorHashedEmail
        {
            get
            {
                return CreatorUser.IsAnonymous
                           ? oxite_CommentAnonymous.HashedEmail
                           : CreatorUser.HashedEmail;
            }
        }

        public string CreatorUrl
        {
            get
            {
                return CreatorUser.IsAnonymous
                           ? oxite_CommentAnonymous.Url
                           : ""; //<- todo?(nheskew) url for real users?
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        public DateTime? Modified
        {
            get
            {
                return ModifiedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : ModifiedDate;
            }
            set
            {
                if (value == null)
                {
                    ModifiedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    ModifiedDate = value.Value;
                }
            }
        }

        public DateTime? Published
        {
            get
            {
                return PublishedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : PublishedDate;
            }
            set
            {
                if (value == null)
                {
                    PublishedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    PublishedDate = value.Value;
                }
            }
        }

        public IEnumerable<ITag> Tags
        {
            get
            {
                return Enumerable.Empty<ITag>();
            }
        }

        public IArea Area
        {
            get
            {
                return oxite_Post.Area;
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
            Modified = SqlDateTime.MaxValue.Value;
            PublishedDate = SqlDateTime.MaxValue.Value;
        }
    }

    #endregion

    #region CommentAnonymous

    partial class oxite_CommentAnonymous : ICommentAnonymous
    {
        #region ICommentAnonymous Members

        public IComment Comment
        {
            get
            {
                return oxite_Comment;
            }
        }

        #endregion
    }

    #endregion

    #region FileResource

    partial class oxite_FileResource : IFileResource
    {
        private byte[] content;

        #region IFileResource Members

        public Guid ID
        {
            get
            {
                return FileResourceID;
            }
            set
            {
                FileResourceID = value;
            }
        }

        public string Name
        {
            get
            {
                return FileResourceName;
            }
            set
            {
                FileResourceName = value;
            }
        }

        public IUser CreatorUser
        {
            get
            {
                return oxite_User;
            }
            set
            {
                oxite_User = (oxite_User)value;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        public DateTime? Modified
        {
            get
            {
                return ModifiedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : ModifiedDate;
            }
            set
            {
                if (value == null)
                {
                    ModifiedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    ModifiedDate = value.Value;
                }
            }
        }

        public byte[] Content
        {
            get
            {
                if (content == null && Data.Length > 0)
                {
                    content = Data.ToArray();
                }

                return content;
            }
            set
            {
                Data = content = value;
            }
        }

        #endregion

        internal void PrepareForInsert()
        {
            Guid creatorUserID = CreatorUserID;
            _oxite_User = default(System.Data.Linq.EntityRef<oxite_User>);
            CreatorUserID = creatorUserID;
        }

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
            Modified = SqlDateTime.MaxValue.Value;
        }
    }

    #endregion

    #region Language

    partial class oxite_Language : ILanguage
    {
        #region ILanguage Members

        public Guid ID
        {
            get
            {
                return LanguageID;
            }
            set
            {
                LanguageID = value;
            }
        }

        public string Name
        {
            get
            {
                return LanguageName;
            }
            set
            {
                LanguageName = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return LanguageDisplayName;
            }
            set
            {
                LanguageDisplayName = value;
            }
        }

        #endregion
    }

    #endregion

    #region Message

    partial class oxite_Message : IMessage
    {
        #region IMessage Members

        public Guid ID
        {
            get
            {
                return MessageID;
            }
            set
            {
                MessageID = value;
            }
        }

        public IUser From
        {
            get
            {
                return oxite_User;
            }
            set
            {
                oxite_User = (oxite_User)value;
            }
        }

        public IEnumerable<IMessageTo> MessageTos
        {
            get
            {
                return this.oxite_MessageTos.Cast<IMessageTo>();
            }
        }

        #endregion
    }

    partial class oxite_MessageTo : IMessageTo
    {
        #region IMessageTo Members

        public IMessage Message
        {
            get
            {
                return oxite_Message;
            }
            set
            {
                oxite_Message = (oxite_Message)value;
            }
        }

        public Guid ID
        {
            get
            {
                return MessageToID;
            }
            set
            {
                MessageToID = value;
            }
        }

        public IUser User
        {
            get
            {
                return oxite_User;
            }
            set
            {
                oxite_User = (oxite_User)value;
            }
        }

        public IMessageToAnonymous MessageToAnonymous
        {
            get
            {
                return oxite_MessageToAnonymous;
            }
            set
            {
                oxite_MessageToAnonymous = (oxite_MessageToAnonymous)value;
            }
        }

        #endregion
    }

    partial class oxite_MessageToAnonymous : IMessageToAnonymous
    {
        #region IMessageToAnonymous Members

        public IMessageTo MessageTo
        {
            get
            {
                return oxite_MessageTo;
            }
            set
            {
                oxite_MessageTo = (oxite_MessageTo)value;
            }
        }

        #endregion
    }

    #endregion

    #region Post

    partial class oxite_Post : IPost, IFeedItem
    {
        #region IFeedItem Members

        public string CreatorName
        {
            get
            {
                return oxite_User.Username;
            }
        }

        #endregion

        #region IPost Members

        public Guid ID
        {
            get
            {
                return PostID;
            }
            set
            {
                PostID = value;
            }
        }

        public IUser CreatorUser
        {
            get
            {
                return oxite_User;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        public DateTime? Modified
        {
            get
            {
                return ModifiedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : ModifiedDate;
            }
            set
            {
                if (value == null)
                {
                    ModifiedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    ModifiedDate = value.Value;
                }
            }
        }

        public DateTime? Published
        {
            get
            {
                return PublishedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : PublishedDate;
            }
            set
            {
                if (value == null)
                {
                    PublishedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    PublishedDate = value.Value;
                }
            }
        }

        public IPost Parent
        {
            get
            {
                return oxite_PostRelationships.Count > 0 ? oxite_PostRelationships.First().oxite_Post1 : null;
            }
        }

        public IArea Area
        {
            get
            {
                return oxite_PostAreaRelationships.Count > 0 ? oxite_PostAreaRelationships.First().oxite_Area : null;
            }
        }

        public IEnumerable<ITag> Tags
        {
            get
            {
                return oxite_PostTagRelationships.Select(ptr => ptr.oxite_Tag).Cast<ITag>();
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
            Modified = SqlDateTime.MaxValue.Value;
            PublishedDate = SqlDateTime.MaxValue.Value;
        }

        partial void OnLoaded()
        {
            this.oxite_PostTagRelationships.ListChanged +=
                new ListChangedEventHandler(oxite_PostTagRelationships_ListChanged);
            this.PropertyChanged += new PropertyChangedEventHandler(oxite_Post_PropertyChanged);
        }

        private void oxite_Post_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "oxite_User")
            {
                updateSearchBody();
            }
        }

        private void oxite_PostTagRelationships_ListChanged(object sender, ListChangedEventArgs e)
        {
            updateSearchBody();
        }

        partial void OnTitleChanged()
        {
            updateSearchBody();
        }

        partial void OnBodyChanged()
        {
            updateSearchBody();
        }

        private void updateSearchBody()
        {
            StringBuilder newSearchBody = new StringBuilder();

            newSearchBody.Append(Title);
            newSearchBody.Append(" ");

            if (oxite_User != null)
            {
                newSearchBody.Append(oxite_User.DisplayName);
                newSearchBody.Append(" ");
            }

            if (oxite_PostTagRelationships.Count > 0)
            {
                newSearchBody.Append(string.Join(", ",
                                                 oxite_PostTagRelationships.Select(t => t.oxite_Tag.Name).ToArray()));
                newSearchBody.Append(" ");
            }

            newSearchBody.Append(Body);

            SearchBody = newSearchBody.ToString();
        }
    }

    #endregion

    #region Role

    partial class oxite_Role : IRole
    {
        #region IRole Members

        public IRole Parent
        {
            get
            {
                return oxite_Role1;
            }
            set
            {
                oxite_Role1 = (oxite_Role)value;
            }
        }

        public Guid ID
        {
            get
            {
                return RoleID;
            }
            set
            {
                RoleID = value;
            }
        }

        public string Name
        {
            get
            {
                return RoleName;
            }
            set
            {
                RoleName = value;
            }
        }

        #endregion
    }

    #endregion

    #region StringResource

    partial class oxite_StringResource : IStringResource
    {
        #region IStringResource Members

        public string Key
        {
            get
            {
                return StringResourceKey;
            }
            set
            {
                StringResourceKey = value;
            }
        }

        public string Value
        {
            get
            {
                return StringResourceValue;
            }
            set
            {
                StringResourceValue = value;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
        }
    }

    partial class oxite_StringResourceVersion : IStringResourceVersion
    {
        #region IStringResourceVersion Members

        public string Key
        {
            get
            {
                return StringResourceKey;
            }
            set
            {
                StringResourceKey = value;
            }
        }

        public string Value
        {
            get
            {
                return StringResourceValue;
            }
            set
            {
                StringResourceValue = value;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
        }
    }

    #endregion

    #region Subscription

    partial class oxite_Subscription : ISubscription
    {
        #region ISubscription Members

        public Guid ID
        {
            get
            {
                return SubscriptionID;
            }
            set
            {
                SubscriptionID = value;
            }
        }

        public IPost Post
        {
            get
            {
                return oxite_Post;
            }
        }

        public IUser User
        {
            get
            {
                return oxite_User;
            }
        }

        public ISubscriptionAnonymous SubscriptionAnonymous
        {
            get
            {
                return oxite_SubscriptionAnonymous;
            }
        }

        #endregion
    }

    #endregion

    #region SubscriptionAnonymous

    partial class oxite_SubscriptionAnonymous : ISubscriptionAnonymous
    {
        #region ISubscriptionAnonymous Members

        public ISubscription Subscription
        {
            get
            {
                return oxite_Subscription;
            }
        }

        #endregion
    }

    #endregion

    #region Tag

    partial class oxite_Tag : ITag
    {
        #region ITag Members

        public ITag Parent
        {
            get
            {
                return oxite_Tag1;
            }
            set
            {
                oxite_Tag1 = (oxite_Tag)value;
            }
        }

        public Guid ID
        {
            get
            {
                return TagID;
            }
            set
            {
                TagID = value;
            }
        }

        public string Name
        {
            get
            {
                return TagName;
            }
            set
            {
                TagName = value;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
        }
    }

    #endregion

    #region Trackback

    partial class oxite_Trackback : ITrackback
    {
        #region ITrackback Members

        public IPost Post
        {
            get
            {
                return oxite_Post;
            }
            set
            {
                oxite_Post = (oxite_Post)value;
            }
        }

        public Guid ID
        {
            get
            {
                return TrackbackID;
            }
            set
            {
                TrackbackID = value;
            }
        }

        public DateTime? Created
        {
            get
            {
                return CreatedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : CreatedDate;
            }
            set
            {
                if (value == null)
                {
                    CreatedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    CreatedDate = value.Value;
                }
            }
        }

        public DateTime? Modified
        {
            get
            {
                return ModifiedDate == SqlDateTime.MaxValue.Value ? (DateTime?)null : ModifiedDate;
            }
            set
            {
                if (value == null)
                {
                    ModifiedDate = SqlDateTime.MaxValue.Value;
                }
                else
                {
                    ModifiedDate = value.Value;
                }
            }
        }

        #endregion

        partial void OnCreated()
        {
            CreatedDate = SqlDateTime.MaxValue.Value;
            Modified = SqlDateTime.MaxValue.Value;
        }
    }

    #endregion

    #region User

    partial class oxite_User : IUser
    {
        #region IUser Members

        public Guid ID
        {
            get
            {
                return UserID;
            }
            set
            {
                UserID = value;
            }
        }

        public bool IsAnonymous
        {
            get
            {
                return Username == "Anonymous";
            }
        }

        public ILanguage LanguageDefault
        {
            get
            {
                return oxite_Language;
            }
            set
            {
                oxite_Language = (oxite_Language)value;
            }
        }

        public IEnumerable<ILanguage> Languages
        {
            get
            {
                return oxite_UserLanguages.Cast<ILanguage>();
            }
        }

        #endregion
    }

    #endregion
}