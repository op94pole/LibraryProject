using BusinessLogic;
using EntityDataModel;
using EntityDataModel.CustomExceptions;
using System;
using System.Collections.Generic;

namespace LibraryConsole
{
    public class LibraryManager
    {
        private BookBL _bookBL;
        private ReservationBL _reservationBL;

        public LibraryManager(BookBL bookBL, ReservationBL reservationBL)
        {
            _bookBL = bookBL;
            _reservationBL = reservationBL;
        }

        public void SearchBook(out List<Book> filteredList)
        {
            var choice = "y";
            filteredList = new List<Book>();

            while (choice.ToLowerInvariant() == "y")
            {
                Console.Clear();
                Console.WriteLine("Ricerca un libro\n");
                Console.Write("Cerca: ");
                var query = Console.ReadLine();
                Console.Clear();

                try
                {
                    int counter = 1;
                    filteredList = _bookBL.SearchBooks(query);

                    filteredList.ForEach(b => Console.WriteLine($"{counter++}. {b.ToString()}"));

                    choice = ConsoleUtility.NewSearch();
                }
                catch (NoMatchingResultsException)
                {
                    Console.Write("Nessuna corrispondenza trovata! Vuoi riprovare? [y/n] ");
                    choice = Console.ReadLine();
                }
            }
        }

        public void ModifyBook()
        {
            string title = "", authorName = "", authorSurname = "", publisher = "";
            string input = "y";

            while (input.ToLowerInvariant() == "y")
            {
                Console.Clear();
                Console.WriteLine("Modifica un libro\n");

                SearchBook(out List<Book> filteredList);

                Console.Write("\nDigita una scelta e premi Invio per modificare il libro. ");
                Int32.TryParse(Console.ReadLine(), out int choice);

                if (!ConsoleUtility.ValidInput(choice, filteredList.Count))
                {
                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }

                ConsoleUtility.ValoriseBook(out title, out authorName, out authorSurname, out publisher);

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(authorName) || string.IsNullOrEmpty(authorSurname) ||
                    string.IsNullOrEmpty(publisher))
                {
                    Console.Write("\nI campi non sono stati valorizzati correttamente! ");

                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }

                try
                {
                    Book toUpdateBook = filteredList[choice - 1];

                    _bookBL.ModifyBook(toUpdateBook, title, authorName, authorSurname, publisher);

                    Console.Write("\nModifica effettuata con successo. Vuoi modificare un altro libro? [y/n] ");
                    input = Console.ReadLine();
                }
                catch (AlreadyExistingBookException)
                {
                    Console.Write("\nImpossibile procedere con la modifica! Libro già presente a sistema.\nVuoi riprovare? [y/n] ");
                    input = Console.ReadLine();
                }
            }
        }

