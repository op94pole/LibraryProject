-- =============================================
-- Author:		<Luca Pulga>
-- Create date: <2024/05/20>
-- Description:	<Return all Books records>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetBooks] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Books b
	ORDER BY b.BookId
END