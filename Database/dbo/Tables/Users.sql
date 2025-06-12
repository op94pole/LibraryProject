CREATE TABLE [dbo].[Users] (
    [UserId]   INT                                               IDENTITY (1, 1) NOT NULL,
    [Username] VARCHAR (50)                                      NOT NULL,
    [Password] VARCHAR (50) MASKED WITH (FUNCTION = 'default()') NOT NULL,
    [Role]     VARCHAR (10)                                      NOT NULL,
    CONSTRAINT [PK__Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [CK__Users__Role] CHECK ([Role]='User' OR [Role]='Admin'),
    CONSTRAINT [UQ__Users] UNIQUE NONCLUSTERED ([Username] ASC, [Password] ASC)
);





