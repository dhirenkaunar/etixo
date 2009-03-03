/****** Object:  Table [dbo].[oxite_Tag]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Tag](
	[ParentTagID] [uniqueidentifier] NOT NULL,
	[TagID] [uniqueidentifier] NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_oxite_Tag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_StringResource]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oxite_StringResource](
	[StringResourceKey] [nvarchar](256) NOT NULL,
	[Language] [varchar](8) NOT NULL,
	[Version] [smallint] NOT NULL,
	[StringResourceValue] [nvarchar](max) NOT NULL,
	[CreatorUserID] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_oxite_StringResource] PRIMARY KEY CLUSTERED 
(
	[StringResourceKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oxite_Role]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Role](
	[ParentRoleID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_oxite_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_oxite_RoleName] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_Language]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oxite_Language](
	[LanguageID] [uniqueidentifier] NOT NULL,
	[LanguageName] [varchar](8) NOT NULL,
	[LanguageDisplayName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_oxite_Language] PRIMARY KEY CLUSTERED 
(
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oxite_Area]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oxite_Area](
	[SiteID] [uniqueidentifier] NOT NULL,
	[AreaID] [uniqueidentifier] NOT NULL,
	[AreaName] [nvarchar](256) NOT NULL,
	[DisplayName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Type] [varchar](25) NOT NULL,
	[TypeUrl] [varchar](25) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_oxite_Area] PRIMARY KEY CLUSTERED 
(
	[AreaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oxite_BackgroundServiceAction]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_BackgroundServiceAction](
	[BackgroundServiceActionID] [uniqueidentifier] NOT NULL,
	[TypeID] [uniqueidentifier] NOT NULL,
	[InProgress] [bit] NOT NULL,
	[Started] [datetime] NULL,
	[Completed] [datetime] NULL,
	[Details] [xml] NOT NULL,
 CONSTRAINT [PK_oxite_BackgroundServiceAction] PRIMARY KEY CLUSTERED 
(
	[BackgroundServiceActionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_AreaRoleRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_AreaRoleRelationship](
	[AreaID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_AreaRoleRelationship] PRIMARY KEY CLUSTERED 
(
	[AreaID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_StringResourceVersion]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oxite_StringResourceVersion](
	[StringResourceKey] [nvarchar](256) NOT NULL,
	[Language] [varchar](8) NOT NULL,
	[Version] [smallint] NOT NULL,
	[StringResourceValue] [nvarchar](max) NOT NULL,
	[CreatorUserID] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[State] [tinyint] NOT NULL,
 CONSTRAINT [PK_oxite_StringResourceVersion] PRIMARY KEY CLUSTERED 
(
	[StringResourceKey] ASC,
	[Language] ASC,
	[Version] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_User](
	[UserID] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[DisplayName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[HashedEmail] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[DefaultLanguageID] [uniqueidentifier] NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_oxite_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_oxite_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_Message]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Message](
	[MessageID] [uniqueidentifier] NOT NULL,
	[FromUserID] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[IsSent] [bit] NOT NULL,
	[SentDate] [datetime] NULL,
 CONSTRAINT [PK_oxite_Message] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_UserRoleRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_UserRoleRelationship](
	[UserID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_UserRoleRelationship] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_UserLanguage]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_UserLanguage](
	[UserID] [uniqueidentifier] NOT NULL,
	[LanguageID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_UserLanguage] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Post](
	[PostID] [uniqueidentifier] NOT NULL,
	[CreatorUserID] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[BodyShort] [nvarchar](max) NOT NULL,
	[State] [tinyint] NOT NULL,
	[Slug] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[PublishedDate] [datetime] NOT NULL,
	[SearchBody] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_oxite_Post] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_FileResource]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oxite_FileResource](
	[SiteID] [uniqueidentifier] NOT NULL,
	[FileResourceID] [uniqueidentifier] NOT NULL,
	[FileResourceName] [nvarchar](256) NOT NULL,
	[CreatorUserID] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NULL,
	[ContentType] [varchar](25) NOT NULL,
	[Path] [nvarchar](1000) NOT NULL,
	[State] [tinyint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_oxite_FileResource] PRIMARY KEY CLUSTERED 
(
	[FileResourceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oxite_Comment]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Comment](
	[PostID] [uniqueidentifier] NOT NULL,
	[CommentID] [uniqueidentifier] NOT NULL,
	[CreatorUserID] [uniqueidentifier] NOT NULL,
	[LanguageID] [uniqueidentifier] NOT NULL,
	[CreatorIP] [bigint] NOT NULL,
	[UserAgent] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[PublishedDate] [datetime] NOT NULL,
	[State] [tinyint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_oxite_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_PostTagRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_PostTagRelationship](
	[PostID] [uniqueidentifier] NOT NULL,
	[TagID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_PostTagRelationship] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC,
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_PostRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_PostRelationship](
	[SiteID] [uniqueidentifier] NOT NULL,
	[ParentPostID] [uniqueidentifier] NOT NULL,
	[PostID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_PostRelationship] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC,
	[ParentPostID] ASC,
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_PostAreaRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_PostAreaRelationship](
	[PostID] [uniqueidentifier] NOT NULL,
	[AreaID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_PostAreaRelationship] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC,
	[AreaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_UserFileResourceRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_UserFileResourceRelationship](
	[UserID] [uniqueidentifier] NOT NULL,
	[FileResourceID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_UserFileResourceRelationship] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[FileResourceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_Trackback]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Trackback](
	[PostID] [uniqueidentifier] NOT NULL,
	[TrackbackID] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](1000) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Body] [nvarchar](500) NOT NULL,
	[Source] [nvarchar](500) NOT NULL,
	[BlogName] [nvarchar](200) NOT NULL,
	[IsTargetInSource] [bit] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_oxite_Trackback] PRIMARY KEY CLUSTERED 
(
	[TrackbackID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_MessageTo]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_MessageTo](
	[MessageID] [uniqueidentifier] NOT NULL,
	[MessageToID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_MessageTo] PRIMARY KEY CLUSTERED 
(
	[MessageToID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_Subscription]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_Subscription](
	[SubscriptionID] [uniqueidentifier] NOT NULL,
	[PostID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_Subscription] PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_SubscriptionAnonymous]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_SubscriptionAnonymous](
	[SubscriptionID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_oxite_SubscriptionAnonymous] PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_MessageToAnonymous]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_MessageToAnonymous](
	[MessageToID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_oxite_MessageToAnonymous] PRIMARY KEY CLUSTERED 
(
	[MessageToID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_CommentRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_CommentRelationship](
	[ParentCommentID] [uniqueidentifier] NOT NULL,
	[CommentID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_CommentRelationship] PRIMARY KEY CLUSTERED 
(
	[ParentCommentID] ASC,
	[CommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_CommentMessageRelationship]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_CommentMessageRelationship](
	[CommentID] [uniqueidentifier] NOT NULL,
	[MessageID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_oxite_CommentMessageRelationship] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC,
	[MessageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oxite_CommentAnonymous]    Script Date: 01/05/2009 22:58:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oxite_CommentAnonymous](
	[CommentID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[HashedEmail] [nvarchar](200) NOT NULL,
	[Url] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_oxite_CommentAnonymous] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_oxite_Area_AreaID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Area] ADD  CONSTRAINT [DF_oxite_Area_AreaID]  DEFAULT (newid()) FOR [AreaID]
GO
/****** Object:  Default [DF_oxite_BackgroundServiceAction_BackgroundServiceActionID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] ADD  CONSTRAINT [DF_oxite_BackgroundServiceAction_BackgroundServiceActionID]  DEFAULT (newid()) FOR [BackgroundServiceActionID]
GO
/****** Object:  Default [DF_oxite_BackgroundServiceAction_InProgress]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] ADD  CONSTRAINT [DF_oxite_BackgroundServiceAction_InProgress]  DEFAULT ((0)) FOR [InProgress]
GO
/****** Object:  Default [DF_oxite_BackgroundServiceAction_Started]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] ADD  CONSTRAINT [DF_oxite_BackgroundServiceAction_Started]  DEFAULT (getdate()) FOR [Started]
GO
/****** Object:  Default [DF_oxite_FileResource_FileResourceID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_FileResource] ADD  CONSTRAINT [DF_oxite_FileResource_FileResourceID]  DEFAULT (newid()) FOR [FileResourceID]
GO
/****** Object:  Default [DF_oxite_Message_MessageID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Message] ADD  CONSTRAINT [DF_oxite_Message_MessageID]  DEFAULT (newid()) FOR [MessageID]
GO
/****** Object:  Default [DF_oxite_Message_IsSent]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Message] ADD  CONSTRAINT [DF_oxite_Message_IsSent]  DEFAULT ((0)) FOR [IsSent]
GO
/****** Object:  Default [DF_oxite_Post_PostID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Post] ADD  CONSTRAINT [DF_oxite_Post_PostID]  DEFAULT (newid()) FOR [PostID]
GO
/****** Object:  Default [DF_oxite_Tag_TagID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Tag] ADD  CONSTRAINT [DF_oxite_Tag_TagID]  DEFAULT (newid()) FOR [TagID]
GO
/****** Object:  Default [DF_oxite_Trackback_TrackbackID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Trackback] ADD  CONSTRAINT [DF_oxite_Trackback_TrackbackID]  DEFAULT (newid()) FOR [TrackbackID]
GO
/****** Object:  Default [DF_oxite_User_UserID]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_User] ADD  CONSTRAINT [DF_oxite_User_UserID]  DEFAULT (newid()) FOR [UserID]
GO
/****** Object:  ForeignKey [FK_oxite_AreaRoleRelationship_oxite_Area]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_AreaRoleRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_AreaRoleRelationship_oxite_Area] FOREIGN KEY([AreaID])
REFERENCES [dbo].[oxite_Area] ([AreaID])
GO
ALTER TABLE [dbo].[oxite_AreaRoleRelationship] CHECK CONSTRAINT [FK_oxite_AreaRoleRelationship_oxite_Area]
GO
/****** Object:  ForeignKey [FK_oxite_AreaRoleRelationship_oxite_Role]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_AreaRoleRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_AreaRoleRelationship_oxite_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[oxite_Role] ([RoleID])
GO
ALTER TABLE [dbo].[oxite_AreaRoleRelationship] CHECK CONSTRAINT [FK_oxite_AreaRoleRelationship_oxite_Role]
GO
/****** Object:  ForeignKey [FK_oxite_Comment_oxite_Language]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Comment]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Comment_oxite_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[oxite_Language] ([LanguageID])
GO
ALTER TABLE [dbo].[oxite_Comment] CHECK CONSTRAINT [FK_oxite_Comment_oxite_Language]
GO
/****** Object:  ForeignKey [FK_oxite_Comment_oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Comment]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Comment_oxite_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_Comment] CHECK CONSTRAINT [FK_oxite_Comment_oxite_Post]
GO
/****** Object:  ForeignKey [FK_oxite_Comment_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Comment]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Comment_oxite_User] FOREIGN KEY([CreatorUserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_Comment] CHECK CONSTRAINT [FK_oxite_Comment_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_CommentAnonymous_oxite_Comment]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_CommentAnonymous]  WITH CHECK ADD  CONSTRAINT [FK_oxite_CommentAnonymous_oxite_Comment] FOREIGN KEY([CommentID])
REFERENCES [dbo].[oxite_Comment] ([CommentID])
GO
ALTER TABLE [dbo].[oxite_CommentAnonymous] CHECK CONSTRAINT [FK_oxite_CommentAnonymous_oxite_Comment]
GO
/****** Object:  ForeignKey [FK_oxite_CommentMessageRelationship_oxite_Comment]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_CommentMessageRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_CommentMessageRelationship_oxite_Comment] FOREIGN KEY([CommentID])
REFERENCES [dbo].[oxite_Comment] ([CommentID])
GO
ALTER TABLE [dbo].[oxite_CommentMessageRelationship] CHECK CONSTRAINT [FK_oxite_CommentMessageRelationship_oxite_Comment]
GO
/****** Object:  ForeignKey [FK_oxite_CommentMessageRelationship_oxite_Message]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_CommentMessageRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_CommentMessageRelationship_oxite_Message] FOREIGN KEY([MessageID])
REFERENCES [dbo].[oxite_Message] ([MessageID])
GO
ALTER TABLE [dbo].[oxite_CommentMessageRelationship] CHECK CONSTRAINT [FK_oxite_CommentMessageRelationship_oxite_Message]
GO
/****** Object:  ForeignKey [FK_oxite_CommentRelationship_oxite_Comment]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_CommentRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_CommentRelationship_oxite_Comment] FOREIGN KEY([CommentID])
REFERENCES [dbo].[oxite_Comment] ([CommentID])
GO
ALTER TABLE [dbo].[oxite_CommentRelationship] CHECK CONSTRAINT [FK_oxite_CommentRelationship_oxite_Comment]
GO
/****** Object:  ForeignKey [FK_oxite_FileResource_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_FileResource]  WITH CHECK ADD  CONSTRAINT [FK_oxite_FileResource_oxite_User] FOREIGN KEY([CreatorUserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_FileResource] CHECK CONSTRAINT [FK_oxite_FileResource_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_Message_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Message]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Message_oxite_User] FOREIGN KEY([FromUserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_Message] CHECK CONSTRAINT [FK_oxite_Message_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_MessageTo_oxite_Message]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_MessageTo]  WITH CHECK ADD  CONSTRAINT [FK_oxite_MessageTo_oxite_Message] FOREIGN KEY([MessageID])
REFERENCES [dbo].[oxite_Message] ([MessageID])
GO
ALTER TABLE [dbo].[oxite_MessageTo] CHECK CONSTRAINT [FK_oxite_MessageTo_oxite_Message]
GO
/****** Object:  ForeignKey [FK_oxite_MessageTo_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_MessageTo]  WITH CHECK ADD  CONSTRAINT [FK_oxite_MessageTo_oxite_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_MessageTo] CHECK CONSTRAINT [FK_oxite_MessageTo_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_MessageToAnonymous_oxite_MessageTo]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_MessageToAnonymous]  WITH CHECK ADD  CONSTRAINT [FK_oxite_MessageToAnonymous_oxite_MessageTo] FOREIGN KEY([MessageToID])
REFERENCES [dbo].[oxite_MessageTo] ([MessageToID])
GO
ALTER TABLE [dbo].[oxite_MessageToAnonymous] CHECK CONSTRAINT [FK_oxite_MessageToAnonymous_oxite_MessageTo]
GO
/****** Object:  ForeignKey [FK_oxite_Post_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Post]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Post_oxite_User] FOREIGN KEY([CreatorUserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_Post] CHECK CONSTRAINT [FK_oxite_Post_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_PostAreaRelationship_oxite_Area]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_PostAreaRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_PostAreaRelationship_oxite_Area] FOREIGN KEY([AreaID])
REFERENCES [dbo].[oxite_Area] ([AreaID])
GO
ALTER TABLE [dbo].[oxite_PostAreaRelationship] CHECK CONSTRAINT [FK_oxite_PostAreaRelationship_oxite_Area]
GO
/****** Object:  ForeignKey [FK_oxite_PostAreaRelationship_oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_PostAreaRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_PostAreaRelationship_oxite_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_PostAreaRelationship] CHECK CONSTRAINT [FK_oxite_PostAreaRelationship_oxite_Post]
GO
/****** Object:  ForeignKey [FK_oxite_PostRelationship_oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_PostRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_PostRelationship_oxite_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_PostRelationship] CHECK CONSTRAINT [FK_oxite_PostRelationship_oxite_Post]
GO
/****** Object:  ForeignKey [FK_oxite_PostRelationship_oxite_Post1]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_PostRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_PostRelationship_oxite_Post1] FOREIGN KEY([ParentPostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_PostRelationship] CHECK CONSTRAINT [FK_oxite_PostRelationship_oxite_Post1]
GO
/****** Object:  ForeignKey [FK_oxite_PostTagRelationship_oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_PostTagRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_PostTagRelationship_oxite_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_PostTagRelationship] CHECK CONSTRAINT [FK_oxite_PostTagRelationship_oxite_Post]
GO
/****** Object:  ForeignKey [FK_oxite_PostTagRelationship_oxite_Tag]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_PostTagRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_PostTagRelationship_oxite_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[oxite_Tag] ([TagID])
GO
ALTER TABLE [dbo].[oxite_PostTagRelationship] CHECK CONSTRAINT [FK_oxite_PostTagRelationship_oxite_Tag]
GO
/****** Object:  ForeignKey [FK_oxite_Role_oxite_Role]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Role]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Role_oxite_Role] FOREIGN KEY([ParentRoleID])
REFERENCES [dbo].[oxite_Role] ([RoleID])
GO
ALTER TABLE [dbo].[oxite_Role] CHECK CONSTRAINT [FK_oxite_Role_oxite_Role]
GO
/****** Object:  ForeignKey [FK_oxite_StringResourceVersion_oxite_StringResource]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_StringResourceVersion]  WITH CHECK ADD  CONSTRAINT [FK_oxite_StringResourceVersion_oxite_StringResource] FOREIGN KEY([StringResourceKey])
REFERENCES [dbo].[oxite_StringResource] ([StringResourceKey])
GO
ALTER TABLE [dbo].[oxite_StringResourceVersion] CHECK CONSTRAINT [FK_oxite_StringResourceVersion_oxite_StringResource]
GO
/****** Object:  ForeignKey [FK_oxite_Subscription_oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Subscription]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Subscription_oxite_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_Subscription] CHECK CONSTRAINT [FK_oxite_Subscription_oxite_Post]
GO
/****** Object:  ForeignKey [FK_oxite_Subscription_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Subscription]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Subscription_oxite_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_Subscription] CHECK CONSTRAINT [FK_oxite_Subscription_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_SubscriptionAnonymous_oxite_Subscription]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_SubscriptionAnonymous]  WITH CHECK ADD  CONSTRAINT [FK_oxite_SubscriptionAnonymous_oxite_Subscription] FOREIGN KEY([SubscriptionID])
REFERENCES [dbo].[oxite_Subscription] ([SubscriptionID])
GO
ALTER TABLE [dbo].[oxite_SubscriptionAnonymous] CHECK CONSTRAINT [FK_oxite_SubscriptionAnonymous_oxite_Subscription]
GO
/****** Object:  ForeignKey [FK_oxite_Tag_oxite_Tag]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Tag]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Tag_oxite_Tag] FOREIGN KEY([ParentTagID])
REFERENCES [dbo].[oxite_Tag] ([TagID])
GO
ALTER TABLE [dbo].[oxite_Tag] CHECK CONSTRAINT [FK_oxite_Tag_oxite_Tag]
GO
/****** Object:  ForeignKey [FK_oxite_Trackback_oxite_Post]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_Trackback]  WITH CHECK ADD  CONSTRAINT [FK_oxite_Trackback_oxite_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[oxite_Post] ([PostID])
GO
ALTER TABLE [dbo].[oxite_Trackback] CHECK CONSTRAINT [FK_oxite_Trackback_oxite_Post]
GO
/****** Object:  ForeignKey [FK_oxite_User_oxite_Language]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_User]  WITH CHECK ADD  CONSTRAINT [FK_oxite_User_oxite_Language] FOREIGN KEY([DefaultLanguageID])
REFERENCES [dbo].[oxite_Language] ([LanguageID])
GO
ALTER TABLE [dbo].[oxite_User] CHECK CONSTRAINT [FK_oxite_User_oxite_Language]
GO
/****** Object:  ForeignKey [FK_oxite_UserFileResourceRelationship_oxite_FileResource]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_UserFileResourceRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_UserFileResourceRelationship_oxite_FileResource] FOREIGN KEY([FileResourceID])
REFERENCES [dbo].[oxite_FileResource] ([FileResourceID])
GO
ALTER TABLE [dbo].[oxite_UserFileResourceRelationship] CHECK CONSTRAINT [FK_oxite_UserFileResourceRelationship_oxite_FileResource]
GO
/****** Object:  ForeignKey [FK_oxite_UserFileResourceRelationship_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_UserFileResourceRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_UserFileResourceRelationship_oxite_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_UserFileResourceRelationship] CHECK CONSTRAINT [FK_oxite_UserFileResourceRelationship_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_UserLanguage_oxite_Language]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_UserLanguage]  WITH CHECK ADD  CONSTRAINT [FK_oxite_UserLanguage_oxite_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[oxite_Language] ([LanguageID])
GO
ALTER TABLE [dbo].[oxite_UserLanguage] CHECK CONSTRAINT [FK_oxite_UserLanguage_oxite_Language]
GO
/****** Object:  ForeignKey [FK_oxite_UserLanguage_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_UserLanguage]  WITH CHECK ADD  CONSTRAINT [FK_oxite_UserLanguage_oxite_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_UserLanguage] CHECK CONSTRAINT [FK_oxite_UserLanguage_oxite_User]
GO
/****** Object:  ForeignKey [FK_oxite_UserRoleRelationship_oxite_Role]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_UserRoleRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_UserRoleRelationship_oxite_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[oxite_Role] ([RoleID])
GO
ALTER TABLE [dbo].[oxite_UserRoleRelationship] CHECK CONSTRAINT [FK_oxite_UserRoleRelationship_oxite_Role]
GO
/****** Object:  ForeignKey [FK_oxite_UserRoleRelationship_oxite_User]    Script Date: 01/05/2009 22:58:35 ******/
ALTER TABLE [dbo].[oxite_UserRoleRelationship]  WITH CHECK ADD  CONSTRAINT [FK_oxite_UserRoleRelationship_oxite_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[oxite_User] ([UserID])
GO
ALTER TABLE [dbo].[oxite_UserRoleRelationship] CHECK CONSTRAINT [FK_oxite_UserRoleRelationship_oxite_User]
GO


IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'OxiteSite')
	CREATE LOGIN [OxiteSite] WITH PASSWORD = N'c9FTw!1', CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
