using DataAccessLayer;
using EntityDataModel;
using EntityDataModel.CustomExceptions;
using EntityDataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class ReservationBL
    {
        //private static string _xmlPath;
        //private IReservationDAO reservationDAO;

        //public BookBL(string xmlPath)
        //{
        //    _xmlPath = xmlPath;
        //    reservationDAO = new ReservationDAOForXML(_xmlPath);
        //}

        private static string _connectionString;
        private static int _reservationDuration;
        private IReservationDAO reservationDAO;

        public ReservationBL(string connectionString, int reservationDuration)
        {
            _connectionString = connectionString;
            _reservationDuration = reservationDuration;
            reservationDAO = new ReservationDAOForDB(_connectionString);
        }

        public List<ReservationsSearchResult> SearchReservations(User currentUser, string query)
        {
            var filteredReservations = new List<ReservationsSearchResult>();

            try
            {
                filteredReservations = reservationDAO.GetReservationsByQuery(currentUser, query);

                if (filteredReservations.Count == 0)
                {
                    throw new NoMatchingResultsException("No matching results.");
                }
            }
            catch { }

            return filteredReservations;
        }

        public void ReserveBook(User currentUser, Book book)
        {
            try
            {
                List<Reservation> reservationsList = reservationDAO.GetReservations();
                var newReservation = new Reservation()
                {
                    ReservationId = (reservationsList.Count == 0 ? 1 : (reservationsList[reservationsList.Count - 1].ReservationId + 1)),
                    UserId = currentUser.UserId,
                    BookId = book.BookId,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddDays(_reservationDuration)
                };

                List<Reservation> currentBookReservations = reservationsList.Where(r =>
                    r.BookId == newReservation.BookId && r.EndDate > DateTime.Now).ToList();
                Reservation currentUserBookReservation = currentBookReservations.Where(r =>
                    r.UserId == currentUser.UserId).OrderByDescending(r => r.EndDate).SingleOrDefault();

                if (currentUserBookReservation != null)
                {
                    throw new AlreadyExistingReservationException("Reservation already existing for this user.");
                }
                if (currentBookReservations.Count == book.Quantity)
                {
                    throw new NoAvailableCopiesException("No copies available");
                }

                reservationDAO.CreateReservation(newReservation);
            }
            catch { }
        }

        public void ReturnBook(User currentUser, Book bookToReturn)
        {
            try
            {
                Reservation currentReservation = reservationDAO.GetReservations().Where(r => r.BookId == bookToReturn.BookId && 
                    r.UserId == currentUser.UserId && r.EndDate > DateTime.Now).SingleOrDefault();

                if (currentReservation == null)
                {
                    throw new NoBookReservationsExistingException("Current book reservation not existing.");
                }

                currentReservation.EndDate = DateTime.Now.Date;

                reservationDAO.UpdateReservation(currentReservation);
            }
            catch { }
        }
    }
}