        public void AddBook()
        {
            string title = "", authorName = "", authorSurname = "", publisher = "";
            string input = "y";

            while (input.ToLowerInvariant() == "y")
            {
                ConsoleUtility.ValoriseBook(out title, out authorName, out authorSurname, out publisher);

                Console.Write("Inserisci la quantità: ");
                Int32.TryParse(Console.ReadLine(), out int quantity);

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(authorName) || string.IsNullOrEmpty(authorSurname) ||
                    string.IsNullOrEmpty(publisher) || quantity <= 0)
                {
                    Console.Write("\nI campi non sono stati valorizzati correttamente! ");

                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }

                _bookBL.InsertBook(title, authorName, authorSurname, publisher, quantity);

                Console.Write("\nInserimento effettuato con successo. Vuoi inserire un altro libro? [y/n] ");
                input = Console.ReadLine();
            }
        }

        public void RemoveBook()
        {
            string response = "";
            string input = "y";

            while (input.ToLowerInvariant() == "y")
            {
                Console.Clear();
                Console.WriteLine("2. Rimuovi un libro\n");

                SearchBook(out List<Book> filteredList);

                Console.WriteLine("\nDigita una scelta e premi Invio per rimuovere il libro.");
                Int32.TryParse(Console.ReadLine(), out int choice);

                if (!ConsoleUtility.ValidInput(choice, filteredList.Count))
                {
                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }

                try
                {
                    Book choicedBook = filteredList[choice - 1];

                    _bookBL.RemoveBook(choicedBook, out response);

                    Console.Write("\nLibro rimosso con successo. Vuoi rimuovere un altro libro? [y/n] ");
                    input = Console.ReadLine();
                }
                catch (ExistingBookReservationException)
                {
                    Console.Write("\nImpossibile procedere con l'eliminazione! Risultano prenotazioni attive del libro.\n");
                    Console.WriteLine(response);

                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }
            }
        }

        public void ShowReservations(User currentUser)
        {
            int counter = 1;
            string choice = "y";

            while (choice.ToLowerInvariant() == "y")
            {
                Console.Clear();
                Console.WriteLine("1. Visualizza lo storico delle prenotazioni\n");
                Console.Write("Cerca: ");
                var query = Console.ReadLine();

                try
                {
                    List<ReservationsSearchResult> filteredList = _reservationBL.SearchReservations(currentUser, query);

                    Console.Clear();

                    filteredList.ForEach(r => Console.WriteLine($"{counter++}. {r.ToString()}"));
                    Console.WriteLine();
                }
                catch (NoMatchingResultsException)
                {
                    Console.Clear();
                    Console.Write("Nessuna corrispondenza trovata! ");
                }

                Console.Write("Desideri effettuare una nuova ricerca? [y/n] ");
                choice = Console.ReadLine();
            }
        }

        public void ReserveBook(User currentUser)
        {
            string input = "y";

            while (input.ToLowerInvariant() == "y")
            {
                Console.Clear();
                Console.WriteLine("5. Richiedi un prestito\n");

                SearchBook(out List<Book> filteredBooksList);

                Console.Write("\nDigita una scelta e premi Invio per prenotare il libro. ");
                Int32.TryParse(Console.ReadLine(), out int choice);

                if (!ConsoleUtility.ValidInput(choice, filteredBooksList.Count))
                {
                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }

                try
                {
                    Book currentBook = filteredBooksList[choice - 1];

                    _reservationBL.ReserveBook(currentUser, currentBook);

                    Console.Write("\nPrenotazione effettuata con successo. Vuoi prenotare un altro libro? [y/n] ");
                    input = Console.ReadLine();
                }                
                catch (NoAvailableCopiesException)
                {
                    Console.WriteLine("\nImpossibile procedere con la prenotazione! Tutte le copie risultano già in prestito.");

                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }
                catch (AlreadyExistingReservationException)
                {
                    Console.WriteLine("\nImpossibile procedere con la prenotazione! Possiedi già questo libro in prestito.");

                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }
            }
        }

        public void ReturnBook(User currentUser)
        {
            string input = "y";

            while (input.ToLowerInvariant() == "y")
            {
                Console.Clear();
                Console.WriteLine("6. Restituisci un libro\n");

                SearchBook(out List<Book> filteredBooksList);

                Console.WriteLine("\nDigita una scelta e premi Invio per restituire il libro.");
                Int32.TryParse(Console.ReadLine(), out int choice);

                if (!ConsoleUtility.ValidInput(choice, filteredBooksList.Count))
                {
                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }

                try
                {
                    Book bookToReturn = filteredBooksList[choice - 1];

                    _reservationBL.ReturnBook(currentUser, bookToReturn);

                    Console.Write("\nLibro restituito con successo. Vuoi restituire un altro libro? [y/n] ");
                    input = Console.ReadLine();
                }
                catch (NoBookReservationsExistingException)
                {
                    Console.Write("\nImpossibile procedere con la restituzione! Non hai questo libro in prestito.\n");

                    if (!ConsoleUtility.Retry())
                    {
                        break;
                    }

                    continue;
                }
            }
        }

        public void Exit()
        {
            System.Environment.Exit(0);
        }
    }
}