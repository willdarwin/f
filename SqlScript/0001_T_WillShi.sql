IF EXISTS (select * From master.dbo.sysdatabases where name='FlowerPot')
BEGIN
Drop Database[FlowerPot]
END
CREATE Database[FlowerPot]
GO
USE [FlowerPot]
GO
/****** Object:  Table [dbo].[Authority]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Authority]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Authority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsController] [bit] NOT NULL,
	[IsAllowedNoneRoles] [bit] NOT NULL,
	[ControllerName] [nvarchar](50) NOT NULL,
	[IsAllowedAllRoles] [bit] NOT NULL,
 CONSTRAINT [PK_ControllerAction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[User]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Role]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleDescription] [nvarchar](256) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[UsersInRoles]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UsersInRoles](
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AuthorityRole]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuthorityRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AuthorityRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsAllowed] [bit] NOT NULL,
	[RoleId] [int] NOT NULL,
	[ControllerActionId] [int] NOT NULL,
 CONSTRAINT [PK_ControllerActionRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[InvitationCode]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvitationCode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvitationCode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
	[Obsolete] [bit] NOT NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_InvitationCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Idea]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Idea]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Idea](
	[IdeaId] [int] IDENTITY(1,1) NOT NULL,
	[IdeaName] [nvarchar](50) NOT NULL,
	[IdeaDescription] [nvarchar](256) NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Idea_1] PRIMARY KEY CLUSTERED 
(
	[IdeaId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Analyzer]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Analyzer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Analyzer](
	[AnalyzerId] [int] IDENTITY(1,1) NOT NULL,
	[AnalyzerName] [nvarchar](50) NULL,
	[CreateTime] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Analyzer] PRIMARY KEY CLUSTERED 
(
	[AnalyzerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ColumnInIdea]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ColumnInIdea]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ColumnInIdea](
	[ColumnId] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[DataTypeId] [tinyint] NOT NULL,
	[ReferedIdeaId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IdeaId] [int] NOT NULL,
 CONSTRAINT [PK_ColumnInIdea] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[FilterInAnalyzer]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FilterInAnalyzer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FilterInAnalyzer](
	[FilterId] [int] IDENTITY(1,1) NOT NULL,
	[FilterName] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[FilterQuery] [nvarchar](max) NULL,
	[AnalyzerId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FilterInAnalyzer] PRIMARY KEY CLUSTERED 
(
	[FilterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ColumnInReference]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ColumnInReference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ColumnInReference](
	[ColumnId] [int] NOT NULL,
	[ReferedColumnId] [int] NOT NULL,
 CONSTRAINT [PK_ColumnInReference] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC,
	[ReferedColumnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ColumnInAnalyzer]    Script Date: 08/23/2013 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ColumnInAnalyzer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ColumnInAnalyzer](
	[ColumnId] [int] NOT NULL,
	[AnalyzerId] [int] NOT NULL,
	[OrderNumber] [int] NOT NULL,
 CONSTRAINT [PK_ColumnInAnalyzer] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC,
	[AnalyzerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Default [DF_Analyzer_IsDeleted]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Analyzer_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[Analyzer]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Analyzer_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Analyzer] ADD  CONSTRAINT [DF_Analyzer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_ColumnInIdea_RIdeaType]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ColumnInIdea_RIdeaType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInIdea]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ColumnInIdea_RIdeaType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ColumnInIdea] ADD  CONSTRAINT [DF_ColumnInIdea_RIdeaType]  DEFAULT ((0)) FOR [ReferedIdeaId]
END


End
GO
/****** Object:  Default [DF_ColumnInIdea_IsDeleted]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ColumnInIdea_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInIdea]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ColumnInIdea_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ColumnInIdea] ADD  CONSTRAINT [DF_ColumnInIdea_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_FilterInAnalyzer_IsDeleted]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_FilterInAnalyzer_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[FilterInAnalyzer]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FilterInAnalyzer_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FilterInAnalyzer] ADD  CONSTRAINT [DF_FilterInAnalyzer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_Idea_IsDeleted]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Idea_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[Idea]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Idea_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Idea] ADD  CONSTRAINT [DF_Idea_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_Role_IsDeleted]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Role_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[Role]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Role_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  Default [DF_User_IsDeleted]    Script Date: 08/23/2013 16:00:49 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_User_IsDeleted]') AND parent_object_id = OBJECT_ID(N'[dbo].[User]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_User_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END


End
GO
/****** Object:  ForeignKey [FK_Analyzer_User]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Analyzer_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Analyzer]'))
ALTER TABLE [dbo].[Analyzer]  WITH CHECK ADD  CONSTRAINT [FK_Analyzer_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Analyzer_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Analyzer]'))
ALTER TABLE [dbo].[Analyzer] CHECK CONSTRAINT [FK_Analyzer_User]
GO
/****** Object:  ForeignKey [FK_ControllerActionRole_ControllerAction]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ControllerActionRole_ControllerAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuthorityRole]'))
ALTER TABLE [dbo].[AuthorityRole]  WITH CHECK ADD  CONSTRAINT [FK_ControllerActionRole_ControllerAction] FOREIGN KEY([ControllerActionId])
REFERENCES [dbo].[Authority] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ControllerActionRole_ControllerAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuthorityRole]'))
ALTER TABLE [dbo].[AuthorityRole] CHECK CONSTRAINT [FK_ControllerActionRole_ControllerAction]
GO
/****** Object:  ForeignKey [FK_ControllerActionRole_Role]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ControllerActionRole_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuthorityRole]'))
ALTER TABLE [dbo].[AuthorityRole]  WITH CHECK ADD  CONSTRAINT [FK_ControllerActionRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ControllerActionRole_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[AuthorityRole]'))
ALTER TABLE [dbo].[AuthorityRole] CHECK CONSTRAINT [FK_ControllerActionRole_Role]
GO
/****** Object:  ForeignKey [FK_ColumnInAnalyzer_Analyzer]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInAnalyzer_Analyzer]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInAnalyzer]'))
ALTER TABLE [dbo].[ColumnInAnalyzer]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInAnalyzer_Analyzer] FOREIGN KEY([AnalyzerId])
REFERENCES [dbo].[Analyzer] ([AnalyzerId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInAnalyzer_Analyzer]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInAnalyzer]'))
ALTER TABLE [dbo].[ColumnInAnalyzer] CHECK CONSTRAINT [FK_ColumnInAnalyzer_Analyzer]
GO
/****** Object:  ForeignKey [FK_ColumnInAnalyzer_ColumnInIdea]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInAnalyzer_ColumnInIdea]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInAnalyzer]'))
ALTER TABLE [dbo].[ColumnInAnalyzer]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInAnalyzer_ColumnInIdea] FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInAnalyzer_ColumnInIdea]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInAnalyzer]'))
ALTER TABLE [dbo].[ColumnInAnalyzer] CHECK CONSTRAINT [FK_ColumnInAnalyzer_ColumnInIdea]
GO
/****** Object:  ForeignKey [FK_ColumnInIdea_Idea]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInIdea_Idea]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInIdea]'))
ALTER TABLE [dbo].[ColumnInIdea]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInIdea_Idea] FOREIGN KEY([IdeaId])
REFERENCES [dbo].[Idea] ([IdeaId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInIdea_Idea]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInIdea]'))
ALTER TABLE [dbo].[ColumnInIdea] CHECK CONSTRAINT [FK_ColumnInIdea_Idea]
GO
/****** Object:  ForeignKey [FK_ColumnInReference_ColumnInIdea]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInReference_ColumnInIdea]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInReference]'))
ALTER TABLE [dbo].[ColumnInReference]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInReference_ColumnInIdea] FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInReference_ColumnInIdea]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInReference]'))
ALTER TABLE [dbo].[ColumnInReference] CHECK CONSTRAINT [FK_ColumnInReference_ColumnInIdea]
GO
/****** Object:  ForeignKey [FK_ColumnInReference_ColumnInIdea1]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInReference_ColumnInIdea1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInReference]'))
ALTER TABLE [dbo].[ColumnInReference]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInReference_ColumnInIdea1] FOREIGN KEY([ReferedColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ColumnInReference_ColumnInIdea1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ColumnInReference]'))
ALTER TABLE [dbo].[ColumnInReference] CHECK CONSTRAINT [FK_ColumnInReference_ColumnInIdea1]
GO
/****** Object:  ForeignKey [FK_FilterInAnalyzer_Analyzer]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FilterInAnalyzer_Analyzer]') AND parent_object_id = OBJECT_ID(N'[dbo].[FilterInAnalyzer]'))
ALTER TABLE [dbo].[FilterInAnalyzer]  WITH CHECK ADD  CONSTRAINT [FK_FilterInAnalyzer_Analyzer] FOREIGN KEY([AnalyzerId])
REFERENCES [dbo].[Analyzer] ([AnalyzerId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FilterInAnalyzer_Analyzer]') AND parent_object_id = OBJECT_ID(N'[dbo].[FilterInAnalyzer]'))
ALTER TABLE [dbo].[FilterInAnalyzer] CHECK CONSTRAINT [FK_FilterInAnalyzer_Analyzer]
GO
/****** Object:  ForeignKey [FK_Idea_User]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Idea_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Idea]'))
ALTER TABLE [dbo].[Idea]  WITH CHECK ADD  CONSTRAINT [FK_Idea_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Idea_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Idea]'))
ALTER TABLE [dbo].[Idea] CHECK CONSTRAINT [FK_Idea_User]
GO
/****** Object:  ForeignKey [FK_InvitationCode_User]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InvitationCode_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvitationCode]'))
ALTER TABLE [dbo].[InvitationCode]  WITH CHECK ADD  CONSTRAINT [FK_InvitationCode_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InvitationCode_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvitationCode]'))
ALTER TABLE [dbo].[InvitationCode] CHECK CONSTRAINT [FK_InvitationCode_User]
GO
/****** Object:  ForeignKey [FK_UsersInRoles_Role]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersInRoles_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersInRoles]'))
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersInRoles_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersInRoles_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersInRoles]'))
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [FK_UsersInRoles_Role]
GO
/****** Object:  ForeignKey [FK_UsersInRoles_User]    Script Date: 08/23/2013 16:00:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersInRoles_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersInRoles]'))
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersInRoles_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersInRoles_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersInRoles]'))
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [FK_UsersInRoles_User]
GO
