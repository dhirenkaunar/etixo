//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public class ArchiveDataBinder : IModelBinder
    {
        #region IModelBinder Members

        public ModelBinderResult BindModel(ModelBindingContext bindingContext)
        {
            return new ModelBinderResult(new ArchiveData(bindingContext.RouteData.Values["archiveData"] as string));
        }

        #endregion
    }
}