-- [AspNetRoleClaims]
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE


ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]

-- [AspNetUserClaims]
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_User_UserId]

-- [AspNetUserLogins]
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_User_UserId]

-- [AspNetUserRoles]
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_User_UserId]

--Down--
ALTER TABLE [dbo].[AspNetRoleClaims]  DROP  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
ALTER TABLE [dbo].[AspNetUserClaims]  DROP  CONSTRAINT [FK_AspNetUserClaims_User_UserId]
ALTER TABLE [dbo].[AspNetUserLogins]  DROP  CONSTRAINT [FK_AspNetUserLogins_User_UserId]
ALTER TABLE [dbo].[AspNetUserRoles]  DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
ALTER TABLE [dbo].[AspNetUserRoles]  DROP  CONSTRAINT [FK_AspNetUserRoles_User_UserId]