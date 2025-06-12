using System;

namespace EntityDataModel
{
    public class Reservation
    {
        //public Reservation()
        //{
        //    StartDate = DateTime.Now.Date;
        //    EndDate = StartDate.AddDays(30).Date;
        //}

        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}