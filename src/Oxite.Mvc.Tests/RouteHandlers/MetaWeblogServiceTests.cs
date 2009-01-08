//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CookComputing.XmlRpc;
using MvcFakes;
using Oxite.Data;
using Oxite.Handlers;
using Oxite.Mvc.Tests.Fakes;
using Xunit;

namespace Oxite.Mvc.Tests.RouteHandlers
{
    public class MetaWeblogServiceTests
    {
        #region NewPost

        [Fact]
        public void NewPostAddsToRepository()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                null);

            Post newPost = new Post() {title = "Test", description = "A Test", dateCreated = DateTime.Now};
            string postIDString = service.NewPost(areaID.ToString(), null, null, newPost, false);

            Assert.NotNull(postIDString);
            Assert.True(postRepository.Saved);
            Assert.NotEqual(Guid.Empty.ToString(), postIDString);
            Assert.Equal(1, postRepository.AddedPosts.Count);
            Assert.Equal("Test", postRepository.AddedPosts[0].Title);
            Assert.Equal("A Test", postRepository.AddedPosts[0].Body);
            Assert.Equal(newPost.dateCreated, postRepository.AddedPosts[0].Created);
        }

        [Fact]
        public void NewPostSetsPublishDateIfPublishTrue()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                null);

            Post newPost = new Post() {title = "Test", description = "A Test", dateCreated = DateTime.Now};

            service.NewPost(areaID.ToString(), null, null, newPost, true);

            Assert.True(DateTime.Today < postRepository.AddedPosts[0].Published);
        }

        [Fact]
        public void NewPostSetsPublishDateToMaxSqlDateIfPublishFalse()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                null);

            Post newPost = new Post() {title = "Test", description = "A Test", dateCreated = DateTime.Now};

            service.NewPost(areaID.ToString(), null, null, newPost, false);

            Assert.Null(postRepository.AddedPosts[0].Published);
        }

        [Fact]
        public void NewPostFaultsForNonParsableBlogID()
        {
            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                new FakeAreaRepository(),
                null,
                new FakeMembershipRepository(),
                new FakePostRepository(),
                null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewPost("asdf", null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }

            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewPostAddsPassedCategoriesAsTags()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();
            Guid tag1ID = Guid.NewGuid();
            Guid tag2ID = Guid.NewGuid();

            Post newPost = new Post()
                           {
                               categories = new[] {"Test1", "Test2"},
                               title = "Test",
                               description = "A Test",
                               dateCreated = DateTime.Now
                           };

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                new FakeTagRepository(new FakeTag() {ID = tag1ID, Name = "Test1"},
                                      new FakeTag() {ID = tag2ID, Name = "Test2"}));

            service.NewPost(areaID.ToString(), null, null, newPost, false);

            Assert.Equal(1, postRepository.AddedPosts.Count);

            IPost newIPost = postRepository.AddedPosts[0];
            Assert.Equal(2, postRepository.AddedPostTags.Count);
            Assert.Contains<Guid>(tag1ID, postRepository.AddedPostTags.Select(pt => pt.Value));
            Assert.Contains<Guid>(tag2ID, postRepository.AddedPostTags.Select(pt => pt.Value));
        }

        [Fact]
        public void NewPostAddsExcerptAsBodyShort()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                new FakeTagRepository());

            Post newPost = new Post()
                           {mt_excerpt = "Preview", title = "Test", description = "A Test", dateCreated = DateTime.Now};
            service.NewPost(areaID.ToString(), null, null, newPost, false);

            Assert.Equal(newPost.mt_excerpt, postRepository.AddedPosts[0].BodyShort);
        }

        [Fact]
        public void NewPostCreatesSlugForEntry()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            Post newPost = new Post() {title = "This is a test", description = "A Test", dateCreated = DateTime.Now};

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                new FakeTagRepository());

            service.NewPost(areaID.ToString(), null, null, newPost, false);

            IPost newIPost = postRepository.AddedPosts[0];
            Assert.Equal("This-is-a-test", newIPost.Slug);
        }

        [Fact]
        public void NewPostUsesPassedSlug()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            Post newPost = new Post()
                           {
                               mt_basename = "Slug",
                               categories = Enumerable.Empty<String>().ToArray(),
                               title = "This is a test",
                               description = "A Test",
                               dateCreated = DateTime.Now
                           };

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                new FakeTagRepository());

            service.NewPost(areaID.ToString(), null, null, newPost, false);

            IPost newIPost = postRepository.AddedPosts[0];
            Assert.Equal("Slug", newIPost.Slug);
        }

        [Fact]
        public void NewPostFaultsOnNullUser()
        {
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(),
                new FakePostRepository(),
                new FakeTagRepository());

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewPost(areaID.ToString(), null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewPostSetsCreatorToUser()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();
            Guid userID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/api"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1", Created = DateTime.Now}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = userID}),
                postRepository,
                null);

            service.NewPost(areaID.ToString(), null, null, new Post(), false);

            FakePost newFakePost = postRepository.AddedPosts[0];
            Assert.Equal(userID, newFakePost.CreatorUserID);
        }

        [Fact]
        public void NewPostFaultsOnNonAuthorRoleUser()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeAreaRepository areaRepository = new FakeAreaRepository();

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Created = DateTime.Now};
            areaRepository.Areas.Add(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                areaRepository,
                null,
                membershipRepository,
                new FakePostRepository(),
                null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.NewPost(area.ID.ToString(), null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        [Fact]
        public void NewPostDoesNotFaultOnAuthorRoleUser()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeAreaRepository areaRepository = new FakeAreaRepository();

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);
            membershipRepository.UserRoles.Add(new KeyValuePair<Guid, Guid>(user.ID, role.ID));

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Created = DateTime.Now};
            areaRepository.Areas.Add(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                areaRepository,
                null,
                membershipRepository,
                new FakePostRepository(),
                null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.NewPost(area.ID.ToString(), null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.Null(expectedException);
        }

        [Fact]
        public void NewPostSetsPassedAuthor()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();
            Guid postAsID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1", Created = DateTime.Now}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = postAsID}),
                postRepository,
                null);

            service.NewPost(areaID.ToString(), null, null, new Post() {wp_author_id = postAsID.ToString()}, false);

            FakePost newFakePost = postRepository.AddedPosts[0];
            Assert.Equal(postAsID, newFakePost.CreatorUserID);
        }

        #endregion

        #region EditPost

        [Fact]
        public void EditPostFaultsOnInvalidEntryID()
        {
            Guid areaID = Guid.NewGuid();
            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                new FakePostRepository(),
                null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.EditPost(areaID.ToString(), null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void EditPostSavesChangesToTextFields()
        {
            FakeAreaRepository areaRepository = new FakeAreaRepository();
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1"};
            areaRepository.Areas.Add(area);

            postRepository.Posts.Add(new FakePost()
                                     {
                                         ID = postID,
                                         Title = "PreTitle",
                                         Body = "PreBody",
                                         BodyShort = "PreBodyShort",
                                         Area = area
                                     });

            Post newPost = new Post()
                           {
                               title = "PostTitle",
                               description = "PostDescription",
                               mt_excerpt = "PostBodyShort",
                               mt_basename = "PostSlug"
                           };

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                areaRepository,
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                null);

            bool success = service.EditPost(postID.ToString(), null, null, newPost, false);

            Assert.True(success);
            Assert.True(postRepository.Saved);
            FakePost edited = postRepository.Posts[0];
            Assert.Equal(newPost.title, edited.Title);
            Assert.Equal(newPost.description, edited.Body);
            Assert.Equal(newPost.mt_excerpt, edited.BodyShort);
            Assert.Equal(newPost.mt_basename, edited.Slug);
        }

        [Fact]
        public void EditPostPublishesIfPublishIsTrue()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();

            postRepository.Posts.Add(new FakePost()
                                     {ID = postID, Title = "PreTitle", Body = "PreBody", BodyShort = "PreBodyShort"});

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                null);

            Post newPost = new Post()
                           {title = "PostTitle", description = "PostDescription", mt_excerpt = "PostBodyShort"};

            service.EditPost(postID.ToString(), null, null, newPost, true);

            FakePost edited = postRepository.Posts[0];
            Assert.True(edited.Published > DateTime.Today);
        }

        [Fact]
        public void EditPostEditsTagList()
        {
            FakePostRepository postRepository = new FakePostRepository();
            FakeTagRepository tagRepository = new FakeTagRepository();
            Guid postID = Guid.NewGuid();
            Guid tag1ID = Guid.NewGuid();
            Guid tag2ID = Guid.NewGuid();
            Guid tag3ID = Guid.NewGuid();

            postRepository.Posts.Add(new FakePost()
                                     {
                                         ID = postID,
                                         Title = "PreTitle",
                                         Body = "PreBody",
                                         BodyShort = "PreBodyShort",
                                         Tags =
                                             new List<FakeTag>(new FakeTag[]
                                                               {
                                                                   new FakeTag() {ID = tag1ID, Name = "Old1"},
                                                                   new FakeTag() {ID = tag2ID, Name = "Both1"}
                                                               }).Cast<ITag>
                                             ()
                                     });

            tagRepository.Tags.Add(new FakeTag() {ID = tag1ID, Name = "Old1"});
            tagRepository.Tags.Add(new FakeTag() {ID = tag2ID, Name = "Both1"});
            tagRepository.Tags.Add(new FakeTag() {ID = tag3ID, Name = "New1"});

            Post newPost = new Post()
                           {
                               categories = new[] {"New1", "Both1"},
                               title = "PostTitle",
                               description = "PostDescription",
                               mt_excerpt = "PostBodyShort"
                           };

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                tagRepository);

            service.EditPost(postID.ToString(), null, null, newPost, false);

            FakePost edited = postRepository.Posts[0];
            Assert.Equal(2, edited.Tags.Count());
            Assert.Contains<Guid>(tag3ID, postRepository.AddedPostTags.Select(pt => pt.Value));
            Assert.Contains<Guid>(tag2ID, postRepository.AddedPostTags.Select(pt => pt.Value));
        }

        [Fact]
        public void EditPostAddsNewTags()
        {
            FakePostRepository postRepository = new FakePostRepository();
            FakeTagRepository tagRepository = new FakeTagRepository();
            Guid postID = Guid.NewGuid();
            Guid tag1ID = Guid.NewGuid();
            Guid tag2ID = Guid.NewGuid();

            postRepository.Posts.Add(new FakePost()
                                     {
                                         ID = postID,
                                         Title = "PreTitle",
                                         Body = "PreBody",
                                         BodyShort = "PreBodyShort",
                                         Tags =
                                             new List<FakeTag>(new FakeTag[]
                                                               {
                                                                   new FakeTag() {ID = tag1ID, Name = "Old1"},
                                                                   new FakeTag() {ID = tag2ID, Name = "Both1"}
                                                               }).Cast<ITag>
                                             ()
                                     });

            tagRepository.Tags.Add(new FakeTag() {ID = tag1ID, Name = "Old1"});
            tagRepository.Tags.Add(new FakeTag() {ID = tag2ID, Name = "Both1"});

            Post newPost = new Post()
                           {
                               categories = new[] {"New1", "Both1"},
                               title = "PostTitle",
                               description = "PostDescription",
                               mt_excerpt = "PostBodyShort"
                           };

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                new FakeAreaRepository(new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                tagRepository);

            service.EditPost(postID.ToString(), null, null, newPost, false);

            FakePost edited = postRepository.Posts[0];
            Assert.Equal(2, edited.Tags.Count());
            FakeTag newTag = tagRepository.AddedTags.Where(t => t.Name == "New1").FirstOrDefault();
            Assert.NotNull(newTag);
            Assert.Contains<Guid>(newTag.ID, postRepository.AddedPostTags.Select(pt => pt.Value));
            Assert.Contains<Guid>(tag2ID, postRepository.AddedPostTags.Select(pt => pt.Value));
        }

        [Fact]
        public void EditPostFaultsOnNullUser()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();

            postRepository.Posts.Add(new FakePost() {ID = postID});

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                null,
                null,
                new FakeMembershipRepository(),
                postRepository,
                null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.EditPost(postID.ToString(), null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void EditPostFaultsOnUserNotInTagRole()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeAreaRepository areaRepository = new FakeAreaRepository();
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();
            Guid authorRole = Guid.NewGuid();

            areaRepository.Areas.Add(new FakeArea() {ID = Guid.NewGuid()});

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Created = DateTime.Now};
            areaRepository.Areas.Add(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            postRepository.Posts.Add(new FakePost() {ID = postID, Area = area});

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid(), AuthorAutoSubscribe = false}),
                areaRepository,
                null,
                membershipRepository,
                postRepository,
                null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.EditPost(postID.ToString(), null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        [Fact]
        public void EditPostDoesntTouchAlreadyPublishedEntrysPublishDate()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();
            DateTime currentPublishDate = DateTime.Now.AddDays(-1);

            postRepository.Posts.Add(new FakePost()
                                     {
                                         ID = postID,
                                         Title = "PreTitle",
                                         Body = "PreBody",
                                         BodyShort = "PreBodyShort",
                                         Published = currentPublishDate
                                     });

            Post newPost = new Post()
                           {
                               title = "PostTitle",
                               description = "PostDescription",
                               mt_excerpt = "PostBodyShort",
                               mt_basename = "PostSlug"
                           };

            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                new FakeAreaRepository(),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                postRepository,
                null);

            service.EditPost(postID.ToString(), null, null, newPost, true);

            FakePost edited = postRepository.Posts[0];
            Assert.Equal(currentPublishDate, edited.Published);
        }

        #endregion

        #region GetPost

        [Fact]
        public void GetPostFaultsOnBadID()
        {
            MetaWeblogService service = new MetaWeblogService(
                () => new RouteCollection(),
                new FakeHttpContext("~/metaweblogapi"),
                null,
                new FakeAreaRepository(),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                new FakePostRepository(),
                null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetPost(Guid.NewGuid().ToString(), null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetPostReturnsPost()
        {
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();
            Guid userID = Guid.NewGuid();
            DateTime now = DateTime.Now;

            FakePost fakePost = new FakePost()
                                {
                                    Title = "Title",
                                    Body = "Body",
                                    Created = now,
                                    Published = now,
                                    ID = postID,
                                    CreatorUser = new FakeUser() {ID = userID, Username = "User"},
                                    BodyShort = "Excerpt",
                                    Slug = "Slug"
                                };
            fakePost.Area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Type = "Blog"};
            fakePost.Tags =
                new List<FakeTag>(new FakeTag[]
                                  {
                                      new FakeTag() {ID = Guid.NewGuid(), Name = "Tag1"},
                                      new FakeTag() {ID = Guid.NewGuid(), Name = "Tag2"}
                                  }).Cast<ITag>();

            postRepository.Posts.Add(fakePost);

            string expectedLink = "http://testhost/Blog1/Slug";

            RouteCollection routes = new RouteCollection();
            routes.MapRoute("BlogPermalink", "{areaName}/{slug}");

            MetaWeblogService service = new MetaWeblogService(
                () => routes,
                new FakeHttpContextWrapper(new FakeHttpContext(new Uri("http://testhost/metaweblogapi"),
                                                               "~/metaweblogapi")),
                null,
                new FakeAreaRepository(),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = userID}),
                postRepository,
                null);

            Post post = service.GetPost(postID.ToString(), null, null);

            Assert.NotNull(post);
            Assert.Equal(fakePost.Title, post.title);
            Assert.Equal(fakePost.Body, post.description);
            Assert.Equal(fakePost.Published, post.dateCreated);
            Assert.Equal(fakePost.CreatorUserID, new Guid(post.userid));
            Assert.Equal(fakePost.BodyShort, post.mt_excerpt);
            Assert.Equal(fakePost.Slug, post.mt_basename);
            foreach (string category in post.categories)
                Assert.Contains<string>(category, fakePost.Tags.Select(t => t.Name));
            Assert.Equal(expectedLink, post.link);
            Assert.Equal(expectedLink, post.permalink);
        }

        [Fact]
        public void GetPostFaultsOnNullUser()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null, null, null,
                                                              new FakeMembershipRepository(), null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetPost(Guid.NewGuid().ToString(), null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetPostFaultsOnUserNotInAuthorRoles()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeAreaRepository areaRepository = new FakeAreaRepository();
            FakePostRepository postRepository = new FakePostRepository();
            Guid postID = Guid.NewGuid();

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Created = DateTime.Now};
            areaRepository.Areas.Add(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            FakePost post = new FakePost() {ID = postID, Area = area};
            postRepository.Posts.Add(post);

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, postRepository, null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.GetPost(postID.ToString(), null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        #endregion

        #region GetUsersBlogs

        [Fact]
        public void GetUsersBlogsFaultsOnNullUser()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null, null, null,
                                                              new FakeMembershipRepository(), null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetUsersBlogs(null, null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetUsersBlogsFaultsOnUserNotInAuthorRoles()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeAreaRepository areaRepository = new FakeAreaRepository();
            FakeArea area1 = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1"};
            areaRepository.Areas.Add(area1);
            FakeArea area2 = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog2"};
            areaRepository.Areas.Add(area2);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area1.ID, role.ID));
            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area2.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(
                null,
                null,
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid()}),
                areaRepository,
                null,
                membershipRepository,
                null,
                null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetUsersBlogs(null, null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetUsersBlogsFiltersBlogsUserHasNoPermissionTo()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);
            membershipRepository.UserRoles.Add(new KeyValuePair<Guid, Guid>(user.ID, role.ID));

            FakeAreaRepository areaRepository = new FakeAreaRepository();
            Guid area1ID = Guid.NewGuid();
            FakeArea area1 = new FakeArea() {ID = area1ID, Name = "Blog1", Type = "Blog"};
            areaRepository.Areas.Add(area1);
            Guid area2ID = Guid.NewGuid();
            FakeArea area2 = new FakeArea() {ID = area2ID, Name = "Blog2", Type = "Blog"};
            areaRepository.Areas.Add(area2);
            areaRepository.Areas.Add(new FakeArea() {ID = Guid.NewGuid(), Name = "Blog3", Type = "Blog"});

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area1.ID, role.ID));
            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area2.ID, role.ID));

            RouteCollection routes = new RouteCollection();
            routes.MapRoute("Blog", "{areaName}");

            MetaWeblogService service = new MetaWeblogService(
                () => routes,
                new FakeHttpContextWrapper(new FakeHttpContext(new Uri("http://testhost/metaweblogapi"),
                                                               "~/metaweblogapi")),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid()}),
                areaRepository,
                null,
                membershipRepository,
                null,
                null);

            BlogInfo[] blogs = service.GetUsersBlogs(null, null, null);

            Assert.Equal(2, blogs.Length);
            Assert.Contains<string>("Blog1", blogs.Select(b => b.blogName));
            Assert.Contains<string>("Blog2", blogs.Select(b => b.blogName));
            Assert.Contains<Guid>(area1ID, blogs.Select(b => new Guid(b.blogid)));
            Assert.Contains<Guid>(area2ID, blogs.Select(b => new Guid(b.blogid)));
            Assert.Contains<string>("http://testhost/Blog1", blogs.Select(b => b.url));
            Assert.Contains<string>("http://testhost/Blog2", blogs.Select(b => b.url));
        }

        #endregion

        #region GetCategories

        [Fact]
        public void GetCategoriesReturnsAllTags()
        {
            Guid areaID = Guid.NewGuid();

            RouteCollection routes = new RouteCollection();
            routes.MapRoute("TagPermalink", "Tags/{tagName}");
            routes.MapRoute("TagPermalinkRss", "Tags/{tagName}/RSS");

            MetaWeblogService service = new MetaWeblogService(
                () => routes,
                new FakeHttpContextWrapper(new FakeHttpContext(new Uri("http://testhost/metaweblogapi"),
                                                               "~/metaweblogapi")),
                null,
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                null,
                new FakeTagRepository(new FakeTag() {ID = Guid.NewGuid(), Name = "Tag1"}));

            CategoryInfo[] cats = service.GetCategories(areaID.ToString(), null, null);

            Assert.Equal(1, cats.Length);
            Assert.Equal("Tag1", cats[0].description);
        }

        [Fact]
        public void GetCategoriesFaultsOnNullUser()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null, new FakeAreaRepository(), null,
                                                              new FakeMembershipRepository(), null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetCategories(Guid.NewGuid().ToString(), null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetCategoriesFaultsOnUserNotInAuthorRole()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeAreaRepository areaRepository = new FakeAreaRepository();
            Guid areaID = Guid.NewGuid();

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = areaID, Name = "Blog1"};
            areaRepository.Areas.Add(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, null, null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.GetCategories(areaID.ToString(), null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        #endregion

        #region GetRecentPosts

        [Fact]
        public void GetRecentPostsReturnsNumberOfPostsInBlog()
        {
            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Type = "Blog"};
            FakeAreaRepository areaRepository = new FakeAreaRepository(area);

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            FakePost post1 = new FakePost()
                             {
                                 ID = Guid.NewGuid(),
                                 CreatorUser = user,
                                 Area = area,
                                 Slug = "Post1",
                                 Created = DateTime.Now
                             };
            FakePost post2 = new FakePost()
                             {
                                 ID = Guid.NewGuid(),
                                 CreatorUser = user,
                                 Area = area,
                                 Slug = "Post2",
                                 Created = DateTime.Now
                             };

            RouteCollection routes = new RouteCollection();
            routes.MapRoute("BlogPermalink", "{areaName}/{slug}");

            MetaWeblogService service = new MetaWeblogService(
                () => routes,
                new FakeHttpContextWrapper(new FakeHttpContext(new Uri("http://testhost/metaweblogapi"),
                                                               "~/metaweblogapi")),
                null,
                areaRepository,
                null,
                new FakeMembershipRepository(user),
                new FakePostRepository(post1, post2),
                null);

            Post[] posts = service.GetRecentPosts(area.ID.ToString(), null, null, 2);

            Assert.NotNull(posts);
            Assert.Equal(2, posts.Length);
            Assert.Contains<Guid>(post1.ID, posts.Select(p => new Guid(p.postid)));
            Assert.Contains<Guid>(post2.ID, posts.Select(p => new Guid(p.postid)));
        }

        [Fact]
        public void GetRecentPostsFaultsOnNullUser()
        {
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                null,
                null,
                null,
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(),
                null,
                null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetRecentPosts(areaID.ToString(), null, null, 0);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetRecentPostsFaultsOnUserNotInAuthorRole()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = areaID, Name = "Blog1", Created = DateTime.Now};
            FakeAreaRepository areaRepository = new FakeAreaRepository(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, new FakePostRepository(), null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.GetRecentPosts(areaID.ToString(), null, null, 0);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        #endregion

        #region NewMediaObject

        [Fact]
        public void NewMediaObjectSavesFile()
        {
            FakeResourceRepository resourceRepository = new FakeResourceRepository();
            Guid userID = Guid.NewGuid();
            Guid areaID = Guid.NewGuid();

            RouteCollection routes = new RouteCollection();
            routes.MapRoute("UserFile", "Users/{username}/Files/{*filePath}");

            MetaWeblogService service = new MetaWeblogService(
                () => routes,
                new FakeHttpContextWrapper(new FakeHttpContext(new Uri("http://testhost/metaweblogapi"),
                                                               "~/metaweblogapi")),
                new FakeConfig(new FakeSite() {ID = Guid.NewGuid()}),
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                resourceRepository,
                new FakeMembershipRepository(new FakeUser() {ID = userID, Username = "User1"}),
                null,
                null);

            UrlData url = service.NewMediaObject(areaID.ToString(), "User1", null,
                                                 new FileData()
                                                 {bits = Enumerable.Empty<byte>().ToArray(), name = "test.txt"});

            Assert.Equal(true, resourceRepository.Saved);
            Assert.Equal("test.txt", resourceRepository.AddedFiles[0].Name);
            Assert.Equal(userID, resourceRepository.AddedUserFiles[0].Key);
            Assert.Equal("http://testhost/Users/User1/Files/test.txt", url.url);
        }

        [Fact]
        public void NewMediaObjectFaultsForNonParsableBlogID()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null, new FakeAreaRepository(), null,
                                                              new FakeMembershipRepository(new FakeUser()
                                                                                           {ID = Guid.NewGuid()}), null,
                                                              null);
            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewPost("asdf", null, null, default(Post), false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }

            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewMediaObjectFaultsForNoBlogWithPassedBlogID()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null, new FakeAreaRepository(), null,
                                                              new FakeMembershipRepository(new FakeUser()
                                                                                           {ID = Guid.NewGuid()}), null,
                                                              null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewMediaObject(Guid.NewGuid().ToString(), null, null, default(FileData));
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewMediaObjectFaultsOnNullUser()
        {
            Guid areaID = Guid.NewGuid();
            MetaWeblogService service = new MetaWeblogService(null, null, null,
                                                              new FakeAreaRepository(new FakeArea()
                                                                                     {ID = areaID, Name = "Blog1"}),
                                                              null, new FakeMembershipRepository(), null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewMediaObject(areaID.ToString(), null, null, default(FileData));
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewMediaObjectFaultsOnUserNotInAuthorRole()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Created = DateTime.Now};
            FakeAreaRepository areaRepository = new FakeAreaRepository(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, null, null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.NewMediaObject(area.ID.ToString(), null, null, default(FileData));
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        #endregion

        #region NewCategory

        [Fact]
        public void NewCategoryAddsTag()
        {
            FakeTagRepository tagRepository = new FakeTagRepository(new FakeTag() {ID = Guid.NewGuid(), Name = "Tag1"});
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(
                null,
                null,
                null,
                new FakeAreaRepository(new FakeArea() {ID = areaID, Name = "Blog1"}),
                null,
                new FakeMembershipRepository(new FakeUser() {ID = Guid.NewGuid()}),
                null,
                tagRepository);

            string newTagID = service.NewCategory(areaID.ToString(), null, null, new NewCategoryInfo() {name = "Tag2"});

            Assert.True(tagRepository.Saved);
            Assert.NotEqual("", newTagID);
            Assert.Equal(1, tagRepository.AddedTags.Count);
            Assert.NotEqual(Guid.Empty, tagRepository.AddedTags[0].ID);
            Assert.Equal("Tag2", tagRepository.AddedTags[0].Name);
        }

        [Fact]
        public void NewCategoryFailsForBadBlogID()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null, new FakeAreaRepository(), null, null,
                                                              null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewCategory(Guid.NewGuid().ToString(), null, null, default(NewCategoryInfo));
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewCategoryFaultForNullUser()
        {
            Guid areaID = Guid.NewGuid();

            MetaWeblogService service = new MetaWeblogService(null, null, null,
                                                              new FakeAreaRepository(new FakeArea()
                                                                                     {ID = areaID, Name = "Blog1"}),
                                                              null, new FakeMembershipRepository(), null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewCategory(areaID.ToString(), null, null, default(NewCategoryInfo));
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewCategoryFaultsForUserNotInAuthorRole()
        {
            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeAreaRepository areaRepository = new FakeAreaRepository();
            FakePostRepository postRepository = new FakePostRepository();
            Guid areaID = Guid.NewGuid();

            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = areaID, Name = "Blog1", Created = DateTime.Now};
            areaRepository.Areas.Add(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.NewCategory(areaID.ToString(), null, null, default(NewCategoryInfo));
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void NewCategoryReturnsOldTagIfNameMatches()
        {
            Guid tagID = Guid.NewGuid();
            Guid areaID = Guid.NewGuid();

            FakeTagRepository tagRepository = new FakeTagRepository(new FakeTag() {ID = tagID, Name = "Tag1"});

            MetaWeblogService service = new MetaWeblogService(null, null, null,
                                                              new FakeAreaRepository(new FakeArea()
                                                                                     {ID = areaID, Name = "Blog1"}),
                                                              null,
                                                              new FakeMembershipRepository(new FakeUser()
                                                                                           {ID = Guid.NewGuid()}), null,
                                                              tagRepository);

            string newTagID = service.NewCategory(areaID.ToString(), null, null, new NewCategoryInfo() {name = "Tag1"});

            Assert.NotEqual("", newTagID);
            Assert.Equal(tagID, new Guid(newTagID));
            Assert.False(tagRepository.Saved);
        }

        #endregion

        #region DeletePost

        [Fact]
        public void DeletePostDeletesPost()
        {
            Guid postID = Guid.NewGuid();

            FakePostRepository postRepository = new FakePostRepository(new FakePost() {ID = postID});

            MetaWeblogService service = new MetaWeblogService(null, null, null, null, null,
                                                              new FakeMembershipRepository(new FakeUser()
                                                                                           {ID = Guid.NewGuid()}),
                                                              postRepository, null);

            bool ret = service.DeletePost(null, postID.ToString(), null, null, false);

            Assert.True(ret);
            Assert.True(postRepository.Saved);
            Assert.Equal(EntityState.Removed, (EntityState)postRepository.Posts[0].State);
        }

        [Fact]
        public void DeletePostFaultsForNullUser()
        {
            MetaWeblogService service = new MetaWeblogService(null, null, null,
                                                              new FakeAreaRepository(new FakeArea()
                                                                                     {
                                                                                         ID = Guid.NewGuid(),
                                                                                         Name = "Blog1"
                                                                                     }), null,
                                                              new FakeMembershipRepository(), null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.DeletePost(null, null, null, null, false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void DeletePostFailsForUserNotInAuthorRole()
        {
            Guid postID = Guid.NewGuid();

            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = Guid.NewGuid(), Name = "Blog1", Created = DateTime.Now};
            FakeAreaRepository areaRepository = new FakeAreaRepository(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository,
                                                              new FakePostRepository(new FakePost()
                                                                                     {ID = postID, Area = area}), null);

            XmlRpcFaultException expectedException = null;
            try
            {
                service.DeletePost(null, postID.ToString(), null, null, false);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedException = fault;
            }
            Assert.NotNull(expectedException);
        }

        #endregion

        #region GetAuthors

        [Fact]
        public void GetAuthorsFaultsIfUserCantAccessBlog()
        {
            Guid areaID = Guid.NewGuid();

            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeUser user = new FakeUser() {ID = Guid.NewGuid()};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = areaID, Name = "Blog1"};
            FakeAreaRepository areaRepository = new FakeAreaRepository(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, null, null);

            XmlRpcFaultException expectedFault = null;
            try
            {
                service.GetAuthors(areaID.ToString(), null, null);
            }
            catch (XmlRpcFaultException fault)
            {
                expectedFault = fault;
            }
            Assert.NotNull(expectedFault);
        }

        [Fact]
        public void GetAuthorsReturnsAllUsersInAuthorRoles()
        {
            Guid areaID = Guid.NewGuid();

            FakeMembershipRepository membershipRepository = new FakeMembershipRepository();
            FakeUser user = new FakeUser()
                            {ID = Guid.NewGuid(), Username = "test", DisplayName = "Test User", HashedEmail = "asdf"};
            membershipRepository.Users.Add(user);
            FakeRole role = new FakeRole() {ID = Guid.NewGuid()};
            membershipRepository.Roles.Add(role);

            FakeArea area = new FakeArea() {ID = areaID, Name = "Blog1"};
            FakeAreaRepository areaRepository = new FakeAreaRepository(area);

            membershipRepository.AreaRoles.Add(new KeyValuePair<Guid, Guid>(area.ID, role.ID));
            membershipRepository.UserRoles.Add(new KeyValuePair<Guid, Guid>(user.ID, role.ID));

            MetaWeblogService service = new MetaWeblogService(null, null, null, areaRepository, null,
                                                              membershipRepository, null, null);

            AuthorInfo[] authors = service.GetAuthors(areaID.ToString(), "test", null);

            Assert.NotNull(authors);
            Assert.Equal(1, authors.Length);
            Assert.Equal(user.ID.ToString(), authors[0].user_id);
            Assert.Equal(user.Username, authors[0].user_login);
            Assert.Equal(user.DisplayName, authors[0].display_name);
            Assert.Equal(user.HashedEmail, authors[0].user_email);
            Assert.Null(authors[0].meta_value);
        }

        #endregion
    }
}