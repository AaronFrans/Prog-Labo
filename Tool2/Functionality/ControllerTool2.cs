using Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Tool2.Utilities;

namespace Tool2.Functionality
{
    public class ControllerTool2
    {
        public static void RunTool2()
        {
            bool endProgram = false;
            while (!endProgram)
            {
                try
                {
                    string connectionString = GetConnectionString();

                    DbBeheer dbBeheer = new DbBeheer(connectionString);
                    Console.Clear();
                    List<Provincie> provincies = Parser.Deserialize();
                    dbBeheer.InsertIntoDB(provincies);

                    Console.Clear();
                    Console.WriteLine("****************************************");
                    Console.WriteLine("Programma Klaar: Het mag gesloten worden.");
                    Console.WriteLine("Alle items zitten in de databank");
                    Console.WriteLine("****************************************");
                    endProgram = true;
                }
                catch (DirectoryNotFoundException dnf)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(dnf.GetType());
                    Console.WriteLine("Geef een geldig pad op dat de WRdata-Output folder bevat.(Als u deze folder niet heeft moet u eerst Tool 1 uitvoeren)");
                    Console.WriteLine("******************************");


                    Console.WriteLine("Als dit probleem blijft voorkomen, geef X in om het programma te sluiten. Kijk of alle files aanwezig zijn");
                    if (Console.ReadLine() == "X")
                    {
                        endProgram = true;
                    }

                }
                catch (FileNotFoundException fnf)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(fnf.GetType());
                    Console.WriteLine("De ProvinciesSerialized.txt file is niet in de WRData-Output folder. Gelieve deze daar te plaatsen.(Als u deze file niet heeft moet u eerst Tool 1 uitvoeren)");
                    Console.WriteLine("******************************");

                }
                catch(SqlException se)
                {
                    Console.Clear();
                    Console.WriteLine("******************************");
                    Console.WriteLine(se.GetType());
                    Console.WriteLine("Er is iets fout met de Sql databank.(kijk of de service van de databank aan staat, of dat de tables de juiste naam hebben.)");
                    Console.WriteLine("Gelieve alle databnken terug leeg te maken");
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
        private static string GetConnectionString()
        {
            
            Console.WriteLine("Geef de Connection String");
            string toReturn = @"" + Console.ReadLine();
            return toReturn;
        }
    }
}