ELSE
	ALTER LOGIN [OxiteSite] WITH PASSWORD = N'c9FTw!1', CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF

GO




DECLARE @Site1ID uniqueidentifier
DECLARE @Language1ID uniqueidentifier
DECLARE @Area1ID uniqueidentifier
DECLARE @Post1ID uniqueidentifier, @Post2ID uniqueidentifier
DECLARE @Tag1ID uniqueidentifier
DECLARE @User1ID uniqueidentifier, @User2ID uniqueidentifier, @Role1ID uniqueidentifier, @Role2ID uniqueidentifier
DECLARE @StringResourceKeyNewComment nvarchar(max)

--Sites
SET @Site1ID = '4F36436B-0782-4a94-BB4C-FD3916734C03'

--Languages
IF NOT EXISTS(SELECT * FROM oxite_Language WHERE LanguageName = 'en-us')
BEGIN
	SET @Language1ID = newid()
	INSERT INTO oxite_Language (LanguageID, LanguageName, LanguageDisplayName) VALUES (@Language1ID, 'en', 'English')
END

--Users
IF NOT EXISTS(SELECT * FROM oxite_User WHERE Username = 'Admin')
BEGIN
	SET @User1ID = newid()
	INSERT INTO
		oxite_User
	(
		UserID,
		Username,
		DisplayName,
		Email,
		HashedEmail,
		Password,
		PasswordSalt,
		DefaultLanguageID,
		Status
	)
	VALUES
	(
		@User1ID,
		'Admin',
		'Oxite Administrator',
		'tech@erikporter.com',
		'099f7ab5964d7d4db3043201cfd14db6',
		'BQWPtrSvaXLSzkU6vOM4XeV/080hsgtsVIjLEPFny7k=',
		'NaCl',
		@Language1ID,
		1
	)
