ALTER TABLE [dbo].[User] ADD CONSTRAINT UQ_User_Email UNIQUE (Email);
ALTER TABLE [dbo].[User] ADD CONSTRAINT UQ_User_PhoneNumber UNIQUE (PhoneNumber);
ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_AccessFailedCount DEFAULT 0 FOR [AccessFailedCount];
ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_EmailConfirmed DEFAULT 0 FOR [EmailConfirmed];
ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_PhoneNumberConfirmed DEFAULT 0 FOR [PhoneNumberConfirmed];
ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_LockoutEnabled DEFAULT 0 FOR [LockoutEnabled];
ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_TwoFactorEnabled DEFAULT 0 FOR [TwoFactorEnabled];
ALTER TABLE [dbo].[User] ADD CONSTRAINT DF_User_Gender DEFAULT 0 FOR [Gender];

UPDATE [dbo].[User]
SET AccessFailedCount = case when AccessFailedCount is null then 0 else AccessFailedCount end,
EmailConfirmed = case when EmailConfirmed is null then 0 else EmailConfirmed end,
PhoneNumberConfirmed = case when PhoneNumberConfirmed is null then 0 else PhoneNumberConfirmed end,
LockoutEnabled = case when LockoutEnabled is null then 0 else LockoutEnabled end,
TwoFactorEnabled = case when TwoFactorEnabled is null then 0 else TwoFactorEnabled end,
Gender = case when Gender is null then 0 else Gender end

--Down--
ALTER TABLE [dbo].[User] DROP CONSTRAINT UQ_User_Email;
ALTER TABLE [dbo].[User] DROP CONSTRAINT UQ_User_PhoneNumber;
ALTER TABLE [dbo].[User] DROP CONSTRAINT DF_User_AccessFailedCount;
ALTER TABLE [dbo].[User] DROP CONSTRAINT DF_User_EmailConfirmed;
ALTER TABLE [dbo].[User] DROP CONSTRAINT DF_User_PhoneNumberConfirmed;
ALTER TABLE [dbo].[User] DROP CONSTRAINT DF_User_LockoutEnabled;
ALTER TABLE [dbo].[User] DROP CONSTRAINT DF_User_TwoFactorEnabled;
ALTER TABLE [dbo].[User] DROP CONSTRAINT DF_User_Gender;