using EntityDataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;

namespace DataAccessLayer
{
    public static class DALUtility<T>
    {
        #region xml DAL
        private static string xmlPath = "Database.xml";

        public static List<T> ReadFile(string parentNode, string childNodes)
        {
            List<T> genericList = new List<T>();
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);

                XmlNodeList nodes = xmlDoc.SelectNodes($"/Library/{parentNode}/{childNodes}");

                foreach (XmlNode xmlNode in nodes)
                {
                    XmlAttributeCollection attributes = xmlNode.Attributes;

                    switch (childNodes)
                    {
                        case "User":
                            User user = new User();

                            user.UserId = Int32.Parse(attributes.GetNamedItem(nameof(User.UserId)).Value);
                            user.Username = attributes.GetNamedItem(nameof(User.Username)).Value;
                            user.Password = attributes.GetNamedItem(nameof(User.Password)).Value;
                            user.Role = (User.UserRole)Enum.Parse(typeof(User.UserRole), attributes.GetNamedItem(nameof(User.Role)).Value);

                            genericList.Add((T)(object)user);
                            break;

                        case "Book":
                            Book book = new Book();

                            book.BookId = Int32.Parse(attributes.GetNamedItem(nameof(Book.BookId)).Value);
                            book.Title = attributes.GetNamedItem(nameof(Book.Title)).Value;
                            book.AuthorName = attributes.GetNamedItem(nameof(Book.AuthorName)).Value;
                            book.AuthorSurname = attributes.GetNamedItem(nameof(Book.AuthorSurname)).Value;
                            book.Publisher = attributes.GetNamedItem(nameof(Book.Publisher)).Value;
                            book.Quantity = Int32.Parse(attributes.GetNamedItem(nameof(Book.Quantity)).Value);

                            genericList.Add((T)(object)book);
                            break;

                        case "Reservation":
                            Reservation reservation = new Reservation();

                            reservation.ReservationId = Int32.Parse(attributes.GetNamedItem(nameof(Reservation.ReservationId)).Value);
                            reservation.UserId = Int32.Parse(attributes.GetNamedItem(nameof(Reservation.UserId)).Value);
                            reservation.BookId = Int32.Parse(attributes.GetNamedItem(nameof(Reservation.BookId)).Value);
                            reservation.StartDate = DateTime.Parse(attributes.GetNamedItem(nameof(Reservation.StartDate)).Value);
                            reservation.EndDate = DateTime.Parse(attributes.GetNamedItem(nameof(Reservation.EndDate)).Value);

                            genericList.Add((T)(object)reservation);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return genericList;
        }

        public static void WriteFile(string parentNode, string childNodes, T item)
        {
            ReadFile(parentNode, childNodes);

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);

                XmlNode rootNode = xmlDoc.SelectSingleNode($"/Library/{parentNode}");

                if (rootNode.SelectNodes(childNodes) == null)
                {
                    rootNode = xmlDoc.CreateElement(parentNode);
                    xmlDoc.AppendChild(rootNode);
                }

                switch (childNodes)
                {
                    case "Book":
                        XmlNode bookNode = xmlDoc.CreateElement(childNodes);

                        AddXmlAttribute(xmlDoc, bookNode, "BookId", (item as Book).BookId.ToString());
                        AddXmlAttribute(xmlDoc, bookNode, "Title", (item as Book).Title);
                        AddXmlAttribute(xmlDoc, bookNode, "AuthorName", (item as Book).AuthorName);
                        AddXmlAttribute(xmlDoc, bookNode, "AuthorSurname", (item as Book).AuthorSurname);
                        AddXmlAttribute(xmlDoc, bookNode, "Publisher", (item as Book).Publisher);
                        AddXmlAttribute(xmlDoc, bookNode, "Quantity", (item as Book).Quantity.ToString());

                        rootNode.AppendChild(bookNode);
                        xmlDoc.Save(xmlPath);
                        break;

                    case "Reservation":
                        XmlNode reservationNode = xmlDoc.CreateElement(childNodes);

                        AddXmlAttribute(xmlDoc, reservationNode, "ReservationId", (item as Reservation).ReservationId.ToString());
                        AddXmlAttribute(xmlDoc, reservationNode, "UserId", (item as Reservation).UserId.ToString());
                        AddXmlAttribute(xmlDoc, reservationNode, "BookId", (item as Reservation).BookId.ToString());
                        AddXmlAttribute(xmlDoc, reservationNode, "StartDate", (item as Reservation).StartDate.ToShortDateString()); //
                        AddXmlAttribute(xmlDoc, reservationNode, "EndDate", (item as Reservation).EndDate.ToShortDateString()); //

                        rootNode.AppendChild(reservationNode);
                        xmlDoc.Save(xmlPath);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void AddXmlAttribute(XmlDocument xmlDoc, XmlNode node, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = xmlDoc.CreateAttribute(attributeName);
            attribute.Value = attributeValue;

            node.Attributes.Append(attribute);
        }

        public static XmlNode FindBookNode(XmlDocument xmlDoc, Book book)
        {
            string xpath = $"/Library/Books/Book[" +
                $"translate(@Title, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') " +
                $"= '{book.Title.ToLowerInvariant()}' and " +
                $"translate(@AuthorName, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') " +
                $"= '{book.AuthorName.ToLowerInvariant()}' and " +
                $"translate(@AuthorSurname, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') " +
                $"= '{book.AuthorSurname.ToLowerInvariant()}' and " +
                $"translate(@Publisher, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') " +
                $"= '{book.Publisher.ToLowerInvariant()}']";

            return xmlDoc.SelectSingleNode(xpath);
        }

        public static XmlNode FindReservationNode(XmlDocument xmlDoc, Reservation reservation)
        {
            string xpath = $"/Library/Reservations/Reservation[@ReservationId='{reservation.ReservationId}']";

            return xmlDoc.SelectSingleNode(xpath);
        }
        #endregion


        #region db DAL

        public static void UsersRead(User user, SqlDataReader reader)
        {
            user.UserId = reader.GetInt32(0); 
            user.Username = reader.GetString(1);
            Enum.TryParse(reader.GetString(2), out User.UserRole role);
            user.Role = role;
        }

        public static void BooksRead(Book book, SqlDataReader reader)
        {
            book.BookId = reader.GetInt32(0);
            book.Title = reader.GetString(1);
            book.AuthorName = reader.GetString(2);
            book.AuthorSurname = reader.GetString(3);
            book.Publisher = reader.GetString(4);
            book.Quantity = reader.GetInt32(5);
        }

        public static void BooksWrite(SqlCommand command, Book book)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@BookId", book.BookId);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@AuthorName", book.AuthorName);
            command.Parameters.AddWithValue("@AuthorSurname", book.AuthorSurname);
            command.Parameters.AddWithValue("@Publisher", book.Publisher);
            command.Parameters.AddWithValue("@Quantity", book.Quantity);
            command.ExecuteNonQuery();
        }

