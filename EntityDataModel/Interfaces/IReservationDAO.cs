using System.Collections.Generic;

namespace EntityDataModel.Interfaces
{
    public interface IReservationDAO
    {
        List<Reservation> GetReservations();
        List<ReservationsSearchResult> GetReservationsByQuery(User currentUser, string query);        
        void UpdateReservation(Reservation reservationToUpdate);
        void CreateReservation(Reservation newReservation);
        void DeleteReservations(int bookId);
    }
}