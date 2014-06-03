--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Data Schema For Version 3.0.0.0

--CREATE TABLE [ConfigurationSetting]
if not exists(select * from sysobjects where name = 'ConfigurationSetting' and xtype = 'U')
CREATE TABLE [dbo].[ConfigurationSetting] (
	[ID] [Int] IDENTITY (1, 1) NOT NULL ,
	[Name] [VarChar] (100) NOT NULL ,
	[Value] [VarChar] (max) NOT NULL ,
	[ModifiedBy] [Varchar] (50) NULL,
	[ModifiedDate] [DateTime] CONSTRAINT [DF__CONFIGURATIONSETTING_MODIFIEDDATE] DEFAULT getdate() NULL,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__CONFIGURATIONSETTING_CREATEDDATE] DEFAULT getdate() NULL,
	[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [DimensionData]
if not exists(select * from sysobjects where name = 'DimensionData' and xtype = 'U')
CREATE TABLE [dbo].[DimensionData] (
	[DimensionDataId] [BigInt] IDENTITY (1, 1) NOT NULL ,
	[Data] [VarBinary] (max) NOT NULL ,
	[DimensionsionStoreId] [BigInt] NOT NULL ,
	[ModifiedBy] [Varchar] (50) NULL,
	[ModifiedDate] [DateTime] CONSTRAINT [DF__DIMENSIONDATA_MODIFIEDDATE] DEFAULT getdate() NULL,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__DIMENSIONDATA_CREATEDDATE] DEFAULT getdate() NULL,
	[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [DimensionStore]
if not exists(select * from sysobjects where name = 'DimensionStore' and xtype = 'U')
CREATE TABLE [dbo].[DimensionStore] (
	[DimensionStoreId] [BigInt] IDENTITY (1, 1) NOT NULL ,
	[RepositoryId] [Int] NOT NULL ,
	[DIdx] [BigInt] NOT NULL ,
	[Name] [VarChar] (50) NOT NULL ,
	[ModifiedBy] [Varchar] (50) NULL,
	[ModifiedDate] [DateTime] CONSTRAINT [DF__DIMENSIONSTORE_MODIFIEDDATE] DEFAULT getdate() NULL,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__DIMENSIONSTORE_CREATEDDATE] DEFAULT getdate() NULL,
	[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [RepositoryActionType]
if not exists(select * from sysobjects where name = 'RepositoryActionType' and xtype = 'U')
CREATE TABLE [dbo].[RepositoryActionType] (
	[RepositoryActionTypeId] [Int] IDENTITY (1, 1) NOT NULL ,
	[Name] [VarChar] (50) NOT NULL )

GO

--CREATE TABLE [RepositoryData]
if not exists(select * from sysobjects where name = 'RepositoryData' and xtype = 'U')
CREATE TABLE [dbo].[RepositoryData] (
	[RepositoryDataId] [BigInt] IDENTITY (1, 1) NOT NULL ,
	[RepositoryId] [Int] NOT NULL ,
	[Data] [VarBinary] (max) NOT NULL ,
	[Keyword] [VarChar] (max) NULL ,
	[ModifiedBy] [Varchar] (50) NULL,
	[ModifiedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDATA_MODIFIEDDATE] DEFAULT getdate() NULL,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDATA_CREATEDDATE] DEFAULT getdate() NULL,
	[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [RepositoryDefinition]
if not exists(select * from sysobjects where name = 'RepositoryDefinition' and xtype = 'U')
CREATE TABLE [dbo].[RepositoryDefinition] (
	[RepositoryId] [Int] IDENTITY (1, 1) NOT NULL ,
	[ItemCount] [Int] NOT NULL ,
	[UniqueKey] [UniqueIdentifier] NOT NULL ,
	[MemorySize] [BigInt] NOT NULL ,
	[Name] [VarChar] (50) NOT NULL ,
	[DefinitionData] [VarBinary] (max) NOT NULL ,
	[VersionHash] [BigInt] NOT NULL ,
	[ModifiedBy] [Varchar] (50) NULL,
	[ModifiedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDEFINITION_MODIFIEDDATE] DEFAULT getdate() NULL,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDEFINITION_CREATEDDATE] DEFAULT getdate() NULL,
	[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [RepositoryLog]
if not exists(select * from sysobjects where name = 'RepositoryLog' and xtype = 'U')
CREATE TABLE [dbo].[RepositoryLog] (
	[RepositoryLogId] [Int] IDENTITY (1, 1) NOT NULL ,
	[IPAddress] [VarChar] (50) NOT NULL ,
	[RepositoryId] [Int] NOT NULL ,
	[Count] [Int] NOT NULL ,
	[ElapsedTime] [Int] NOT NULL ,
	[UsedCache] [Bit] NOT NULL ,
	[QueryId] [UniqueIdentifier] NOT NULL ,
	[Query] [VarChar] (max) NULL ,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYLOG_CREATEDDATE] DEFAULT getdate() NULL)

GO

--CREATE TABLE [RepositoryStat]
if not exists(select * from sysobjects where name = 'RepositoryStat' and xtype = 'U')
CREATE TABLE [dbo].[RepositoryStat] (
	[RepositoryStatId] [Int] IDENTITY (1, 1) NOT NULL ,
	[RepositoryId] [Int] NOT NULL ,
	[Elapsed] [Int] NOT NULL ,
	[RepositoryActionTypeId] [Int] NOT NULL ,
	[Count] [Int] NOT NULL ,
	[ItemCount] [Int] NOT NULL ,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYSTAT_CREATEDDATE] DEFAULT getdate() NULL)

GO

--CREATE TABLE [ServerStat]
if not exists(select * from sysobjects where name = 'ServerStat' and xtype = 'U')
CREATE TABLE [dbo].[ServerStat] (
	[ServerStatId] [Int] IDENTITY (1, 1) NOT NULL ,
	[MemoryUsageTotal] [BigInt] NOT NULL ,
	[MemoryUsageAvailable] [BigInt] NOT NULL ,
	[RepositoryInMem] [Int] NOT NULL ,
	[RepositoryLoadDelta] [Int] NOT NULL ,
	[RepositoryUnloadDelta] [Int] NOT NULL ,
	[RepositoryTotal] [Int] NOT NULL ,
	[RepositoryCreateDelta] [BigInt] NOT NULL ,
	[RepositoryDeleteDelta] [BigInt] NOT NULL ,
	[ProcessorUsage] [Int] NOT NULL ,
	[AddedDate] [DateTime] NOT NULL CONSTRAINT [DF__SERVERSTAT_ADDEDDATE] DEFAULT (getdate()),
	[MemoryUsageProcess] [BigInt] NOT NULL )

GO

--CREATE TABLE [UserAccount]
if not exists(select * from sysobjects where name = 'UserAccount' and xtype = 'U')
CREATE TABLE [dbo].[UserAccount] (
	[UserId] [Int] IDENTITY (1, 1) NOT NULL ,
	[UniqueKey] [UniqueIdentifier] NOT NULL ,
	[UserName] [VarChar] (50) NOT NULL ,
	[Password] [VarChar] (50) NOT NULL ,
	[ModifiedBy] [Varchar] (50) NULL,
	[ModifiedDate] [DateTime] CONSTRAINT [DF__USERACCOUNT_MODIFIEDDATE] DEFAULT getdate() NULL,
	[CreatedBy] [Varchar] (50) NULL,
	[CreatedDate] [DateTime] CONSTRAINT [DF__USERACCOUNT_CREATEDDATE] DEFAULT getdate() NULL,
	[Timestamp] [timestamp] NOT NULL
)

GO

--##SECTION BEGIN [FIELD CREATE]
--TABLE [ConfigurationSetting] ADD FIELDS
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ID' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [ID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Name' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [Name] [VarChar] (100) NOT NULL 
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Value' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [Value] [VarChar] (max) NOT NULL 
GO
--TABLE [DimensionData] ADD FIELDS
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'DimensionDataId' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [DimensionDataId] [BigInt] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Data' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [Data] [VarBinary] (max) NOT NULL 
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'DimensionsionStoreId' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [DimensionsionStoreId] [BigInt] NOT NULL 
GO
--TABLE [DimensionStore] ADD FIELDS
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'DimensionStoreId' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [DimensionStoreId] [BigInt] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryId' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [RepositoryId] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'DIdx' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [DIdx] [BigInt] NOT NULL 
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Name' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [Name] [VarChar] (50) NOT NULL 
GO
--TABLE [RepositoryActionType] ADD FIELDS
if exists(select * from sys.objects where name = 'RepositoryActionType' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryActionTypeId' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] ADD [RepositoryActionTypeId] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryActionType' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Name' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] ADD [Name] [VarChar] (50) NOT NULL 
GO
--TABLE [RepositoryData] ADD FIELDS
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryDataId' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [RepositoryDataId] [BigInt] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryId' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [RepositoryId] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Data' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [Data] [VarBinary] (max) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Keyword' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [Keyword] [VarChar] (max) NULL 
GO
--TABLE [RepositoryDefinition] ADD FIELDS
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryId' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [RepositoryId] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ItemCount' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [ItemCount] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UniqueKey' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [UniqueKey] [UniqueIdentifier] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'MemorySize' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [MemorySize] [BigInt] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Name' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [Name] [VarChar] (50) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'DefinitionData' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [DefinitionData] [VarBinary] (max) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'VersionHash' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [VersionHash] [BigInt] NOT NULL 
GO
--TABLE [RepositoryLog] ADD FIELDS
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryLogId' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [RepositoryLogId] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'IPAddress' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [IPAddress] [VarChar] (50) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryId' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [RepositoryId] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Count' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [Count] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ElapsedTime' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [ElapsedTime] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UsedCache' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [UsedCache] [Bit] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'QueryId' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [QueryId] [UniqueIdentifier] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Query' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [Query] [VarChar] (max) NULL 
GO
--TABLE [RepositoryStat] ADD FIELDS
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryStatId' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [RepositoryStatId] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryId' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [RepositoryId] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Elapsed' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [Elapsed] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryActionTypeId' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [RepositoryActionTypeId] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Count' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [Count] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ItemCount' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [ItemCount] [Int] NOT NULL 
GO
--TABLE [ServerStat] ADD FIELDS
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ServerStatId' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [ServerStatId] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'MemoryUsageTotal' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [MemoryUsageTotal] [BigInt] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'MemoryUsageAvailable' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [MemoryUsageAvailable] [BigInt] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryInMem' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [RepositoryInMem] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryLoadDelta' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [RepositoryLoadDelta] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryUnloadDelta' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [RepositoryUnloadDelta] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryTotal' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [RepositoryTotal] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryCreateDelta' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [RepositoryCreateDelta] [BigInt] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RepositoryDeleteDelta' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [RepositoryDeleteDelta] [BigInt] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ProcessorUsage' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [ProcessorUsage] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'AddedDate' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [AddedDate] [DateTime] NOT NULL CONSTRAINT [DF__SERVERSTAT_ADDEDDATE] DEFAULT (getdate())
if exists(select * from sys.objects where name = 'ServerStat' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'MemoryUsageProcess' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] ADD [MemoryUsageProcess] [BigInt] NOT NULL 
GO
--TABLE [UserAccount] ADD FIELDS
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UserId' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [UserId] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UniqueKey' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [UniqueKey] [UniqueIdentifier] NOT NULL 
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UserName' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [UserName] [VarChar] (50) NOT NULL 
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Password' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [Password] [VarChar] (50) NOT NULL 
GO
--##SECTION END [FIELD CREATE]

--##SECTION BEGIN [AUDIT TRAIL CREATE]

--APPEND AUDIT TRAIL CREATE FOR TABLE [ConfigurationSetting]
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CONFIGURATIONSETTING_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [ConfigurationSetting]
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CONFIGURATIONSETTING_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [ConfigurationSetting]
if exists(select * from sys.objects where name = 'ConfigurationSetting' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'ConfigurationSetting')
ALTER TABLE [dbo].[ConfigurationSetting] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [DimensionData]
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__DIMENSIONDATA_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [DimensionData]
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__DIMENSIONDATA_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [DimensionData]
if exists(select * from sys.objects where name = 'DimensionData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'DimensionData')
ALTER TABLE [dbo].[DimensionData] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [DimensionStore]
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__DIMENSIONSTORE_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [DimensionStore]
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__DIMENSIONSTORE_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [DimensionStore]
if exists(select * from sys.objects where name = 'DimensionStore' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'DimensionStore')
ALTER TABLE [dbo].[DimensionStore] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [RepositoryData]
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDATA_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [RepositoryData]
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDATA_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [RepositoryData]
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [RepositoryDefinition]
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDEFINITION_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [RepositoryDefinition]
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__REPOSITORYDEFINITION_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [RepositoryDefinition]
if exists(select * from sys.objects where name = 'RepositoryDefinition' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'RepositoryDefinition')
ALTER TABLE [dbo].[RepositoryDefinition] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [RepositoryLog]
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'RepositoryLog' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYLOG_CREATEDDATE] DEFAULT getdate() NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [RepositoryStat]
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'RepositoryStat' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__REPOSITORYSTAT_CREATEDDATE] DEFAULT getdate() NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [UserAccount]
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__USERACCOUNT_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [UserAccount]
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__USERACCOUNT_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [UserAccount]
if exists(select * from sys.objects where name = 'UserAccount' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'UserAccount')
ALTER TABLE [dbo].[UserAccount] ADD [Timestamp] [timestamp] NOT NULL

GO

--##SECTION END [AUDIT TRAIL CREATE]

--##SECTION BEGIN [AUDIT TRAIL REMOVE]

--REMOVE AUDIT TRAIL CREATE FOR TABLE [RepositoryActionType]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] DROP COLUMN [CreatedBy]
if exists (select * from sys.objects where name = 'DF__REPOSITORYACTIONTYPE_CREATEDDATE' and [type] = 'D')
ALTER TABLE [dbo].[RepositoryActionType] DROP CONSTRAINT [DF__REPOSITORYACTIONTYPE_CREATEDDATE]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] DROP COLUMN [CreatedDate]

--REMOVE AUDIT TRAIL MODIFY FOR TABLE [RepositoryActionType]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] DROP COLUMN [ModifiedBy]
if exists (select * from sys.objects where name = 'DF__REPOSITORYACTIONTYPE_MODIFIEDDATE' and [type] = 'D')
ALTER TABLE [dbo].[RepositoryActionType] DROP CONSTRAINT [DF__REPOSITORYACTIONTYPE_MODIFIEDDATE]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] DROP COLUMN [ModifiedDate]