END
ELSE
BEGIN
	SELECT @User1ID = UserID FROM oxite_User WHERE Username = 'Admin'
END

IF NOT EXISTS(SELECT * FROM oxite_User WHERE Username = 'Anonymous')
BEGIN
	SET @User2ID = newid()
	INSERT INTO
		oxite_User
	(
		UserID,
		Username,
		DisplayName,
		Email,
		HashedEmail,
		Password,
		PasswordSalt,
		DefaultLanguageID,
		Status
	)
	VALUES
	(
		@User2ID,
		'Anonymous',
		'',
		'',
		'',
		'',
		'',
		@Language1ID,
		1
	)
END
ELSE
BEGIN
	SELECT @User2ID = UserID FROM oxite_User WHERE Username = 'Anonymous'
END

--UserLanguages
IF NOT EXISTS(SELECT * FROM oxite_UserLanguage WHERE UserID = @User1ID AND LanguageID = @Language1ID)
	INSERT INTO oxite_UserLanguage (UserID, LanguageID) VALUES (@User1ID, @Language1ID)
IF NOT EXISTS(SELECT * FROM oxite_UserLanguage WHERE UserID = @User2ID AND LanguageID = @Language1ID)
	INSERT INTO oxite_UserLanguage (UserID, LanguageID) VALUES (@User2ID, @Language1ID)

