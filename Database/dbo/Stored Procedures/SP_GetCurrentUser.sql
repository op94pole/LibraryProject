-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SP_GetCurrentUser
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(50) = NULL,
	@Password VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT UserId, Username, Role FROM Users
	WHERE Username = @Username AND Password = @Password
END