--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [RepositoryActionType]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'RepositoryActionType')
ALTER TABLE [dbo].[RepositoryActionType] DROP COLUMN [Timestamp]

GO

--REMOVE AUDIT TRAIL MODIFY FOR TABLE [RepositoryLog]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] DROP COLUMN [ModifiedBy]
if exists (select * from sys.objects where name = 'DF__REPOSITORYLOG_MODIFIEDDATE' and [type] = 'D')
ALTER TABLE [dbo].[RepositoryLog] DROP CONSTRAINT [DF__REPOSITORYLOG_MODIFIEDDATE]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] DROP COLUMN [ModifiedDate]

--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [RepositoryLog]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'RepositoryLog')
ALTER TABLE [dbo].[RepositoryLog] DROP COLUMN [Timestamp]

GO

--REMOVE AUDIT TRAIL MODIFY FOR TABLE [RepositoryStat]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] DROP COLUMN [ModifiedBy]
if exists (select * from sys.objects where name = 'DF__REPOSITORYSTAT_MODIFIEDDATE' and [type] = 'D')
ALTER TABLE [dbo].[RepositoryStat] DROP CONSTRAINT [DF__REPOSITORYSTAT_MODIFIEDDATE]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] DROP COLUMN [ModifiedDate]

--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [RepositoryStat]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'RepositoryStat')
ALTER TABLE [dbo].[RepositoryStat] DROP COLUMN [Timestamp]

