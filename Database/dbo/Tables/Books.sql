CREATE TABLE [dbo].[Books] (
    [BookId]        INT          IDENTITY (1, 1) NOT NULL,
    [Title]         VARCHAR (50) NOT NULL,
    [AuthorName]    VARCHAR (50) NOT NULL,
    [AuthorSurname] VARCHAR (50) NOT NULL,
    [Publisher]     VARCHAR (50) NOT NULL,
    [Quantity]      INT          NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([BookId] ASC),
    CONSTRAINT [UQ__Books] UNIQUE NONCLUSTERED ([Title] ASC, [AuthorName] ASC, [AuthorSurname] ASC, [Publisher] ASC)
);



