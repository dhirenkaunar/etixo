//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Data
{
    public interface IPostRepository
    {
        event EventHandler<OxiteCancelDataItemEventArgs<IPost>> PostAdding;
        event EventHandler<OxiteDataItemEventArgs<IPost>> PostAdded;
        event EventHandler<OxiteCancelDataItemEventArgs<IPost>> PostEditing;
        event EventHandler<OxiteDataItemEventArgs<IPost>> PostEdited;
        event EventHandler<OxiteCancelDataItemEventArgs<IPost>> PostRemoving;
        event EventHandler<OxiteDataItemEventArgs<IPost>> PostRemoved;

        event EventHandler<OxiteCancelDataItemEventArgs<IComment>> CommentAdding;
        event EventHandler<OxiteDataItemEventArgs<IComment>> CommentAdded;
        event EventHandler<OxiteCancelDataItemEventArgs<IComment>> CommentEditing;
        event EventHandler<OxiteDataItemEventArgs<IComment>> CommentEdited;
        event EventHandler<OxiteCancelDataItemEventArgs<IComment>> CommentRemoving;
        event EventHandler<OxiteDataItemEventArgs<IComment>> CommentRemoved;

        IPost GetPost(Guid id);
        IPost GetPost(IArea area, string slug);
        IPost GetPost(Guid parentPostID, string slug);
        IPost GetParentPost(Guid postID);
        IQueryable<IPost> GetChildPosts(Guid postID);
        IPageOfAList<IPost> GetPosts(Guid siteID, int pageIndex, int pageSize);
        IPageOfAList<IPost> GetPosts(Guid siteID, PublishedFilter publishedFilter, int pageIndex, int pageSize);
        IPageOfAList<IPost> GetPosts(Guid siteID, int year, int month, int day, int pageIndex, int pageSize);
        IPageOfAList<IPost> GetPosts(IArea area, int year, int month, int day, int pageIndex, int pageSize);
        IEnumerable<IPost> GetPosts(Guid siteID, DateTime startDate, DateTime endDate);
        IPageOfAList<IPost> GetPosts(IArea area, int pageIndex, int pageSize);
        IPageOfAList<IPost> GetPosts(IArea area, PublishedFilter publishedFilter, int pageIndex, int pageSize);
        IPageOfAList<IPost> GetPosts(Guid siteID, string term, int pageIndex, int pageSize);
        IPageOfAList<IPost> GetPosts(Guid siteID, ITag tag, int pageIndex, int pageSize);
        Dictionary<Guid, string> GetPostsWithFullSlug(Guid siteID);
        IComment GetComment(Guid commentID);
        IEnumerable<IComment> GetComments(IPost post);
        IPageOfAList<IComment> GetComments(Guid siteID, int pageIndex, int pageSize);
        IPageOfAList<IComment> GetComments(IArea area, int pageIndex, int pageSize);
        IPageOfAList<IComment> GetComments(ITag tag, int pageIndex, int pageSize);
        IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(Guid siteID);
        IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(IArea area);
        IEnumerable<DateTime> GetYearMonthsOfPosts(Guid siteID);
        Dictionary<IComment, Guid> GetCommentsWithPostSubscriptionsWithoutMessages(TimeSpan checkForNewInterval);
        IEnumerable<ISubscription> GetSubscriptions(IComment comment);

        Type GetCommentAnonymousType();

        IPost CreatePost();
        IComment CreateComment();
        ICommentAnonymous CreateCommentAnonymous();
        ISubscription CreateSubscription();
        ISubscriptionAnonymous CreateSubscriptionAnonymous();

        void AddPost(IPost post, ISubscription subscription);

        void AddComment(IComment comment, ICommentAnonymous commentAnonymous, ISubscription subscription,
                        ISubscriptionAnonymous subscriptionAnonymous);

        void AddPostToTag(Guid postID, Guid tagID);
        void RemovePostFromTag(Guid postID, Guid tagID);
        void AddPostRelationship(Guid siteID, Guid postID, Guid parentID);
        void RemovePostRelationship(Guid postID);
        void RemovePost(Guid postID);
        void SubmitChanges();
    }
}