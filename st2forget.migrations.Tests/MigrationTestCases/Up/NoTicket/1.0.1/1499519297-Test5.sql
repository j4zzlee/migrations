ALTER TABLE [dbo].[User] ALTER COLUMN [AccessFailedCount] int NOT NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [EmailConfirmed] bit NOT NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [LockoutEnabled] bit NOT NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [PhoneNumberConfirmed] bit NOT NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [TwoFactorEnabled] bit NOT NULL
--Down--
ALTER TABLE [dbo].[User] ALTER COLUMN [TwoFactorEnabled] bit NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [PhoneNumberConfirmed] bit NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [LockoutEnabled] bit NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [EmailConfirmed] bit NULL
ALTER TABLE [dbo].[User] ALTER COLUMN [AccessFailedCount] int NULL