USE [ga]
GO
/****** Object:  Table [dbo].[basecollectionitem]    Script Date: 11/10/2015 10:45:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[basecollectionitem](
	[TableId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NULL,
	[ItemProcessed] [bit] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_basecollectionitem] PRIMARY KEY CLUSTERED 
(
	[TableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[collectionitemqueue]    Script Date: 11/10/2015 10:45:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[collectionitemqueue](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemTitle] [varchar](128) NULL,
	[ItemUrl] [varchar](256) NULL,
	[ItemDescription] [varchar](256) NULL,
	[ItemTags] [varchar](256) NULL,
	[ItemProcessed] [bit] NULL,
 CONSTRAINT [PK_collectionitemqueue] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[collectionitemqueue] ON 

INSERT [dbo].[collectionitemqueue] ([ItemID], [ItemTitle], [ItemUrl], [ItemDescription], [ItemTags], [ItemProcessed]) VALUES (1, N'RethinkDB', N'http://rethinkdb.com/', N'A really cool nosql db which updates its clients in realtime.', N'database,nosql, open source', 0)
INSERT [dbo].[collectionitemqueue] ([ItemID], [ItemTitle], [ItemUrl], [ItemDescription], [ItemTags], [ItemProcessed]) VALUES (2, N'MongoDB', N'http://www.mongodb.com', N'Another nosql database that stores data as JSON', N'database, nosql, open source', 0)
INSERT [dbo].[collectionitemqueue] ([ItemID], [ItemTitle], [ItemUrl], [ItemDescription], [ItemTags], [ItemProcessed]) VALUES (3, N'SQL Datareader Read', N'https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader.read%28v=vs.110%29.aspx', N'Comprehensive tutorial on SQL Data Read', N'.net,sql,ado', 0)
SET IDENTITY_INSERT [dbo].[collectionitemqueue] OFF
