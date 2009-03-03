//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.BackgroundServices
{
    public interface IBackgroundService
    {
        string Name { get; set; }
        bool Enabled { get; set; }
        TimeSpan Interval { get; set; }
        TimeSpan Timeout { get; set; }
        void Run();
    }
}