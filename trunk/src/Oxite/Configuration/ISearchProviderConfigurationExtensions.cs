//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Reflection;
using Oxite.Search;

namespace Oxite.Configuration
{
    public static class ISearchProviderConfigurationExtensions
    {
        private static readonly object loctite = new Object();
        private static ISearchProvider providerInstance;

        public static ISearchProvider GetInstance(this ISearchProviderConfiguration searchProviderConfig)
        {
            if (providerInstance == null)
            {
                lock (loctite)
                {
                    if (providerInstance == null)
                    {
                        // tocheck(nheskew) not (yet) used - string name = searchProviderConfig.ProviderName;
                        string[] typeParts = searchProviderConfig.ProviderType.Split(',');
                        string typeName = Assembly.CreateQualifiedName(typeParts[1].Trim(), typeParts[0].Trim());
                        Type type = Type.GetType(typeName);

                        providerInstance = (ISearchProvider)Activator.CreateInstance(type);
                    }
                }
            }

            return providerInstance;
        }
    }
}