using BusinessLogic;
using EntityDataModel;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LibraryConsole
{
    public class ConsoleManager
    {
        private User currentUser;

        private LibraryManager _libraryManager;
        private UserBL _userBL;

        public ConsoleManager(LibraryManager libraryManager, UserBL userBL)
        {
            _libraryManager = libraryManager;
            _userBL = userBL;
        }

        public void StartLibraryConsole()
        {
            Console.WriteLine("Ti diamo il benvenuto in LIBRARY CONSOLE APP\n");
            Console.WriteLine("Premi un tasto qualsiasi per procedere con il login");
            Console.ReadKey();

            DoLogin();
        }

        public void DoLogin()
        {
            bool success = default;

            while (!success)
            {
                Console.Clear();
                Console.WriteLine("Login.................\n");
                Console.Write("Inserisci lo username: ");
                var username = Console.ReadLine();
                Console.Write("Inserisci la password: ");
                var password = Console.ReadLine();

                try
                {
                    success = _userBL.ValidateLogin(username, password, out currentUser);
                }
                catch
                {
                    Console.WriteLine("\nUsername o password errati! Riprova.");
                    Thread.Sleep(1000);

                    continue;
                }

                Console.Clear();
                Console.WriteLine($"Benvenuto {currentUser.Username}! [{currentUser.Role}]");
                Thread.Sleep(1000);

                GetMenu();
            }
        }

        public void GetMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Ricerca un libro");

            if (currentUser.Role == User.UserRole.Admin)
            {
                Console.WriteLine("2. Modifica un libro");
                Console.WriteLine("3. Inserisci un libro");
                Console.WriteLine("4. Rimuovi un libro");
                Console.WriteLine("5. Visualizza lo storico delle prenotazioni");
                Console.WriteLine("6. Richiedi un prestito");
                Console.WriteLine("7. Restituisci un libro");
                Console.WriteLine("8. Esci");
            }
            else
            {
                Console.WriteLine("2. Visualizza lo storico delle prenotazioni");
                Console.WriteLine("3. Chiedi un prestito");
                Console.WriteLine("4. Restituisci un libro");
                Console.WriteLine("5. Esci");
            }

            MakeChoice();
        }

        public void MakeChoice()
        {
            Console.WriteLine("\nDigita una scelta e premi Invio.");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _libraryManager.SearchBook(out List<Book> filteredList);
                    break;

                case "2":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.ModifyBook();
                    else
                        _libraryManager.ShowReservations(currentUser);
                    break;

                case "3":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.AddBook();
                    else
                        _libraryManager.ReserveBook(currentUser);
                    break;

                case "4":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.RemoveBook();
                    else
                        _libraryManager.ReturnBook(currentUser);
                    break;

                case "5":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.ShowReservations(currentUser);
                    else
                        _libraryManager.Exit();
                    break;

                case "6":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.ReserveBook(currentUser);
                    else
                        Console.WriteLine("Scelta non valida!");
                    break;

                case "7":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.ReturnBook(currentUser);
                    else
                        Console.WriteLine("Scelta non valida!");
                    break;

                case "8":
                    if (currentUser.Role == User.UserRole.Admin)
                        _libraryManager.Exit();
                    else
                    {
                        Console.WriteLine("Scelta non valida!");
                        Thread.Sleep(1000);
                    }
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Scelta non valida!");
                    Thread.Sleep(1000);
                    break;
            }

            GetMenu();
        }
    }
}