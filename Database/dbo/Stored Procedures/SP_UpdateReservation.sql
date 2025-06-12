-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdateReservation]
	-- Add the parameters for the stored procedure here
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
	update Reservations 
	set EndDate = @EndDate where UserId = @UserId 
	and BookId = @BookId
	and StartDate = @StartDate 

END