        public static void BooksDelete(SqlCommand command, Book book)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BookId", book.BookId);
            command.ExecuteNonQuery();
        }

        public static void ReservationsRead(Reservation reservation, SqlDataReader reader)
        {
            reservation.ReservationId = reader.GetInt32(0);
            reservation.UserId = reader.GetInt32(1);
            reservation.BookId = reader.GetInt32(2);
            reservation.StartDate = reader.GetDateTime(3);
            reservation.EndDate = reader.GetDateTime(4);
        }

        public static void ReservationsSearchResultRead(ReservationsSearchResult reservation, SqlDataReader reader)
        {
            reservation.BookTitle = reader.GetString(0);
            reservation.UserUsername = reader.GetString(1);
            reservation.StartDate = reader.GetDateTime(2);
            reservation.EndDate = reader.GetDateTime(3);
            reservation.State = reader.GetString(4);
        }

        public static void ReservationsWrite(SqlCommand command, Reservation reservation)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", reservation.UserId);
            command.Parameters.AddWithValue("@BookId", reservation.BookId);
            command.Parameters.AddWithValue("@StartDate", reservation.StartDate);
            command.Parameters.AddWithValue("@EndDate", reservation.EndDate);
            command.ExecuteNonQuery();
        }

        public static void ReservationsDelete (SqlCommand command, int bookId)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BookId", bookId);
            command.ExecuteNonQuery();
        }

        #endregion
    }
}