GO

--REMOVE AUDIT TRAIL CREATE FOR TABLE [ServerStat]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] DROP COLUMN [CreatedBy]
if exists (select * from sys.objects where name = 'DF__SERVERSTAT_CREATEDDATE' and [type] = 'D')
ALTER TABLE [dbo].[ServerStat] DROP CONSTRAINT [DF__SERVERSTAT_CREATEDDATE]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] DROP COLUMN [CreatedDate]

--REMOVE AUDIT TRAIL MODIFY FOR TABLE [ServerStat]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] DROP COLUMN [ModifiedBy]
if exists (select * from sys.objects where name = 'DF__SERVERSTAT_MODIFIEDDATE' and [type] = 'D')
ALTER TABLE [dbo].[ServerStat] DROP CONSTRAINT [DF__SERVERSTAT_MODIFIEDDATE]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] DROP COLUMN [ModifiedDate]

--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [ServerStat]
if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'ServerStat')
ALTER TABLE [dbo].[ServerStat] DROP COLUMN [Timestamp]

GO

--##SECTION END [AUDIT TRAIL REMOVE]

--##SECTION BEGIN [RENAME PK]

--RENAME EXISTING PRIMARY KEYS IF NECESSARY
DECLARE @pkfixConfigurationSetting varchar(500)
SET @pkfixConfigurationSetting = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'ConfigurationSetting')
if @pkfixConfigurationSetting <> '' and (BINARY_CHECKSUM(@pkfixConfigurationSetting) <> BINARY_CHECKSUM('PK_CONFIGURATIONSETTING')) exec('sp_rename '''+@pkfixConfigurationSetting+''', ''PK_CONFIGURATIONSETTING''')
DECLARE @pkfixDimensionData varchar(500)
SET @pkfixDimensionData = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'DimensionData')
if @pkfixDimensionData <> '' and (BINARY_CHECKSUM(@pkfixDimensionData) <> BINARY_CHECKSUM('PK_DIMENSIONDATA')) exec('sp_rename '''+@pkfixDimensionData+''', ''PK_DIMENSIONDATA''')
DECLARE @pkfixDimensionStore varchar(500)
SET @pkfixDimensionStore = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'DimensionStore')
if @pkfixDimensionStore <> '' and (BINARY_CHECKSUM(@pkfixDimensionStore) <> BINARY_CHECKSUM('PK_DIMENSIONSTORE')) exec('sp_rename '''+@pkfixDimensionStore+''', ''PK_DIMENSIONSTORE''')
DECLARE @pkfixRepositoryActionType varchar(500)
SET @pkfixRepositoryActionType = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'RepositoryActionType')
if @pkfixRepositoryActionType <> '' and (BINARY_CHECKSUM(@pkfixRepositoryActionType) <> BINARY_CHECKSUM('PK_REPOSITORYACTIONTYPE')) exec('sp_rename '''+@pkfixRepositoryActionType+''', ''PK_REPOSITORYACTIONTYPE''')
DECLARE @pkfixRepositoryData varchar(500)
SET @pkfixRepositoryData = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'RepositoryData')
if @pkfixRepositoryData <> '' and (BINARY_CHECKSUM(@pkfixRepositoryData) <> BINARY_CHECKSUM('PK_REPOSITORYDATA')) exec('sp_rename '''+@pkfixRepositoryData+''', ''PK_REPOSITORYDATA''')
DECLARE @pkfixRepositoryDefinition varchar(500)
SET @pkfixRepositoryDefinition = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'RepositoryDefinition')
if @pkfixRepositoryDefinition <> '' and (BINARY_CHECKSUM(@pkfixRepositoryDefinition) <> BINARY_CHECKSUM('PK_REPOSITORYDEFINITION')) exec('sp_rename '''+@pkfixRepositoryDefinition+''', ''PK_REPOSITORYDEFINITION''')
DECLARE @pkfixRepositoryLog varchar(500)
SET @pkfixRepositoryLog = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'RepositoryLog')
if @pkfixRepositoryLog <> '' and (BINARY_CHECKSUM(@pkfixRepositoryLog) <> BINARY_CHECKSUM('PK_REPOSITORYLOG')) exec('sp_rename '''+@pkfixRepositoryLog+''', ''PK_REPOSITORYLOG''')
DECLARE @pkfixRepositoryStat varchar(500)
SET @pkfixRepositoryStat = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'RepositoryStat')
if @pkfixRepositoryStat <> '' and (BINARY_CHECKSUM(@pkfixRepositoryStat) <> BINARY_CHECKSUM('PK_REPOSITORYSTAT')) exec('sp_rename '''+@pkfixRepositoryStat+''', ''PK_REPOSITORYSTAT''')
DECLARE @pkfixServerStat varchar(500)
SET @pkfixServerStat = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'ServerStat')
if @pkfixServerStat <> '' and (BINARY_CHECKSUM(@pkfixServerStat) <> BINARY_CHECKSUM('PK_SERVERSTAT')) exec('sp_rename '''+@pkfixServerStat+''', ''PK_SERVERSTAT''')
DECLARE @pkfixUserAccount varchar(500)
SET @pkfixUserAccount = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'UserAccount')
if @pkfixUserAccount <> '' and (BINARY_CHECKSUM(@pkfixUserAccount) <> BINARY_CHECKSUM('PK_USERACCOUNT')) exec('sp_rename '''+@pkfixUserAccount+''', ''PK_USERACCOUNT''')
GO

