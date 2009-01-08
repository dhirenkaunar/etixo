//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakePostRepository : IPostRepository
    {
        public FakePostRepository()
        {
            Posts = new List<FakePost>();
            AddedPosts = new List<FakePost>();
            AddedPostTags = new List<KeyValuePair<Guid, Guid>>();
            Saved = false;
        }

        public FakePostRepository(params FakePost[] posts)
            : this()
        {
            Posts.AddRange(posts);
        }

        public List<FakePost> Posts { get; set; }
        public List<FakePost> AddedPosts { get; set; }
        public List<KeyValuePair<Guid, Guid>> AddedPostTags { get; set; }
        public bool Saved { get; set; }

        #region IPostRepository Members

        public event EventHandler<OxiteCancelDataItemEventArgs<IPost>> PostAdding;

        public event EventHandler<OxiteDataItemEventArgs<IPost>> PostAdded;

        public event EventHandler<OxiteCancelDataItemEventArgs<IPost>> PostEditing;

        public event EventHandler<OxiteDataItemEventArgs<IPost>> PostEdited;

        public event EventHandler<OxiteCancelDataItemEventArgs<IPost>> PostRemoving;

        public event EventHandler<OxiteDataItemEventArgs<IPost>> PostRemoved;

        public event EventHandler<OxiteCancelDataItemEventArgs<IComment>> CommentAdding;

        public event EventHandler<OxiteDataItemEventArgs<IComment>> CommentAdded;

        public event EventHandler<OxiteCancelDataItemEventArgs<IComment>> CommentEditing;

        public event EventHandler<OxiteDataItemEventArgs<IComment>> CommentEdited;

        public event EventHandler<OxiteCancelDataItemEventArgs<IComment>> CommentRemoving;

        public event EventHandler<OxiteDataItemEventArgs<IComment>> CommentRemoved;

        public IPost GetPost(Guid id)
        {
            return Posts.Where(p => p.ID == id).FirstOrDefault();
        }

        public IPost GetPost(IArea area, string slug)
        {
            throw new NotImplementedException();
        }

        public IPost GetPost(Guid parentPostID, string slug)
        {
            throw new NotImplementedException();
        }

        public IPost GetParentPost(Guid postID)
        {
            throw new NotImplementedException();
        }

        public System.Linq.IQueryable<IPost> GetChildPosts(Guid postID)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IPost> GetPosts(Guid siteID, int pageIndex, int pageSize)
        {
            return new PageOfAList<IPost>(Posts.Skip((pageIndex - 1) * pageSize).Take(pageSize).OfType<IPost>(), pageIndex, pageSize, Posts.Count);
        }

        public IPageOfAList<IPost> GetPosts(Guid siteID, PublishedFilter publishedFilter, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IPost> GetPosts(Guid siteID, int year, int month, int day, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IPost> GetPosts(IArea area, int year, int month, int day, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPost> GetPosts(Guid siteID, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IPost> GetPosts(IArea area, int pageIndex, int pageSize)
        {
            return Posts.Where(p => p.Area != null && p.Area.ID == area.ID).GetPage(pageIndex, pageSize);
        }

        public IPageOfAList<IPost> GetPosts(IArea area, PublishedFilter publishedFilter, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IPost> GetPosts(Guid siteID, string term, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IPost> GetPosts(Guid siteID, ITag tag, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Guid, string> GetPostsWithFullSlug(Guid siteID)
        {
            throw new NotImplementedException();
        }

        public IComment GetComment(Guid commentID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IComment> GetComments(IPost post)
        {
            return new[] { new FakeComment(), new FakeComment() };
        }

        public IPageOfAList<IComment> GetComments(Guid siteID, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IComment> GetComments(IArea area, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IPageOfAList<IComment> GetComments(ITag tag, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(Guid siteID)
        {
            return Enumerable.Empty<KeyValuePair<DateTime, int>>();
        }

        public IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(IArea area)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DateTime> GetYearMonthsOfPosts(Guid siteID)
        {
            throw new NotImplementedException();
        }

        public Dictionary<IComment, Guid> GetCommentsWithPostSubscriptionsWithoutMessages(TimeSpan checkForNewInterval)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISubscription> GetSubscriptions(IComment comment)
        {
            throw new NotImplementedException();
        }

        public Type GetCommentAnonymousType()
        {
            throw new NotImplementedException();
        }

        public IPost CreatePost()
        {
            return new FakePost();
        }

        public IComment CreateComment()
        {
            throw new NotImplementedException();
        }

        public ICommentAnonymous CreateCommentAnonymous()
        {
            throw new NotImplementedException();
        }

        public ISubscription CreateSubscription()
        {
            throw new NotImplementedException();
        }

        public ISubscriptionAnonymous CreateSubscriptionAnonymous()
        {
            throw new NotImplementedException();
        }

        public void AddPost(IPost post, ISubscription subscription)
        {
            AddedPosts.Add((FakePost)post);
        }

        public void AddComment(IComment comment, ICommentAnonymous commentAnonymous, ISubscription subscription,
                               ISubscriptionAnonymous subscriptionAnonymous)
        {
            throw new NotImplementedException();
        }

        public void AddPostToTag(Guid postID, Guid tagID)
        {
            AddedPostTags.Add(new KeyValuePair<Guid, Guid>(postID, tagID));
        }

        public void RemovePostFromTag(Guid postID, Guid tagID)
        {
            KeyValuePair<Guid, Guid>? foundPostTag =
                AddedPostTags.Where(pt => pt.Key == postID && pt.Value == tagID).FirstOrDefault();

            if (foundPostTag.HasValue)
            {
                AddedPostTags.Remove(foundPostTag.Value);
            }
        }

        public void AddPostRelationship(Guid siteID, Guid postID, Guid parentID)
        {
            throw new NotImplementedException();
        }

        public void RemovePostRelationship(Guid postID)
        {
            throw new NotImplementedException();
        }

        public void RemovePost(Guid postID)
        {
            GetPost(postID).State = (byte)EntityState.Removed;
        }

        public void SubmitChanges()
        {
            Saved = true;
        }

        #endregion
    }
}