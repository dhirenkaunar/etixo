//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Oxite.Data
{
    public static class ICommentAnonymousExtensions
    {
        public static string ToJson(this ICommentAnonymous commentAnonymous, Func<Type> getType)
        {
            string serializedCommentAnonymous = string.Empty;
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(getType());

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                dcjs.WriteObject(ms, commentAnonymous);

                serializedCommentAnonymous = Encoding.Default.GetString(ms.ToArray());
                ms.Close();
            }

            return serializedCommentAnonymous;
        }
    }
}