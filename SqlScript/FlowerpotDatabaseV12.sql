USE [FlowerPot]
GO
/****** Object:  Table [dbo].[User]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Role]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleDesc] [nvarchar](256) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersInRoles]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInRoles](
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Analyzer]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Idea]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Idea](
	[IdeaId] [int] IDENTITY(1,1) NOT NULL,
	[IdeaName] [nvarchar](50) NOT NULL,
	[IdeaDesc] [nvarchar](256) NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Idea_1] PRIMARY KEY CLUSTERED 
(
	[IdeaId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FilterInAnalyzer]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FilterInAnalyzer](
	[FilterId] [int] NOT NULL,
	[FilterName] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[FilterQuery] [nvarchar](max) NULL,
	[AnalyzerId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ColumnInIdea]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ColumnInIdea](
	[ColumnId] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[DataTypeId] [tinyint] NOT NULL,
	[RIdeaType] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IdeaId] [int] NOT NULL,
 CONSTRAINT [PK_ColumnInIdea] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ColumnInAnalyzer]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ColumnInAnalyzer](
	[ColumnId] [int] NOT NULL,
	[AnalyzerId] [int] NOT NULL,
 CONSTRAINT [PK_ColumnInAnalyzer] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC,
	[AnalyzerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ColumnInReference]    Script Date: 06/03/2013 11:18:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ColumnInReference](
	[ColumnId] [int] NOT NULL,
	[RefColumnId] [int] NOT NULL,
 CONSTRAINT [PK_ColumnInReference] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC,
	[RefColumnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Analyzer_IsDeleted]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[Analyzer] ADD  CONSTRAINT [DF_Analyzer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_ColumnInIdea_RIdeaType]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInIdea] ADD  CONSTRAINT [DF_ColumnInIdea_RIdeaType]  DEFAULT ((0)) FOR [RIdeaType]
GO
/****** Object:  Default [DF_ColumnInIdea_IsDeleted]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInIdea] ADD  CONSTRAINT [DF_ColumnInIdea_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_FilterInAnalyzer_IsDeleted]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[FilterInAnalyzer] ADD  CONSTRAINT [DF_FilterInAnalyzer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_Idea_IsDeleted]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[Idea] ADD  CONSTRAINT [DF_Idea_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_Role_IsDeleted]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_User_IsDeleted]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  ForeignKey [FK_Analyzer_User]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[Analyzer]  WITH CHECK ADD  CONSTRAINT [FK_Analyzer_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Analyzer] CHECK CONSTRAINT [FK_Analyzer_User]
GO
/****** Object:  ForeignKey [FK_ColumnInAnalyzer_Analyzer]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInAnalyzer]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInAnalyzer_Analyzer] FOREIGN KEY([AnalyzerId])
REFERENCES [dbo].[Analyzer] ([AnalyzerId])
GO
ALTER TABLE [dbo].[ColumnInAnalyzer] CHECK CONSTRAINT [FK_ColumnInAnalyzer_Analyzer]
GO
/****** Object:  ForeignKey [FK_ColumnInAnalyzer_ColumnInIdea]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInAnalyzer]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInAnalyzer_ColumnInIdea] FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
ALTER TABLE [dbo].[ColumnInAnalyzer] CHECK CONSTRAINT [FK_ColumnInAnalyzer_ColumnInIdea]
GO
/****** Object:  ForeignKey [FK_ColumnInIdea_Idea]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInIdea]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInIdea_Idea] FOREIGN KEY([IdeaId])
REFERENCES [dbo].[Idea] ([IdeaId])
GO
ALTER TABLE [dbo].[ColumnInIdea] CHECK CONSTRAINT [FK_ColumnInIdea_Idea]
GO
/****** Object:  ForeignKey [FK_ColumnInReference_ColumnInIdea]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInReference]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInReference_ColumnInIdea] FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
ALTER TABLE [dbo].[ColumnInReference] CHECK CONSTRAINT [FK_ColumnInReference_ColumnInIdea]
GO
/****** Object:  ForeignKey [FK_ColumnInReference_ColumnInIdea1]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[ColumnInReference]  WITH CHECK ADD  CONSTRAINT [FK_ColumnInReference_ColumnInIdea1] FOREIGN KEY([RefColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
ALTER TABLE [dbo].[ColumnInReference] CHECK CONSTRAINT [FK_ColumnInReference_ColumnInIdea1]
GO
/****** Object:  ForeignKey [FK_FilterInAnalyzer_Analyzer]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[FilterInAnalyzer]  WITH CHECK ADD  CONSTRAINT [FK_FilterInAnalyzer_Analyzer] FOREIGN KEY([AnalyzerId])
REFERENCES [dbo].[Analyzer] ([AnalyzerId])
GO
ALTER TABLE [dbo].[FilterInAnalyzer] CHECK CONSTRAINT [FK_FilterInAnalyzer_Analyzer]
GO
/****** Object:  ForeignKey [FK_Idea_User]    Script Date: 06/03/2013 11:18:55 ******/
ALTER TABLE [dbo].[Idea]  WITH CHECK ADD  CONSTRAINT [FK_Idea_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Idea] CHECK CONSTRAINT [FK_Idea_User]
GO
/****** Object:  ForeignKey [FK_UsersInRoles_Role]    Script Date: 06/03/2013 11:18:56 ******/
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersInRoles_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [FK_UsersInRoles_Role]
GO
/****** Object:  ForeignKey [FK_UsersInRoles_User]    Script Date: 06/03/2013 11:18:56 ******/
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersInRoles_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [FK_UsersInRoles_User]
GO
