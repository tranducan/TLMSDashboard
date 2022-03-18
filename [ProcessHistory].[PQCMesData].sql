USE [ERPSOFT]
GO

CREATE TABLE [ProcessHistory].[PQCMesData](
	[PQCMesDataId] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[POCode] [nvarchar](200) NOT NULL,
	[LotNumber] [nvarchar](100) NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[Site] [nvarchar](20) NOT NULL,
	[Line] [nvarchar](20) NOT NULL,
	[Process] [nvarchar](10) NOT NULL,
	[Attribute] [nvarchar](20) NOT NULL,
	[AttributeType] [nvarchar](20) NOT NULL,
	[Quantity] [decimal](6, 0) NOT NULL,
	[Flag] [nvarchar](10) NULL,
	[Inspector] [nvarchar](20) NOT NULL,
	[InspectDateTime] [datetime2](7) NOT NULL,
	[LastTimeModified] [datetime2](7) NOT NULL,
	[LastModifiedUser] [varchar](50) NOT NULL,
	[VersionNumber] [timestamp] NOT NULL,
 CONSTRAINT [PK_ocessHistory__PQCMesData_PQCMesDataId] PRIMARY KEY CLUSTERED 
(
	[PQCMesDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ProcessHistory].[PQCMesData] ADD  CONSTRAINT [DF__ProcessHistory__PQCMesData_InspectDateTime]  DEFAULT (getdate()) FOR [InspectDateTime]
GO

ALTER TABLE [ProcessHistory].[PQCMesData] ADD  CONSTRAINT [DF__ProcessHistory__PQCMesData_LastTimeModified]  DEFAULT (getdate()) FOR [LastTimeModified]
GO

ALTER TABLE [ProcessHistory].[PQCMesData] ADD  CONSTRAINT [DF__ProcessHistory__PQCMesData__LastModifiedUser]  DEFAULT (suser_sname()) FOR [LastModifiedUser]
GO


