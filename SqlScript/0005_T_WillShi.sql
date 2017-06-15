USE [FlowerPot]
GO

/****** Object:  Table [dbo].[Analyzer]    Script Date: 08/30/2013 14:35:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Analyzer](
	[AnalyzerId] [int] IDENTITY(1,1) NOT NULL,
	[AnalyzerName] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[SelectQuery] [nvarchar](max) NULL,
	[WhereQuery] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Analyzert] PRIMARY KEY CLUSTERED 
(
	[AnalyzerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Analyzer]  WITH CHECK ADD  CONSTRAINT [FK_Analyzer_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[Analyzer] CHECK CONSTRAINT [FK_Analyzer_User]
GO

ALTER TABLE [dbo].[Analyzer] ADD  CONSTRAINT [DF_Analyzert_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