--Roles
IF NOT EXISTS(SELECT * FROM oxite_Role WHERE RoleName = 'SiteOwner')
BEGIN
	SET @Role1ID = newid()
	INSERT INTO
		oxite_Role
	(
		ParentRoleID,
		RoleID,
		RoleName
	)
	VALUES
	(
		@Role1ID,
		@Role1ID,
		'SiteOwner'
	)
END
ELSE
BEGIN
	SELECT @Role1ID = RoleID FROM oxite_Role WHERE RoleName = 'SiteOwner'
END

IF NOT EXISTS(SELECT * FROM oxite_Role WHERE RoleName = 'AreaOwner')
BEGIN
	SET @Role2ID = newid()
	INSERT INTO
		oxite_Role
	(
		ParentRoleID,
		RoleID,
		RoleName
	)
	VALUES
	(
		@Role2ID,
		@Role2ID,
		'AreaOwner'
	)
END
ELSE
BEGIN
	SELECT @Role2ID = RoleID FROM oxite_Role WHERE RoleName = 'AreaOwner'
END

--UserRoleRelationships
IF NOT EXISTS(SELECT * FROM oxite_UserRoleRelationship WHERE UserID = @User1ID AND RoleID = @Role1ID)
	INSERT INTO
		oxite_UserRoleRelationship
	(
		UserID,
		RoleID
	)
	VALUES
	(
		@User1ID,
		@Role1ID
	)

