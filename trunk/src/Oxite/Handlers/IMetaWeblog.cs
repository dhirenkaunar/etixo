//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using CookComputing.XmlRpc;

namespace Oxite.Handlers
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        public string title { get; set; }

        public string link { get; set; }

        [XmlRpcMissingMapping(MappingAction.Error)]
        public string description { get; set; }

        public string[] categories { get; set; }

        [XmlRpcMissingMapping(MappingAction.Error)]
        public DateTime dateCreated { get; set; }

        public string permalink { get; set; }

        public string postid { get; set; }

        public string userid { get; set; }

        public string mt_excerpt { get; set; }

        public string mt_basename { get; set; }

        public string wp_author { get; set; }

        public string wp_author_id { get; set; }

        public string wp_author_display_name { get; set; }
    }

    public struct UrlData
    {
        public string url { get; set; }
    }

    public struct FileData
    {
        public string name { get; set; }

        public string type { get; set; }

        public byte[] bits { get; set; }
    }

    public struct CategoryInfo
    {
        public string description { get; set; }
        public string htmlUrl { get; set; }
        public string rssUrl { get; set; }
    }

    public struct BlogInfo
    {
        public string url { get; set; }
        public string blogid { get; set; }
        public string blogName { get; set; }
    }

    public struct NewCategoryInfo
    {
        public string name { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string slug { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int parent_id { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string description { get; set; }
    }

    public struct AuthorInfo
    {
        public string user_id { get; set; }
        public string user_login { get; set; }
        public string display_name { get; set; }
        public string user_email { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string meta_value { get; set; }
    }

    public interface IMetaWeblog
    {
        [XmlRpcMethod("metaWeblog.newPost")]
        string NewPost(string blogId, string username, string password, Post post, bool publish);

        [XmlRpcMethod("metaWeblog.editPost")]
        bool EditPost(string postId, string username, string password, Post post, bool publish);

        [XmlRpcMethod("metaWeblog.getPost")]
        Post GetPost(string postId, string username, string password);

        [XmlRpcMethod("metaWeblog.newMediaObject")]
        UrlData NewMediaObject(string blogId, string username, string password, FileData file);

        [XmlRpcMethod("metaWeblog.getCategories")]
        CategoryInfo[] GetCategories(string blogId, string username, string password);

        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        Post[] GetRecentPosts(string blogId, string username, string password, int numberOfPosts);

        [XmlRpcMethod("blogger.getUsersBlogs")]
        BlogInfo[] GetUsersBlogs(string apikey, string username, string password);

        [XmlRpcMethod("wp.newCategory")]
        string NewCategory(string blog_id, string username, string password, NewCategoryInfo newCategory);

        [XmlRpcMethod("blogger.deletePost")]
        bool DeletePost(string appkey, string postid, string username, string password, bool publish);

        [XmlRpcMethod("wp.getAuthors")]
        AuthorInfo[] GetAuthors(string blog_id, string username, string password);
    }
}