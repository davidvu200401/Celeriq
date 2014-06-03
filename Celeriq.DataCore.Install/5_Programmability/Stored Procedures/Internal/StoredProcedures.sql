--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.

--##SECTION BEGIN [INTERNAL STORED PROCS]

--This SQL is generated for internal stored procedures for table [ConfigurationSetting]
if exists(select * from sys.objects where name = 'gen_ConfigurationSetting_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_ConfigurationSetting_Delete]
GO

CREATE PROCEDURE [dbo].[gen_ConfigurationSetting_Delete]
(
	@Original_ID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[ConfigurationSetting] 
WHERE 
	[ID] = @Original_ID;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_ConfigurationSetting_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_ConfigurationSetting_Insert]
GO

CREATE PROCEDURE [dbo].[gen_ConfigurationSetting_Insert]
(
	@ID [Int] = null,
	@Name [VarChar] (100),
	@Value [VarChar] (max),
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@ID < 0) SET @ID = NULL;
if ((@ID IS NULL))
BEGIN
INSERT INTO [dbo].[ConfigurationSetting]
(
	[Name],
	[Value],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Name,
	@Value,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[ConfigurationSetting] on
INSERT INTO [dbo].[ConfigurationSetting]
(
	[ID],
	[Name],
	[Value],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@ID,
	@Name,
	@Value,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[ConfigurationSetting] off
END


SELECT 
	[dbo].[ConfigurationSetting].[ID],
	[dbo].[ConfigurationSetting].[Name],
	[dbo].[ConfigurationSetting].[Value],
	[dbo].[ConfigurationSetting].[CreatedBy],
	[dbo].[ConfigurationSetting].[CreatedDate],
	[dbo].[ConfigurationSetting].[ModifiedBy],
	[dbo].[ConfigurationSetting].[ModifiedDate],
	[dbo].[ConfigurationSetting].[Timestamp]

FROM
[dbo].[ConfigurationSetting]
WHERE
	[dbo].[ConfigurationSetting].[ID] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_ConfigurationSetting_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_ConfigurationSetting_Update]
GO

CREATE PROCEDURE [dbo].[gen_ConfigurationSetting_Update]
(
	@Name [VarChar] (100),
	@Value [VarChar] (max),
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_ID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[ConfigurationSetting] 
SET
	[Name] = @Name,
	[Value] = @Value,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[ConfigurationSetting].[ID] = @Original_ID AND
	[dbo].[ConfigurationSetting].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[ConfigurationSetting].[ID],
	[dbo].[ConfigurationSetting].[Name],
	[dbo].[ConfigurationSetting].[Value],
	[dbo].[ConfigurationSetting].[CreatedBy],
	[dbo].[ConfigurationSetting].[CreatedDate],
	[dbo].[ConfigurationSetting].[ModifiedBy],
	[dbo].[ConfigurationSetting].[ModifiedDate],
	[dbo].[ConfigurationSetting].[Timestamp]
FROM 
[dbo].[ConfigurationSetting]
WHERE
	[dbo].[ConfigurationSetting].[ID] = @Original_ID
GO

--This SQL is generated for internal stored procedures for table [DimensionData]
if exists(select * from sys.objects where name = 'gen_DimensionData_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_DimensionData_Delete]
GO

CREATE PROCEDURE [dbo].[gen_DimensionData_Delete]
(
	@DimensionStore_DimensionStoreId [BigInt] = null,--Entity Framework Required Parent Keys be passed in: Table 'DimensionStore'
	@Original_DimensionDataId [BigInt]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[DimensionData] 
WHERE 
	[DimensionDataId] = @Original_DimensionDataId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_DimensionData_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_DimensionData_Insert]
GO

CREATE PROCEDURE [dbo].[gen_DimensionData_Insert]
(
	@Data [VarBinary] (max),
	@DimensionDataId [BigInt] = null,
	@DimensionsionStoreId [BigInt],
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@DimensionDataId < 0) SET @DimensionDataId = NULL;
if ((@DimensionDataId IS NULL))
BEGIN
INSERT INTO [dbo].[DimensionData]
(
	[Data],
	[DimensionsionStoreId],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Data,
	@DimensionsionStoreId,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[DimensionData] on
INSERT INTO [dbo].[DimensionData]
(
	[DimensionDataId],
	[Data],
	[DimensionsionStoreId],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@DimensionDataId,
	@Data,
	@DimensionsionStoreId,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[DimensionData] off
END


SELECT 
	[dbo].[DimensionData].[Data],
	[dbo].[DimensionData].[DimensionDataId],
	[dbo].[DimensionData].[DimensionsionStoreId],
	[dbo].[DimensionData].[CreatedBy],
	[dbo].[DimensionData].[CreatedDate],
	[dbo].[DimensionData].[ModifiedBy],
	[dbo].[DimensionData].[ModifiedDate],
	[dbo].[DimensionData].[Timestamp]

FROM
[dbo].[DimensionData]
WHERE
	[dbo].[DimensionData].[DimensionDataId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_DimensionData_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_DimensionData_Update]
GO

CREATE PROCEDURE [dbo].[gen_DimensionData_Update]
(
	@Data [VarBinary] (max),
	@DimensionsionStoreId [BigInt],
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_DimensionDataId [BigInt],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[DimensionData] 
SET
	[Data] = @Data,
	[DimensionsionStoreId] = @DimensionsionStoreId,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[DimensionData].[DimensionDataId] = @Original_DimensionDataId AND
	[dbo].[DimensionData].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[DimensionData].[Data],
	[dbo].[DimensionData].[DimensionDataId],
	[dbo].[DimensionData].[DimensionsionStoreId],
	[dbo].[DimensionData].[CreatedBy],
	[dbo].[DimensionData].[CreatedDate],
	[dbo].[DimensionData].[ModifiedBy],
	[dbo].[DimensionData].[ModifiedDate],
	[dbo].[DimensionData].[Timestamp]
FROM 
[dbo].[DimensionData]
WHERE
	[dbo].[DimensionData].[DimensionDataId] = @Original_DimensionDataId
GO

--This SQL is generated for internal stored procedures for table [DimensionStore]
if exists(select * from sys.objects where name = 'gen_DimensionStore_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_DimensionStore_Delete]
GO

CREATE PROCEDURE [dbo].[gen_DimensionStore_Delete]
(
	@RepositoryDefinition_RepositoryId [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'RepositoryDefinition'
	@Original_DimensionStoreId [BigInt]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[DimensionStore] 
WHERE 
	[DimensionStoreId] = @Original_DimensionStoreId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_DimensionStore_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_DimensionStore_Insert]
GO

CREATE PROCEDURE [dbo].[gen_DimensionStore_Insert]
(
	@DIdx [BigInt],
	@DimensionStoreId [BigInt] = null,
	@Name [VarChar] (50),
	@RepositoryId [Int],
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@DimensionStoreId < 0) SET @DimensionStoreId = NULL;
if ((@DimensionStoreId IS NULL))
BEGIN
INSERT INTO [dbo].[DimensionStore]
(
	[DIdx],
	[Name],
	[RepositoryId],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@DIdx,
	@Name,
	@RepositoryId,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[DimensionStore] on
INSERT INTO [dbo].[DimensionStore]
(
	[DimensionStoreId],
	[DIdx],
	[Name],
	[RepositoryId],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@DimensionStoreId,
	@DIdx,
	@Name,
	@RepositoryId,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[DimensionStore] off
END


SELECT 
	[dbo].[DimensionStore].[DIdx],
	[dbo].[DimensionStore].[DimensionStoreId],
	[dbo].[DimensionStore].[Name],
	[dbo].[DimensionStore].[RepositoryId],
	[dbo].[DimensionStore].[CreatedBy],
	[dbo].[DimensionStore].[CreatedDate],
	[dbo].[DimensionStore].[ModifiedBy],
	[dbo].[DimensionStore].[ModifiedDate],
	[dbo].[DimensionStore].[Timestamp]

FROM
[dbo].[DimensionStore]
WHERE
	[dbo].[DimensionStore].[DimensionStoreId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_DimensionStore_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_DimensionStore_Update]
GO

CREATE PROCEDURE [dbo].[gen_DimensionStore_Update]
(
	@DIdx [BigInt],
	@Name [VarChar] (50),
	@RepositoryId [Int],
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_DimensionStoreId [BigInt],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[DimensionStore] 
SET
	[DIdx] = @DIdx,
	[Name] = @Name,
	[RepositoryId] = @RepositoryId,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[DimensionStore].[DimensionStoreId] = @Original_DimensionStoreId AND
	[dbo].[DimensionStore].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[DimensionStore].[DIdx],
	[dbo].[DimensionStore].[DimensionStoreId],
	[dbo].[DimensionStore].[Name],
	[dbo].[DimensionStore].[RepositoryId],
	[dbo].[DimensionStore].[CreatedBy],
	[dbo].[DimensionStore].[CreatedDate],
	[dbo].[DimensionStore].[ModifiedBy],
	[dbo].[DimensionStore].[ModifiedDate],
	[dbo].[DimensionStore].[Timestamp]
FROM 
[dbo].[DimensionStore]
WHERE
	[dbo].[DimensionStore].[DimensionStoreId] = @Original_DimensionStoreId
GO

--This SQL is generated for internal stored procedures for table [RepositoryActionType]
if exists(select * from sys.objects where name = 'gen_RepositoryActionType_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryActionType_Delete]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryActionType_Delete]
(
	@Original_RepositoryActionTypeId [Int]
)
AS
SET NOCOUNT OFF;

GO

if exists(select * from sys.objects where name = 'gen_RepositoryActionType_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryActionType_Insert]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryActionType_Insert]
(
	@Name [VarChar] (50),
	@RepositoryActionTypeId [Int] = null

)
AS
SET NOCOUNT OFF;


SELECT 
	[dbo].[RepositoryActionType].[Name],
	[dbo].[RepositoryActionType].[RepositoryActionTypeId]

FROM
[dbo].[RepositoryActionType]
WHERE
	[dbo].[RepositoryActionType].[RepositoryActionTypeId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_RepositoryActionType_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryActionType_Update]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryActionType_Update]
(
	@Name [VarChar] (50),
	@Original_RepositoryActionTypeId [Int]
)
AS

SET NOCOUNT ON;
SELECT
	[dbo].[RepositoryActionType].[Name],
	[dbo].[RepositoryActionType].[RepositoryActionTypeId]
FROM 
[dbo].[RepositoryActionType]
WHERE
	[dbo].[RepositoryActionType].[RepositoryActionTypeId] = @Original_RepositoryActionTypeId
GO

--This SQL is generated for internal stored procedures for table [RepositoryData]
if exists(select * from sys.objects where name = 'gen_RepositoryData_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryData_Delete]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryData_Delete]
(
	@RepositoryDefinition_RepositoryId [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'RepositoryDefinition'
	@Original_RepositoryDataId [BigInt]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[RepositoryData] 
WHERE 
	[RepositoryDataId] = @Original_RepositoryDataId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_RepositoryData_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryData_Insert]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryData_Insert]
(
	@Data [VarBinary] (max),
	@Keyword [VarChar] (max) = null,
	@RepositoryDataId [BigInt] = null,
	@RepositoryId [Int],
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@RepositoryDataId < 0) SET @RepositoryDataId = NULL;
if ((@RepositoryDataId IS NULL))
BEGIN
INSERT INTO [dbo].[RepositoryData]
(
	[Data],
	[Keyword],
	[RepositoryId],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Data,
	@Keyword,
	@RepositoryId,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[RepositoryData] on
INSERT INTO [dbo].[RepositoryData]
(
	[RepositoryDataId],
	[Data],
	[Keyword],
	[RepositoryId],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@RepositoryDataId,
	@Data,
	@Keyword,
	@RepositoryId,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[RepositoryData] off
END


SELECT 
	[dbo].[RepositoryData].[Data],
	[dbo].[RepositoryData].[Keyword],
	[dbo].[RepositoryData].[RepositoryDataId],
	[dbo].[RepositoryData].[RepositoryId],
	[dbo].[RepositoryData].[CreatedBy],
	[dbo].[RepositoryData].[CreatedDate],
	[dbo].[RepositoryData].[ModifiedBy],
	[dbo].[RepositoryData].[ModifiedDate],
	[dbo].[RepositoryData].[Timestamp]

FROM
[dbo].[RepositoryData]
WHERE
	[dbo].[RepositoryData].[RepositoryDataId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_RepositoryData_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryData_Update]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryData_Update]
(
	@Data [VarBinary] (max),
	@Keyword [VarChar] (max) = null,
	@RepositoryId [Int],
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_RepositoryDataId [BigInt],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[RepositoryData] 
SET
	[Data] = @Data,
	[Keyword] = @Keyword,
	[RepositoryId] = @RepositoryId,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[RepositoryData].[RepositoryDataId] = @Original_RepositoryDataId AND
	[dbo].[RepositoryData].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[RepositoryData].[Data],
	[dbo].[RepositoryData].[Keyword],
	[dbo].[RepositoryData].[RepositoryDataId],
	[dbo].[RepositoryData].[RepositoryId],
	[dbo].[RepositoryData].[CreatedBy],
	[dbo].[RepositoryData].[CreatedDate],
	[dbo].[RepositoryData].[ModifiedBy],
	[dbo].[RepositoryData].[ModifiedDate],
	[dbo].[RepositoryData].[Timestamp]
FROM 
[dbo].[RepositoryData]
WHERE
	[dbo].[RepositoryData].[RepositoryDataId] = @Original_RepositoryDataId
GO

--This SQL is generated for internal stored procedures for table [RepositoryDefinition]
if exists(select * from sys.objects where name = 'gen_RepositoryDefinition_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryDefinition_Delete]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryDefinition_Delete]
(
	@Original_RepositoryId [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[RepositoryDefinition] 
WHERE 
	[RepositoryId] = @Original_RepositoryId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_RepositoryDefinition_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryDefinition_Insert]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryDefinition_Insert]
(
	@DefinitionData [VarBinary] (max),
	@ItemCount [Int],
	@MemorySize [BigInt],
	@Name [VarChar] (50),
	@RepositoryId [Int] = null,
	@UniqueKey [UniqueIdentifier],
	@VersionHash [BigInt],
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@RepositoryId < 0) SET @RepositoryId = NULL;
if ((@RepositoryId IS NULL))
BEGIN
INSERT INTO [dbo].[RepositoryDefinition]
(
	[DefinitionData],
	[ItemCount],
	[MemorySize],
	[Name],
	[UniqueKey],
	[VersionHash],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@DefinitionData,
	@ItemCount,
	@MemorySize,
	@Name,
	@UniqueKey,
	@VersionHash,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[RepositoryDefinition] on
INSERT INTO [dbo].[RepositoryDefinition]
(
	[RepositoryId],
	[DefinitionData],
	[ItemCount],
	[MemorySize],
	[Name],
	[UniqueKey],
	[VersionHash],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@RepositoryId,
	@DefinitionData,
	@ItemCount,
	@MemorySize,
	@Name,
	@UniqueKey,
	@VersionHash,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[RepositoryDefinition] off
END


SELECT 
	[dbo].[RepositoryDefinition].[DefinitionData],
	[dbo].[RepositoryDefinition].[ItemCount],
	[dbo].[RepositoryDefinition].[MemorySize],
	[dbo].[RepositoryDefinition].[Name],
	[dbo].[RepositoryDefinition].[RepositoryId],
	[dbo].[RepositoryDefinition].[UniqueKey],
	[dbo].[RepositoryDefinition].[VersionHash],
	[dbo].[RepositoryDefinition].[CreatedBy],
	[dbo].[RepositoryDefinition].[CreatedDate],
	[dbo].[RepositoryDefinition].[ModifiedBy],
	[dbo].[RepositoryDefinition].[ModifiedDate],
	[dbo].[RepositoryDefinition].[Timestamp]

FROM
[dbo].[RepositoryDefinition]
WHERE
	[dbo].[RepositoryDefinition].[RepositoryId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_RepositoryDefinition_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryDefinition_Update]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryDefinition_Update]
(
	@DefinitionData [VarBinary] (max),
	@ItemCount [Int],
	@MemorySize [BigInt],
	@Name [VarChar] (50),
	@UniqueKey [UniqueIdentifier],
	@VersionHash [BigInt],
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_RepositoryId [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[RepositoryDefinition] 
SET
	[DefinitionData] = @DefinitionData,
	[ItemCount] = @ItemCount,
	[MemorySize] = @MemorySize,
	[Name] = @Name,
	[UniqueKey] = @UniqueKey,
	[VersionHash] = @VersionHash,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[RepositoryDefinition].[RepositoryId] = @Original_RepositoryId AND
	[dbo].[RepositoryDefinition].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[RepositoryDefinition].[DefinitionData],
	[dbo].[RepositoryDefinition].[ItemCount],
	[dbo].[RepositoryDefinition].[MemorySize],
	[dbo].[RepositoryDefinition].[Name],
	[dbo].[RepositoryDefinition].[RepositoryId],
	[dbo].[RepositoryDefinition].[UniqueKey],
	[dbo].[RepositoryDefinition].[VersionHash],
	[dbo].[RepositoryDefinition].[CreatedBy],
	[dbo].[RepositoryDefinition].[CreatedDate],
	[dbo].[RepositoryDefinition].[ModifiedBy],
	[dbo].[RepositoryDefinition].[ModifiedDate],
	[dbo].[RepositoryDefinition].[Timestamp]
FROM 
[dbo].[RepositoryDefinition]
WHERE
	[dbo].[RepositoryDefinition].[RepositoryId] = @Original_RepositoryId
GO

--This SQL is generated for internal stored procedures for table [RepositoryLog]
if exists(select * from sys.objects where name = 'gen_RepositoryLog_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryLog_Delete]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryLog_Delete]
(
	@RepositoryDefinition_RepositoryId [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'RepositoryDefinition'
	@Original_RepositoryLogId [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[RepositoryLog] 
WHERE 
	[RepositoryLogId] = @Original_RepositoryLogId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_RepositoryLog_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryLog_Insert]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryLog_Insert]
(
	@Count [Int],
	@ElapsedTime [Int],
	@IPAddress [VarChar] (50),
	@Query [VarChar] (max) = null,
	@QueryId [UniqueIdentifier],
	@RepositoryId [Int],
	@RepositoryLogId [Int] = null,
	@UsedCache [Bit],
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
IF (@RepositoryLogId < 0) SET @RepositoryLogId = NULL;
if ((@RepositoryLogId IS NULL))
BEGIN
INSERT INTO [dbo].[RepositoryLog]
(
	[Count],
	[ElapsedTime],
	[IPAddress],
	[Query],
	[QueryId],
	[RepositoryId],
	[UsedCache],
	[CreatedDate],
	[CreatedBy]
)
VALUES
(
	@Count,
	@ElapsedTime,
	@IPAddress,
	@Query,
	@QueryId,
	@RepositoryId,
	@UsedCache,
	@CreatedDate,
	@CreatedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[RepositoryLog] on
INSERT INTO [dbo].[RepositoryLog]
(
	[RepositoryLogId],
	[Count],
	[ElapsedTime],
	[IPAddress],
	[Query],
	[QueryId],
	[RepositoryId],
	[UsedCache],
	[CreatedDate],
	[CreatedBy]
)
VALUES
(
	@RepositoryLogId,
	@Count,
	@ElapsedTime,
	@IPAddress,
	@Query,
	@QueryId,
	@RepositoryId,
	@UsedCache,
	@CreatedDate,
	@CreatedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[RepositoryLog] off
END


SELECT 
	[dbo].[RepositoryLog].[Count],
	[dbo].[RepositoryLog].[ElapsedTime],
	[dbo].[RepositoryLog].[IPAddress],
	[dbo].[RepositoryLog].[Query],
	[dbo].[RepositoryLog].[QueryId],
	[dbo].[RepositoryLog].[RepositoryId],
	[dbo].[RepositoryLog].[RepositoryLogId],
	[dbo].[RepositoryLog].[UsedCache],
	[dbo].[RepositoryLog].[CreatedBy],
	[dbo].[RepositoryLog].[CreatedDate]

FROM
[dbo].[RepositoryLog]
WHERE
	[dbo].[RepositoryLog].[RepositoryLogId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_RepositoryLog_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryLog_Update]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryLog_Update]
(
	@Count [Int],
	@ElapsedTime [Int],
	@IPAddress [VarChar] (50),
	@Query [VarChar] (max) = null,
	@QueryId [UniqueIdentifier],
	@RepositoryId [Int],
	@UsedCache [Bit],
	@Original_RepositoryLogId [Int]
)
AS

SET NOCOUNT ON;
UPDATE 
	[dbo].[RepositoryLog] 
SET
	[Count] = @Count,
	[ElapsedTime] = @ElapsedTime,
	[IPAddress] = @IPAddress,
	[Query] = @Query,
	[QueryId] = @QueryId,
	[RepositoryId] = @RepositoryId,
	[UsedCache] = @UsedCache

WHERE
	[dbo].[RepositoryLog].[RepositoryLogId] = @Original_RepositoryLogId


if (@@RowCount = 0) return;

SELECT
	[dbo].[RepositoryLog].[Count],
	[dbo].[RepositoryLog].[ElapsedTime],
	[dbo].[RepositoryLog].[IPAddress],
	[dbo].[RepositoryLog].[Query],
	[dbo].[RepositoryLog].[QueryId],
	[dbo].[RepositoryLog].[RepositoryId],
	[dbo].[RepositoryLog].[RepositoryLogId],
	[dbo].[RepositoryLog].[UsedCache],
	[dbo].[RepositoryLog].[CreatedBy],
	[dbo].[RepositoryLog].[CreatedDate]
FROM 
[dbo].[RepositoryLog]
WHERE
	[dbo].[RepositoryLog].[RepositoryLogId] = @Original_RepositoryLogId
GO

--This SQL is generated for internal stored procedures for table [RepositoryStat]
if exists(select * from sys.objects where name = 'gen_RepositoryStat_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryStat_Delete]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryStat_Delete]
(
	@RepositoryActionType_RepositoryActionTypeId [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'RepositoryActionType'
	@RepositoryDefinition_RepositoryId [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'RepositoryDefinition'
	@Original_RepositoryStatId [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[RepositoryStat] 
WHERE 
	[RepositoryStatId] = @Original_RepositoryStatId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_RepositoryStat_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryStat_Insert]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryStat_Insert]
(
	@Count [Int],
	@Elapsed [Int],
	@ItemCount [Int],
	@RepositoryActionTypeId [Int],
	@RepositoryId [Int],
	@RepositoryStatId [Int] = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
IF (@RepositoryStatId < 0) SET @RepositoryStatId = NULL;
if ((@RepositoryStatId IS NULL))
BEGIN
INSERT INTO [dbo].[RepositoryStat]
(
	[Count],
	[Elapsed],
	[ItemCount],
	[RepositoryActionTypeId],
	[RepositoryId],
	[CreatedDate],
	[CreatedBy]
)
VALUES
(
	@Count,
	@Elapsed,
	@ItemCount,
	@RepositoryActionTypeId,
	@RepositoryId,
	@CreatedDate,
	@CreatedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[RepositoryStat] on
INSERT INTO [dbo].[RepositoryStat]
(
	[RepositoryStatId],
	[Count],
	[Elapsed],
	[ItemCount],
	[RepositoryActionTypeId],
	[RepositoryId],
	[CreatedDate],
	[CreatedBy]
)
VALUES
(
	@RepositoryStatId,
	@Count,
	@Elapsed,
	@ItemCount,
	@RepositoryActionTypeId,
	@RepositoryId,
	@CreatedDate,
	@CreatedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[RepositoryStat] off
END


SELECT 
	[dbo].[RepositoryStat].[Count],
	[dbo].[RepositoryStat].[Elapsed],
	[dbo].[RepositoryStat].[ItemCount],
	[dbo].[RepositoryStat].[RepositoryActionTypeId],
	[dbo].[RepositoryStat].[RepositoryId],
	[dbo].[RepositoryStat].[RepositoryStatId],
	[dbo].[RepositoryStat].[CreatedBy],
	[dbo].[RepositoryStat].[CreatedDate]

FROM
[dbo].[RepositoryStat]
WHERE
	[dbo].[RepositoryStat].[RepositoryStatId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_RepositoryStat_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_RepositoryStat_Update]
GO

CREATE PROCEDURE [dbo].[gen_RepositoryStat_Update]
(
	@Count [Int],
	@Elapsed [Int],
	@ItemCount [Int],
	@RepositoryActionTypeId [Int],
	@RepositoryId [Int],
	@Original_RepositoryStatId [Int]
)
AS

SET NOCOUNT ON;
UPDATE 
	[dbo].[RepositoryStat] 
SET
	[Count] = @Count,
	[Elapsed] = @Elapsed,
	[ItemCount] = @ItemCount,
	[RepositoryActionTypeId] = @RepositoryActionTypeId,
	[RepositoryId] = @RepositoryId

WHERE
	[dbo].[RepositoryStat].[RepositoryStatId] = @Original_RepositoryStatId


if (@@RowCount = 0) return;

SELECT
	[dbo].[RepositoryStat].[Count],
	[dbo].[RepositoryStat].[Elapsed],
	[dbo].[RepositoryStat].[ItemCount],
	[dbo].[RepositoryStat].[RepositoryActionTypeId],
	[dbo].[RepositoryStat].[RepositoryId],
	[dbo].[RepositoryStat].[RepositoryStatId],
	[dbo].[RepositoryStat].[CreatedBy],
	[dbo].[RepositoryStat].[CreatedDate]
FROM 
[dbo].[RepositoryStat]
WHERE
	[dbo].[RepositoryStat].[RepositoryStatId] = @Original_RepositoryStatId
GO

--This SQL is generated for internal stored procedures for table [ServerStat]
if exists(select * from sys.objects where name = 'gen_ServerStat_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_ServerStat_Delete]
GO

CREATE PROCEDURE [dbo].[gen_ServerStat_Delete]
(
	@Original_ServerStatId [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[ServerStat] 
WHERE 
	[ServerStatId] = @Original_ServerStatId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_ServerStat_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_ServerStat_Insert]
GO

CREATE PROCEDURE [dbo].[gen_ServerStat_Insert]
(
	@AddedDate [DateTime],
	@MemoryUsageAvailable [BigInt],
	@MemoryUsageProcess [BigInt],
	@MemoryUsageTotal [BigInt],
	@ProcessorUsage [Int],
	@RepositoryCreateDelta [BigInt],
	@RepositoryDeleteDelta [BigInt],
	@RepositoryInMem [Int],
	@RepositoryLoadDelta [Int],
	@RepositoryTotal [Int],
	@RepositoryUnloadDelta [Int],
	@ServerStatId [Int] = null

)
AS
SET NOCOUNT OFF;

IF (@ServerStatId < 0) SET @ServerStatId = NULL;
if ((@ServerStatId IS NULL))
BEGIN
INSERT INTO [dbo].[ServerStat]
(
	[AddedDate],
	[MemoryUsageAvailable],
	[MemoryUsageProcess],
	[MemoryUsageTotal],
	[ProcessorUsage],
	[RepositoryCreateDelta],
	[RepositoryDeleteDelta],
	[RepositoryInMem],
	[RepositoryLoadDelta],
	[RepositoryTotal],
	[RepositoryUnloadDelta]
)
VALUES
(
	@AddedDate,
	@MemoryUsageAvailable,
	@MemoryUsageProcess,
	@MemoryUsageTotal,
	@ProcessorUsage,
	@RepositoryCreateDelta,
	@RepositoryDeleteDelta,
	@RepositoryInMem,
	@RepositoryLoadDelta,
	@RepositoryTotal,
	@RepositoryUnloadDelta
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[ServerStat] on
INSERT INTO [dbo].[ServerStat]
(
	[ServerStatId],
	[AddedDate],
	[MemoryUsageAvailable],
	[MemoryUsageProcess],
	[MemoryUsageTotal],
	[ProcessorUsage],
	[RepositoryCreateDelta],
	[RepositoryDeleteDelta],
	[RepositoryInMem],
	[RepositoryLoadDelta],
	[RepositoryTotal],
	[RepositoryUnloadDelta]
)
VALUES
(
	@ServerStatId,
	@AddedDate,
	@MemoryUsageAvailable,
	@MemoryUsageProcess,
	@MemoryUsageTotal,
	@ProcessorUsage,
	@RepositoryCreateDelta,
	@RepositoryDeleteDelta,
	@RepositoryInMem,
	@RepositoryLoadDelta,
	@RepositoryTotal,
	@RepositoryUnloadDelta
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[ServerStat] off
END


SELECT 
	[dbo].[ServerStat].[AddedDate],
	[dbo].[ServerStat].[MemoryUsageAvailable],
	[dbo].[ServerStat].[MemoryUsageProcess],
	[dbo].[ServerStat].[MemoryUsageTotal],
	[dbo].[ServerStat].[ProcessorUsage],
	[dbo].[ServerStat].[RepositoryCreateDelta],
	[dbo].[ServerStat].[RepositoryDeleteDelta],
	[dbo].[ServerStat].[RepositoryInMem],
	[dbo].[ServerStat].[RepositoryLoadDelta],
	[dbo].[ServerStat].[RepositoryTotal],
	[dbo].[ServerStat].[RepositoryUnloadDelta],
	[dbo].[ServerStat].[ServerStatId]

FROM
[dbo].[ServerStat]
WHERE
	[dbo].[ServerStat].[ServerStatId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_ServerStat_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_ServerStat_Update]
GO

CREATE PROCEDURE [dbo].[gen_ServerStat_Update]
(
	@AddedDate [DateTime],
	@MemoryUsageAvailable [BigInt],
	@MemoryUsageProcess [BigInt],
	@MemoryUsageTotal [BigInt],
	@ProcessorUsage [Int],
	@RepositoryCreateDelta [BigInt],
	@RepositoryDeleteDelta [BigInt],
	@RepositoryInMem [Int],
	@RepositoryLoadDelta [Int],
	@RepositoryTotal [Int],
	@RepositoryUnloadDelta [Int],
	@Original_ServerStatId [Int]
)
AS

SET NOCOUNT ON;
UPDATE 
	[dbo].[ServerStat] 
SET
	[AddedDate] = @AddedDate,
	[MemoryUsageAvailable] = @MemoryUsageAvailable,
	[MemoryUsageProcess] = @MemoryUsageProcess,
	[MemoryUsageTotal] = @MemoryUsageTotal,
	[ProcessorUsage] = @ProcessorUsage,
	[RepositoryCreateDelta] = @RepositoryCreateDelta,
	[RepositoryDeleteDelta] = @RepositoryDeleteDelta,
	[RepositoryInMem] = @RepositoryInMem,
	[RepositoryLoadDelta] = @RepositoryLoadDelta,
	[RepositoryTotal] = @RepositoryTotal,
	[RepositoryUnloadDelta] = @RepositoryUnloadDelta

WHERE
	[dbo].[ServerStat].[ServerStatId] = @Original_ServerStatId


if (@@RowCount = 0) return;

SELECT
	[dbo].[ServerStat].[AddedDate],
	[dbo].[ServerStat].[MemoryUsageAvailable],
	[dbo].[ServerStat].[MemoryUsageProcess],
	[dbo].[ServerStat].[MemoryUsageTotal],
	[dbo].[ServerStat].[ProcessorUsage],
	[dbo].[ServerStat].[RepositoryCreateDelta],
	[dbo].[ServerStat].[RepositoryDeleteDelta],
	[dbo].[ServerStat].[RepositoryInMem],
	[dbo].[ServerStat].[RepositoryLoadDelta],
	[dbo].[ServerStat].[RepositoryTotal],
	[dbo].[ServerStat].[RepositoryUnloadDelta],
	[dbo].[ServerStat].[ServerStatId]
FROM 
[dbo].[ServerStat]
WHERE
	[dbo].[ServerStat].[ServerStatId] = @Original_ServerStatId
GO

--This SQL is generated for internal stored procedures for table [UserAccount]
if exists(select * from sys.objects where name = 'gen_UserAccount_Delete' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_UserAccount_Delete]
GO

CREATE PROCEDURE [dbo].[gen_UserAccount_Delete]
(
	@Original_UserId [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[UserAccount] 
WHERE 
	[UserId] = @Original_UserId;

if (@@RowCount = 0) return;


GO

if exists(select * from sys.objects where name = 'gen_UserAccount_Insert' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_UserAccount_Insert]
GO

CREATE PROCEDURE [dbo].[gen_UserAccount_Insert]
(
	@Password [VarChar] (50),
	@UniqueKey [UniqueIdentifier],
	@UserId [Int] = null,
	@UserName [VarChar] (50),
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@UserId < 0) SET @UserId = NULL;
if ((@UserId IS NULL))
BEGIN
INSERT INTO [dbo].[UserAccount]
(
	[Password],
	[UniqueKey],
	[UserName],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Password,
	@UniqueKey,
	@UserName,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[UserAccount] on
INSERT INTO [dbo].[UserAccount]
(
	[UserId],
	[Password],
	[UniqueKey],
	[UserName],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@UserId,
	@Password,
	@UniqueKey,
	@UserName,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[UserAccount] off
END


SELECT 
	[dbo].[UserAccount].[Password],
	[dbo].[UserAccount].[UniqueKey],
	[dbo].[UserAccount].[UserId],
	[dbo].[UserAccount].[UserName],
	[dbo].[UserAccount].[CreatedBy],
	[dbo].[UserAccount].[CreatedDate],
	[dbo].[UserAccount].[ModifiedBy],
	[dbo].[UserAccount].[ModifiedDate],
	[dbo].[UserAccount].[Timestamp]

FROM
[dbo].[UserAccount]
WHERE
	[dbo].[UserAccount].[UserId] = SCOPE_IDENTITY();
GO

if exists(select * from sys.objects where name = 'gen_UserAccount_Update' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')
	drop procedure [dbo].[gen_UserAccount_Update]
GO

CREATE PROCEDURE [dbo].[gen_UserAccount_Update]
(
	@Password [VarChar] (50),
	@UniqueKey [UniqueIdentifier],
	@UserName [VarChar] (50),
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_UserId [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[UserAccount] 
SET
	[Password] = @Password,
	[UniqueKey] = @UniqueKey,
	[UserName] = @UserName,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[UserAccount].[UserId] = @Original_UserId AND
	[dbo].[UserAccount].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[UserAccount].[Password],
	[dbo].[UserAccount].[UniqueKey],
	[dbo].[UserAccount].[UserId],
	[dbo].[UserAccount].[UserName],
	[dbo].[UserAccount].[CreatedBy],
	[dbo].[UserAccount].[CreatedDate],
	[dbo].[UserAccount].[ModifiedBy],
	[dbo].[UserAccount].[ModifiedDate],
	[dbo].[UserAccount].[Timestamp]
FROM 
[dbo].[UserAccount]
WHERE
	[dbo].[UserAccount].[UserId] = @Original_UserId
GO

--##SECTION END [INTERNAL STORED PROCS]