--StringResources
SET @StringResourceKeyNewComment = 'Oxite.4F36436B07824A94BB4CFD3916734C03.Messages.NewComment'
IF NOT EXISTS(SELECT * FROM oxite_StringResource WHERE StringResourceKey = @StringResourceKeyNewComment)
	INSERT INTO
		oxite_StringResource
	(
		StringResourceKey,
		[Language],
		Version,
		StringResourceValue,
		CreatorUserID,
		[CreatedDate]
	)
	VALUES
	(
		@StringResourceKeyNewComment,
		'en',
		1,
		'<h1>New Comment on {Site.Name}</h1>
<h2>{User.Name} commented on ''{Post.Title}'' at {Post.Published}</h2>
<p>{Comment.Body}</p>
<a href="{Comment.Permalink}">{Comment.Permalink}</a>',
		@User1ID,
		getUtcDate()
	)
IF NOT EXISTS(SELECT * FROM oxite_StringResourceVersion WHERE StringResourceKey = @StringResourceKeyNewComment AND [Language] = 'en' AND Version = 1)
	INSERT INTO
		oxite_StringResourceVersion
	(
		StringResourceKey,
		[Language],
		Version,
		StringResourceValue,
		CreatorUserID,
		[CreatedDate],
		State
	)
	SELECT
		StringResourceKey,
		[Language],
		Version,
		StringResourceValue,
		CreatorUserID,
		[CreatedDate],
		1 AS State
	FROM
		oxite_StringResource
	WHERE
		StringResourceKey = @StringResourceKeyNewComment

