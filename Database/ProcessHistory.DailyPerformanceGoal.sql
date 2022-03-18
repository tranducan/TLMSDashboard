USE [ERPSOFT]



CREATE TABLE [ProcessHistory].[DailyPerformanceGoal](
	[DailyPerformanceGoaId] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[Site] [nvarchar](20) NULL,
	[Line] [nvarchar](20) NULL,
	[Process] [nvarchar](10) NOT NULL,
	[OutputTarget] [decimal](6, 4) NOT NULL,
	[NotGoodTarget] [decimal](6, 4) NOT NULL,
	[ReworkTarget] [decimal](6, 4) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7)  NULL,
	[LastTimeModified] [datetime2](7) NOT NULL,
	[LastModifiedUser] [varchar](50) NOT NULL,
	[VersionNumber] [timestamp] NOT NULL
) ON [PRIMARY]


ALTER TABLE [ProcessHistory].[DailyPerformanceGoal] ADD  CONSTRAINT [DF__ProcessHistory__DailyPerformanceGoal_StartDate]  DEFAULT (getdate()) FOR [StartDate]


ALTER TABLE [ProcessHistory].[DailyPerformanceGoal] ADD  CONSTRAINT [DF__ProcessHistory__DailyPerformanceGoal_LastTimeModified]  DEFAULT (getdate()) FOR [LastTimeModified]


ALTER TABLE [ProcessHistory].[DailyPerformanceGoal] ADD  CONSTRAINT [DF__ProcessHistory__DailyPerformanceGoal__LastModifiedUser]  DEFAULT (suser_sname()) FOR [LastModifiedUser]


