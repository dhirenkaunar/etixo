﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Mvc.ViewModels.Extensions
{
    public static class UserViewModelExtensions
    {
        public static bool GetCanAccessAdmin(this UserViewModel userViewModel)
        {
            if (userViewModel == null)
                return false;

            return userViewModel.CanAccessAdmin;
        }
    }
}
