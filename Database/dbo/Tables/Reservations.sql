CREATE TABLE [dbo].[Reservations] (
    [ReservationId] INT  IDENTITY (1, 1) NOT NULL,
    [UserId]        INT  NOT NULL,
    [BookId]        INT  NOT NULL,
    [StartDate]     DATE NOT NULL,
    [EndDate]       DATE NOT NULL,
    CONSTRAINT [PK__Reservations] PRIMARY KEY CLUSTERED ([ReservationId] ASC),
    CONSTRAINT [FK__Reservations_Books] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]),
    CONSTRAINT [FK__Reservations_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);





