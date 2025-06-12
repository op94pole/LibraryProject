using BusinessLogic;
using System;
using System.Configuration;

namespace LibraryConsole
{
    public class Program
    {
        private static string xmlPath = ConfigurationManager.ConnectionStrings["Library"].ToString();
        private static string connectionString = ConfigurationManager.ConnectionStrings["Library"].ConnectionString.ToString();
        private static int reservationDuration = Int32.Parse(ConfigurationManager.AppSettings["ReservationDuration"]);

        static void Main(string[] args)
        {
            UserBL userBL = new UserBL(connectionString);
            BookBL bookBL = new BookBL(connectionString);
            ReservationBL reservationBL = new ReservationBL(connectionString, reservationDuration);

            LibraryManager libraryManager = new LibraryManager(bookBL, reservationBL);
            ConsoleManager consoleManager = new ConsoleManager(libraryManager, userBL);

            consoleManager.StartLibraryConsole();
        }
    }
}