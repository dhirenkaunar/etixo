//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Oxite.Data
{
    public class OxitePostRepository : IPostRepository
    {
        private OxiteDataContext dataContext;

        public OxitePostRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region Query

        public virtual IPost GetPost(Guid id)
        {
            return (from p in dataContext.oxite_Posts
                    where p.PostID == id
                    select p).FirstOrDefault();
        }

        public virtual IPost GetPost(Guid parentPostID, string slug)
        {
            if (parentPostID == Guid.Empty)
            {
                return (from pr in dataContext.oxite_PostRelationships
                        join p in dataContext.oxite_Posts on pr.PostID equals p.PostID
                        where pr.ParentPostID == pr.PostID && string.Compare(p.Slug, slug, true) == 0
                        select p).FirstOrDefault();
            }

            return (from pr in dataContext.oxite_PostRelationships
                    join p in dataContext.oxite_Posts on pr.PostID equals p.PostID
                    where pr.ParentPostID == parentPostID && string.Compare(p.Slug, slug, true) == 0
                    select p).FirstOrDefault();
        }

        public virtual IPost GetPost(IArea area, string slug)
        {
            if (area == null || area.ID == Guid.Empty || string.IsNullOrEmpty(slug))
            {
                return null;
            }

            return (from bpr in dataContext.oxite_PostAreaRelationships
                    join p in dataContext.oxite_Posts on bpr.PostID equals p.PostID
                    where bpr.AreaID == area.ID && string.Compare(p.Slug, slug, true) == 0
                    select p).FirstOrDefault();
        }

        public virtual IPost GetParentPost(Guid postID)
        {
            return (from pr in dataContext.oxite_PostRelationships
                    join p in dataContext.oxite_Posts on pr.PostID equals p.PostID
                    where pr.PostID == postID
                    select p).FirstOrDefault();
        }

        public virtual IQueryable<IPost> GetChildPosts(Guid postID)
        {
            return (from pr in dataContext.oxite_PostRelationships
                    join p in dataContext.oxite_Posts on pr.PostID equals p.PostID
                    where pr.ParentPostID == postID && pr.PostID != postID
                    select p).Cast<IPost>();
        }

        public virtual IPageOfAList<IPost> GetPosts(Guid siteID, int pageIndex, int pageSize)
        {
            return getPosts(siteID, PublishedFilter.Published,
                            query => query.Where(p => p.State == (byte)EntityState.Normal), pageIndex, pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(Guid siteID, PublishedFilter publishedFilter, int pageIndex,
                                                    int pageSize)
        {
            return getPosts(siteID, publishedFilter, q => q.Where(p => p.State != (byte)EntityState.Removed), pageIndex,
                            pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(IArea area, int pageIndex, int pageSize)
        {
            return getPosts(area, PublishedFilter.Published,
                            query => query.Where(p => p.State == (byte)EntityState.Normal), pageIndex, pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(IArea area, PublishedFilter publishedFilter, int pageIndex,
                                                    int pageSize)
        {
            return getPosts(area, publishedFilter, q => q.Where(p => p.State != (byte)EntityState.Removed), pageIndex,
                            pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(IArea area, int year, int month, int day, int pageIndex,
                                                    int pageSize)
        {
            return getPosts(area).Visible().InYear(year).InMonth(month).InDay(day).GetPage(pageIndex, pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(Guid siteID, int year, int month, int day, int pageIndex,
                                                    int pageSize)
        {
            return getPosts(siteID).Visible().InYear(year).InMonth(month).InDay(day).GetPage(pageIndex, pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(Guid siteID, string term, int pageIndex, int pageSize)
        {
            return getPosts(siteID, PublishedFilter.Published, query => query.Where(p => p.SearchBody.Contains(term)),
                            pageIndex, pageSize);
        }

        public virtual IPageOfAList<IPost> GetPosts(Guid siteID, ITag tag, int pageIndex, int pageSize)
        {
            var childTags = from t in dataContext.oxite_Tags
                            where t.TagID == tag.ID || t.ParentTagID == tag.ID
                            select t;

            return (from ptr in dataContext.oxite_PostTagRelationships
                    join p in dataContext.oxite_Posts on ptr.PostID equals p.PostID
                    join pbr in dataContext.oxite_PostAreaRelationships on p.PostID equals pbr.PostID
                    join a in dataContext.oxite_Areas on pbr.AreaID equals a.AreaID
                    where a.SiteID == siteID && childTags.Contains(ptr.oxite_Tag)
                    orderby p.PublishedDate
                    select p)
                .Visible()
                .GetPage(pageIndex, pageSize);
        }

        public virtual IEnumerable<IPost> GetPosts(Guid siteID, DateTime startDate, DateTime endDate)
        {
            return (from b in dataContext.oxite_Areas
                    join pbr in dataContext.oxite_PostAreaRelationships on b.AreaID equals pbr.AreaID
                    join p in dataContext.oxite_Posts on pbr.PostID equals p.PostID
                    where b.SiteID == siteID && p.PublishedDate >= startDate && p.PublishedDate < endDate
                    orderby p.PublishedDate
                    select p).Cast<IPost>();
        }

        //TODO: (erikpo) Convert this function to pure linq
        public virtual Dictionary<Guid, string> GetPostsWithFullSlug(Guid siteID)
        {
            Dictionary<Guid, string> postPaths = new Dictionary<Guid, string>(10);

            var query = from pr in dataContext.oxite_PostRelationships
                        join p in dataContext.oxite_Posts on pr.PostID equals p.PostID
                        where pr.SiteID == siteID
                        select p;

            foreach (var item in query)
            {
                postPaths.Add(item.PostID, generatePagePath(item));
            }

            Dictionary<Guid, string> posts = new Dictionary<Guid, string>(postPaths.Count);

            foreach (var item in postPaths.OrderBy(pp => pp.Value))
            {
                posts.Add(item.Key, item.Value);
            }

            return posts;
        }

        public virtual IComment GetComment(Guid commentID)
        {
            return (from c in dataContext.oxite_Comments
                    where c.CommentID == commentID
                    select c).FirstOrDefault();
        }

        public virtual IEnumerable<IComment> GetComments(IPost post)
        {
            return (from c in dataContext.oxite_Comments
                    where c.PostID == post.ID
                    orderby c.PublishedDate
                    select c).Visible().Cast<IComment>();
        }

        public virtual IPageOfAList<IComment> GetComments(Guid siteID, int pageIndex, int pageSize)
        {
            return (from a in dataContext.oxite_Areas
                    join par in dataContext.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                    join c in dataContext.oxite_Comments on par.PostID equals c.PostID
                    where a.SiteID == siteID
                    orderby c.PublishedDate descending
                    select c)
                .Visible()
                .GetPage<oxite_Comment, IComment>(pageIndex, pageSize);
        }

        public virtual IPageOfAList<IComment> GetComments(IArea area, int pageIndex, int pageSize)
        {
            return (from par in dataContext.oxite_PostAreaRelationships
                    join c in dataContext.oxite_Comments on par.PostID equals c.PostID
                    where par.AreaID == area.ID
                    orderby c.PublishedDate descending
                    select c)
                .Visible()
                .GetPage<oxite_Comment, IComment>(pageIndex, pageSize);
        }

        public virtual IPageOfAList<IComment> GetComments(ITag tag, int pageIndex, int pageSize)
        {
            return (from ptr in dataContext.oxite_PostTagRelationships
                    join c in dataContext.oxite_Comments on ptr.PostID equals c.PostID
                    where ptr.TagID == tag.ID
                    orderby c.PublishedDate descending
                    select c)
                .Visible()
                .GetPage<oxite_Comment, IComment>(pageIndex, pageSize);
        }

        public virtual IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(Guid siteID)
        {
            var query = from b in dataContext.oxite_Areas
                        join pbr in dataContext.oxite_PostAreaRelationships on b.AreaID equals pbr.AreaID
                        join p in dataContext.oxite_Posts on pbr.PostID equals p.PostID
                        where b.SiteID == siteID
                        select p;

            return query.Visible().ArchiveList();
        }

        public virtual IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(IArea area)
        {
            var query = from b in dataContext.oxite_Areas
                        join pbr in dataContext.oxite_PostAreaRelationships on b.AreaID equals pbr.AreaID
                        join p in dataContext.oxite_Posts on pbr.PostID equals p.PostID
                        where b.AreaID == area.ID
                        select p;

            return query.Visible().ArchiveList();
        }

        public virtual IEnumerable<DateTime> GetYearMonthsOfPosts(Guid siteID)
        {
            return (from b in dataContext.oxite_Areas
                    join pbr in dataContext.oxite_PostAreaRelationships on b.AreaID equals pbr.AreaID
                    join p in dataContext.oxite_Posts on pbr.PostID equals p.PostID
                    where b.SiteID == siteID
                    orderby p.PublishedDate
                    group p by new DateTime(p.PublishedDate.Year, p.PublishedDate.Month, 1)
                    into results
                        select results.Key);
        }

        public virtual Type GetCommentAnonymousType()
        {
            return typeof (oxite_CommentAnonymous);
        }

        public virtual Dictionary<IComment, Guid> GetCommentsWithPostSubscriptionsWithoutMessages(
            TimeSpan checkForNewInterval)
        {
            Dictionary<IComment, Guid> list = new Dictionary<IComment, Guid>(50);
            DateTime endDate = DateTime.Now.ToUniversalTime();
            DateTime startDate = endDate.Add(-checkForNewInterval);

            var query = from c in dataContext.oxite_Comments
                        join p in dataContext.oxite_Posts on c.PostID equals p.PostID
                        join s in dataContext.oxite_Subscriptions on p.PostID equals s.PostID
                        join cm in dataContext.oxite_CommentMessageRelationships on c.CommentID equals cm.CommentID into
                            commentMessages
                        from commentMessageResult in commentMessages.DefaultIfEmpty()
                        where
                            c.State == (byte)EntityState.Normal &&
                            c.PublishedDate > startDate &&
                            c.PublishedDate <= endDate &&
                            p.PublishedDate <= DateTime.Now.ToUniversalTime()
                        orderby c.PublishedDate
                        select
                            new
                            {
                                Comment = c,
                                MessageID = commentMessageResult == null ? Guid.Empty : commentMessageResult.MessageID
                            };

            foreach (var item in query)
            {
                list.Add(item.Comment, item.MessageID);
            }

            return list;
        }

        public virtual IEnumerable<ISubscription> GetSubscriptions(IComment comment)
        {
            return (from c in dataContext.oxite_Comments
                    join p in dataContext.oxite_Posts on c.PostID equals p.PostID
                    join s in dataContext.oxite_Subscriptions on p.PostID equals s.PostID
                    where c.CommentID == comment.ID
                    select s).Cast<ISubscription>();
        }

        protected virtual IQueryable<oxite_Post> getPosts(Guid siteID)
        {
            return from a in dataContext.oxite_Areas
                   join par in dataContext.oxite_PostAreaRelationships on a.AreaID equals par.AreaID
                   join p in dataContext.oxite_Posts on par.PostID equals p.PostID
                   where a.SiteID == siteID
                   orderby p.PublishedDate descending
                   select p;
        }

        protected virtual IQueryable<oxite_Post> getPosts(IArea area)
        {
            return from pbr in dataContext.oxite_PostAreaRelationships
                   join p in dataContext.oxite_Posts on pbr.PostID equals p.PostID
                   where pbr.AreaID == area.ID
                   select p;
        }

        protected virtual IPageOfAList<IPost> getPosts(Guid siteID, PublishedFilter publishedFilter,
                                                       Func<IQueryable<oxite_Post>, IQueryable<oxite_Post>>
                                                           executeAdditionalQuery, int pageIndex, int pageSize)
        {
            return getPosts(getPosts(siteID), publishedFilter, executeAdditionalQuery, pageIndex, pageSize);
        }

        protected virtual IPageOfAList<IPost> getPosts(IArea area, PublishedFilter publishedFilter,
                                                       Func<IQueryable<oxite_Post>, IQueryable<oxite_Post>>
                                                           executeAdditionalQuery, int pageIndex, int pageSize)
        {
            return getPosts(getPosts(area), publishedFilter, executeAdditionalQuery, pageIndex, pageSize);
        }

        protected virtual IPageOfAList<IPost> getPosts(IQueryable<oxite_Post> query, PublishedFilter publishedFilter,
                                                       Func<IQueryable<oxite_Post>, IQueryable<oxite_Post>>
                                                           executeAdditionalQuery, int pageIndex, int pageSize)
        {
            if (publishedFilter != PublishedFilter.All)
            {
                if (publishedFilter == PublishedFilter.Published)
                {
                    query = query.Where(e => e.PublishedDate < DateTime.Now.ToUniversalTime());
                }
                else if (publishedFilter == PublishedFilter.NotPublished)
                {
                    query = query.Where(e => e.PublishedDate >= DateTime.Now.ToUniversalTime());
                }
            }

            if (executeAdditionalQuery != null)
            {
                query = executeAdditionalQuery(query);
            }

            return query.OrderByDescending(p => p.PublishedDate).GetPage(pageIndex, pageSize);
        }

        protected virtual string generatePagePath(IPost post)
        {
            StringBuilder sb = new StringBuilder(50);
            IPost currentPost = post;

            sb.Append("/");
            sb.Append(currentPost.Slug);

            while (currentPost.ID != currentPost.Parent.ID)
            {
                currentPost = currentPost.Parent;

                sb.Insert(0, currentPost.Slug);
                sb.Insert(0, "/");
            }

            return sb.ToString();
        }

        #endregion

        #region Create

        public virtual IComment CreateComment()
        {
            return new oxite_Comment();
        }

        public virtual ICommentAnonymous CreateCommentAnonymous()
        {
            return new oxite_CommentAnonymous();
        }

        public virtual IPost CreatePost()
        {
            return new oxite_Post();
        }

        public virtual ISubscription CreateSubscription()
        {
            return new oxite_Subscription();
        }

        public virtual ISubscriptionAnonymous CreateSubscriptionAnonymous()
        {
            return new oxite_SubscriptionAnonymous();
        }

        #endregion

        #region Add

        public virtual void AddPost(IPost post, ISubscription subscription)
        {
            OxiteCancelDataItemEventArgs<IPost> eventArgs = new OxiteCancelDataItemEventArgs<IPost>(post);
            OnPostAdding(eventArgs);
            if (eventArgs.Cancel)
            {
                return;
            }

            if (string.IsNullOrEmpty(post.Body))
            {
                throw new ArgumentNullException("post.Body");
            }
            if (string.IsNullOrEmpty(post.Title))
            {
                throw new ArgumentNullException("post.Title");
            }
            if (string.IsNullOrEmpty(post.Slug))
            {
                throw new ArgumentNullException("post.Slug");
            }
            if (post.State == 0)
            {
                throw new ArgumentNullException("post.State");
            }

            if (post.ID == Guid.Empty)
            {
                post.ID = Guid.NewGuid();
            }
            if (post.BodyShort == null)
            {
                post.BodyShort = "";
            }
            post.Created = post.Modified = DateTime.Now.ToUniversalTime();

            dataContext.oxite_Posts.InsertOnSubmit((oxite_Post)post);

            if (subscription != null)
            {
                if (subscription.ID == Guid.Empty)
                {
                    subscription.ID = Guid.NewGuid();
                }
                subscription.PostID = post.ID;

                dataContext.oxite_Subscriptions.InsertOnSubmit((oxite_Subscription)subscription);
            }
        }

        public virtual void AddComment(IComment comment, ICommentAnonymous commentAnonymous, ISubscription subscription,
                                       ISubscriptionAnonymous subscriptionAnonymous)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }

            if (comment.ID == Guid.Empty)
            {
                comment.ID = Guid.NewGuid();
            }
            comment.Created = comment.Modified = DateTime.Now.ToUniversalTime();

            OxiteCancelDataItemEventArgs<IComment> eventArgs = new OxiteCancelDataItemEventArgs<IComment>(comment);
            OnCommentAdding(eventArgs);
            if (eventArgs.Cancel)
            {
                return;
            }

            if (comment.State == (byte)EntityState.Normal)
            {
                comment.Published = comment.Created;
            }

            dataContext.oxite_Comments.InsertOnSubmit((oxite_Comment)comment);

            if (commentAnonymous != null)
            {
                commentAnonymous.CommentID = comment.ID;

                dataContext.GetTable<oxite_CommentAnonymous>().InsertOnSubmit((oxite_CommentAnonymous)commentAnonymous);
            }

            if (subscription != null)
            {
                if (subscription.ID == Guid.Empty)
                {
                    subscription.ID = Guid.NewGuid();
                }
                subscription.PostID = comment.PostID;

                dataContext.oxite_Subscriptions.InsertOnSubmit((oxite_Subscription)subscription);

                if (subscriptionAnonymous != null)
                {
                    subscriptionAnonymous.SubscriptionID = subscription.ID;

                    dataContext.GetTable<oxite_SubscriptionAnonymous>().InsertOnSubmit(
                        (oxite_SubscriptionAnonymous)subscriptionAnonymous);
                }
            }
        }

        public virtual void AddPostToTag(Guid postID, Guid tagID)
        {
            if (!(dataContext.oxite_PostTagRelationships.Where(ptr => ptr.PostID == postID && ptr.TagID == tagID)).Any())
            {
                dataContext.oxite_PostTagRelationships.InsertOnSubmit(new oxite_PostTagRelationship
                                                                      {PostID = postID, TagID = tagID});
            }
        }

        public virtual void RemovePostFromTag(Guid postID, Guid tagID)
        {
            oxite_PostTagRelationship ptrInstance =
                dataContext.oxite_PostTagRelationships.Where(ptr => ptr.PostID == postID && ptr.TagID == tagID).
                    FirstOrDefault();

            if (ptrInstance != null)
            {
                dataContext.oxite_PostTagRelationships.DeleteOnSubmit(ptrInstance);
            }
        }

        public virtual void AddPostRelationship(Guid siteID, Guid postID, Guid parentID)
        {
            if (siteID == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("siteID");
            }
            if (postID == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("postID");
            }

            dataContext.oxite_PostRelationships.InsertOnSubmit(new oxite_PostRelationship
                                                               {
                                                                   SiteID = siteID,
                                                                   ParentPostID =
                                                                       parentID != Guid.Empty ? parentID : postID,
                                                                   PostID = postID
                                                               });
        }

        public virtual void RemovePostRelationship(Guid postID)
        {
            oxite_PostRelationship relationship =
                dataContext.oxite_PostRelationships.Where(r => r.PostID == postID).FirstOrDefault();

            if (relationship != null)
            {
                dataContext.oxite_PostRelationships.DeleteOnSubmit(relationship);
            }
        }

        #endregion

        #region Remove

        public virtual void RemovePost(Guid postID)
        {
            IPost post = GetPost(postID);

            if (post != null)
            {
                post.State = (byte)EntityState.Removed;
            }
        }

        #endregion

        #region Save

        public virtual void SubmitChanges()
        {
            ChangeSet changes = dataContext.GetChangeSet();
            List<IPost> postsToBeAdded = new List<IPost>();
            List<IPost> postsToBeEdited = new List<IPost>();
            List<IPost> postsToBeRemoved = new List<IPost>();
            List<IComment> commentsToBeAdded = new List<IComment>();
            List<IComment> commentsToBeEdited = new List<IComment>();
            List<IComment> commentsToBeRemoved = new List<IComment>();

            foreach (object item in changes.Inserts)
            {
                if (item is IPost)
                {
                    postsToBeAdded.Add((IPost)item);
                }
                else if (item is IComment)
                {
                    commentsToBeAdded.Add((IComment)item);
                }
            }

            foreach (object item in changes.Updates)
            {
                if (item is IPost)
                {
                    postsToBeEdited.Add((IPost)item);
                }
                else if (item is IComment)
                {
                    commentsToBeEdited.Add((IComment)item);
                }
            }

            foreach (object item in changes.Deletes)
            {
                if (item is IPost)
                {
                    postsToBeRemoved.Add((IPost)item);
                }
                else if (item is IComment)
                {
                    commentsToBeRemoved.Add((IComment)item);
                }
            }

            if (postsToBeEdited.Count > 0)
            {
                foreach (IPost post in postsToBeEdited)
                {
                    OxiteCancelDataItemEventArgs<IPost> e = new OxiteCancelDataItemEventArgs<IPost>(post);
                    OnPostEditing(e);
                    if (e.Cancel)
                    {
                        return;
                    }
                }
            }

            if (postsToBeRemoved.Count > 0)
            {
                foreach (IPost post in postsToBeRemoved)
                {
                    OxiteCancelDataItemEventArgs<IPost> e = new OxiteCancelDataItemEventArgs<IPost>(post);
                    OnPostRemoving(e);
                    if (e.Cancel)
                    {
                        return;
                    }
                }
            }

            if (commentsToBeEdited.Count > 0)
            {
                foreach (IComment comment in commentsToBeEdited)
                {
                    OxiteCancelDataItemEventArgs<IComment> e = new OxiteCancelDataItemEventArgs<IComment>(comment);
                    OnCommentEditing(e);
                    if (e.Cancel)
                    {
                        return;
                    }
                }
            }

            if (commentsToBeRemoved.Count > 0)
            {
                foreach (IComment comment in commentsToBeRemoved)
                {
                    OxiteCancelDataItemEventArgs<IComment> e = new OxiteCancelDataItemEventArgs<IComment>(comment);
                    OnCommentRemoving(e);
                    if (e.Cancel)
                    {
                        return;
                    }
                }
            }

            //try
            //{
            dataContext.SubmitChanges(ConflictMode.FailOnFirstConflict);

            if (postsToBeAdded.Count > 0)
            {
                foreach (IPost post in postsToBeAdded)
                {
                    OnPostAdded(new OxiteDataItemEventArgs<IPost>(post));
                }
            }

            if (postsToBeEdited.Count > 0)
            {
                foreach (IPost post in postsToBeEdited)
                {
                    OnPostEdited(new OxiteDataItemEventArgs<IPost>(post));
                }
            }

            if (postsToBeRemoved.Count > 0)
            {
                foreach (IPost post in postsToBeRemoved)
                {
                    OnPostRemoved(new OxiteDataItemEventArgs<IPost>(post));
                }
            }

            if (commentsToBeAdded.Count > 0)
            {
                foreach (IComment comment in commentsToBeAdded)
                {
                    OnCommentAdded(new OxiteDataItemEventArgs<IComment>(comment));
                }
            }

            if (commentsToBeEdited.Count > 0)
            {
                foreach (IComment comment in commentsToBeEdited)
                {
                    OnCommentEdited(new OxiteDataItemEventArgs<IComment>(comment));
                }
            }

            if (commentsToBeRemoved.Count > 0)
            {
                foreach (IComment comment in commentsToBeRemoved)
                {
                    OnCommentRemoved(new OxiteDataItemEventArgs<IComment>(comment));
                }
            }
            //}
            //catch
            //{
            //    if (postsToBeAdded.Count > 0)
            //        foreach (IPost post in postsToBeAdded)
            //            post.ID = Guid.Empty;
            //    if (commentsToBeAdded.Count > 0)
            //        foreach (IComment comment in commentsToBeAdded)
            //            comment.ID = Guid.Empty;

            //    throw;
            //}
        }

        #endregion

        #region Events

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

        protected virtual void OnPostAdding(OxiteCancelDataItemEventArgs<IPost> e)
        {
            if (PostAdding != null)
            {
                PostAdding(this, e);
            }
        }

        protected virtual void OnPostAdded(OxiteDataItemEventArgs<IPost> e)
        {
            if (PostAdded != null)
            {
                PostAdded(this, e);
            }
        }

        protected virtual void OnPostEditing(OxiteCancelDataItemEventArgs<IPost> e)
        {
            if (PostEditing != null)
            {
                PostEditing(this, e);
            }
        }

        protected virtual void OnPostEdited(OxiteDataItemEventArgs<IPost> e)
        {
            if (PostEdited != null)
            {
                PostEdited(this, e);
            }
        }

        protected virtual void OnPostRemoving(OxiteCancelDataItemEventArgs<IPost> e)
        {
            if (PostRemoving != null)
            {
                PostRemoving(this, e);
            }
        }

        protected virtual void OnPostRemoved(OxiteDataItemEventArgs<IPost> e)
        {
            if (PostRemoved != null)
            {
                PostRemoved(this, e);
            }
        }

        protected virtual void OnCommentAdding(OxiteCancelDataItemEventArgs<IComment> e)
        {
            if (CommentAdding != null)
            {
                CommentAdding(this, e);
            }
        }

        protected virtual void OnCommentAdded(OxiteDataItemEventArgs<IComment> e)
        {
            if (CommentAdded != null)
            {
                CommentAdded(this, e);
            }
        }

        protected virtual void OnCommentEditing(OxiteCancelDataItemEventArgs<IComment> e)
        {
            if (CommentEditing != null)
            {
                CommentEditing(this, e);
            }
        }

        protected virtual void OnCommentEdited(OxiteDataItemEventArgs<IComment> e)
        {
            if (CommentEdited != null)
            {
                CommentEdited(this, e);
            }
        }

        protected virtual void OnCommentRemoving(OxiteCancelDataItemEventArgs<IComment> e)
        {
            if (CommentRemoving != null)
            {
                CommentRemoving(this, e);
            }
        }

        protected virtual void OnCommentRemoved(OxiteDataItemEventArgs<IComment> e)
        {
            if (CommentRemoved != null)
            {
                CommentRemoved(this, e);
            }
        }

        #endregion
    }
}