using EntityDataModel;
using System;

namespace LibraryConsole
{
    public static class ConsoleUtility
    {
        public static bool ValidInput(int choice, int listCount)
        {
            if (choice == 0 || choice > listCount)
            {
                Console.Clear();
                Console.Write("Scelta non valida! ");

                return false;
            }

            return true;
        }

        public static bool Retry()
        {
            Console.Write("Vuoi riprovare? [y/n] ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "y":
                    Console.Clear();
                    return true;

                default:
                    Console.Clear();
                    return false;
            }
        }

        public static string NewSearch()
        {
            Console.Write("\nDesideri effettuare una nuova ricerca? [y/n] ");
            var choice = Console.ReadLine();

            return choice;
        }

        public static void ValoriseBook(out string title, out string authorName, out string authorSurname, out string publisher)
        {
            Console.Clear();
            Console.Write("Inserisci il titolo: ");
            title = Console.ReadLine();
            Console.Write("Inserisci il nome dell'autore: ");
            authorName = Console.ReadLine();
            Console.Write("Inserisci il cognome dell'autore: ");
            authorSurname = Console.ReadLine();
            Console.Write("Inserisci la casa editrice: ");
            publisher = Console.ReadLine();
        }
    }
}