--##SECTION END [RENAME PK]

--##SECTION BEGIN [DROP PK]

--##SECTION END [DROP PK]

--##SECTION BEGIN [CREATE PK]

--PRIMARY KEY FOR TABLE [ConfigurationSetting]
if not exists(select * from sysobjects where name = 'PK_CONFIGURATIONSETTING' and xtype = 'PK')
ALTER TABLE [dbo].[ConfigurationSetting] WITH NOCHECK ADD 
CONSTRAINT [PK_CONFIGURATIONSETTING] PRIMARY KEY CLUSTERED
(
	[ID]
)
GO
--PRIMARY KEY FOR TABLE [DimensionData]
if not exists(select * from sysobjects where name = 'PK_DIMENSIONDATA' and xtype = 'PK')
ALTER TABLE [dbo].[DimensionData] WITH NOCHECK ADD 
CONSTRAINT [PK_DIMENSIONDATA] PRIMARY KEY CLUSTERED
(
	[DimensionDataId]
)
GO
--PRIMARY KEY FOR TABLE [DimensionStore]
if not exists(select * from sysobjects where name = 'PK_DIMENSIONSTORE' and xtype = 'PK')
ALTER TABLE [dbo].[DimensionStore] WITH NOCHECK ADD 
CONSTRAINT [PK_DIMENSIONSTORE] PRIMARY KEY CLUSTERED
(
	[DimensionStoreId]
)
GO
--PRIMARY KEY FOR TABLE [RepositoryActionType]
if not exists(select * from sysobjects where name = 'PK_REPOSITORYACTIONTYPE' and xtype = 'PK')
ALTER TABLE [dbo].[RepositoryActionType] WITH NOCHECK ADD 
CONSTRAINT [PK_REPOSITORYACTIONTYPE] PRIMARY KEY CLUSTERED
(
	[RepositoryActionTypeId]
)
GO
--PRIMARY KEY FOR TABLE [RepositoryData]
if not exists(select * from sysobjects where name = 'PK_REPOSITORYDATA' and xtype = 'PK')
ALTER TABLE [dbo].[RepositoryData] WITH NOCHECK ADD 
CONSTRAINT [PK_REPOSITORYDATA] PRIMARY KEY CLUSTERED
(
	[RepositoryDataId]
)
GO
--PRIMARY KEY FOR TABLE [RepositoryDefinition]
if not exists(select * from sysobjects where name = 'PK_REPOSITORYDEFINITION' and xtype = 'PK')
ALTER TABLE [dbo].[RepositoryDefinition] WITH NOCHECK ADD 
CONSTRAINT [PK_REPOSITORYDEFINITION] PRIMARY KEY CLUSTERED
(
	[RepositoryId]
)
GO
--PRIMARY KEY FOR TABLE [RepositoryLog]
if not exists(select * from sysobjects where name = 'PK_REPOSITORYLOG' and xtype = 'PK')
ALTER TABLE [dbo].[RepositoryLog] WITH NOCHECK ADD 
CONSTRAINT [PK_REPOSITORYLOG] PRIMARY KEY CLUSTERED
(
	[RepositoryLogId]
)
GO
--PRIMARY KEY FOR TABLE [RepositoryStat]
if not exists(select * from sysobjects where name = 'PK_REPOSITORYSTAT' and xtype = 'PK')
ALTER TABLE [dbo].[RepositoryStat] WITH NOCHECK ADD 
CONSTRAINT [PK_REPOSITORYSTAT] PRIMARY KEY CLUSTERED
(
	[RepositoryStatId]
)
GO
--PRIMARY KEY FOR TABLE [ServerStat]
if not exists(select * from sysobjects where name = 'PK_SERVERSTAT' and xtype = 'PK')
ALTER TABLE [dbo].[ServerStat] WITH NOCHECK ADD 
CONSTRAINT [PK_SERVERSTAT] PRIMARY KEY CLUSTERED
(
	[ServerStatId]
)
GO
--PRIMARY KEY FOR TABLE [UserAccount]
if not exists(select * from sysobjects where name = 'PK_USERACCOUNT' and xtype = 'PK')
ALTER TABLE [dbo].[UserAccount] WITH NOCHECK ADD 
CONSTRAINT [PK_USERACCOUNT] PRIMARY KEY CLUSTERED
(
	[UserId]
)
GO
--##SECTION END [CREATE PK]

