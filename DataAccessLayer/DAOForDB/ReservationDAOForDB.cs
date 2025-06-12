using EntityDataModel;
using EntityDataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer
{
    public class ReservationDAOForDB : IReservationDAO
    {
        private readonly string _connectionString;

        public ReservationDAOForDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Reservation> GetReservations()
        {
            var reservations = new List<Reservation>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_GetReservations", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new Reservation();

                            DALUtility<Reservation>.ReservationsRead(reservation, reader);
                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }

        public List<ReservationsSearchResult> GetReservationsByQuery(User currentUser, string query) 
        {
            string[] splittedQuery = query.Split(' ');
            var filteredReservations = new List<ReservationsSearchResult>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var subQuery in splittedQuery)
                {
                    using (var command = new SqlCommand("SP_GetReservationsByQuery", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SubQuery", subQuery);
                        command.Parameters.AddWithValue("@ConnectedUserId", currentUser.UserId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reservation = new ReservationsSearchResult();

                                DALUtility<Reservation>.ReservationsSearchResultRead(reservation, reader);
                                filteredReservations.Add(reservation);
                            }
                        }
                    }
                }
            }

            if (String.IsNullOrEmpty(query) || splittedQuery.Count() == 1)
            {
                return filteredReservations;
            }

            return filteredReservations.GroupBy(r => new { r.BookTitle, r.UserUsername, r.StartDate, r.EndDate }).
                ToList().Where(g => g.Count() > 1).Select(g => g.First()).ToList();
        }

        public void UpdateReservation(Reservation updatedReservation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_UpdateReservation", connection))
                {
                    DALUtility<Reservation>.ReservationsWrite(command, updatedReservation);
                }
            }
        }

        public void CreateReservation(Reservation newReservation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_CreateReservation", connection))
                {
                    DALUtility<Reservation>.ReservationsWrite(command, newReservation);
                }
            }
        }

        public void DeleteReservations(int bookId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_DeleteReservation", connection))
                {
                    DALUtility<Reservation>.ReservationsDelete(command, bookId);
                }
            }
        }
    }
}