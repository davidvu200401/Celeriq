--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Static Data For Version 3.0.0.0

if (SERVERPROPERTY('EngineEdition') <> 5) --NOT AZURE
exec sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

--INSERT STATIC DATA FOR TABLE [RepositoryActionType]
SET identity_insert [dbo].[RepositoryActionType] on
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 1)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('Query',1)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 2)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('LoadData',2)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 3)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('SaveData',3)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 4)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('UnloadData',4)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 5)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('Reset',5)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 6)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('ExportSchema',6)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 7)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('Backup',7)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 8)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('Restore',8)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 9)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('Compress',9)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 10)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('Shutdown',10)
if not exists(select * from [dbo].[RepositoryActionType] where ([RepositoryActionTypeId] = 11)) INSERT INTO [dbo].[RepositoryActionType] ([Name],[RepositoryActionTypeId]) values ('DeleteData',11)
SET identity_insert [dbo].[RepositoryActionType] off

--INSERT STATIC DATA FOR TABLE [UserAccount]
SET identity_insert [dbo].[UserAccount] on
if not exists(select * from [dbo].[UserAccount] where ([UserId] = 1)) INSERT INTO [dbo].[UserAccount] ([Password],[UniqueKey],[UserId],[UserName]) values ('password','00000000-0000-0000-0000-000000000000',1,'root')
SET identity_insert [dbo].[UserAccount] off

if (SERVERPROPERTY('EngineEdition') <> 5) --NOT AZURE
exec sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'