--##SECTION BEGIN [AUDIT TABLES PK]

--DROP PRIMARY KEY FOR TABLE [__AUDIT__CONFIGURATIONSETTING]
if exists(select * from sys.objects where name = 'PK___AUDIT__CONFIGURATIONSETTING' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__CONFIGURATIONSETTING] DROP CONSTRAINT [PK___AUDIT__CONFIGURATIONSETTING]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__DIMENSIONDATA]
if exists(select * from sys.objects where name = 'PK___AUDIT__DIMENSIONDATA' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__DIMENSIONDATA] DROP CONSTRAINT [PK___AUDIT__DIMENSIONDATA]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__DIMENSIONSTORE]
if exists(select * from sys.objects where name = 'PK___AUDIT__DIMENSIONSTORE' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__DIMENSIONSTORE] DROP CONSTRAINT [PK___AUDIT__DIMENSIONSTORE]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__REPOSITORYACTIONTYPE]
if exists(select * from sys.objects where name = 'PK___AUDIT__REPOSITORYACTIONTYPE' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__REPOSITORYACTIONTYPE] DROP CONSTRAINT [PK___AUDIT__REPOSITORYACTIONTYPE]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__REPOSITORYDATA]
if exists(select * from sys.objects where name = 'PK___AUDIT__REPOSITORYDATA' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__REPOSITORYDATA] DROP CONSTRAINT [PK___AUDIT__REPOSITORYDATA]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__REPOSITORYDEFINITION]
if exists(select * from sys.objects where name = 'PK___AUDIT__REPOSITORYDEFINITION' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__REPOSITORYDEFINITION] DROP CONSTRAINT [PK___AUDIT__REPOSITORYDEFINITION]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__REPOSITORYLOG]
if exists(select * from sys.objects where name = 'PK___AUDIT__REPOSITORYLOG' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__REPOSITORYLOG] DROP CONSTRAINT [PK___AUDIT__REPOSITORYLOG]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__REPOSITORYSTAT]
if exists(select * from sys.objects where name = 'PK___AUDIT__REPOSITORYSTAT' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__REPOSITORYSTAT] DROP CONSTRAINT [PK___AUDIT__REPOSITORYSTAT]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__SERVERSTAT]
if exists(select * from sys.objects where name = 'PK___AUDIT__SERVERSTAT' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__SERVERSTAT] DROP CONSTRAINT [PK___AUDIT__SERVERSTAT]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__USERACCOUNT]
if exists(select * from sys.objects where name = 'PK___AUDIT__USERACCOUNT' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')
ALTER TABLE [dbo].[__AUDIT__USERACCOUNT] DROP CONSTRAINT [PK___AUDIT__USERACCOUNT]
GO

--##SECTION END [AUDIT TABLES PK]

--FOREIGN KEY RELATIONSHIP [DimensionStore] -> [DimensionData] ([DimensionStore].[DimensionStoreId] -> [DimensionData].[DimensionsionStoreId])
if not exists(select * from sysobjects where name = 'FK__DIMENSIONDATA_DIMENSIONSTORE' and xtype = 'F')
ALTER TABLE [dbo].[DimensionData] ADD 
CONSTRAINT [FK__DIMENSIONDATA_DIMENSIONSTORE] FOREIGN KEY 
(
	[DimensionsionStoreId]
) REFERENCES [dbo].[DimensionStore] (
	[DimensionStoreId]
)
GO

--FOREIGN KEY RELATIONSHIP [RepositoryDefinition] -> [DimensionStore] ([RepositoryDefinition].[RepositoryId] -> [DimensionStore].[RepositoryId])
if not exists(select * from sysobjects where name = 'FK__DIMENSIONSTORE_REPOSITORYDEFINITION' and xtype = 'F')
ALTER TABLE [dbo].[DimensionStore] ADD 
CONSTRAINT [FK__DIMENSIONSTORE_REPOSITORYDEFINITION] FOREIGN KEY 
(
	[RepositoryId]
) REFERENCES [dbo].[RepositoryDefinition] (
	[RepositoryId]
)
GO

--FOREIGN KEY RELATIONSHIP [RepositoryDefinition] -> [RepositoryData] ([RepositoryDefinition].[RepositoryId] -> [RepositoryData].[RepositoryId])
if not exists(select * from sysobjects where name = 'FK__REPOSITORYDATA_REPOSITORYDEFINITION' and xtype = 'F')
ALTER TABLE [dbo].[RepositoryData] ADD 
CONSTRAINT [FK__REPOSITORYDATA_REPOSITORYDEFINITION] FOREIGN KEY 
(
	[RepositoryId]
) REFERENCES [dbo].[RepositoryDefinition] (
	[RepositoryId]
)
GO

--FOREIGN KEY RELATIONSHIP [RepositoryDefinition] -> [RepositoryLog] ([RepositoryDefinition].[RepositoryId] -> [RepositoryLog].[RepositoryId])
if not exists(select * from sysobjects where name = 'FK__REPOSITORYLOG_REPOSITORYDEFINITION' and xtype = 'F')
ALTER TABLE [dbo].[RepositoryLog] ADD 
CONSTRAINT [FK__REPOSITORYLOG_REPOSITORYDEFINITION] FOREIGN KEY 
(
	[RepositoryId]
) REFERENCES [dbo].[RepositoryDefinition] (
	[RepositoryId]
)
GO

--FOREIGN KEY RELATIONSHIP [RepositoryActionType] -> [RepositoryStat] ([RepositoryActionType].[RepositoryActionTypeId] -> [RepositoryStat].[RepositoryActionTypeId])
if not exists(select * from sysobjects where name = 'FK__REPOSITORYSTAT_REPOSITORYACTIONTYPE' and xtype = 'F')
ALTER TABLE [dbo].[RepositoryStat] ADD 
CONSTRAINT [FK__REPOSITORYSTAT_REPOSITORYACTIONTYPE] FOREIGN KEY 
(
	[RepositoryActionTypeId]
) REFERENCES [dbo].[RepositoryActionType] (
	[RepositoryActionTypeId]
)
GO

--FOREIGN KEY RELATIONSHIP [RepositoryDefinition] -> [RepositoryStat] ([RepositoryDefinition].[RepositoryId] -> [RepositoryStat].[RepositoryId])
if not exists(select * from sysobjects where name = 'FK__REPOSITORYSTAT_REPOSITORYDEFINITION' and xtype = 'F')
ALTER TABLE [dbo].[RepositoryStat] ADD 
CONSTRAINT [FK__REPOSITORYSTAT_REPOSITORYDEFINITION] FOREIGN KEY 
(
	[RepositoryId]
) REFERENCES [dbo].[RepositoryDefinition] (
	[RepositoryId]
)
GO

--##SECTION BEGIN [CREATE INDEXES]

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_DIMENSIONDATA_DIMENSIONSIONSTOREID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_DIMENSIONDATA_DIMENSIONSIONSTOREID] ON [dbo].[DimensionData]
GO

