//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using Oxite.Data;

namespace Oxite.Mvc
{
    public static class StringExtensions
    {
        public static IPost LoadPage(this string path, Func<Guid, string, IPost> getPost)
        {
            IPost post = null;
            Guid parentPostID = Guid.Empty;
            string[] pagePathParts = path.Split('/');

            for (int i = 0; i < pagePathParts.Length; i++)
            {
                post = getPost(parentPostID, pagePathParts[i]);

                if (post != null)
                {
                    parentPostID = post.ID;
                }
                else
                {
                    break;
                }
            }

            return post;
        }

        public static ICommentAnonymous ToCommentAnonymous(this string serializedCommentAnonymous, Func<Type> getType)
        {
            ICommentAnonymous commentAnonymous;
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(getType());

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                byte[] bytes = Encoding.Unicode.GetBytes(serializedCommentAnonymous);
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;

                commentAnonymous = dcjs.ReadObject(ms) as ICommentAnonymous;
                ms.Close();
            }

            return commentAnonymous;
        }

        public static string ComputeHash(this string value)
        {
            string hash = value;

            if (value != null)
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] data = Encoding.ASCII.GetBytes(value);
                data = md5.ComputeHash(data);
                hash = "";
                for (int i = 0; i < data.Length; i++)
                {
                    hash += data[i].ToString("x2");
                }
            }

            return hash;
        }
    }
}