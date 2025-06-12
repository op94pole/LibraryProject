using System.Collections.Generic;

namespace EntityDataModel.Interfaces
{
    public interface IBookDAO
    {
        List<Book> GetBooks();
        List<Book> GetBooksByQuery(string search);
        void UpdateBook(Book updatedBook);
        void CreateBook(Book newBook);
        void DeleteBook(Book bookToDelete);
    }
}