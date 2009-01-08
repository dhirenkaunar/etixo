//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using Oxite.Data;

namespace Oxite.Mvc
{
    public static class NameValueCollectionExtensions
    {
        public static ICommentAnonymous LoadCommentAnonymous(this NameValueCollection collection,
                                                             Func<ICommentAnonymous> createCommentAnonymous,
                                                             ModelStateDictionary modelState)
        {
            ICommentAnonymous commentAnonymous = null;

            try
            {
                commentAnonymous = createCommentAnonymous();
                commentAnonymous.Name = collection["name"].IsRequired();
            }
            catch (ValidationException)
            {
                modelState.Add("AnonymousUser.Name", new ModelState() {AttemptedValue = collection["name"]});
                modelState["AnonymousUser.Name"].Errors.Add(new ModelError("Name is required."));
            }

            try
            {
                if (commentAnonymous == null)
                {
                    commentAnonymous = createCommentAnonymous();
                }

                if (!string.IsNullOrEmpty(collection["email"]))
                {
                    commentAnonymous.Email = collection["email"].IsRequired().IsEmail();
                    commentAnonymous.HashedEmail = commentAnonymous.Email.ComputeHash();
                }
                else
                {
                    commentAnonymous.HashedEmail = collection["hashedEmail"].IsRequired();
                }
            }
            catch (ValidationException vex)
            {
                modelState.Add("AnonymousUser.Email", new ModelState() {AttemptedValue = collection["email"]});
                modelState["AnonymousUser.Email"].Errors.Add(
                    new ModelError(string.Format("Email is required. ({0})", vex.Message)));
            }

            if (commentAnonymous != null)
            {
                commentAnonymous.Url = string.Empty;
                if (!string.IsNullOrEmpty(collection["url"]))
                {
                    try
                    {
                        commentAnonymous.Url = collection["url"].IsUrl();
                    }
                    catch (ValidationException vex)
                    {
                        modelState.Add("AnonymousUser.Url", new ModelState() {AttemptedValue = collection["url"]});
                        modelState["AnonymousUser.Url"].Errors.Add(
                            new ModelError(
                                string.Format("Url you're trying to add doesn't really look like a URL. ({0})",
                                              vex.Message)));
                    }
                }
            }

            return commentAnonymous;
        }


        public static string LoadCommentBody(this NameValueCollection collection, ModelStateDictionary modelState)
        {
            string commentBody = "";

            try
            {
                commentBody = collection["body"].IsRequired();
            }
            catch (ValidationException)
            {
                modelState.Add("Comment.Body", new ModelState() {AttemptedValue = collection["body"]});
                modelState["Comment.Body"].Errors.Add(new ModelError("A comment is not very useful when empty."));
            }

            return commentBody;
        }

        public static IPost LoadPost(this NameValueCollection collection, Func<IPost> createPost,
                                     ModelStateDictionary modelState)
        {
            IPost post = null;
            byte state = (byte)EntityState.NotSet;
            bool isPublished = false;

            try
            {
                post = createPost();
                post.Slug = collection["slug"] != null ? collection["slug"].IsSlug() : null;
            }
            catch (ValidationException)
            {
                modelState.Add("Post.Slug", new ModelState() {AttemptedValue = collection["slug"]});
                modelState["Post.Slug"].Errors.Add(new ModelError("Slug is required."));
            }

            try
            {
                if (post == null)
                {
                    post = createPost();
                }
                post.Title = collection["title"].IsRequired();
            }
            catch (ValidationException)
            {
                modelState.Add("Post.Title", new ModelState() {AttemptedValue = collection["title"]});
                modelState["Post.Title"].Errors.Add(new ModelError("Title is required."));
            }

            try
            {
                if (post == null)
                {
                    post = createPost();
                }
                post.Body = collection["body"].IsRequired();
            }
            catch (ValidationException)
            {
                modelState.Add("Post.Body", new ModelState() {AttemptedValue = collection["body"]});
                modelState["Post.Body"].Errors.Add(new ModelError("Body is required."));
            }

            if (byte.TryParse(collection["postState"], out state))
            {
                if (post == null)
                {
                    post = createPost();
                }
                post.State = state;
            }

            bool.TryParse(collection["isPublished"], out isPublished);

            try
            {
                if (isPublished)
                {
                    post.Published = DateTime.Parse(collection["publishDate"].IsRequired());
                }
                else
                {
                    post.Published = null;
                }
            }
            catch (ValidationException)
            {
                modelState.Add("Post.PublishDate", new ModelState() {AttemptedValue = collection["publishDate"]});
                modelState["Post.PublishDate"].Errors.Add(new ModelError("If publishing a date is required."));
            }
            catch (FormatException)
            {
                modelState.Add("Post.PublishDate", new ModelState() {AttemptedValue = collection["publishDate"]});
                modelState["Post.PublishDate"].Errors.Add(new ModelError("Publish date specified cannot be parsed."));
            }

            if (post != null)
            {
                post.BodyShort = collection["bodyShort"];
                post.Modified = DateTime.Now.ToUniversalTime();
            }

            return post;
        }