--INDEX FOR TABLE [DimensionData] COLUMNS:[DimensionsionStoreId]
if not exists(select * from sys.indexes where name = 'IDX_DIMENSIONDATA_DIMENSIONSIONSTOREID')
CREATE NONCLUSTERED INDEX [IDX_DIMENSIONDATA_DIMENSIONSIONSTOREID] ON [dbo].[DimensionData] ([DimensionsionStoreId] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_DIMENSIONSTORE_REPOSITORYID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_DIMENSIONSTORE_REPOSITORYID] ON [dbo].[DimensionStore]
GO

--INDEX FOR TABLE [DimensionStore] COLUMNS:[RepositoryId]
if not exists(select * from sys.indexes where name = 'IDX_DIMENSIONSTORE_REPOSITORYID')
CREATE NONCLUSTERED INDEX [IDX_DIMENSIONSTORE_REPOSITORYID] ON [dbo].[DimensionStore] ([RepositoryId] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_DIMENSIONSTORE_DIDX' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_DIMENSIONSTORE_DIDX] ON [dbo].[DimensionStore]
GO

--INDEX FOR TABLE [DimensionStore] COLUMNS:[DIdx]
if not exists(select * from sys.indexes where name = 'IDX_DIMENSIONSTORE_DIDX')
CREATE NONCLUSTERED INDEX [IDX_DIMENSIONSTORE_DIDX] ON [dbo].[DimensionStore] ([DIdx] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_REPOSITORYDATA_REPOSITORYID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_REPOSITORYDATA_REPOSITORYID] ON [dbo].[RepositoryData]
GO

--INDEX FOR TABLE [RepositoryData] COLUMNS:[RepositoryId]
if not exists(select * from sys.indexes where name = 'IDX_REPOSITORYDATA_REPOSITORYID')
CREATE NONCLUSTERED INDEX [IDX_REPOSITORYDATA_REPOSITORYID] ON [dbo].[RepositoryData] ([RepositoryId] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_REPOSITORYDEFINITION_UNIQUEKEY' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_REPOSITORYDEFINITION_UNIQUEKEY] ON [dbo].[RepositoryDefinition]
GO

--INDEX FOR TABLE [RepositoryDefinition] COLUMNS:[UniqueKey]
if not exists(select * from sys.indexes where name = 'IDX_REPOSITORYDEFINITION_UNIQUEKEY')
CREATE NONCLUSTERED INDEX [IDX_REPOSITORYDEFINITION_UNIQUEKEY] ON [dbo].[RepositoryDefinition] ([UniqueKey] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_REPOSITORYLOG_REPOSITORYID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_REPOSITORYLOG_REPOSITORYID] ON [dbo].[RepositoryLog]
GO

--INDEX FOR TABLE [RepositoryLog] COLUMNS:[RepositoryId]
if not exists(select * from sys.indexes where name = 'IDX_REPOSITORYLOG_REPOSITORYID')
CREATE NONCLUSTERED INDEX [IDX_REPOSITORYLOG_REPOSITORYID] ON [dbo].[RepositoryLog] ([RepositoryId] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_REPOSITORYSTAT_REPOSITORYACTIONTYPEID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_REPOSITORYSTAT_REPOSITORYACTIONTYPEID] ON [dbo].[RepositoryStat]
GO

--INDEX FOR TABLE [RepositoryStat] COLUMNS:[RepositoryActionTypeId]
if not exists(select * from sys.indexes where name = 'IDX_REPOSITORYSTAT_REPOSITORYACTIONTYPEID')
CREATE NONCLUSTERED INDEX [IDX_REPOSITORYSTAT_REPOSITORYACTIONTYPEID] ON [dbo].[RepositoryStat] ([RepositoryActionTypeId] ASC)
GO

--DELETE INDEX
if exists(select * from sys.indexes where name = 'IDX_REPOSITORYSTAT_REPOSITORYID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_REPOSITORYSTAT_REPOSITORYID] ON [dbo].[RepositoryStat]
GO

--INDEX FOR TABLE [RepositoryStat] COLUMNS:[RepositoryId]
if not exists(select * from sys.indexes where name = 'IDX_REPOSITORYSTAT_REPOSITORYID')
CREATE NONCLUSTERED INDEX [IDX_REPOSITORYSTAT_REPOSITORYID] ON [dbo].[RepositoryStat] ([RepositoryId] ASC)
GO

--##SECTION END [CREATE INDEXES]

--##SECTION BEGIN [TENANT INDEXES]

--##SECTION END [TENANT INDEXES]

--##SECTION BEGIN [REMOVE DEFAULTS]

--BEGIN DEFAULTS FOR TABLE [ConfigurationSetting]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ConfigurationSetting') AND COL_NAME(sc.id,sc.colid)='ID' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ConfigurationSetting] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ConfigurationSetting') AND COL_NAME(sc.id,sc.colid)='Name' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ConfigurationSetting] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ConfigurationSetting') AND COL_NAME(sc.id,sc.colid)='Value' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ConfigurationSetting] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [ConfigurationSetting]
GO

--BEGIN DEFAULTS FOR TABLE [DimensionData]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionData') AND COL_NAME(sc.id,sc.colid)='Data' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionData] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionData') AND COL_NAME(sc.id,sc.colid)='DimensionDataId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionData] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionData') AND COL_NAME(sc.id,sc.colid)='DimensionsionStoreId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionData] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [DimensionData]
GO

