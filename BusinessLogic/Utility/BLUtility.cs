using EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class BLUtility
    {
        public static Book AlreadyExistingBook(Book book, List<Book> booksList)
        {
            var alreadyExistingBook = new Book();

            try
            {
                alreadyExistingBook = booksList.Where(b =>
                String.Equals(b.Title, book.Title, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(b.AuthorName, book.AuthorName, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(b.AuthorSurname, book.AuthorSurname, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(b.Publisher, book.Publisher, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            }
            catch { }

            return alreadyExistingBook;
        }
    }
}