--Areas
IF NOT EXISTS(SELECT * FROM oxite_Area WHERE SiteID = @Site1ID AND AreaName = 'Blog')
BEGIN
	SET @Area1ID = '66F2AF76-8F03-4621-8114-CAA137AFF185'
	INSERT INTO
		[oxite_Area]
	(
		SiteID,
		AreaID,
		AreaName,
		DisplayName,
		Description,
		Type,
		TypeUrl,
		CreatedDate,
		ModifiedDate
	)
	VALUES
	(
		@Site1ID,
		@Area1ID,
		'Blog',
		'',
		'',
		'Blog',
		'',
		getUtcDate(),
		getUtcDate()
	)
END
ELSE
BEGIN
	SELECT @Area1ID = [AreaID] FROM [oxite_Area] WHERE SiteID = @Site1ID AND [AreaName] = 'Blog'
END

--AreaRoleRelationships
IF NOT EXISTS(SELECT * FROM oxite_AreaRoleRelationship WHERE AreaID = @Area1ID AND RoleID = @Role1ID)
	INSERT INTO
		oxite_AreaRoleRelationship
	(
		AreaID,
		RoleID
	)
	VALUES
	(
		@Area1ID,
		@Role1ID
	)
IF NOT EXISTS(SELECT * FROM oxite_AreaRoleRelationship WHERE AreaID = @Area1ID AND RoleID = @Role2ID)
	INSERT INTO
		oxite_AreaRoleRelationship
	(
		AreaID,
		RoleID
	)
	VALUES
	(
		@Area1ID,
		@Role2ID
	)

