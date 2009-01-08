//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Data
{
    public interface ITrackbackRepository
    {
        ITrackback GetTrackback(IPost post, string url);
        ITrackback CreateTrackback();
        void AddTrackback(ITrackback trackback);
        void SubmitChanges();
    }
}