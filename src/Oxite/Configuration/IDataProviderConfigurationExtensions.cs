//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Configuration;
using System.Reflection;
using Oxite.Data;

namespace Oxite.Configuration
{
    public static class IDataProviderConfigurationExtensions
    {
        //private static IOxiteDataProvider providerInstance;
        //private static readonly object loctite = new Object();
        public static IOxiteDataProvider GetInstance(this IDataProviderConfiguration dataProviderConfiguration)
        {
            //if (providerInstance == null)
            //{
            //    lock (loctite)
            //    {
            //        if (providerInstance == null)
            //        {
            string[] typeParts = dataProviderConfiguration.Type.Split(',');
            string typeName = Assembly.CreateQualifiedName(typeParts[1].Trim(), typeParts[0].Trim());
            Type type = Type.GetType(typeName);

            return
                (IOxiteDataProvider)
                Activator.CreateInstance(type,
                                         new object[]
                                         {
                                             ConfigurationManager.ConnectionStrings[
                                                 dataProviderConfiguration.ConnectionStringName].ConnectionString
                                         });
            //        }
            //    }
            //}

            //return providerInstance;
        }
    }
}