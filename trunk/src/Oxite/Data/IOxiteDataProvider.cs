//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Data
{
    public interface IOxiteDataProvider
    {
        IAreaRepository AreaRepository { get; }
        IBackgroundServiceActionRepository BackgroundServiceActionRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        IMembershipRepository MembershipRepository { get; }
        IMessageRepository MessageRepository { get; }
        IPostRepository PostRepository { get; }
        IResourceRepository ResourceRepository { get; }
        ITagRepository TagRepository { get; }
        ITrackbackRepository TrackbackRepository { get; }
    }
}