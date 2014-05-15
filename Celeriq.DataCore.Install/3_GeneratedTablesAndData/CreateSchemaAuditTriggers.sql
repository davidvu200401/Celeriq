--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Audit Triggers For Version 3.0.0.0

--##SECTION BEGIN [AUDIT TRIGGERS]

--DROP ANY AUDIT TRIGGERS FOR [dbo].[ConfigurationSetting]
if exists(select * from sysobjects where name = '__TR_ConfigurationSetting__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_ConfigurationSetting__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_ConfigurationSetting__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_ConfigurationSetting__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_ConfigurationSetting__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_ConfigurationSetting__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[DimensionData]
if exists(select * from sysobjects where name = '__TR_DimensionData__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_DimensionData__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_DimensionData__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_DimensionData__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_DimensionData__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_DimensionData__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[DimensionStore]
if exists(select * from sysobjects where name = '__TR_DimensionStore__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_DimensionStore__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_DimensionStore__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_DimensionStore__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_DimensionStore__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_DimensionStore__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[RepositoryActionType]
if exists(select * from sysobjects where name = '__TR_RepositoryActionType__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryActionType__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryActionType__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryActionType__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryActionType__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryActionType__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[RepositoryData]
if exists(select * from sysobjects where name = '__TR_RepositoryData__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryData__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryData__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryData__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryData__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryData__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[RepositoryDefinition]
if exists(select * from sysobjects where name = '__TR_RepositoryDefinition__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryDefinition__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryDefinition__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryDefinition__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryDefinition__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryDefinition__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[RepositoryLog]
if exists(select * from sysobjects where name = '__TR_RepositoryLog__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryLog__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryLog__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryLog__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryLog__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryLog__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[RepositoryStat]
if exists(select * from sysobjects where name = '__TR_RepositoryStat__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryStat__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryStat__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryStat__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_RepositoryStat__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_RepositoryStat__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[ServerStat]
if exists(select * from sysobjects where name = '__TR_ServerStat__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_ServerStat__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_ServerStat__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_ServerStat__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_ServerStat__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_ServerStat__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[UserAccount]
if exists(select * from sysobjects where name = '__TR_UserAccount__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_UserAccount__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_UserAccount__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_UserAccount__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_UserAccount__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_UserAccount__DELETE]
GO

--##SECTION END [AUDIT TRIGGERS]

