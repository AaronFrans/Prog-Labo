using Objects;
using System;
using System.Collections.Generic;
using Tool2.Utilities;

namespace Tool2
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=DESKTOP-CQ5M5QL\SQLEXPRESS;Initial Catalog=Labo;Integrated Security=True";

            DbBeheer dbBeheer = new DbBeheer(connectionString);
            Console.WriteLine("dbBeheer Aangemaakt");
            List<Provincie> provincies = Parser.Deserialize();
            Console.WriteLine("provincies Aangemaakt");
            dbBeheer.InsertIntoDB(provincies);

            Console.WriteLine();
        }
    }
}
