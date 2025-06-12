-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdateBook] 
	-- Add the parameters for the stored procedure here	
	@BookId int,
	@Title VARCHAR(100) = NULL,
	@AuthorName VARCHAR(50) = NULL,
	@AuthorSurname VARCHAR(50) = NULL,
	@Publisher VARCHAR(50) = NULL,
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Books 
	SET Title = @Title, 
	AuthorName = @AuthorName, 
	AuthorSurname = @AuthorSurname,
	Publisher = @Publisher,
	Quantity = @Quantity
	WHERE BookId = @BookId
END