using EntityDataModel;
using EntityDataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DataAccessLayer
{
    public class BookDAOForXML : IBookDAO
    {
        private readonly string _xmlPath;

        public BookDAOForXML(string xmlPath) 
        {
            _xmlPath = xmlPath;
        }

        public List<Book> GetBooks()
        {
            List<Book> booksList = DALUtility<Book>.ReadFile("Books", "Book");

            return booksList;
        }

        public List<Book> GetBooksByQuery(string query)
        {
            string[] splittedQuery = query.Split(' ');
            List<Book> filteredBooksList = GetBooks().Where(b => splittedQuery.Any(s =>
                b.Title.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.AuthorName.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.AuthorSurname.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.Publisher.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

            return filteredBooksList;
        }

        public void UpdateBook(Book updatedBook)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(_xmlPath);

                string xpath = $"/Library/Books/Book[@BookId='{updatedBook.BookId}']";
                XmlNode bookNode = xmlDoc.SelectSingleNode(xpath);

                bookNode.Attributes["Title"].Value = updatedBook.Title;
                bookNode.Attributes["AuthorName"].Value = updatedBook.AuthorName;
                bookNode.Attributes["AuthorSurname"].Value = updatedBook.AuthorSurname;
                bookNode.Attributes["Publisher"].Value = updatedBook.Publisher;
                bookNode.Attributes["Quantity"].Value = updatedBook.Quantity.ToString();

                xmlDoc.Save(_xmlPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CreateBook(Book newBook)
        {
            DALUtility<Book>.WriteFile("Books", "Book", newBook);
        }

        public void DeleteBook(Book bookToDelete)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(_xmlPath);

                XmlNode bookNode = DALUtility<Book>.FindBookNode(xmlDoc, bookToDelete);
                XmlNode parentNode = bookNode.ParentNode;

                List<Reservation> bookAssociatedReservations = DALUtility<Reservation>.ReadFile("Reservations", "Reservation").Where(r =>
                r.BookId == bookToDelete.BookId).ToList(); 

                foreach (var reservation in bookAssociatedReservations) 
                {
                    XmlNode reservationNode = DALUtility<Reservation>.FindReservationNode(xmlDoc, reservation); 
                    XmlNode _parentNode = reservationNode.ParentNode; 

                    _parentNode.RemoveChild(reservationNode); 
                }

                parentNode.RemoveChild(bookNode);
                xmlDoc.Save(_xmlPath); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}