--BEGIN DEFAULTS FOR TABLE [DimensionStore]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionStore') AND COL_NAME(sc.id,sc.colid)='DIdx' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionStore] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionStore') AND COL_NAME(sc.id,sc.colid)='DimensionStoreId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionStore] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionStore') AND COL_NAME(sc.id,sc.colid)='Name' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionStore] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('DimensionStore') AND COL_NAME(sc.id,sc.colid)='RepositoryId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [DimensionStore] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [DimensionStore]
GO

--BEGIN DEFAULTS FOR TABLE [RepositoryActionType]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryActionType') AND COL_NAME(sc.id,sc.colid)='Name' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryActionType] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryActionType') AND COL_NAME(sc.id,sc.colid)='RepositoryActionTypeId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryActionType] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [RepositoryActionType]
GO

--BEGIN DEFAULTS FOR TABLE [RepositoryData]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryData') AND COL_NAME(sc.id,sc.colid)='Data' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryData] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryData') AND COL_NAME(sc.id,sc.colid)='Keyword' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryData] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryData') AND COL_NAME(sc.id,sc.colid)='RepositoryDataId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryData] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryData') AND COL_NAME(sc.id,sc.colid)='RepositoryId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryData] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [RepositoryData]
GO

