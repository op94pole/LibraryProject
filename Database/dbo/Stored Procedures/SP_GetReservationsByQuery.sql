-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReservationsByQuery]
	-- Add the parameters for the stored procedure here
	@SubQuery varchar(50), 
	@ConnectedUserId varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ConnectedUserRole varchar(50);

    SELECT @ConnectedUserRole = Role
    FROM Users
    WHERE UserId = @ConnectedUserId;

    -- Insert statements for procedure here
	WITH ReservationDetails AS (
        SELECT 
            b.Title, 
            u.Username, 
            r.StartDate, 
            r.EndDate,
            CASE 
                WHEN r.EndDate > GETDATE() THEN 'attiva'
                WHEN r.EndDate < GETDATE() THEN 'non attiva' 
            END AS ReservationState
        FROM 
            Reservations r
            JOIN Books b ON r.BookId = b.BookId 
            JOIN Users u ON r.UserId = u.UserId
    )
    SELECT
        rd.Title,
        rd.Username,
        rd.StartDate,
        rd.EndDate,
        rd.ReservationState
    FROM 
        ReservationDetails rd
        JOIN Users u ON rd.Username = u.Username
        JOIN Books b ON rd.Title = b.Title
        JOIN Reservations r ON (rd.StartDate = r.StartDate AND rd.EndDate = r.EndDate AND rd.Username = u.Username)
    WHERE 
        (
            @ConnectedUserRole = 'Admin' AND 
            (
                rd.Username LIKE '%' + @SubQuery + '%' OR
                rd.Title LIKE '%' + @SubQuery + '%' OR
                b.AuthorName LIKE '%' + @SubQuery + '%' OR
                b.AuthorSurname LIKE '%' + @SubQuery + '%' OR
                b.Publisher LIKE '%' + @SubQuery + '%' OR
                rd.ReservationState LIKE @SubQuery + '%'
            )
        )
        OR
        (
            @ConnectedUserRole = 'User' AND u.UserId = @ConnectedUserId AND
            (
                rd.Title LIKE '%' + @SubQuery + '%' OR
                b.AuthorName LIKE '%' + @SubQuery + '%' OR
                b.AuthorSurname LIKE '%' + @SubQuery + '%' OR
                b.Publisher LIKE '%' + @SubQuery + '%' OR
                rd.ReservationState LIKE @SubQuery + '%'
            )
        )
		order by rd.EndDate, rd.Title, rd.Username
END