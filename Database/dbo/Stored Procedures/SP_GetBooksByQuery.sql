-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetBooksByQuery] 
	-- Add the parameters for the stored procedure here
	@SubQuery varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Books b
	WHERE b.Title like '%' + @SubQuery + '%' or
	b.AuthorName like '%' + @SubQuery + '%' or
	b.AuthorSurname like '%' + @SubQuery + '%' or
	b.Publisher like '%' + @SubQuery + '%'
	ORDER BY b.BookId
END