        public static ISubscriptionAnonymous LoadSubscriptionAnonymous(this NameValueCollection collection,
                                                                       Func<ISubscriptionAnonymous>
                                                                           createSubscriptionAnonymous,
                                                                       ModelStateDictionary modelState)
        {
            ISubscriptionAnonymous subscriptionAnonymous = null;

            if (collection.IsTrue("subscribe"))
            {
                try
                {
                    subscriptionAnonymous = createSubscriptionAnonymous();
                    subscriptionAnonymous.Name = collection["name"].IsRequired();
                }
                catch (ValidationException)
                {
                    modelState.Add("AnonymousUser.Name", new ModelState() {AttemptedValue = collection["name"]});
                    modelState["AnonymousUser.Name"].Errors.Add(new ModelError("Name is required to subscribe."));
                }

                try
                {
                    if (subscriptionAnonymous == null)
                    {
                        subscriptionAnonymous = createSubscriptionAnonymous();
                    }

                    subscriptionAnonymous.Email = collection["email"].IsRequired().IsEmail();
                }
                catch (ValidationException vex)
                {
                    modelState.Add("AnonymousUser.Email", new ModelState() {AttemptedValue = collection["email"]});
                    modelState["AnonymousUser.Email"].Errors.Add(
                        new ModelError(string.Format("Email is required to subscribe. ({0})", vex.Message)));
                }
            }

            return subscriptionAnonymous;
        }

        public static IEnumerable<ITag> LoadTags(this NameValueCollection collection, Func<string, ITag> getTag,
                                                 Func<string, ITag> createTag, ModelStateDictionary modelState)
        {
            List<ITag> tags = new List<ITag>();

            if (!string.IsNullOrEmpty(collection["tags"]))
            {
                try
                {
                    string[] tagNames = collection["tags"].Split(',');
                    foreach (string tagName in tagNames)
                    {
                        ITag tag = getTag(tagName.Trim().IsTag());

                        if (tag == null)
                        {
                            tag = createTag(tagName.Trim().IsTag());
                        }

                        tags.Add(tag);
                    }
                }
                catch (ValidationException vex)
                {
                    //todo(nheskew): need better error description and handling
                    modelState.Add("Post.Tags", new ModelState() {AttemptedValue = collection["tags"]});
                    modelState["Post.Tags"].Errors.Add(
                        new ModelError(string.Format("Tags couldn't be modified because: {0}", vex)));
                }
            }

            return tags;
        }

        public static bool IsTrue(this NameValueCollection collection, string key)
        {
            return !string.IsNullOrEmpty(key) && collection.GetValues(key) != null &&
                   collection.GetValues(key)[0] == "true";
        }

        public static string ToQueryString(this NameValueCollection queryString)
        {
            if (queryString.Count > 0)
            {
                StringBuilder qs = new StringBuilder();

                qs.Append("?");
                for (int i = 0; i < queryString.Count; i++)
                {
                    if (i > 0)
                    {
                        qs.Append("&");
                    }
                    qs.AppendFormat("{0}={1}", queryString.Keys[i], queryString[i]);
                }

                return qs.ToString();
            }

            return string.Empty;
        }
    }
}