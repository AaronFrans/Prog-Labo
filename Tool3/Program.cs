using System;
using System.Collections.Generic;

namespace Tool3
{
    class Program
    {
        static void Main(string[] args)
        {
            DbRequest dbRequest = new DbRequest(@"Data Source=DESKTOP-CQ5M5QL\SQLEXPRESS;Initial Catalog=Labo;Integrated Security=True");
            Console.WriteLine("geef gemeentenaam");
            string input = Console.ReadLine();
            dbRequest.RequestStraatIDs(input);
            Console.ReadLine();
        }
    }
}
