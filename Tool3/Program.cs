using System;
using System.Collections.Generic;

namespace Tool3
{
    class Program
    {
        static void Main(string[] args)
        {
            DbRequest dbRequest = new DbRequest(@"Data Source=DESKTOP-CQ5M5QL\SQLEXPRESS;Initial Catalog=Labo;Integrated Security=True");
            Console.WriteLine("geef straatnaam");
            string input1 = Console.ReadLine(); 
            Console.WriteLine("geef gemeentenaam");
            string input2 = Console.ReadLine();
            dbRequest.RequestStraat(input1, input2);

            Console.ReadLine();
        }
    }
}
