--Generated Upgrade For Version 3.0.0.0.22
--Generated on 2013-12-22 15:14:49

--ADD COLUMN [RepositoryData].[Keyword]
if exists(select * from sys.objects where name = 'RepositoryData' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Keyword' and o.name = 'RepositoryData')
ALTER TABLE [dbo].[RepositoryData] ADD [Keyword] [VarChar] (max) NULL 

GO

