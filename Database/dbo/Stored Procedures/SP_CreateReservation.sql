-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SP_CreateReservation 
	-- Add the parameters for the stored procedure here@UserId int,
	@UserId int,
	@BookId int,
	@StartDate date,
	@EndDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into Reservations (UserId, BookId, StartDate, EndDate)
	values (@UserId, @BookId, @StartDate, @EndDate)
END