USE [FlowerPot]
GO
/****** Object:  Table [dbo].[U1_Row]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_Row](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[IdeaId] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[U1_ShortText]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_ShortText](
	[ShortTextId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](500) NULL,
	[ColumnId] [int] NOT NULL,
	[RowId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ShortTextId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[U1_Number]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_Number](
	[NumberId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [decimal](18, 2) NULL,
	[ColumnId] [int] NOT NULL,
	[RowId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NumberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[U1_Money]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_Money](
	[MoneyId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [decimal](18, 2) NULL,
	[ColumnId] [int] NOT NULL,
	[RowId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MoneyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[U1_LongText]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_LongText](
	[LongTextId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[ColumnId] [int] NOT NULL,
	[RowId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LongTextId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[U1_Datetime]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_Datetime](
	[DatetimeId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [datetime] NULL,
	[ColumnId] [int] NOT NULL,
	[RowId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DatetimeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[U1_ComplexType]    Script Date: 08/27/2013 13:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[U1_ComplexType](
	[ComplexTypeId] [int] IDENTITY(1,1) NOT NULL,
	[RefRowId] [int] NULL,
	[ColumnId] [int] NOT NULL,
	[RowId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ComplexTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK__U1_Comple__Colum__4CA06362]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_ComplexType]  WITH CHECK ADD FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
/****** Object:  ForeignKey [FK__U1_Comple__RowId__4D94879B]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_ComplexType]  WITH CHECK ADD FOREIGN KEY([RowId])
REFERENCES [dbo].[U1_Row] ([RowId])
GO
/****** Object:  ForeignKey [FK__U1_Dateti__Colum__3B75D760]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Datetime]  WITH CHECK ADD FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
/****** Object:  ForeignKey [FK__U1_Dateti__RowId__3C69FB99]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Datetime]  WITH CHECK ADD FOREIGN KEY([RowId])
REFERENCES [dbo].[U1_Row] ([RowId])
GO
/****** Object:  ForeignKey [FK__U1_LongTe__Colum__46E78A0C]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_LongText]  WITH CHECK ADD FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
/****** Object:  ForeignKey [FK__U1_LongTe__RowId__47DBAE45]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_LongText]  WITH CHECK ADD FOREIGN KEY([RowId])
REFERENCES [dbo].[U1_Row] ([RowId])
GO
/****** Object:  ForeignKey [FK__U1_Money__Column__300424B4]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Money]  WITH CHECK ADD FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
/****** Object:  ForeignKey [FK__U1_Money__RowId__30F848ED]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Money]  WITH CHECK ADD FOREIGN KEY([RowId])
REFERENCES [dbo].[U1_Row] ([RowId])
GO
/****** Object:  ForeignKey [FK__U1_Number__Colum__35BCFE0A]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Number]  WITH CHECK ADD FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
/****** Object:  ForeignKey [FK__U1_Number__RowId__36B12243]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Number]  WITH CHECK ADD FOREIGN KEY([RowId])
REFERENCES [dbo].[U1_Row] ([RowId])
GO
/****** Object:  ForeignKey [FK__U1_Row__IdeaId__2B3F6F97]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_Row]  WITH CHECK ADD FOREIGN KEY([IdeaId])
REFERENCES [dbo].[Idea] ([IdeaId])
GO
/****** Object:  ForeignKey [FK__U1_ShortT__Colum__412EB0B6]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_ShortText]  WITH CHECK ADD FOREIGN KEY([ColumnId])
REFERENCES [dbo].[ColumnInIdea] ([ColumnId])
GO
/****** Object:  ForeignKey [FK__U1_ShortT__RowId__4222D4EF]    Script Date: 08/27/2013 13:56:57 ******/
ALTER TABLE [dbo].[U1_ShortText]  WITH CHECK ADD FOREIGN KEY([RowId])
REFERENCES [dbo].[U1_Row] ([RowId])
GO