--Posts
IF NOT EXISTS(SELECT * FROM oxite_Post WHERE Title = 'World.Hello()')
BEGIN
	SET @Post1ID = newid()
	INSERT INTO oxite_Post (PostID, CreatorUserID, Title, Body, BodyShort, State, Slug, [CreatedDate], [ModifiedDate], [PublishedDate], SearchBody) VALUES
	(
		@Post1ID,
		@User1ID,
		'World.Hello()',
		'Welcome to Oxite! &nbsp;This is a sample application targeting developers built on <a href="http://asp.net/mvc">ASP.NET MVC</a>. &nbsp;Make any changes you like. &nbsp;If you build a feature you think other developers would be interested in and would like to share your code go to the <a href="http://www.codeplex.com/oxite">Oxite Code Plex project</a> to see how you can contribute.<br /><br />To get started, sign in with "Admin" and "pa$$w0rd" and click on the Admin tab.<br /><br />For more information about <a href="http://oxite.net">Oxite</a> visit the default <a href="/About">About</a> page.',
		'Welcome to Oxite! &nbsp;This is a sample application targeting developers built on <a href="http://asp.net/mvc">ASP.NET MVC</a>. &nbsp;Make any changes you like. &nbsp;If you build a feature you think other developers would be interested in and would like to share your code go to the <a href="http://www.codeplex.com/oxite">Oxite Code Plex project</a> to see how you can contribute.<br /><br />To get started, sign in with "Admin" and "pa$$w0rd" and click on the Admin tab.<br /><br />For more information about <a href="http://oxite.net">Oxite</a> visit the default <a href="/About">About</a> page.',
		1,
		'Hello-World',
		getUtcDate(),
		getUtcDate(),
		getUtcDate(),
		''
	)
END
ELSE
BEGIN
	SELECT @Post1ID = PostID FROM oxite_Post WHERE Title = 'World.Hello()'
END

