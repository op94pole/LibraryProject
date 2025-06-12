using System;

namespace EntityDataModel
{
    public class ReservationsSearchResult
    {
        private string state;

        public string BookTitle { get; set; }
        public string UserUsername { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get => EndDate > DateTime.Now ? "attiva" : "non attiva";  set =>  state = string.Empty; }

        public override string ToString()
        {
            return $"{BookTitle}, {UserUsername}, {StartDate.ToShortDateString()} - {EndDate.ToShortDateString()}, {State}";
        }
    }
}