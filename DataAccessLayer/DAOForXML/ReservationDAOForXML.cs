using EntityDataModel;
using EntityDataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DataAccessLayer
{
    public class ReservationDAOForXML : IReservationDAO
    {
        private readonly string _xmlPath;

        public ReservationDAOForXML(string xmlPath)
        {
            _xmlPath = xmlPath;
        }

        public List<Reservation> GetReservations()
        {
            List<Reservation> reservationsList = DALUtility<Reservation>.ReadFile("Reservations", "Reservation");

            return reservationsList;
        }

        public List<ReservationsSearchResult> GetReservationsByQuery(User currentUser, string query)
        {
            string[] splittedQuery = query.Split(' ');
            var list = new List<ReservationsSearchResult>();

            foreach (var reservation in GetReservations())
            {
                try
                {
                    User associatedUser = DALUtility<User>.ReadFile("Users", "User").SingleOrDefault(u =>
                    u.UserId == reservation.UserId);

                    Book associatedBook = DALUtility<Book>.ReadFile("Books", "Book").SingleOrDefault(b =>
                    b.BookId == reservation.BookId);

                    string state = reservation.EndDate > DateTime.Now ? "attiva" : "non attiva";

                    bool userFilter = currentUser.Role == User.UserRole.Admin && splittedQuery.Any(s =>
                        associatedUser.Username.ToLower().Contains(s.ToLower()));

                    bool bookFilter = splittedQuery.Any(s => associatedBook.Title.ToLower().Contains(s.ToLower()) ||
                        associatedBook.AuthorName.ToLower().Contains(s.ToLower()) ||
                        associatedBook.AuthorSurname.ToLower().Contains(s.ToLower()) ||
                        associatedBook.Publisher.ToLower().Contains(s.ToLower()));

                    bool stateFilter = state.Equals(query, StringComparison.OrdinalIgnoreCase);

                    if ((currentUser.Role == User.UserRole.Admin && (userFilter || bookFilter || stateFilter)) ||
                        (currentUser.Role == User.UserRole.Admin && (reservation.UserId == currentUser.UserId &&
                        (bookFilter || stateFilter))))
                    {
                        ReservationsSearchResult rsr = new ReservationsSearchResult()
                        {
                            BookTitle = associatedBook.Title,
                            UserUsername = associatedUser.Username,
                            StartDate = reservation.StartDate,
                            EndDate = reservation.EndDate,
                            State = state,
                        };

                        list.Add(rsr);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return list;
        }

        public void CreateReservation(Reservation newReservation)
        {
            DALUtility<Reservation>.WriteFile("Reservations", "Reservation", newReservation);
        }

        public void UpdateReservation(Reservation reservationToUpdate)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(_xmlPath);

                XmlNode reservationNode = DALUtility<Reservation>.FindReservationNode(xmlDoc, reservationToUpdate);
                reservationNode.Attributes["EndDate"].Value = DateTime.Now.Date.ToString();

                xmlDoc.Save(_xmlPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteReservations(int bookId)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(_xmlPath);

                var bookAssociatedReservations = (List<Reservation>)DALUtility<Reservation>.ReadFile("Reservations", "Reservation").Where(r =>
                r.BookId == bookId);

                foreach (var reservation in bookAssociatedReservations)
                {
                    XmlNode reservationNode = DALUtility<Reservation>.FindReservationNode(xmlDoc, reservation);
                    XmlNode parentNode = reservationNode.ParentNode;

                    parentNode.RemoveChild(reservationNode);
                }

                xmlDoc.Save(_xmlPath);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}