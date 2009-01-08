//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Security.Cryptography;
using System.Text;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc
{
    public class AntiForgeryToken
    {
        public static readonly string TicksInputName = "__antiForgeryTicks";
        public static readonly string TokenInputName = "__antiForgeryToken";

        private string key;

        public AntiForgeryToken(BaseController controller, string ticks)
        {
            key = controller.Config.Site.ID + ticks;
        }

        public string GetHash(string salt)
        {
            return (key + salt).ComputeHash();
        }
    }
}