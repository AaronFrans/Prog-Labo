using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Tool3.Functionality
{
    public class ControllerTool3
    {

        public static void RunTool3()
        {
            bool endProgram = false;
            string connectionString = GetConnectionString();
            while (!endProgram)
            {
                try
                {
                    DbRequest dbRequest = new DbRequest(connectionString);
                    RunDbRequests(dbRequest);
                    endProgram = AskToQuit();
                }
                catch(NotAnIntException nie)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(nie.GetType());
                    Console.WriteLine(nie.Message);
                    Console.WriteLine("******************************");
                }
                catch (SqlException se )
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(se.GetType());
                    Console.WriteLine("Er is iets fout met de Sql databank.(kijk of de service van de dfatabank aan staat, of dat de tables de juiste naam hebben.)");
                    Console.WriteLine("******************************");
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(e.GetType());
                    Console.WriteLine(e.Message);
                    Console.WriteLine("******************************");

                    Console.WriteLine("Dit was een onverwachte fout gelieve het programma af te sluiten door X in te vullen");
                    if (Console.ReadLine() == "X")
                    {
                        endProgram = true;
                    }
                }
            }
        }

        private static void RunDbRequests(DbRequest dbRequest)
        {

            Console.Clear();
            Console.WriteLine("Welke query wilt u uitvoeren?");
            Console.WriteLine("Query 1: Alle straten van een opgegeven gemeente naam.");
            Console.WriteLine("Query 2: Alle straten alphabetisch van een opgegeven gemeente ID ");
            Console.WriteLine("Query 3: Een straat via een opgegeven straat ID.");
            Console.WriteLine("Query 4: Een straat via een opgegeven straat en gemeente naam.");
            if (!int.TryParse(Console.ReadLine(), out int inputQuerySelector))
                throw new NotAnIntException("Het getal opgegeven om een query te kiezen is niet van het type int.");
            while (inputQuerySelector < 1 || inputQuerySelector > 4)
            {
                Console.WriteLine("Geef aub een geldige input:");
                Console.WriteLine("1: Alle straten van een opgegeven gemeente naam.");
                Console.WriteLine("2: Alle straten alphabetisch van een opgegeven gemeente ID ");
                Console.WriteLine("3: Een straat via een opgegeven straat ID.");
                Console.WriteLine("4: Een straat via een opgegeven straat en gemeente naam.");
                if (!int.TryParse(Console.ReadLine(), out inputQuerySelector))
                    throw new NotAnIntException("Het getal opgegeven om een query te kiezen is niet van het type int.");
            }
            switch (inputQuerySelector)
            {
                case 1:
                    QueryStratenforGemeenteWithNaam(dbRequest);
                    break;
                case 2:
                    QueryStratenforGemeenteWithID(dbRequest);
                    break;
                case 3:
                    QueryStraatWithID(dbRequest);
                    break;
                case 4:
                    QueryStraatWithGemeenteEnStraatNaam(dbRequest);
                    break;
            }
        }
        private static void QueryStratenforGemeenteWithNaam(DbRequest dbRequest)
        {
            Console.WriteLine("Geef de gemeente naam die u wilt opzoeken.");
            string input = Console.ReadLine();
            dbRequest.RequestStraatIDs(input);
        }
        private static void QueryStratenforGemeenteWithID(DbRequest dbRequest)
        {
            Console.WriteLine("Geef de gemeente ID die u wilt opzoeken.");
            if (!int.TryParse(Console.ReadLine(), out int input))
                throw new NotAnIntException("Het opgegeven getal bij query 2 is niet van het type int");
            dbRequest.RequestStraatIDs(input);
        }
        private static void QueryStraatWithID(DbRequest dbRequest)
        {
            Console.WriteLine("Geef de straat ID die u wilt opzoeken.");
            if (!int.TryParse(Console.ReadLine(), out int input))
                throw new NotAnIntException("Het opgegeven getal bij query 3 is niet van het type int");
            dbRequest.RequestStraat(input);
        }
        private static void QueryStraatWithGemeenteEnStraatNaam(DbRequest dbRequest)
        {
            Console.WriteLine("Geef de gemeente naam die u wilt opzoeken.");
            string inputGemeente = Console.ReadLine();
            Console.WriteLine("Geef de straat naam die u wilt opzoeken.");
            string inputStraat = Console.ReadLine();
            dbRequest.RequestStraat(inputStraat, inputGemeente);
        }
        private static string GetConnectionString()
        {

            Console.WriteLine("Geef de Connection String");
            string toReturn = @"" + Console.ReadLine();
            return toReturn;
        }

        private static bool AskToQuit()
        {
            Console.WriteLine("Wilt u Tool 3 stoppen? Y/N");
            string wantToQuitAnswer = Console.ReadLine();
            while (!(wantToQuitAnswer.Equals("Y") || wantToQuitAnswer.Equals("N")))
            {
                Console.WriteLine("Geef aub een geldig antwoord: Y of N");
                wantToQuitAnswer = Console.ReadLine();
            }
            if (wantToQuitAnswer.Equals("N"))
            {
                return false; 
            }
            else
            {
                return true;
            }

        }
    }
}

