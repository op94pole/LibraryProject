using DataAccessLayer;
using EntityDataModel;
using EntityDataModel.CustomExceptions;
using EntityDataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class BookBL
    {
        //private static string _xmlPath;
        //private IUserDAO userDAO;
        //private IBookDAO bookDAO;
        //private IReservationDAO reservationDAO;

        //public BookBL(string xmlPath)
        //{
        //    _xmlPath = xmlPath;
        //    userDAO = new UserDAOForXML(_xmlPath);
        //    bookDAO = new BookDAOForXML(_xmlPath);
        //    reservationDAO = new ReservationDAOForXML(_xmlPath);
        //}

        static string _connectionString;
        readonly IUserDAO userDAO;
        readonly IBookDAO bookDAO;
        readonly IReservationDAO reservationDAO;

        public BookBL(string connectionString)
        {
            _connectionString = connectionString;
            userDAO = new UserDAOForDB(_connectionString);
            bookDAO = new BookDAOForDB(_connectionString);
            reservationDAO = new ReservationDAOForDB(_connectionString);
        }

        public List<Book> SearchBooks(string query)
        {
            var filteredBooksList = new List<Book>();

            try
            {
                filteredBooksList = bookDAO.GetBooksByQuery(query);

                if (filteredBooksList.Count == 0)
                {
                    throw new NoMatchingResultsException();
                }
            }
            catch { }

            return filteredBooksList;
        }

        public void ModifyBook(Book bookToUpdate, string title, string authorName, string authorSurname, string publisher)
        {
            try
            {
                List<Book> booksList = bookDAO.GetBooks();
                var updatedBook = new Book()
                {
                    Title = title,
                    AuthorName = authorName,
                    AuthorSurname = authorSurname,
                    Publisher = publisher
                };

                if (BLUtility.AlreadyExistingBook(updatedBook, booksList) != null)
                {
                    throw new AlreadyExistingBookException("Already existing book.");
                }

                bookToUpdate.Title = title;
                bookToUpdate.AuthorName = authorName;
                bookToUpdate.AuthorSurname = authorSurname;
                bookToUpdate.Publisher = publisher;

                bookDAO.UpdateBook(bookToUpdate);
            }
            catch { }
        }

        public void InsertBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            try
            {
                List<Book> booksList = bookDAO.GetBooks();
                var newBook = new Book()
                {
                    Title = title,
                    AuthorName = authorName,
                    AuthorSurname = authorSurname,
                    Publisher = publisher,
                    Quantity = quantity
                };

                if (BLUtility.AlreadyExistingBook(newBook, booksList) == null)
                {
                    newBook.BookId = booksList.Count == 0 ? 1 : booksList[booksList.Count - 1].BookId + 1;

                    bookDAO.CreateBook(newBook);
                }
                else
                {
                    var existingBook = BLUtility.AlreadyExistingBook(newBook, booksList);
                    existingBook.Quantity += newBook.Quantity;

                    bookDAO.UpdateBook(existingBook);
                }
            }
            catch { }
        }

        public void RemoveBook(Book bookToDelete, out string response)
        {
            response = "";
            var booksReservations = new List<Reservation>();

            try
            {
                booksReservations = reservationDAO.GetReservations().Where(r =>
                    r.BookId == bookToDelete.BookId && r.EndDate > DateTime.Now).ToList();
            }
            catch { }

            if (booksReservations.Count > 0)
            {
                string currentResponse = "";
                int counter = 0;

                foreach (Reservation reservation in booksReservations)
                {
                    try
                    {
                        User associatedUser = userDAO.GetUsers().SingleOrDefault(u => u.UserId == reservation.UserId);
                        Book associatedBook = bookDAO.GetBooks().SingleOrDefault(b => b.BookId == reservation.BookId);

                        counter++;
                        currentResponse = $"{counter}. {associatedBook.Title} - {associatedBook.AuthorName} {associatedBook.AuthorSurname}" +
                            $" - {associatedBook.Publisher}, {associatedUser.Username}, {reservation.StartDate:dd/MM/yyyy} " +
                            $"- {reservation.EndDate:dd/MM/yyyy}\n";
                        response += currentResponse;
                    }
                    catch (Exception ex) { }
                }

                throw new ExistingBookReservationException("Existing book reservations.");
            }

            try
            {
                reservationDAO.DeleteReservations(bookToDelete.BookId);
                bookDAO.DeleteBook(bookToDelete);
            }
            catch { }
        }
    }
}