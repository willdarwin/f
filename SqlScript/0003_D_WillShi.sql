USE [FlowerPot]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[User]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([UserId], [UserName], [Email], [Password], [IsDeleted]) VALUES (1, N'admin', N'admin@volvo.com', N'HJKL:"', 0)
SET IDENTITY_INSERT [dbo].[User] OFF
/****** Object:  Table [dbo].[Authority]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[UsersInRoles]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[AuthorityRole]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[Analyzer]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[InvitationCode]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[Idea]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[Idea] ON
INSERT [dbo].[Idea] ([IdeaId], [IdeaName], [IdeaDescription], [CreateTime], [IsDeleted], [UserId]) VALUES (1, N'费用类型', N'呵呵', CAST(0x0000A22700E1520A AS DateTime), 0, 1)
INSERT [dbo].[Idea] ([IdeaId], [IdeaName], [IdeaDescription], [CreateTime], [IsDeleted], [UserId]) VALUES (2, N'每日费用明细', N'呵呵', CAST(0x0000A22700E22DBA AS DateTime), 0, 1)
SET IDENTITY_INSERT [dbo].[Idea] OFF
/****** Object:  Table [dbo].[ColumnInIdea]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[ColumnInIdea] ON
INSERT [dbo].[ColumnInIdea] ([ColumnId], [ColumnName], [DataTypeId], [ReferedIdeaId], [CreateTime], [IsDeleted], [IdeaId]) VALUES (1, N'费用类型', 4, 0, CAST(0x0000A22700E1520A AS DateTime), 0, 1)
INSERT [dbo].[ColumnInIdea] ([ColumnId], [ColumnName], [DataTypeId], [ReferedIdeaId], [CreateTime], [IsDeleted], [IdeaId]) VALUES (2, N'日期', 2, 0, CAST(0x0000A22700E22DBA AS DateTime), 0, 2)
INSERT [dbo].[ColumnInIdea] ([ColumnId], [ColumnName], [DataTypeId], [ReferedIdeaId], [CreateTime], [IsDeleted], [IdeaId]) VALUES (3, N'费用类型', 5, 1, CAST(0x0000A22700E22DBA AS DateTime), 0, 2)
INSERT [dbo].[ColumnInIdea] ([ColumnId], [ColumnName], [DataTypeId], [ReferedIdeaId], [CreateTime], [IsDeleted], [IdeaId]) VALUES (4, N'金额', 0, 0, CAST(0x0000A22700E22DBA AS DateTime), 0, 2)
SET IDENTITY_INSERT [dbo].[ColumnInIdea] OFF
/****** Object:  Table [dbo].[FilterInAnalyzer]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[U1_Row]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[U1_Row] ON
INSERT [dbo].[U1_Row] ([RowId], [IdeaId], [Version], [IsDeleted]) VALUES (1, 1, 0, 0)
INSERT [dbo].[U1_Row] ([RowId], [IdeaId], [Version], [IsDeleted]) VALUES (2, 1, 0, 0)
INSERT [dbo].[U1_Row] ([RowId], [IdeaId], [Version], [IsDeleted]) VALUES (3, 2, 0, 0)
SET IDENTITY_INSERT [dbo].[U1_Row] OFF
/****** Object:  Table [dbo].[U1_ShortText]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[U1_ShortText] ON
INSERT [dbo].[U1_ShortText] ([ShortTextId], [Value], [ColumnId], [RowId], [IsDeleted]) VALUES (1, N'收入', 1, 1, 0)
INSERT [dbo].[U1_ShortText] ([ShortTextId], [Value], [ColumnId], [RowId], [IsDeleted]) VALUES (2, N'支出', 1, 2, 0)
SET IDENTITY_INSERT [dbo].[U1_ShortText] OFF
/****** Object:  Table [dbo].[U1_Number]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[U1_Money]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[U1_Money] ON
INSERT [dbo].[U1_Money] ([MoneyId], [Value], [ColumnId], [RowId], [IsDeleted]) VALUES (1, CAST(10.00 AS Decimal(18, 2)), 4, 3, 0)
SET IDENTITY_INSERT [dbo].[U1_Money] OFF
/****** Object:  Table [dbo].[U1_LongText]    Script Date: 08/27/2013 13:53:15 ******/
/****** Object:  Table [dbo].[U1_Datetime]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[U1_Datetime] ON
INSERT [dbo].[U1_Datetime] ([DatetimeId], [Value], [ColumnId], [RowId], [IsDeleted]) VALUES (1, CAST(0x0000A17600000000 AS DateTime), 2, 3, 0)
SET IDENTITY_INSERT [dbo].[U1_Datetime] OFF
/****** Object:  Table [dbo].[U1_ComplexType]    Script Date: 08/27/2013 13:53:15 ******/
SET IDENTITY_INSERT [dbo].[U1_ComplexType] ON
INSERT [dbo].[U1_ComplexType] ([ComplexTypeId], [RefRowId], [ColumnId], [RowId], [IsDeleted]) VALUES (1, 1, 3, 3, 0)
SET IDENTITY_INSERT [dbo].[U1_ComplexType] OFF
/****** Object:  Table [dbo].[ColumnInReference]    Script Date: 08/27/2013 13:53:15 ******/
INSERT [dbo].[ColumnInReference] ([ColumnId], [ReferedColumnId]) VALUES (3, 1)
/****** Object:  Table [dbo].[ColumnInAnalyzer]    Script Date: 08/27/2013 13:53:15 ******/
