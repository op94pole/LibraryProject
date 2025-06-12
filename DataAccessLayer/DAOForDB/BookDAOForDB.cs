using EntityDataModel;
using EntityDataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer
{
    public class BookDAOForDB : IBookDAO
    {
        private readonly string _connectionString;

        public BookDAOForDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Book> GetBooks()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_GetBooks", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book();

                            DALUtility<Book>.BooksRead(book, reader);
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public List<Book> GetBooksByQuery(string selectQuery)
        {
            string[] splittedQuery = selectQuery.Split(' ');
            var allRetrievedBooks = new List<Book>();

            if (String.IsNullOrEmpty(selectQuery))
            {
                return GetBooks();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var subQuery in splittedQuery)
                {
                    using (var command = new SqlCommand("SP_GetBooksByQuery", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SubQuery", subQuery);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var book = new Book();

                                DALUtility<Book>.BooksRead(book, reader);
                                allRetrievedBooks.Add(book);
                            }
                        }
                    }
                }

                if (splittedQuery.Count() == 1)
                {
                    return allRetrievedBooks;
                }

                return allRetrievedBooks.GroupBy(b => b.BookId).ToList().Where(g => g.Count() > 1).
                    Select(g => g.First()).ToList(); 
            }
        }

        public void UpdateBook(Book updatedBook)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_UpdateBook", connection))
                {
                    DALUtility<Book>.BooksWrite(command, updatedBook);
                }
            }
        }

        public void CreateBook(Book newBook)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_CreateBook", connection))
                {
                    DALUtility<Book>.BooksWrite(command, newBook);
                }
            }
        }

        public void DeleteBook(Book bookToDelete)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_DeleteBook", connection))
                {
                    DALUtility<Book>.BooksDelete(command, bookToDelete);
                }
            }
        }
    }
}