--BEGIN DEFAULTS FOR TABLE [RepositoryDefinition]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='DefinitionData' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='ItemCount' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='MemorySize' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='Name' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='RepositoryId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='UniqueKey' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryDefinition') AND COL_NAME(sc.id,sc.colid)='VersionHash' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryDefinition] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [RepositoryDefinition]
GO

--BEGIN DEFAULTS FOR TABLE [RepositoryLog]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='Count' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='ElapsedTime' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='IPAddress' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='Query' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='QueryId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='RepositoryId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='RepositoryLogId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryLog') AND COL_NAME(sc.id,sc.colid)='UsedCache' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryLog] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [RepositoryLog]
GO

--BEGIN DEFAULTS FOR TABLE [RepositoryStat]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryStat') AND COL_NAME(sc.id,sc.colid)='Count' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryStat') AND COL_NAME(sc.id,sc.colid)='Elapsed' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryStat') AND COL_NAME(sc.id,sc.colid)='ItemCount' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryStat') AND COL_NAME(sc.id,sc.colid)='RepositoryActionTypeId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryStat') AND COL_NAME(sc.id,sc.colid)='RepositoryId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('RepositoryStat') AND COL_NAME(sc.id,sc.colid)='RepositoryStatId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [RepositoryStat] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [RepositoryStat]
GO

--BEGIN DEFAULTS FOR TABLE [ServerStat]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='AddedDate' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='MemoryUsageAvailable' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='MemoryUsageProcess' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='MemoryUsageTotal' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='ProcessorUsage' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='RepositoryCreateDelta' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='RepositoryDeleteDelta' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='RepositoryInMem' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='RepositoryLoadDelta' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='RepositoryTotal' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='RepositoryUnloadDelta' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('ServerStat') AND COL_NAME(sc.id,sc.colid)='ServerStatId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [ServerStat] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [ServerStat]
GO

--BEGIN DEFAULTS FOR TABLE [UserAccount]
DECLARE @defaultName varchar(max)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('UserAccount') AND COL_NAME(sc.id,sc.colid)='Password' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [UserAccount] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('UserAccount') AND COL_NAME(sc.id,sc.colid)='UniqueKey' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [UserAccount] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('UserAccount') AND COL_NAME(sc.id,sc.colid)='UserId' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [UserAccount] DROP CONSTRAINT ' + @defaultName)
SET @defaultName = (select top 1 o.name FROM sysconstraints sc left join sys.objects o on sc.constid = o.object_id where sc.id=OBJECT_ID('UserAccount') AND COL_NAME(sc.id,sc.colid)='UserName' AND OBJECTPROPERTY(sc.constid, 'IsDefaultCnst') = 1)
if @defaultName IS NOT NULL
exec('ALTER TABLE [UserAccount] DROP CONSTRAINT ' + @defaultName)
--END DEFAULTS FOR TABLE [UserAccount]
GO

--##SECTION END [REMOVE DEFAULTS]

--##SECTION BEGIN [CREATE DEFAULTS]

--BEGIN DEFAULTS FOR TABLE [ServerStat]
if not exists(select * from sys.objects where name = 'DF__SERVERSTAT_ADDEDDATE' and type = 'D' and type_desc = 'DEFAULT_CONSTRAINT')
ALTER TABLE [dbo].[ServerStat] ADD CONSTRAINT [DF__SERVERSTAT_ADDEDDATE] DEFAULT (getdate()) FOR [AddedDate]

--END DEFAULTS FOR TABLE [ServerStat]
GO

--##SECTION END [CREATE DEFAULTS]

if not exists(select * from sys.objects where [name] = '__nhydrateschema' and [type] = 'U')
BEGIN
CREATE TABLE [__nhydrateschema] (
[dbVersion] [varchar] (50) NOT NULL,
[LastUpdate] [datetime] NOT NULL,
[ModelKey] [uniqueidentifier] NOT NULL,
[History] [nvarchar](max) NOT NULL
)
if not exists(select * from sys.objects where [name] = '__pk__nhydrateschema' and [type] = 'PK')
ALTER TABLE [__nhydrateschema] WITH NOCHECK ADD CONSTRAINT [__pk__nhydrateschema] PRIMARY KEY CLUSTERED ([ModelKey])
END
GO

if not exists(select * from sys.objects where name = '__nhydrateobjects' and [type] = 'U')
CREATE TABLE [dbo].[__nhydrateobjects]
(
	[rowid] [bigint] IDENTITY(1,1) NOT NULL,
	[id] [uniqueidentifier] NULL,
	[name] [varchar](500) NOT NULL,
	[type] [varchar](10) NOT NULL,
	[schema] [varchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Hash] [varchar](32) NULL,
	[ModelKey] [uniqueidentifier] NOT NULL,
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_name')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_name] ON [dbo].[__nhydrateobjects]
(
	[name] ASC
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_schema')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_schema] ON [dbo].[__nhydrateobjects] 
(
	[schema] ASC
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_type')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_type] ON [dbo].[__nhydrateobjects] 
(
	[type] ASC
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_modelkey')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_modelkey] ON [dbo].[__nhydrateobjects] 
(
	[ModelKey] ASC
)

if not exists(select * from sys.indexes where name = '__pk__nhydrateobjects')
ALTER TABLE [dbo].[__nhydrateobjects] ADD CONSTRAINT [__pk__nhydrateobjects] PRIMARY KEY CLUSTERED 
(
	[rowid] ASC
)
GO

