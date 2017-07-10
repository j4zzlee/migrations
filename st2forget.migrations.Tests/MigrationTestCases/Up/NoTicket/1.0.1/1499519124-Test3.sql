ALTER TABLE [dbo].[User] ADD [AccessFailedCount] int NULL;
ALTER TABLE [dbo].[User] ADD [ConcurrencyStamp] [nvarchar](max) NULL;
ALTER TABLE [dbo].[User] ADD [EmailConfirmed] [bit] NULL;
ALTER TABLE [dbo].[User] ADD [PhoneNumberConfirmed] [bit] NULL;
ALTER TABLE [dbo].[User] ADD [LockoutEnabled] [bit] NULL;
ALTER TABLE [dbo].[User] ADD [LockoutEnd] [datetimeoffset](7) NULL;
ALTER TABLE [dbo].[User] ADD [NormalizedEmail] [nvarchar](256) NULL;
ALTER TABLE [dbo].[User] ADD [NormalizedUserName] [nvarchar](256) NULL;
ALTER TABLE [dbo].[User] ADD [TwoFactorEnabled] [bit] NULL;

--Down--
ALTER TABLE [dbo].[User] DROP COLUMN [AccessFailedCount]
ALTER TABLE [dbo].[User] DROP COLUMN [ConcurrencyStamp]
ALTER TABLE [dbo].[User] DROP COLUMN [EmailConfirmed]
ALTER TABLE [dbo].[User] DROP COLUMN [PhoneNumberConfirmed]
ALTER TABLE [dbo].[User] DROP COLUMN [LockoutEnabled]
ALTER TABLE [dbo].[User] DROP COLUMN [LockoutEnd]
ALTER TABLE [dbo].[User] DROP COLUMN [NormalizedEmail]
ALTER TABLE [dbo].[User] DROP COLUMN [NormalizedUserName]
ALTER TABLE [dbo].[User] DROP COLUMN [TwoFactorEnabled]