IF NOT EXISTS(SELECT * FROM oxite_Post WHERE Title = 'About')
BEGIN
	SET @Post2ID = newid()
	INSERT INTO oxite_Post (PostID, CreatorUserID, Title, Body, BodyShort, State, Slug, [CreatedDate], [ModifiedDate], [PublishedDate], SearchBody) VALUES
	(
		@Post2ID,
		@User1ID,
		'About',
		'<p>Welcome to the Oxite Sample! Since this is a sample, we thought we would use this handy about page to explain a few things about the code and about the thoughts that went into its creation.</p>
<h3>What is this?</h3>    
<p>This is a simple blog engine written using <a href="http://asp.net/mvc">ASP.NET MVC</a>, and is designed with two main goals:</p>
<ol class="normal">
	<li>To provide a sample of ''core blog functionality'' in a reusable fashion. &nbsp;Blogs are simple and well understood by many developers, but the set of basic functions that a blog needs to implement (trackbacks, rss, comments, etc.) are fairly complex.  Hopefully this code helps.</li>
	<li>To provide a real-world sample written using <a href="http://asp.net/mvc">ASP.NET MVC</a>.</li>
</ol>
<p>We aren''t a sample-building team (more on what we are in a bit). &nbsp;We couldn''t sit down and build this code base just to give out to folks, so this code is also the foundation of a real project of ours, <a href="http://visitmix.com">MIX Online</a>. &nbsp;We also created this project to be the base of our own personal blogs as well; check out <a href="http://www.codeplex.com/oxite/Wiki/View.aspx?title=oxitesites&amp;referringTitle=Home">this page on CodePlex to see a list of sites that use Oxite</a> (and use the comments area to tell us about your site).</p>
<h3>What this isn''t</h3>
<p>This is not an ''off the shelf'' blogging package. &nbsp;If you aren''t a developer and just want to get blogging then you should look at one of these great blogging products: <a href="http://graffiticms.com/">Graffiti</a>, <a href="http://subtextproject.com/">SubText</a>, <a href="http://www.dotnetblogengine.net/">Blog Engine .NET</a>, <a href="http://dasblog.info/">dasBlog</a> or a hosted service like <a href="http://wordpress.com/">Wordpress</a></p>
<p>Oxite is also not ready to be an enterprise blogging solution (for you and a thousand other bloggers at your company), although we did design it to be capable of hosting multiple blogs. &nbsp;For that type of solution, a set of provisioning tools to create new blogs would need to be added. &nbsp;Oxite is code though, so you can extend it and customize it to support whatever you need.</p>
<h3>Where to go from here (expanding on this sample)</h3>
<p>You can extend Oxite in whatever way you need or wish, but if you are looking for some ideas here are a few thoughts we''ve already had around new functionality:</p>
<ul>
	<li>Adding a rich-text-editor for post and page editing. &nbsp;We use <a href="http://writer.live.com">Windows Live Writer</a> to post and edit our blog posts, so this isn''t a real issue for our day to day use of the site, but adding an editor like <a href="http://developer.yahoo.com/yui/editor/">http://developer.yahoo.com/yui/editor/</a> would be great.</li>
	<li>Adding UI for managing the creation of new Areas, setting up users and user permissions, etc. &nbsp;If you decided to use Oxite to host many blogs together on one site, or to use the same Oxite database to run many sites (yes, it can do both of those!) then it would be nice to have some UI for managing all those contributors.</li>
	<li>And whatever great idea you have!</li>
</ul>
<h3>Getting the code, reporting bugs, and contributing to this project</h3>
<p><a href="http://codeplex.com/Oxite">Oxite is hosted on CodePlex</a>, so you can grab the latest code from there (you can <a href="http://www.codeplex.com/oxite/SourceControl/ListDownloadableCommits.aspx">see all of our check-ins</a> and also <a href="http://www.codeplex.com/oxite/Release/ProjectReleases.aspx">specific releases</a> when we feel like significant changes have been made), read discussions, file bugs and even submit suggestions for changes. &nbsp;If you''ve made some code changes that you feel should make it back into the Oxite code, then CodePlex is the place to tell us about it!</p>
<h3>About us</h3>
<p>Oxite is a project built by the team behind <a href="http://channel9.msdn.com/">Channel 9</a> (and <a href="http://channel8.msdn.com/">Channel 8</a>, <a href="http://on10.net/">Channel 10</a>, <a href="http://edge.technet.com/">TechNet Edge</a>, <a href="http://visitmix.com/">Mix Online</a>): Erik Porter, Nathan Heskew, Mike Sampson and Duncan Mackenzie. &nbsp;You can find out more about our team and our projects in our <a href="http://channel9.msdn.com/tags/evnet/">various posts and videos on Channel 9</a>.</p>
<p><a href="http://validator.w3.org/check?uri=referer"><img src="/Content/images/valid-xhtml10-blue.png" alt="Valid XHTML 1.0 Strict" height="31" width="88" /></a> <a href="http://jigsaw.w3.org/css-validator/"><img style="border:0;width:88px;height:31px" src="/Content/images/vcss-blue.gif" alt="Valid CSS!" /></a> <a href="http://validator.w3.org/feed/check.cgi"><img src="/Content/images/valid-rss.png" alt="[Valid RSS]" title="Validate my RSS feed" /></a></p>',
		'About',
		1,
		'About',
		getUtcDate(),
		getUtcDate(),
		getUtcDate(),
		''
	)
END
ELSE
BEGIN
	SELECT @Post2ID = PostID FROM oxite_Post WHERE Title = 'About'
END

--Update Post SearchBody
UPDATE
	oxite_Post
SET
	SearchBody = Title + ' ' + (SELECT DisplayName FROM oxite_User WHERE UserID = CreatorUserID) + ' ' + Body

--PostAreaRelationships
IF NOT EXISTS(SELECT * FROM [oxite_PostAreaRelationship] WHERE AreaID = @Area1ID AND PostID = @Post1ID)
	INSERT INTO
		[oxite_PostAreaRelationship]
	(
		PostID,
		AreaID
	)
	VALUES
	(
		@Post1ID,
		@Area1ID
	)

--PostRelationships
IF NOT EXISTS(SELECT * FROM oxite_PostRelationship WHERE ParentPostID = @Post2ID AND PostID = @Post2ID)
	INSERT INTO
		oxite_PostRelationship
	(
		SiteID,
		ParentPostID,
		PostID
	)
	VALUES
	(
		@Site1ID,
		@Post2ID,
		@Post2ID
	)

--Tags
IF NOT EXISTS(SELECT * FROM oxite_Tag WHERE TagName = 'Oxite')
BEGIN
	SET @Tag1ID = newid()
	INSERT INTO
		oxite_Tag
	(
		ParentTagID,
		TagID,
		TagName,
		CreatedDate
	)
	VALUES
	(
		@Tag1ID,
		@Tag1ID,
		'Oxite',
		getUtcDate()
	)
END
ELSE
BEGIN
	SELECT @Tag1ID = TagID FROM oxite_Tag WHERE TagName = 'Oxite'
END

--PostTagRelationships
IF NOT EXISTS(SELECT * FROM oxite_PostTagRelationship WHERE TagID = @Tag1ID AND PostID = @Post1ID)
	INSERT INTO
		oxite_PostTagRelationship
	(
		TagID,
		PostID
	)
	VALUES
	(
		@Tag1ID,
		@Post1ID
	)

GO

CREATE USER [OxiteSite] FOR LOGIN [OxiteSite]
GO

EXEC sp_addrolemember N'db_owner', N'OxiteSite'
GO
