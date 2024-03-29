﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Model
{
    public class PostSubscription : EntityBase
    {
        public Post Post { get; set; }
        public UserBase User { get; set; }
    }
}
